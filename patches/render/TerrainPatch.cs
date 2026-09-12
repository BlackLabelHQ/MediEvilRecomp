using RecompOne.Runtime.Context;
using RecompOne.Runtime.Events;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class TerrainPatch
{
    const uint FreeTerrData = 0x800EEDC0u;
    const uint PrimBuffer0 = FreeTerrData + 0x04u;
    const uint PrimBuffer1 = FreeTerrData + 0x08u;
    const uint CaptureListPtr = FreeTerrData + 0x0Cu;
    const uint RenderPolyPtr = FreeTerrData + 0x10u;
    const uint MaxCap = FreeTerrData + 0x14u;
    const uint BufferLimit0 = FreeTerrData + 0x18u;
    const uint BufferLimit1 = FreeTerrData + 0x1Cu;
    const uint ViewDistance = FreeTerrData + 0x24u;
    const uint BackOfOt = FreeTerrData + 0x2Cu;
    const uint OldViewDistance = FreeTerrData + 0x26u;
    
    const uint MainViewPortSlot = 0x5E0u;
    const uint VpOtSize = 0x68u;
    const uint VpOtSizeBits = 0x6Au;
    const uint VpOtzShift = 0x6Cu;
    const uint VpViewDistance = 0x6Eu;
    const uint VpOt0 = 0x70u;
    const uint VpOt1 = 0x74u;
    const uint VpWorkOt = 0x78u;

    const uint OtBase = 0x803F0000u;
    const uint OtStride = 0x8000u;
    
    const uint TerrainFog = 0x800F17B8u;
    const uint FogTablePtr = TerrainFog + 0x00u;
    const uint FogEntries = TerrainFog + 0x04u;

    const uint FogBase = 0x80308000u;
    const int FogShift = 5;
    const int FogPadEntries = 64;
    const int FogFull = 0xFFF;

    const uint ViewPlaneSvecs = 0x800EEA04u;
    const int PlaneHalfWidth = 160;
    const int PlaneHalfHeight = 120;
    const float CaptureOvershoot = 1.25f;
    const float NearOvershoot = 1.6f;
    
    public const int MaxCapture = 2048;
    const uint CaptureBase = 0x80301000u;
    
    public const uint CaptureListAddress = CaptureListPtr;
    const uint RenderPtrBase = 0x80310000u;

    public const int PrimCapacity = 8192;
    const int PrimCap = PrimCapacity;
    const uint PrimBase = 0x80320000u;
    const uint PrimStride = 0x68000u;
    const uint PrimSlack = 128u;
    
    public const uint NearClip = 4u;
    
    public static float DrawDistanceScale = 1f;
    public static bool BetterTerrain = true;
    
    public static bool NearClamp => BetterTerrain;

    public static uint NearOtz(uint otz)
    {
        if (!NearClamp) return otz;

        int near = (short)RecompOne.Runtime.Gte.ReadControl(26) / 2;

        for (int z = 17; z <= 19; z++)
            if ((int)RecompOne.Runtime.Gte.Read(z) < near)
                return otz;

        return (int)otz < NearClip ? NearClip : otz;
    }

    const bool GrowTable = true;

    static short _baseDistance;
    static int _baseReach;
    static int _baseOtBits;
    static int _baseOtSize;
    static int _baseShift;
    static int _shiftDelta;

    public const int BaseSubdivOtz = 4096;

    public static int SubdivOtz => BetterTerrain ? 0 : BaseSubdivOtz >> _shiftDelta;
    
    public static void Register()
    {
        Event.AddListener<RuntimeReadyEvent>(_ =>
        {
            DrawDistanceScale = Math.Clamp(RecompOne.Runtime.Runtime.View.GetFloat("DrawDistanceScale", 1f), 1f, 3f);
            BetterTerrain = RecompOne.Runtime.Runtime.View.GetBool("BetterTerrain", true);
        });
        Event.AddListener<OverlayLoadedEvent>(_ => { TerrainCapture.Forget(); _baseDistance = 0; _baseReach = 0; _baseOtBits = 0; _baseOtSize = 0; _baseShift = 0; _shiftDelta = 0; });
    }
    
    public static bool Capture(CpuContext c, IMemory m)
    {
        m.WriteU32(CaptureListPtr, CaptureBase);
        c.A1 = MaxCapture;
        c.A2 = CaptureBase;

        m.WriteU32(PrimBuffer0, PrimBase);
        m.WriteU32(PrimBuffer1, PrimBase + PrimStride);
        m.WriteU32(BufferLimit0, PrimBase + PrimStride - PrimSlack);
        m.WriteU32(BufferLimit1, PrimBase + PrimStride * 2u - PrimSlack);
        m.WriteU32(MaxCap, PrimCap);
        
        FrustumCorners(m);
        DrawDistance(c, m);
        ClampBackOfOt(c, m);
        
        return TerrainCapture.Capture(c, m);
    }
    
    static void FrustumCorners(IMemory m)
    {
        float wide = Display.WideAspect;
        float source = Display.SourceAspect > 0f ? Display.SourceAspect : 4f / 3f;
        float scale = wide > 0f ? wide / source : 1f;
        
        short half = (short)Math.Clamp((int)MathF.Round(PlaneHalfWidth * scale * CaptureOvershoot), PlaneHalfWidth, 1024);
        short tall = (short)Math.Clamp((int)MathF.Round(PlaneHalfHeight * NearOvershoot), PlaneHalfHeight, 1024);
        
        Write(m, 0, (short)-half, tall);
        Write(m, 1, (short)-half, (short)-tall);
        Write(m, 2, half, (short)-tall);
        Write(m, 3, half, tall);
    }
    
    static void Write(IMemory m, int index, short x, short y)
    {
        uint p = ViewPlaneSvecs + (uint)index * 8u;
        m.WriteU16(p, (ushort)x);
        m.WriteU16(p + 2u, (ushort)y);
    }
    
    static int Reach(CpuContext c, IMemory m)
    {
        uint viewport = m.ReadU32(c.GP + MainViewPortSlot);
        if (viewport == 0u) return 0;

        int reach = m.ReadU16(viewport + VpViewDistance);
        if (reach <= 0) return 0;

        if (_baseReach == 0) _baseReach = reach;

        int wanted = _baseReach;
        while (wanted < 0x8000 && wanted * 2 <= _baseReach * DrawDistanceScale) wanted *= 2;

        if (_baseOtBits == 0) _baseOtBits = m.ReadU16(viewport + VpOtSizeBits);
        if (_baseShift == 0) _baseShift = m.ReadU16(viewport + VpOtzShift);

        if (wanted != reach)
        {
            _shiftDelta = 0;

            m.WriteU16(viewport + VpViewDistance, (ushort)wanted);
            reach = wanted;
        }

        return reach;
    }

    static int GrowOt2(IMemory m, uint viewport, int delta)
    {
        if (delta <= 0) return 0;

        uint oldSize = _baseOtSize != 0 ? (uint)_baseOtSize : m.ReadU16(viewport + VpOtSize);
        if (oldSize == 0) return 0;

        _baseOtSize = (int)oldSize;

        uint work = m.ReadU32(viewport + VpWorkOt);
        uint ot0 = m.ReadU32(viewport + VpOt0);
        uint ot1 = m.ReadU32(viewport + VpOt1);
        if (work != ot0 && work != ot1) return 0;

        int grow = 0;
        while (grow < delta && (oldSize << (grow + 1)) <= 0xFFFFu && (oldSize << (grow + 1)) * 4u <= OtStride) grow++;

        if (grow == 0) return 0;
        
        uint size = oldSize << grow;

        m.WriteU16(viewport + VpOtSizeBits, (ushort)(_baseOtBits + grow));
        m.WriteU16(viewport + VpOtSize, (ushort)size);
        m.WriteU32(viewport + VpOt0, OtBase);
        m.WriteU32(viewport + VpOt1, OtBase + OtStride);
        m.WriteU32(viewport + VpWorkOt, work == ot0 ? OtBase : OtBase + OtStride);

        Blank(m, OtBase, size);
        Blank(m, OtBase + OtStride, size);

        return grow;
    }

    static void Blank(IMemory m, uint ot, uint size)
    {
        m.WriteU32(ot, 0x00FFFFFFu);
        for (uint i = 1; i < size; i++) m.WriteU32(ot + i * 4u, (ot + (i - 1) * 4u) & 0x00FFFFFFu);
    }

    static void ClampBackOfOt(CpuContext c, IMemory m)
    {
        uint viewport = m.ReadU32(c.GP + MainViewPortSlot);
        if (viewport == 0u) return;

        int size = m.ReadU16(viewport + VpOtSize);
        if (size <= 0) return;

        int back = (int)m.ReadU32(BackOfOt);
        if (back > size) m.WriteU32(BackOfOt, (uint)size);
    }

    static void Fog(IMemory m, int distance)
    {
        int entries = distance >> FogShift;
        if (entries <= 0) return;

        m.WriteU32(FogTablePtr, FogBase);
        m.WriteU16(FogEntries, (ushort)entries);

        uint pad = FogBase + (uint)entries * 2u;
        for (int i = 0; i < FogPadEntries; i++) m.WriteU16(pad + (uint)i * 2u, FogFull);
    }

    static int Log2(int value)
    {
        int n = 0;
        while ((1 << n) < value) n++;
        return n;
    }
    static void DrawDistance(CpuContext c, IMemory m)
    {
        short current = (short)m.ReadU16(ViewDistance);
        if (current <= 0) return;
        if (_baseDistance == 0) _baseDistance = current;
        
        int ceiling = Reach(c, m);
        if (ceiling <= 0) return;
        
        int scaled = Math.Clamp((int)MathF.Round(_baseDistance * DrawDistanceScale), _baseDistance, ceiling);
        if (scaled == current) return;
        
        m.WriteU16(ViewDistance, (ushort)(short)scaled);
        m.WriteU16(OldViewDistance, (ushort)(short)(scaled - 1));

        Fog(m, scaled);
    }
}
