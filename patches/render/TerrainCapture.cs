using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class TerrainCapture
{
    const uint ViewPlaneSvecs = 0x800EEA04u;
    const uint SquaresCaptured = 0x80300000u;
    const uint CurrentLevel = 0x800EEE2Cu;
    const uint CurrentMap = 0x800EEE7Cu;
    const uint ViewDistance = 0x800EEDE4u;

    const uint GpCameraTarget = 0x560u;
    const uint GpProjDepth = 0x5B0u;
    const uint GpBasePoint = 0x5B4u;
    const uint GpTitleWorld = 0x648u;

    const uint CameraMatrix = 0x60u;
    const uint MatrixTranslation = 0x14u;

    const uint TlwsCamProjectedY = 0xACu;
    const uint EntMove = 0x80u;
    const uint EntMatrixY = 0x28u;
    const uint MoveGroundPoly = 0x80u;
    const uint MoveGroundPosY = 0x9Eu;

    const uint MapPolyGrid = 0x6Cu;
    const uint GridXSquares = 0x00u;
    const uint GridZSquares = 0x01u;
    const uint GridIdTable = 0x08u;
    const uint GridDataBase = 0x0Cu;

    const int SquareStride = 8;
    const uint SquarePolyCount = 0u;
    const uint SquareMinVertexY = 2u;
    const uint SquarePolyIds = 4u;

    const ushort CapturedBit = 0x8000;

    const int MaxGridRows = 129;

    static readonly short[] _low = new short[MaxGridRows];
    static readonly short[] _high = new short[MaxGridRows];

    readonly record struct Square(uint Address, int Distance, int Polys);

    static readonly List<Square> _found = [];
    static readonly List<uint> _flagged = [];

    static uint _resolvedFor;
    static uint _len;
    static uint _shift;
    static uint _xnum;
    static uint _znum;
    static uint _ymax;

    static long _stepXy;
    static long _stepXz;
    static long _stepZy;
    static long _stepZz;

    public static int Captured { get; private set; }

    public static void Forget()
    {
        _resolvedFor = 0u;
        _flagged.Clear();
    }

    static bool Resolve(CpuContext c, IMemory m, uint grid)
    {
        if (_resolvedFor == grid) return true;

        int squaresX = m.ReadU8(grid + GridXSquares);
        int squaresZ = m.ReadU8(grid + GridZSquares);
        if (squaresX is < 1 or > MaxGridRows || squaresZ is < 1 or > MaxGridRows) return false;

        ReadOnlySpan<uint> candidates = [0x5ACu, 0x5BCu, 0x5C0u, 0x5C4u, 0x5C8u];

        uint shift = 0u;
        uint len = 0u;
        uint xnum = 0u;
        uint znum = 0u;

        foreach (var slot in candidates)
        {
            uint value = m.ReadU32(c.GP + slot);

            if (value is < 1u or > 15u) continue;

            foreach (var mate in candidates)
            {
                if (mate == slot || m.ReadU32(c.GP + mate) != 1u << (int)value) continue;

                shift = slot;
                len = mate;
                break;
            }

            if (shift != 0u) break;
        }

        foreach (var slot in candidates)
        {
            if (slot == shift || slot == len) continue;

            uint value = m.ReadU32(c.GP + slot);

            if (value == squaresX && xnum == 0u) xnum = slot;
            else if (value == squaresZ && znum == 0u) znum = slot;
        }

        uint ymax = 0u;
        foreach (var slot in candidates)
            if (slot != shift && slot != len && slot != xnum && slot != znum)
                ymax = slot;

        if (shift == 0u || len == 0u || xnum == 0u || znum == 0u || ymax == 0u) return false;

        _shift = shift;
        _len = len;
        _xnum = xnum;
        _znum = znum;
        _ymax = ymax;
        _resolvedFor = grid;

        return true;
    }

    public static bool Capture(CpuContext c, IMemory m)
    {
        uint camera = c.A0;
        uint matrix = camera + CameraMatrix;

        Release(m);
        c.V0 = 0u;

        uint map = m.ReadU32(CurrentMap);
        if (!Ram(map)) return false;

        uint grid = m.ReadU32(map + MapPolyGrid);
        if (!Ram(grid) || !Resolve(c, m, grid)) return false;

        Span<int> gridX = stackalloc int[5];
        Span<int> gridZ = stackalloc int[5];

        if (!Project(c, m, matrix, gridX, gridZ)) return false;

        Rows(gridX, gridZ);
        Scan(c, m, matrix, grid);

        m.WriteU32(SquaresCaptured, 0u);

        c.V0 = (uint)Captured;
        return false;
    }

    static void Release(IMemory m)
    {
        foreach (var square in _flagged)
            m.WriteU16(square + SquarePolyCount, (ushort)(m.ReadU16(square + SquarePolyCount) & ~CapturedBit));

        _flagged.Clear();
        Captured = 0;
    }

    static bool Ram(uint address)
    {
        return address >= 0x80010000u && address < 0x80400000u;
    }

    static bool Project(CpuContext c, IMemory m, uint matrix, Span<int> gridX, Span<int> gridZ)
    {
        int planeY = (short)m.ReadU16(ViewPlaneSvecs + 2u);
        int planeZ = (short)m.ReadU16(ViewPlaneSvecs + 4u);
        if (planeZ == 0) return false;

        int distance = (short)m.ReadU16(ViewDistance);
        long radius = distance * Root((long)planeY * planeY + (long)planeZ * planeZ) / planeZ;
        if (radius <= 0) return false;

        int camX = (int)m.ReadU32(matrix + MatrixTranslation);
        int camY = (int)m.ReadU32(matrix + MatrixTranslation + 4u);
        int camZ = (int)m.ReadU32(matrix + MatrixTranslation + 8u);

        int projY = Height(c, m) + (int)(((long)(int)m.ReadU32(c.GP + GpProjDepth) * distance) >> 12);
        projY = Math.Min(projY, (int)m.ReadU32(c.GP + _ymax));

        int baseX = (short)m.ReadU16(c.GP + GpBasePoint);
        int baseZ = (short)m.ReadU16(c.GP + GpBasePoint + 4u);
        int shift = (int)m.ReadU32(c.GP + _shift);

        for (int corner = 0; corner < 4; corner++)
        {
            uint svec = ViewPlaneSvecs + (uint)corner * 8u;

            Rotate(m, matrix, (short)m.ReadU16(svec), (short)m.ReadU16(svec + 2u), (short)m.ReadU16(svec + 4u),
                out long dx, out long dy, out long dz);

            long x = 0x8000;
            long z = 0x8000;

            if (dy > 0)
            {
                long span = ((long)(projY - camY) << 8) / dy;
                x = span * dx >> 8;
                z = span * dz >> 8;
            }

            if (x is >= 0x8000 or <= -0x8000 || z is >= 0x8000 or <= -0x8000 || Root(x * x + z * z) >= radius)
            {
                long flat = Root(dx * dx + dz * dz);
                if (flat == 0) flat = 1;

                x = dx * radius / flat;
                z = dz * radius / flat;
            }

            gridX[corner] = (int)(camX + x - baseX) >> shift;
            gridZ[corner] = (int)(camZ + z - baseZ) >> shift;
        }

        gridX[4] = (camX - baseX) >> shift;
        gridZ[4] = (camZ - baseZ) >> shift;

        return true;
    }

    static int Height(CpuContext c, IMemory m)
    {
        if (m.ReadU32(CurrentLevel) == 0u)
        {
            uint world = m.ReadU32(c.GP + GpTitleWorld);
            return Ram(world) ? (int)m.ReadU32(world + TlwsCamProjectedY) : 0;
        }

        uint target = m.ReadU32(c.GP + GpCameraTarget);
        if (!Ram(target)) return 0;

        uint move = m.ReadU32(target + EntMove);
        if (Ram(move) && Ram(m.ReadU32(move + MoveGroundPoly))) return (short)m.ReadU16(move + MoveGroundPosY);

        return (int)m.ReadU32(target + EntMatrixY);
    }

    static short Cell(IMemory m, uint matrix, int row, int column)
    {
        return (short)m.ReadU16(matrix + (uint)(row * 3 + column) * 2u);
    }

    static void Rotate(IMemory m, uint matrix, int x, int y, int z, out long ox, out long oy, out long oz)
    {
        ox = (Cell(m, matrix, 0, 0) * (long)x + Cell(m, matrix, 0, 1) * (long)y +
              Cell(m, matrix, 0, 2) * (long)z) >> 12;
        oy = (Cell(m, matrix, 1, 0) * (long)x + Cell(m, matrix, 1, 1) * (long)y +
              Cell(m, matrix, 1, 2) * (long)z) >> 12;
        oz = (Cell(m, matrix, 2, 0) * (long)x + Cell(m, matrix, 2, 1) * (long)y +
              Cell(m, matrix, 2, 2) * (long)z) >> 12;
    }

    static void RotateTransposed(IMemory m, uint matrix, int x, int y, int z, out long oy, out long oz)
    {
        oy = (Cell(m, matrix, 0, 1) * (long)x + Cell(m, matrix, 1, 1) * (long)y +
              Cell(m, matrix, 2, 1) * (long)z) >> 12;
        oz = (Cell(m, matrix, 0, 2) * (long)x + Cell(m, matrix, 1, 2) * (long)y +
              Cell(m, matrix, 2, 2) * (long)z) >> 12;
    }

    static long Root(long value)
    {
        return value <= 0 ? 0 : (long)Math.Sqrt(value);
    }

    static void Rows(Span<int> gridX, Span<int> gridZ)
    {
        for (int row = 0; row < MaxGridRows; row++)
        {
            _low[row] = short.MaxValue;
            _high[row] = short.MinValue;
        }

        Line(gridX[0], gridZ[0], gridX[1], gridZ[1]);
        Line(gridX[1], gridZ[1], gridX[2], gridZ[2]);
        Line(gridX[2], gridZ[2], gridX[3], gridZ[3]);
        Line(gridX[3], gridZ[3], gridX[4], gridZ[4]);
        Line(gridX[4], gridZ[4], gridX[0], gridZ[0]);
        Line(gridX[3], gridZ[3], gridX[0], gridZ[0]);

        Widen();
    }

    static void Widen()
    {
        int first = MaxGridRows;
        int last = -1;

        for (int row = 0; row < MaxGridRows; row++)
        {
            if (_low[row] > _high[row]) continue;

            if (row < first) first = row;
            last = row;

            if (_low[row] > short.MinValue + 1) _low[row]--;
            if (_high[row] < short.MaxValue - 1) _high[row]++;
        }

        if (last < 0) return;

        if (first > 0) Carry(first, first - 1);
        if (last < MaxGridRows - 1) Carry(last, last + 1);
    }

    static void Carry(int from, int to)
    {
        _low[to] = Math.Min(_low[to], _low[from]);
        _high[to] = Math.Max(_high[to], _high[from]);
    }

    static void Line(int x0, int z0, int x1, int z1)
    {
        int steps = Math.Max(Math.Abs(x1 - x0), Math.Abs(z1 - z0));
        if (steps == 0)
        {
            Mark(x0, z0);
            return;
        }

        for (int step = 0; step <= steps; step++)
            Mark(x0 + (x1 - x0) * step / steps, z0 + (z1 - z0) * step / steps);
    }

    static void Mark(int x, int z)
    {
        if (z is < 0 or >= MaxGridRows) return;

        short column = (short)Math.Clamp(x, short.MinValue, short.MaxValue);

        if (column < _low[z]) _low[z] = column;
        if (column > _high[z]) _high[z] = column;
    }

    static void Scan(CpuContext c, IMemory m, uint matrix, uint grid)
    {
        uint ids = m.ReadU32(grid + GridIdTable);
        uint squares = m.ReadU32(grid + GridDataBase);
        if (!Ram(ids) || !Ram(squares)) return;

        int xnum = (int)m.ReadU32(c.GP + _xnum);
        int znum = (int)m.ReadU32(c.GP + _znum);
        int shift = (int)m.ReadU32(c.GP + _shift);
        int length = (int)m.ReadU32(c.GP + _len);

        int baseX = (short)m.ReadU16(c.GP + GpBasePoint);
        int baseZ = (short)m.ReadU16(c.GP + GpBasePoint + 4u);

        int camX = (int)m.ReadU32(matrix + MatrixTranslation);
        int camY = (int)m.ReadU32(matrix + MatrixTranslation + 4u);
        int camZ = (int)m.ReadU32(matrix + MatrixTranslation + 8u);

        int planeY = (short)m.ReadU16(ViewPlaneSvecs + 2u);
        int planeZ = (short)m.ReadU16(ViewPlaneSvecs + 4u);

        RotateTransposed(m, matrix, length, 0, 0, out _stepXy, out _stepXz);
        RotateTransposed(m, matrix, 0, 0, length, out _stepZy, out _stepZz);

        uint list = m.ReadU32(TerrainPatch.CaptureListAddress);
        int room = TerrainPatch.MaxCapture;
        int budget = TerrainPatch.PrimCapacity;
        int rows = Math.Min(znum, MaxGridRows);

        _found.Clear();

        for (int z = 0; z < rows; z++)
        {
            int first = Math.Max(0, (int)_low[z]);
            int last = Math.Min(xnum - 1, (int)_high[z]);
            if (first > last) continue;

            int worldZ = (z << shift) + baseZ - camZ;

            for (int x = first; x <= last; x++)
            {
                int id = (short)m.ReadU16(ids + (uint)(x + z * xnum) * 2u);
                if (id < 0) continue;

                uint square = squares + (uint)id * SquareStride;

                int polys = m.ReadU16(square + SquarePolyCount) & ~CapturedBit;
                if (polys == 0) continue;

                if (!Ram(m.ReadU32(square + SquarePolyIds))) continue;

                int worldX = (x << shift) + baseX - camX;
                int lift = (short)m.ReadU16(square + SquareMinVertexY) - camY;

                if (lift > 0 && !Visible(matrix, m, worldX, lift, worldZ, planeY, planeZ)) continue;

                int middleX = worldX + (length >> 1);
                int middleZ = worldZ + (length >> 1);

                _found.Add(new Square(square, middleX * middleX + middleZ * middleZ, polys));
            }
        }

        int spent = 0;
        int wanted = 0;

        foreach (var square in _found) wanted += square.Polys;

        if (wanted > budget) _found.Sort(static (left, right) => left.Distance.CompareTo(right.Distance));

        foreach (var square in _found)
        {
            if (Captured >= room || spent + square.Polys > budget) break;

            ushort count = m.ReadU16(square.Address + SquarePolyCount);
            uint entry = list + (uint)Captured * SquareStride;

            m.WriteU16(entry + SquarePolyCount, (ushort)(count & ~CapturedBit));
            m.WriteU32(entry + SquarePolyIds, m.ReadU32(square.Address + SquarePolyIds));
            m.WriteU16(square.Address + SquarePolyCount, (ushort)(count | CapturedBit));

            _flagged.Add(square.Address);
            spent += square.Polys;
            Captured++;
        }

    }

    static bool Visible(uint matrix, IMemory m, int x, int y, int z, int planeY, int planeZ)
    {
        RotateTransposed(m, matrix, x, y, z, out long baseY, out long baseZ);

        return Inside(baseY, baseZ, planeY, planeZ) ||
               Inside(baseY + _stepZy, baseZ + _stepZz, planeY, planeZ) ||
               Inside(baseY + _stepXy + _stepZy, baseZ + _stepXz + _stepZz, planeY, planeZ) ||
               Inside(baseY + _stepXy, baseZ + _stepXz, planeY, planeZ);
    }

    static bool Inside(long vy, long vz, int planeY, int planeZ)
    {
        return vy * planeZ <= vz * planeY;
    }
}
