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
    private static bool _betterRendering;

    public static bool NoTriangleSubdivision
    {
        get => _betterRendering;
        set
        {
            _betterRendering = value;
            RecompOne.Runtime.Hle.NativeGeometry.Enabled = false;
            TerrainFloatProjection.Reset();
            EntityFloatProjection.Reset();
            RecompOne.Runtime.Hle.NativeGeometry.DepthBuffer = value;
            RecompOne.Runtime.Hle.NativeGeometry.Enabled = value;
        }
    }
    
    public static bool NearClamp => NoTriangleSubdivision;

    public static uint NearOtz(uint otz)
    {
        if (!NearClamp) return otz;

        int near = (short)RecompOne.Runtime.Gte.ReadControl(26) / 2;

        for (int z = 17; z <= 19; z++)
            if ((int)RecompOne.Runtime.Gte.Read(z) < near)
                return otz;

        return (int)otz < NearClip ? NearClip : otz;
    }

    static short _baseDistance;
    static int _baseReach;
    static int _baseOtBits;
    static int _baseOtSize;
    static int _baseShift;
    static uint _viewport;
    static uint _originalOt0;
    static uint _originalOt1;
    static int _lastDistance;

    public const int BaseSubdivOtz = 4096;

    public static int SubdivOtz => NoTriangleSubdivision ? 0 : BaseSubdivOtz;
    
    public static void Register()
    {
        Event.AddListener<RuntimeReadyEvent>(_ =>
        {
            DrawDistanceScale = Math.Clamp(RecompOne.Runtime.Runtime.View.GetFloat("DrawDistanceScale", 1f), 1f, 3f);
            NoTriangleSubdivision = RecompOne.Runtime.Runtime.View.GetBool("BetterRendering",
                RecompOne.Runtime.Runtime.View.GetBool("NoTriangleSubdivision",
                    RecompOne.Runtime.Runtime.View.GetBool("BetterTerrain", false)));
        });
        Event.AddListener<OverlayLoadedEvent>(_ => TerrainCapture.Forget());
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
    
    public static void PrepareFrame(CpuContext c, IMemory m)
    {
        uint viewport = m.ReadU32(c.GP + MainViewPortSlot);
        if (viewport == 0u || c.A0 != viewport) return;
        if (_viewport != viewport)
        {
            if (_viewport != 0u) RestoreViewport(m);
            _viewport = viewport;
            _baseReach = m.ReadU16(viewport + VpViewDistance);
            _baseOtSize = m.ReadU16(viewport + VpOtSize);
            _baseOtBits = m.ReadU16(viewport + VpOtSizeBits);
            _baseShift = m.ReadU16(viewport + VpOtzShift);
            _originalOt0 = m.ReadU32(viewport + VpOt0);
            _originalOt1 = m.ReadU32(viewport + VpOt1);
            _baseDistance = 0;
            _lastDistance = 0;
        }
        if (_baseReach <= 0 || _baseOtSize <= 0 || _baseShift > 15) return;

        int wanted = Math.Clamp((int)MathF.Ceiling(_baseReach * DrawDistanceScale), _baseReach, 32768);
        int size = _baseOtSize;
        while ((long)size << _baseShift < wanted && size * 8 <= OtStride) size *= 2;
        int reach = (int)Math.Min(wanted, (long)size << _baseShift);
        uint ot0 = size > _baseOtSize ? OtBase : _originalOt0;
        uint ot1 = size > _baseOtSize ? OtBase + OtStride : _originalOt1;
        uint old0 = m.ReadU32(viewport + VpOt0);
        uint old1 = m.ReadU32(viewport + VpOt1);
        uint work = m.ReadU32(viewport + VpWorkOt);
        if (work != old0 && work != old1) return;
        if (old0 != ot0 || m.ReadU16(viewport + VpOtSize) != size)
        {
            Blank(m, ot0, (uint)size);
            Blank(m, ot1, (uint)size);
            m.WriteU32(viewport + VpOt0, ot0);
            m.WriteU32(viewport + VpOt1, ot1);
            m.WriteU32(viewport + VpWorkOt, work == old0 ? ot0 : ot1);
            m.WriteU16(viewport + VpOtSize, (ushort)size);
            m.WriteU16(viewport + VpOtSizeBits, (ushort)Log2(size));
            if (m.ReadU32(0x1F800008u) == viewport)
                m.WriteU32(0x1F80000Cu, work == old0 ? ot0 : ot1);
        }
        m.WriteU16(viewport + VpViewDistance, (ushort)reach);
    }

    public static void ReleaseViewport(CpuContext c, IMemory m)
    {
        if (c.A0 != _viewport || _viewport == 0u) return;
        RestoreViewport(m);
        _viewport = 0;
        _baseDistance = 0;
        _lastDistance = 0;
    }

    static void RestoreViewport(IMemory m)
    {
        uint work = m.ReadU32(_viewport + VpWorkOt);
        uint ot0 = m.ReadU32(_viewport + VpOt0);
        m.WriteU32(_viewport + VpWorkOt, work == ot0 ? _originalOt0 : _originalOt1);
        m.WriteU32(_viewport + VpOt0, _originalOt0);
        m.WriteU32(_viewport + VpOt1, _originalOt1);
        m.WriteU16(_viewport + VpOtSize, (ushort)_baseOtSize);
        m.WriteU16(_viewport + VpOtSizeBits, (ushort)_baseOtBits);
        m.WriteU16(_viewport + VpOtzShift, (ushort)_baseShift);
        m.WriteU16(_viewport + VpViewDistance, (ushort)_baseReach);
    }

    public static uint MeshClipDistance(uint distance, IMemory m)
    {
        if (_viewport == 0u || m.ReadU32(0x1F800088u) != _viewport || distance == 0u)
            return distance;
        return (uint)Math.Min(65535, Math.Ceiling(distance * DrawDistanceScale));
    }

    static void Blank(IMemory m, uint ot, uint size)
    {
        m.WriteU32(ot, 0x00FFFFFFu);
        for (uint i = 1; i < size; i++) m.WriteU32(ot + i * 4u, (ot + (i - 1) * 4u) & 0x00FFFFFFu);
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
        if (_viewport == 0u) return;
        short current = (short)m.ReadU16(ViewDistance);
        if (current <= 0) return;
        if (_baseDistance == 0 || current != _lastDistance) _baseDistance = current;
        
        int ceiling = Math.Min(32767, (int)m.ReadU16(_viewport + VpViewDistance));
        if (ceiling <= 0) return;
        
        int scaled = Math.Clamp((int)MathF.Round(_baseDistance * DrawDistanceScale), _baseDistance, ceiling);
        _lastDistance = scaled;
        if (scaled == current) return;
        
        m.WriteU16(ViewDistance, (ushort)(short)scaled);
        m.WriteU16(OldViewDistance, (ushort)(short)(scaled - 1));

        Fog(m, scaled);
    }
}
