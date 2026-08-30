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
    const uint OldViewDistance = FreeTerrData + 0x26u;
    
    const uint MainViewPortSlot = 0x5E0u;
    const uint VpOtSizeBits = 0x6Au;
    const uint VpOtzShift = 0x6Cu;
    const uint VpViewDistance = 0x6Eu;
    
    const uint TerrainFog = 0x800F17B8u;
    const uint FogTablePtr = TerrainFog + 0x00u;
    const uint FogEntries = TerrainFog + 0x04u;

    const uint FogBase = 0x80370000u;
    const int FogShift = 5;
    const int FogPadEntries = 64;
    const int FogFull = 0xFFF;

    const uint ViewPlaneSvecs = 0x800EEA04u;
    const int PlaneHalfWidth = 160;
    const int PlaneHalfHeight = 120;
    const float CaptureOvershoot = 1.25f;
    
    public const int MaxCapture = 512;
    const uint CaptureBase = 0x80301000u;
    const uint RenderPtrBase = 0x80310000u;

    const int PrimCap = 2400;
    const uint PrimBase = 0x80320000u;
    const uint PrimStride = 0x20000u;
    const uint PrimSlack = 128u;
    
    public static float DrawDistanceScale = 1f;
    
    static short _baseDistance;
    static int _baseReach;
    
    public static void Register()
    {
        Event.AddListener<RuntimeReadyEvent>(_ =>
            DrawDistanceScale = RecompOne.Runtime.Runtime.View.GetFloat("DrawDistanceScale", 1f));
        Event.AddListener<OverlayLoadedEvent>(_ => { _baseDistance = 0; _baseReach = 0; });
    }
    
    public static void Capture(CpuContext c, IMemory m)
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
    }
    
    static void FrustumCorners(IMemory m)
    {
        float wide = Display.WideAspect;
        float source = Display.SourceAspect > 0f ? Display.SourceAspect : 4f / 3f;
        float scale = wide > 0f ? wide / source : 1f;
        
        short half = (short)Math.Clamp((int)MathF.Round(PlaneHalfWidth * scale * CaptureOvershoot), PlaneHalfWidth, 1024);
        
        Write(m, 0, (short)-half, PlaneHalfHeight);
        Write(m, 1, (short)-half, -PlaneHalfHeight);
        Write(m, 2, half, -PlaneHalfHeight);
        Write(m, 3, half, PlaneHalfHeight);
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

        if (wanted != reach)
        {
            int bits = m.ReadU16(viewport + VpOtSizeBits);
            m.WriteU16(viewport + VpViewDistance, (ushort)wanted);
            m.WriteU16(viewport + VpOtzShift, (ushort)(Log2(wanted) - bits));
            reach = wanted;
        }
        return reach;
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
