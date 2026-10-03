using RecompOne.Runtime.Context;
using RecompOne.Runtime.Dispatch;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;

namespace Recompiled;

//copied from the recompiled code to apply the correct culling
public static class CullPatch
{
    const uint RenderPolyPtr = 0x800EEDD0u;
    const uint RenderPtrBase = 0x80310000u;

    const int StageWidth = 512;

    static int Widened => (int)MathF.Ceiling(StageWidth * Ratio);

    static float Ratio
    {
        get
        {
            float wide = Display.WideAspect;
            if (wide <= 0f) return 1f;

            float source = Display.SourceAspect > 0f ? Display.SourceAspect : 4f / 3f;
            return wide / source;
        }
    }

    static float _cachedWide = float.NaN;
    static float _cachedSource = float.NaN;
    static int _bias;
    static uint _maskX;

    static void Refresh()
    {
        float wide = Display.WideAspect;
        float source = Display.SourceAspect;
        if (wide == _cachedWide && source == _cachedSource) return;

        _cachedWide = wide;
        _cachedSource = source;

        int widened = Widened;
        _bias = (widened - StageWidth) / 2;

        uint bound = StageWidth;
        while (bound < (uint)widened) bound <<= 1;
        _maskX = ~(bound - 1u) & 0xFFFFu;
    }

    public static uint CullMaskX
    {
        get
        {
            Refresh();
            return _maskX;
        }
    }

    const uint VpDispW = 0x1F800098u;

    static int CullEdgeFor(int dispW)
    {
        Refresh();
        return (int)MathF.Round(dispW * (Ratio - 1f) * 0.5f);
    }

    static int CullEdgeX(PSMemory mem)
    {
        return CullEdgeFor((int)mem.ReadU16(VpDispW));
    }

    public static uint CullBias(uint packed)
    {
        Refresh();
        int x = (short)packed + _bias;
        return (packed & 0xFFFF0000u) | ((uint)x & 0xFFFFu);
    }

    public static void func_80021CEC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        mem.WriteU32(RenderPolyPtr, RenderPtrBase);
        c.SP = c.SP - 0xB0u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
       
        mem.WriteU32((c.SP + 0x14u), c.S1);
       
        mem.WriteU32((c.SP + 0x18u), c.S2);
       
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
       
        mem.WriteU32((c.SP + 0x20u), c.S4);
       
        mem.WriteU32((c.SP + 0x24u), c.S5);
       
        mem.WriteU32((c.SP + 0x28u), c.S6);
       
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
       
        mem.WriteU32((c.SP + 0x30u), c.FP);
       
        c.V0 = 0x800E0000u;
        c.T0 = 0x1F800000u;
        c.T1 = 0x800E0000u;
        c.T4 = 0x800F0000u;
        c.A2 = 0x800E0000u;
        c.S0 = 0x800E0000u;
        c.S3 = 0x1F800000u;
        c.S5 = 0x1F800000u;
        c.V0 = c.V0 | 0xEDC0u;
        c.T0 = c.T0 | 0x0004u;
        c.T1 = c.T1 | 0xDD94u;
        c.T4 = c.T4 | 0x17B8u;
        c.A2 = c.A2 | 0xDB7Cu;
        c.S0 = c.S0 | 0xDB78u;
        c.S3 = c.S3 | 0x008Cu;
        c.S5 = c.S5 | 0x009Cu;
        c.T0 = mem.ReadU32(c.T0);
        c.T1 = mem.ReadU32(c.T1);
        c.T4 = mem.ReadU32(c.T4);
        c.A2 = mem.ReadU32(c.A2);
        c.S0 = mem.ReadU32(c.S0);
        c.S3 = (uint)(short)mem.ReadU16(c.S3);
        c.S5 = mem.ReadU32(c.S5);
        c.T0 = c.T0 << 2;
        c.T0 = c.V0 + c.T0;
        c.A0 = mem.ReadU32((c.T0 + 0x4u));
        c.V1 = mem.ReadU32((c.T0 + 0x18u));
        c.S4 = mem.ReadU32((c.V0 + 0x2Cu));
        c.V1 = c.V1 - 0xD0u;
        c.T2 = mem.ReadU32((c.V0 + 0xCu));
        c.T3 = mem.ReadU32((c.V0 + 0x30u));
        c.S2 = 0u;
        mem.WriteU32((c.SP + 0x34u), c.T1);
       
        mem.WriteU32((c.SP + 0x40u), c.T4);
       
        mem.WriteU32((c.SP + 0x38u), c.T2);
       
        mem.WriteU32((c.SP + 0x3Cu), c.T3);
       
        c.S6 = 0x09000000u;
        c.S7 = 0x0C000000u;
        c.FP = 0x00FF0000u;
        c.FP = c.FP | 0xFFFFu;
        c.T6 = mem.ReadU32((c.V0 + 0x14u));
        c.T7 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x58u), c.T6);
       
        mem.WriteU32((c.SP + 0x5Cu), c.T7);
       
        L80021DC4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T0 = mem.ReadU32((c.SP + 0x3Cu));
        c.T1 = mem.ReadU32((c.SP + 0x38u));
        if ((int)c.T0 <= 0) {
            goto L80022A88;
        }
        c.S1 = mem.ReadU16(c.T1);
        c.T0 = c.T0 - 0x1u;
        c.A3 = mem.ReadU32((c.T1 + 0x4u));
        c.T1 = c.T1 + 0x8u;
        mem.WriteU32((c.SP + 0x3Cu), c.T0);
       
        mem.WriteU32((c.SP + 0x38u), c.T1);
       
        L80021DEC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S1 <= 0) {
            c.T0 = mem.ReadU16(c.A3);
            goto L80021DC4;
        }
        c.T0 = mem.ReadU16(c.A3);
        c.S1 = c.S1 - 0x1u;
        c.T1 = c.T0 << 4;
        c.T2 = c.T0 << 2;
        c.A1 = c.S0 + c.T1;
        c.A1 = c.A1 + c.T2;
        c.T9 = mem.ReadU16((c.A1 + 0xAu));
        c.A3 = c.A3 + 0x2u;
        c.At = c.T9 & 0x0004u;
        if (c.At != 0u) {
            c.T0 = c.T9 & 0x0001u;
            goto L80021DEC;
        }
        c.T0 = c.T9 & 0x0001u;
        if (c.T0 != 0u) {
            goto L800223F0;
        }
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022A80;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            c.T0 = RecompOne.Runtime.Gte.Read(24);
           
            goto L80022A80;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        c.T5 = 0xFF000000u;
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        c.T5 = c.T5 | CullMaskX;
        if ((int)c.T0 <= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80021DEC;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        c.T1 = RecompOne.Runtime.Gte.Read(12);
       
        c.T2 = RecompOne.Runtime.Gte.Read(13);
       
        c.T3 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T2) & c.T7;
        c.T7 = CullBias(c.T3) & c.T7;
        if (c.T7 != 0u) {
            goto L80021DEC;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T2);
       
        mem.WriteU32((c.A0 + 0x20u), c.T3);
        TerrainFloatProjection.Store(12, c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(13, c.A0 + 0x14u, c.T2);
        TerrainFloatProjection.Store(14, c.A0 + 0x20u, c.T3);
       
        c.T0 = RecompOne.Runtime.Gte.Read(17);
       
        c.T1 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021EFC;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021EFC: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80021F14;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80021F14: ;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80021DEC;
        }
        c.At = c.T9 & 0x0018u;
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L80021F80;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L80021F78;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(17);
       
        c.T1 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021F58;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021F58: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80021F70;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80021F70: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L80021F90;
        L80021F78: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L80021F90;
        L80021F80: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L80021F90: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80021FA4;
        }
        c.T0 = c.T1 + 0u;
        L80021FA4: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x5Cu));
            goto L80021DEC;
        }
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x34000000u;
            goto L80022014;
        }
        c.T1 = 0x34000000u;
        c.T1 = 0x36000000u;
        L80022014: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L800220F8;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L800220F8: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 2000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L80022110;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L80022160;
        }
        L80022110: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        goto L80021DEC;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        goto L80021DEC;
        L80022160: ;
        c.T0 = c.A0 + 0x28u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        mem.WriteU32((c.SP + 0x98u), c.T0);
       
        mem.WriteU32((c.SP + 0x9Cu), c.T1);
       
        mem.WriteU32((c.SP + 0xA0u), c.T2);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        c.T7 = 0x55550000u;
        c.T7 = c.T7 | 0x5556u;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T9 = c.T9 << 16;
        c.T6 = c.T6 & 0xFFFFu;
        c.T6 = c.T6 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T6);
       
        mem.WriteU32((c.T1 + 0x20u), c.T6);
       
        mem.WriteU32((c.T2 + 0x20u), c.T6);
       
        c.T3 = mem.ReadU8((c.A0 + 0x6u));
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = mem.ReadU8((c.A0 + 0x5u));
        c.T4 = mem.ReadU8((c.A0 + 0x11u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Du));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T3 = mem.ReadU8((c.A0 + 0x4u));
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T3 = c.T3 + c.T4;
        c.T3 = c.T5 + c.T3;
        { var _r = (long)(int)c.T3 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T3 = (uint)((int)c.T3 >> 31);
        c.T8 = c.HI;
        c.T3 = c.T8 - c.T3;
        c.T3 = c.T3 & 0x00FFu;
        c.T6 = c.T6 & 0x00FFu;
        c.T9 = c.T9 & 0x00FFu;
        c.T6 = c.T6 << 16;
        c.T9 = c.T9 << 8;
        c.T6 = c.T6 | c.T9;
        c.T6 = c.T6 | c.T3;
        mem.WriteU32((c.T0 + 0x1Cu), c.T6);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T6);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T6);
       
        c.T3 = mem.ReadU8((c.A0 + 0xCu));
        c.T4 = mem.ReadU8((c.A0 + 0x18u));
        c.T5 = mem.ReadU8((c.A0 + 0x24u));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = mem.ReadU8((c.A0 + 0xDu));
        c.T4 = mem.ReadU8((c.A0 + 0x19u));
        c.T5 = mem.ReadU8((c.A0 + 0x25u));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T6 = c.T6 & 0x00FFu;
        c.T9 = c.T9 & 0x00FFu;
        c.T9 = c.T9 << 8;
        c.T6 = c.T6 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T6);
        c.T3 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU32((c.A0 + 0x4u));
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0xFF00u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T3);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x14u), c.T3);
       
        mem.WriteU32((c.T2 + 0x10u), c.T5);
       
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T3);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T3);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        c.T3 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0xFF00u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU32((c.T1 + 0x14u), c.T3);
       
        mem.WriteU32((c.T1 + 0x10u), c.T4);
       
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T5);
        mem.WriteU32((c.T2 + 0x8u), c.T3);
       
        mem.WriteU32((c.T2 + 0x4u), c.T4);
       
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T5);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T2 + 0x28u;
        goto L80021DEC;
        L800223F0: ;
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A1 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        c.T3 = c.T3 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x50u), c.T3);
       
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = (c.SP + 0x54u); mem.WriteU32(_ad, _sw); }
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        TerrainFloatProjection.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022A80;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80022A80;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L800224B8;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80021DEC;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        L800224B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800224D4;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800224D4: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800224EC;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800224EC: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.T3 = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022504;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022504: ;
        c.T2 = c.T0 + 0u;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.T0 = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.T0 == 0u) {
            goto L80021DEC;
        }
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L8002258C;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L80022580;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022548;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022548: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L80022560;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L80022560: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022578;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022578: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002259C;
        L80022580: ;
        c.T0 = c.T2 + 0u;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002259C;
        L8002258C: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002259C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800225B0;
        }
        c.T0 = c.T1 + 0u;
        L800225B0: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            goto L80021DEC;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
       
        c.T4 = RecompOne.Runtime.Gte.Read(13);
       
        c.T6 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            goto L80021DEC;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T3);
       
        mem.WriteU32((c.A0 + 0x20u), c.T4);
       
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        TerrainFloatProjection.StoreFirst(c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(12, c.A0 + 0x14u, c.T3);
        TerrainFloatProjection.Store(13, c.A0 + 0x20u, c.T4);
        TerrainFloatProjection.Store(14, c.A0 + 0x2Cu, c.T6);
       
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T4 = mem.ReadU16((c.A1 + 0x12u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x30u), (ushort)c.T4);
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x3C000000u;
            goto L80022660;
        }
        c.T1 = 0x3C000000u;
        c.T1 = 0x3E000000u;
        L80022660: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(16);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x50u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L8002278C;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L8002278C: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 1000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L800227A4;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L800227CC;
        }
        L800227A4: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x34u;
        goto L80021DEC;
        L800227CC: ;
        c.T0 = c.A0 + 0x34u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        c.T3 = c.T2 + 0x28u;
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Cu));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Eu));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 16;
        c.T8 = c.T8 & 0xFFFFu;
        c.T8 = c.T8 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T8);
       
        mem.WriteU32((c.T1 + 0x20u), c.T8);
       
        mem.WriteU32((c.T2 + 0x20u), c.T8);
       
        mem.WriteU32((c.T3 + 0x20u), c.T8);
       
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = mem.ReadU8((c.A0 + 0x11u));
        c.T7 = mem.ReadU8((c.A0 + 0x1Du));
        c.T8 = c.T4 + c.T5;
        c.T8 = (uint)((int)c.T8 >> 1);
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T9 = c.T6 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 1);
        c.T4 = c.T4 + c.T5;
        c.T4 = (uint)((int)c.T4 >> 1);
        c.T8 = c.T8 << 16;
        c.T9 = c.T9 << 8;
        c.T4 = c.T4 | c.T8;
        c.T4 = c.T4 | c.T9;
        mem.WriteU32((c.T0 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
       
        c.T4 = mem.ReadU8((c.A0 + 0xCu));
        c.T5 = mem.ReadU8((c.A0 + 0x18u));
        c.T6 = mem.ReadU8((c.A0 + 0x24u));
        c.T7 = mem.ReadU8((c.A0 + 0x30u));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = mem.ReadU8((c.A0 + 0xDu));
        c.T5 = mem.ReadU8((c.A0 + 0x19u));
        c.T6 = mem.ReadU8((c.A0 + 0x25u));
        c.T7 = mem.ReadU8((c.A0 + 0x31u));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 8;
        c.T8 = c.T8 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T3 + 0x24u), (ushort)c.T8);
        c.T9 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU16((c.A0 + 0x4u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T5 = c.T7 | c.T5;
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T9);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T3 + 0x14u), c.T9);
       
        mem.WriteU32((c.T3 + 0x10u), c.T5);
       
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T9);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T3 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T3 + 0xEu), (ushort)c.T6);
        c.T9 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = mem.ReadU32((c.A0 + 0x2Cu));
        c.T6 = mem.ReadU16((c.A0 + 0x28u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x2Au));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T1 + 0x14u), c.T5);
       
        mem.WriteU32((c.T1 + 0x10u), c.T6);
       
        mem.WriteU32((c.T2 + 0x8u), c.T5);
       
        mem.WriteU32((c.T2 + 0x14u), c.T9);
       
        mem.WriteU32((c.T2 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x10u), c.T4);
       
        mem.WriteU32((c.T3 + 0x4u), c.T4);
       
        mem.WriteU32((c.T3 + 0x8u), c.T9);
       
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x30u));
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0xCu), (ushort)c.T9);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T3 + 0x28u;
        goto L80021DEC;
        L80022A80: ;
        L80022A88: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
       
        L80022A90: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S2 <= 0) {
            goto L80022AB8;
        }
        c.T1 = c.T1 - 0x4u;
        c.T0 = mem.ReadU32(c.T1);
        c.S2 = c.S2 - 0x1u;
        c.T9 = (uint)(short)mem.ReadU16((c.T0 + 0xAu));
        c.T9 = c.T9 & 0xFFFBu;
        mem.WriteU16((c.T0 + 0xAu), (ushort)c.T9);
        goto L80022A90;
        L80022AB8: ;
        mem.WriteU32((c.V0 + 0x10u), c.T1);
       
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0xB0u;
        return;
    }

    public static void func_80021DEC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        L80021DEC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S1 <= 0) {
            c.T0 = mem.ReadU16(c.A3);
            MediEvil_game.func_80021DC4(c, m);
            return;
        }
        c.T0 = mem.ReadU16(c.A3);
        c.S1 = c.S1 - 0x1u;
        c.T1 = c.T0 << 4;
        c.T2 = c.T0 << 2;
        c.A1 = c.S0 + c.T1;
        c.A1 = c.A1 + c.T2;
        c.T9 = mem.ReadU16((c.A1 + 0xAu));
        c.A3 = c.A3 + 0x2u;
        c.At = c.T9 & 0x0004u;
        if (c.At != 0u) {
            c.T0 = c.T9 & 0x0001u;
            goto L80021DEC;
        }
        c.T0 = c.T9 & 0x0001u;
        if (c.T0 != 0u) {
            MediEvil_game.func_800223F0(c, m);
            return;
        }
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            MediEvil_game.func_80022A80(c, m);
            return;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            c.T0 = RecompOne.Runtime.Gte.Read(24);
           
            MediEvil_game.func_80022A80(c, m);
            return;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        c.T5 = 0xFF000000u;
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        c.T5 = c.T5 | CullMaskX;
        if ((int)c.T0 <= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80021DEC;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        c.T1 = RecompOne.Runtime.Gte.Read(12);
       
        c.T2 = RecompOne.Runtime.Gte.Read(13);
       
        c.T3 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T2) & c.T7;
        c.T7 = CullBias(c.T3) & c.T7;
        if (c.T7 != 0u) {
            goto L80021DEC;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T2);
       
        mem.WriteU32((c.A0 + 0x20u), c.T3);
        TerrainFloatProjection.Store(12, c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(13, c.A0 + 0x14u, c.T2);
        TerrainFloatProjection.Store(14, c.A0 + 0x20u, c.T3);
       
        c.T0 = RecompOne.Runtime.Gte.Read(17);
       
        c.T1 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021EFC;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021EFC: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80021F14;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80021F14: ;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80021DEC;
        }
        c.At = c.T9 & 0x0018u;
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L80021F80;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L80021F78;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(17);
       
        c.T1 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021F58;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021F58: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80021F70;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80021F70: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L80021F90;
        L80021F78: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L80021F90;
        L80021F80: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L80021F90: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80021FA4;
        }
        c.T0 = c.T1 + 0u;
        L80021FA4: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x5Cu));
            goto L80021DEC;
        }
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        MediEvil_game.func_80022000(c, m);
    }

    public static void func_80022000(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x34000000u;
            goto L80022014;
        }
        c.T1 = 0x34000000u;
        c.T1 = 0x36000000u;
        L80022014: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L800220F8;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L800220F8: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 2000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L80022110;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L80022160;
        }
        L80022110: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        MediEvil_game.func_80021DEC(c, m);
        return;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        MediEvil_game.func_80021DEC(c, m);
        return;
        L80022160: ;
        c.T0 = c.A0 + 0x28u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        mem.WriteU32((c.SP + 0x98u), c.T0);
       
        mem.WriteU32((c.SP + 0x9Cu), c.T1);
       
        mem.WriteU32((c.SP + 0xA0u), c.T2);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        c.T7 = 0x55550000u;
        c.T7 = c.T7 | 0x5556u;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T9 = c.T9 << 16;
        c.T6 = c.T6 & 0xFFFFu;
        c.T6 = c.T6 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T6);
       
        mem.WriteU32((c.T1 + 0x20u), c.T6);
       
        mem.WriteU32((c.T2 + 0x20u), c.T6);
       
        c.T3 = mem.ReadU8((c.A0 + 0x6u));
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = mem.ReadU8((c.A0 + 0x5u));
        c.T4 = mem.ReadU8((c.A0 + 0x11u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Du));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T3 = mem.ReadU8((c.A0 + 0x4u));
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T3 = c.T3 + c.T4;
        c.T3 = c.T5 + c.T3;
        { var _r = (long)(int)c.T3 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T3 = (uint)((int)c.T3 >> 31);
        c.T8 = c.HI;
        c.T3 = c.T8 - c.T3;
        c.T3 = c.T3 & 0x00FFu;
        c.T6 = c.T6 & 0x00FFu;
        c.T9 = c.T9 & 0x00FFu;
        c.T6 = c.T6 << 16;
        c.T9 = c.T9 << 8;
        c.T6 = c.T6 | c.T9;
        c.T6 = c.T6 | c.T3;
        mem.WriteU32((c.T0 + 0x1Cu), c.T6);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T6);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T6);
       
        c.T3 = mem.ReadU8((c.A0 + 0xCu));
        c.T4 = mem.ReadU8((c.A0 + 0x18u));
        c.T5 = mem.ReadU8((c.A0 + 0x24u));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = mem.ReadU8((c.A0 + 0xDu));
        c.T4 = mem.ReadU8((c.A0 + 0x19u));
        c.T5 = mem.ReadU8((c.A0 + 0x25u));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T6 = c.T6 & 0x00FFu;
        c.T9 = c.T9 & 0x00FFu;
        c.T9 = c.T9 << 8;
        c.T6 = c.T6 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T6);
        c.T3 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU32((c.A0 + 0x4u));
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0xFF00u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T3);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x14u), c.T3);
       
        mem.WriteU32((c.T2 + 0x10u), c.T5);
       
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T3);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T3);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        c.T3 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0xFF00u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU32((c.T1 + 0x14u), c.T3);
       
        mem.WriteU32((c.T1 + 0x10u), c.T4);
       
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T5);
        mem.WriteU32((c.T2 + 0x8u), c.T3);
       
        mem.WriteU32((c.T2 + 0x4u), c.T4);
       
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T5);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T2 + 0x28u;
        MediEvil_game.func_80021DEC(c, m);
        return;
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A1 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        c.T3 = c.T3 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x50u), c.T3);
       
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = (c.SP + 0x54u); mem.WriteU32(_ad, _sw); }
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        TerrainFloatProjection.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022A80;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80022A80;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L800224B8;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            MediEvil_game.func_80021DEC(c, m);
            return;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        L800224B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800224D4;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800224D4: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800224EC;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800224EC: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.T3 = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022504;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022504: ;
        c.T2 = c.T0 + 0u;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.T0 = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.T0 == 0u) {
            MediEvil_game.func_80021DEC(c, m);
            return;
        }
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L8002258C;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L80022580;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022548;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022548: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L80022560;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L80022560: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022578;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022578: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002259C;
        L80022580: ;
        c.T0 = c.T2 + 0u;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002259C;
        L8002258C: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002259C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800225B0;
        }
        c.T0 = c.T1 + 0u;
        L800225B0: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            MediEvil_game.func_80021DEC(c, m);
            return;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
       
        c.T4 = RecompOne.Runtime.Gte.Read(13);
       
        c.T6 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            MediEvil_game.func_80021DEC(c, m);
            return;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T3);
       
        mem.WriteU32((c.A0 + 0x20u), c.T4);
       
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        TerrainFloatProjection.StoreFirst(c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(12, c.A0 + 0x14u, c.T3);
        TerrainFloatProjection.Store(13, c.A0 + 0x20u, c.T4);
        TerrainFloatProjection.Store(14, c.A0 + 0x2Cu, c.T6);
       
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T4 = mem.ReadU16((c.A1 + 0x12u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x30u), (ushort)c.T4);
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x3C000000u;
            goto L80022660;
        }
        c.T1 = 0x3C000000u;
        c.T1 = 0x3E000000u;
        L80022660: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(16);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x50u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L8002278C;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L8002278C: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 1000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L800227A4;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L800227CC;
        }
        L800227A4: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x34u;
        MediEvil_game.func_80021DEC(c, m);
        return;
        L800227CC: ;
        c.T0 = c.A0 + 0x34u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        c.T3 = c.T2 + 0x28u;
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Cu));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Eu));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 16;
        c.T8 = c.T8 & 0xFFFFu;
        c.T8 = c.T8 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T8);
       
        mem.WriteU32((c.T1 + 0x20u), c.T8);
       
        mem.WriteU32((c.T2 + 0x20u), c.T8);
       
        mem.WriteU32((c.T3 + 0x20u), c.T8);
       
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = mem.ReadU8((c.A0 + 0x11u));
        c.T7 = mem.ReadU8((c.A0 + 0x1Du));
        c.T8 = c.T4 + c.T5;
        c.T8 = (uint)((int)c.T8 >> 1);
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T9 = c.T6 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 1);
        c.T4 = c.T4 + c.T5;
        c.T4 = (uint)((int)c.T4 >> 1);
        c.T8 = c.T8 << 16;
        c.T9 = c.T9 << 8;
        c.T4 = c.T4 | c.T8;
        c.T4 = c.T4 | c.T9;
        mem.WriteU32((c.T0 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
       
        c.T4 = mem.ReadU8((c.A0 + 0xCu));
        c.T5 = mem.ReadU8((c.A0 + 0x18u));
        c.T6 = mem.ReadU8((c.A0 + 0x24u));
        c.T7 = mem.ReadU8((c.A0 + 0x30u));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = mem.ReadU8((c.A0 + 0xDu));
        c.T5 = mem.ReadU8((c.A0 + 0x19u));
        c.T6 = mem.ReadU8((c.A0 + 0x25u));
        c.T7 = mem.ReadU8((c.A0 + 0x31u));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 8;
        c.T8 = c.T8 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T3 + 0x24u), (ushort)c.T8);
        c.T9 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU16((c.A0 + 0x4u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T5 = c.T7 | c.T5;
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T9);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T3 + 0x14u), c.T9);
       
        mem.WriteU32((c.T3 + 0x10u), c.T5);
       
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T9);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T3 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T3 + 0xEu), (ushort)c.T6);
        c.T9 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = mem.ReadU32((c.A0 + 0x2Cu));
        c.T6 = mem.ReadU16((c.A0 + 0x28u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x2Au));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T1 + 0x14u), c.T5);
       
        mem.WriteU32((c.T1 + 0x10u), c.T6);
       
        mem.WriteU32((c.T2 + 0x8u), c.T5);
       
        mem.WriteU32((c.T2 + 0x14u), c.T9);
       
        mem.WriteU32((c.T2 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x10u), c.T4);
       
        mem.WriteU32((c.T3 + 0x4u), c.T4);
       
        mem.WriteU32((c.T3 + 0x8u), c.T9);
       
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x30u));
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0xCu), (ushort)c.T9);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T3 + 0x28u;
        MediEvil_game.func_80021DEC(c, m);
        return;
        L80022A80: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
       
        L80022A90: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S2 <= 0) {
            goto L80022AB8;
        }
        c.T1 = c.T1 - 0x4u;
        c.T0 = mem.ReadU32(c.T1);
        c.S2 = c.S2 - 0x1u;
        c.T9 = (uint)(short)mem.ReadU16((c.T0 + 0xAu));
        c.T9 = c.T9 & 0xFFFBu;
        mem.WriteU16((c.T0 + 0xAu), (ushort)c.T9);
        goto L80022A90;
        L80022AB8: ;
        mem.WriteU32((c.V0 + 0x10u), c.T1);
       
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0xB0u;
        return;
    }

    public static void func_800223F0(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A1 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        c.T3 = c.T3 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x50u), c.T3);
       
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = (c.SP + 0x54u); mem.WriteU32(_ad, _sw); }
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        TerrainFloatProjection.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022A80;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80022A80;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L800224B8;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            MediEvil_game.func_80021DEC(c, m);
            return;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        L800224B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800224D4;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800224D4: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800224EC;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800224EC: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.T3 = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022504;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022504: ;
        c.T2 = c.T0 + 0u;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.T0 = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.T0 == 0u) {
            MediEvil_game.func_80021DEC(c, m);
            return;
        }
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L8002258C;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L80022580;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022548;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022548: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L80022560;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L80022560: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022578;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022578: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002259C;
        L80022580: ;
        c.T0 = c.T2 + 0u;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002259C;
        L8002258C: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002259C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800225B0;
        }
        c.T0 = c.T1 + 0u;
        L800225B0: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            MediEvil_game.func_80021DEC(c, m);
            return;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
       
        c.T4 = RecompOne.Runtime.Gte.Read(13);
       
        c.T6 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            MediEvil_game.func_80021DEC(c, m);
            return;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T3);
       
        mem.WriteU32((c.A0 + 0x20u), c.T4);
       
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        TerrainFloatProjection.StoreFirst(c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(12, c.A0 + 0x14u, c.T3);
        TerrainFloatProjection.Store(13, c.A0 + 0x20u, c.T4);
        TerrainFloatProjection.Store(14, c.A0 + 0x2Cu, c.T6);
       
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T4 = mem.ReadU16((c.A1 + 0x12u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x30u), (ushort)c.T4);
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x3C000000u;
            goto L80022660;
        }
        c.T1 = 0x3C000000u;
        c.T1 = 0x3E000000u;
        L80022660: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(16);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x50u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L8002278C;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L8002278C: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 1000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L800227A4;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L800227CC;
        }
        L800227A4: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x34u;
        MediEvil_game.func_80021DEC(c, m);
        return;
        L800227CC: ;
        c.T0 = c.A0 + 0x34u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        c.T3 = c.T2 + 0x28u;
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Cu));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Eu));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 16;
        c.T8 = c.T8 & 0xFFFFu;
        c.T8 = c.T8 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T8);
       
        mem.WriteU32((c.T1 + 0x20u), c.T8);
       
        mem.WriteU32((c.T2 + 0x20u), c.T8);
       
        mem.WriteU32((c.T3 + 0x20u), c.T8);
       
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = mem.ReadU8((c.A0 + 0x11u));
        c.T7 = mem.ReadU8((c.A0 + 0x1Du));
        c.T8 = c.T4 + c.T5;
        c.T8 = (uint)((int)c.T8 >> 1);
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T9 = c.T6 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 1);
        c.T4 = c.T4 + c.T5;
        c.T4 = (uint)((int)c.T4 >> 1);
        c.T8 = c.T8 << 16;
        c.T9 = c.T9 << 8;
        c.T4 = c.T4 | c.T8;
        c.T4 = c.T4 | c.T9;
        mem.WriteU32((c.T0 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
       
        c.T4 = mem.ReadU8((c.A0 + 0xCu));
        c.T5 = mem.ReadU8((c.A0 + 0x18u));
        c.T6 = mem.ReadU8((c.A0 + 0x24u));
        c.T7 = mem.ReadU8((c.A0 + 0x30u));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = mem.ReadU8((c.A0 + 0xDu));
        c.T5 = mem.ReadU8((c.A0 + 0x19u));
        c.T6 = mem.ReadU8((c.A0 + 0x25u));
        c.T7 = mem.ReadU8((c.A0 + 0x31u));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 8;
        c.T8 = c.T8 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T3 + 0x24u), (ushort)c.T8);
        c.T9 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU16((c.A0 + 0x4u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T5 = c.T7 | c.T5;
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T9);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T3 + 0x14u), c.T9);
       
        mem.WriteU32((c.T3 + 0x10u), c.T5);
       
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T9);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T3 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T3 + 0xEu), (ushort)c.T6);
        c.T9 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = mem.ReadU32((c.A0 + 0x2Cu));
        c.T6 = mem.ReadU16((c.A0 + 0x28u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x2Au));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T1 + 0x14u), c.T5);
       
        mem.WriteU32((c.T1 + 0x10u), c.T6);
       
        mem.WriteU32((c.T2 + 0x8u), c.T5);
       
        mem.WriteU32((c.T2 + 0x14u), c.T9);
       
        mem.WriteU32((c.T2 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x10u), c.T4);
       
        mem.WriteU32((c.T3 + 0x4u), c.T4);
       
        mem.WriteU32((c.T3 + 0x8u), c.T9);
       
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x30u));
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0xCu), (ushort)c.T9);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T3 + 0x28u;
        MediEvil_game.func_80021DEC(c, m);
        return;
        L80022A80: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
       
        L80022A90: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S2 <= 0) {
            goto L80022AB8;
        }
        c.T1 = c.T1 - 0x4u;
        c.T0 = mem.ReadU32(c.T1);
        c.S2 = c.S2 - 0x1u;
        c.T9 = (uint)(short)mem.ReadU16((c.T0 + 0xAu));
        c.T9 = c.T9 & 0xFFFBu;
        mem.WriteU16((c.T0 + 0xAu), (ushort)c.T9);
        goto L80022A90;
        L80022AB8: ;
        mem.WriteU32((c.V0 + 0x10u), c.T1);
       
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0xB0u;
        return;
    }

    public static void func_80024A90(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x88u;
        mem.WriteU32((c.SP + 0x6Cu), c.S1);
       
        c.S1 = mem.ReadU32((c.GP + 0x460u));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        mem.WriteU32((c.SP + 0x68u), c.S0);
       
        mem.WriteU32((c.SP + 0x84u), c.RA);
       
        mem.WriteU32((c.SP + 0x80u), c.S6);
       
        mem.WriteU32((c.SP + 0x7Cu), c.S5);
       
        mem.WriteU32((c.SP + 0x78u), c.S4);
       
        mem.WriteU32((c.SP + 0x74u), c.S3);
       
        mem.WriteU32((c.SP + 0x70u), c.S2);
       
        c.A1 = mem.ReadU32((c.S1 + 0xCu));
        c.S0 = c.A0 + 0u;
        c.RA = 0x80024ACCu;
        MediEvil_game.func_800A4E3C(c, m);
        c.S3 = 0u + 0u;
        c.S2 = c.S3 + 0u;
        c.A0 = c.S0 + 0u;
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V1 = mem.ReadU16((c.S0 + 0x14u));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.V0 = mem.ReadU16((c.V0 + 0x14u));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V1 = mem.ReadU16((c.A0 + 0x18u));
        c.V0 = mem.ReadU16((c.V0 + 0x18u));
        c.A1 = mem.ReadU16((c.S1 + 0x2Cu));
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 - c.A1;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V1 = mem.ReadU16((c.A0 + 0x1Cu));
        c.V0 = mem.ReadU16((c.V0 + 0x1Cu));
        c.A1 = c.SP + 0x10u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.RA = 0x80024B30u;
        MediEvil_game.func_800A4880(c, m);
        c.S4 = 0x1F800000u;
        c.S4 = mem.ReadU32((c.S4 + 0x34u));
        c.T4 = mem.ReadU32(c.S4);
        c.T5 = mem.ReadU32((c.S4 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S4 + 0x8u));
        c.T5 = mem.ReadU32((c.S4 + 0xCu));
        c.T6 = mem.ReadU32((c.S4 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S4 + 0x14u));
        c.T5 = mem.ReadU32((c.S4 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S4 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T3 = mem.ReadU32((c.S1 + 0x1Cu));
        c.V1 = mem.ReadU16((c.S1 + 0xAu));
        c.T2 = mem.ReadU32((c.S1 + 0x18u));
        if (c.V1 == 0u) {
            c.T8 = c.S3 + 0u;
            goto L80024C94;
        }
        c.T8 = c.S3 + 0u;
        c.T8 = 0x00000001u;
        c.V0 = mem.ReadU32((c.S1 + 0x4u));
        c.V1 = c.V1 - 0x1u;
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            mem.WriteU16((c.S1 + 0xAu), (ushort)c.V1);
            goto L80024C14;
        }
        mem.WriteU16((c.S1 + 0xAu), (ushort)c.V1);
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.V1 = mem.ReadU8(c.V0);
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x48u), c.V0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.V1 = mem.ReadU8((c.V0 + 0x1u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.V1 = mem.ReadU8((c.V0 + 0x2u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x50u), c.V0);
       
        L80024C14: ;
        c.V0 = mem.ReadU32((c.S1 + 0x4u));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            goto L80024C94;
        }
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V1 = mem.ReadU8(c.V0);
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x58u), c.V0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V1 = mem.ReadU8((c.V0 + 0x1u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x5Cu), c.V0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V1 = mem.ReadU8((c.V0 + 0x2u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x60u), c.V0);
       
        L80024C94: ;
        c.T5 = mem.ReadU16((c.S1 + 0x28u));
        if ((int)c.T5 <= 0) {
            c.S0 = 0x00000003u;
            goto L80025220;
        }
        c.S0 = 0x00000003u;
        c.T4 = 0xFF000000u;
        c.T4 = c.T4 | CullMaskX;
        c.T7 = 0x00FF0000u;
        c.T7 = c.T7 | 0xFFFFu;
        c.T6 = 0xFF000000u;
        c.T9 = c.SP + 0x18u;
        c.T1 = c.T3 + 0xAu;
        L80024CC0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.A0 = (uint)(short)mem.ReadU16((c.T1 - 0x6u));
        c.V1 = (uint)(short)mem.ReadU16((c.T1 - 0x4u));
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.A0 = c.A0 << 3;
        c.A0 = c.T2 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.T2 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.A0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.A0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.V1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.V1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.V0 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.V1 = mem.ReadU32(c.T3);
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L80024D64;
        }
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0xCu;
        c.T0 = c.T3 + c.V0;
        c.S3 = c.T0 + 0u;
        c.V0 = c.T0 + 0x8u;
        mem.WriteU32((c.SP + 0x18u), c.V0);
       
        c.V0 = c.T0 + 0x14u;
        mem.WriteU32((c.SP + 0x1Cu), c.V0);
       
        c.V0 = c.T0 + 0x20u;
        mem.WriteU32((c.SP + 0x20u), c.V0);
       
        c.V0 = c.T0 + 0x2Cu;
        goto L80024DAC;
        L80024D64: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0xCu;
        c.T0 = c.T3 + c.V0;
        c.S2 = c.T0 + 0u;
        c.V0 = c.T0 + 0x10u;
        mem.WriteU32((c.SP + 0x18u), c.V0);
       
        c.V0 = c.T0 + 0x18u;
        mem.WriteU32((c.SP + 0x1Cu), c.V0);
       
        c.V0 = c.T0 + 0x20u;
        mem.WriteU32((c.SP + 0x20u), c.V0);
       
        c.V0 = c.T0 + 0x28u;
        L80024DAC: ;
        mem.WriteU32((c.SP + 0x24u), c.V0);
       
        c.S5 = mem.ReadU32((c.SP + 0x18u));
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.S5; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw, (mem.ReadU32(c.S1 + 0x4u) & 1u) != 0); }
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V0 + 0x4u)));
        TerrainFloatProjection.Rtps(12, false);
        c.V0 = 0x00000001u;
        if (c.T8 != c.V0) {
            c.V0 = 0x00000006u;
            goto L8002502C;
        }
        c.V0 = 0x00000006u;
        c.V1 = mem.ReadU32(c.T3);
        if (c.V1 != c.V0) {
            c.A0 = c.SP + 0x58u;
            goto L80024E20;
        }
        c.A0 = c.SP + 0x58u;
        c.A0 = c.SP + 0x48u;
        c.V0 = c.S3 + 0x4u;
        mem.WriteU32((c.SP + 0x28u), c.V0);
       
        c.V0 = c.S3 + 0x10u;
        mem.WriteU32((c.SP + 0x2Cu), c.V0);
       
        c.V0 = c.S3 + 0x1Cu;
        mem.WriteU32((c.SP + 0x30u), c.V0);
       
        c.V0 = c.S3 + 0x28u;
        goto L80024E3C;
        L80024E20: ;
        c.V0 = c.S2 + 0xCu;
        mem.WriteU32((c.SP + 0x28u), c.V0);
       
        c.V0 = c.S2 + 0x14u;
        mem.WriteU32((c.SP + 0x2Cu), c.V0);
       
        c.V0 = c.S2 + 0x1Cu;
        mem.WriteU32((c.SP + 0x30u), c.V0);
       
        c.V0 = c.S2 + 0x24u;
        L80024E3C: ;
        mem.WriteU32((c.SP + 0x34u), c.V0);
       
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V1 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x38u), c.V1);
       
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x3Cu), c.V0);
       
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
       
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.V0);
       
        c.V0 = mem.ReadU32(c.A0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8(c.V1, (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x38u));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x38u));
        c.V0 = mem.ReadU32((c.A0 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x2u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = mem.ReadU32(c.A0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x2Cu));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8(c.V1, (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x2Cu));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = mem.ReadU32((c.A0 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x2Cu));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x2u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = mem.ReadU32(c.A0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x30u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8(c.V1, (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x30u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = mem.ReadU32((c.A0 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x30u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x2u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x44u));
        c.V0 = mem.ReadU32(c.A0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8(c.V1, (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x44u));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x44u));
        c.V0 = mem.ReadU32((c.A0 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x2u), (byte)c.V0);
        L8002502C: ;
        c.T1 = c.T1 + 0x74u;
        c.T3 = c.T3 + 0x74u;
        c.V0 = c.SP + 0x38u;
        mem.WriteU32(c.V0, RecompOne.Runtime.Gte.Read(16));
        mem.WriteU32((c.V0 + 0x4u), RecompOne.Runtime.Gte.Read(17));
        mem.WriteU32((c.V0 + 0x8u), RecompOne.Runtime.Gte.Read(18));
        mem.WriteU32((c.V0 + 0xCu), RecompOne.Runtime.Gte.Read(19));
        c.V1 = mem.ReadU32((c.SP + 0x44u));
        c.A2 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = (int)c.V1 < (int)c.A2 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L80025064;
        }
        c.V1 = c.A2 + 0u;
        L80025064: ;
        c.A1 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = (int)c.V1 < (int)c.A1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L8002507C;
        }
        c.V1 = c.A1 + 0u;
        L8002507C: ;
        c.A0 = mem.ReadU32((c.SP + 0x38u));
        c.V0 = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L80025094;
        }
        c.V1 = c.A0 + 0u;
        L80025094: ;
        if ((int)c.V1 < 0) {
            goto L80025214;
        }
        c.V0 = mem.ReadU32((c.S1 + 0x4u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = 0x800F0000u;
            goto L800250BC;
        }
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x1214u));
        c.A0 = c.V0 - 0x1u;
        goto L800250F8;
        L800250BC: ;
        c.A0 = c.A0 + c.A1;
        c.A0 = c.A0 + c.A2;
        c.V0 = mem.ReadU32((c.SP + 0x44u));
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.A0 = c.A0 + c.V0;
        c.V1 = c.V1 + 0x2u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A0 = (uint)((int)c.A0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.A0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = (int)c.A0 < 2 ? 1u : 0u;
            goto L80025214;
        }
        c.V0 = (int)c.A0 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L80025214;
        }
        L800250F8: ;
        c.S5 = mem.ReadU32((c.SP + 0x1Cu));
        c.S6 = mem.ReadU32((c.SP + 0x20u));
        c.S4 = mem.ReadU32((c.SP + 0x24u));
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.S5; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw, (mem.ReadU32(c.S1 + 0x4u) & 1u) != 0); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.S6; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw, (mem.ReadU32(c.S1 + 0x4u) & 1u) != 0); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); var _ad = c.S4; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(14, _ad, _sw, (mem.ReadU32(c.S1 + 0x4u) & 1u) != 0); }
        c.V0 = mem.ReadU32((c.SP + 0x18u));
        c.V0 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V0) & c.T4;
        if (c.V0 == 0u) {
            goto L80025180;
        }
        c.V0 = mem.ReadU32((c.SP + 0x1Cu));
        c.V0 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V0) & c.T4;
        if (c.V0 == 0u) {
            goto L80025180;
        }
        c.V0 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V0) & c.T4;
        if (c.V0 == 0u) {
            goto L80025180;
        }
        c.V0 = mem.ReadU32((c.SP + 0x24u));
        c.V0 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V0) & c.T4;
        if (c.V0 != 0u) {
            c.A1 = 0u + 0u;
            goto L800251C0;
        }
        c.A1 = 0u + 0u;
        L80025180: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.A0 = c.A0 << 2;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.V1 = mem.ReadU32(c.T0);
        c.A0 = c.A0 + c.V0;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.V1 & c.T6;
        c.V0 = c.V0 & c.T7;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
       
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T7;
        c.V0 = c.V0 & c.T6;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
       
        goto L80025214;
        L800251C0: ;
        c.A3 = 0x00000003u;
        c.A2 = c.T9 + 0xCu;
        L800251C8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32(c.A2);
        c.V1 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V1) & CullMaskX;
        if (c.V0 == 0u) {
            c.V0 = c.V1 & c.T6;
            goto L800251F4;
        }
        c.V0 = c.V1 & c.T6;
        if (c.V0 != 0u) {
            goto L80025208;
        }
        c.A1 = c.A1 | 0x0001u;
        goto L80025200;
        L800251F4: ;
        if (c.V0 == 0u) {
            goto L80025208;
        }
        c.A1 = c.A1 | 0x0002u;
        L80025200: ;
        if (c.A1 == c.S0) {
            goto L80025180;
        }
        L80025208: ;
        c.A3 = c.A3 - 0x1u;
        if ((int)c.A3 >= 0) {
            c.A2 = c.A2 - 0x4u;
            goto L800251C8;
        }
        c.A2 = c.A2 - 0x4u;
        L80025214: ;
        c.T5 = c.T5 - 0x1u;
        if ((int)c.T5 > 0) {
            goto L80024CC0;
        }
        L80025220: ;
        c.V0 = 0x800F0000u;
        c.V1 = mem.ReadU32((c.GP + 0x5E0u));
        c.V0 = mem.ReadU16((c.V0 - 0x1220u));
        mem.WriteU16((c.V1 + 0xC0u), (ushort)c.V0);
        RecompOne.Runtime.Gte.WriteControl(26, c.V0);
        c.RA = mem.ReadU32((c.SP + 0x84u));
        c.S6 = mem.ReadU32((c.SP + 0x80u));
        c.S5 = mem.ReadU32((c.SP + 0x7Cu));
        c.S4 = mem.ReadU32((c.SP + 0x78u));
        c.S3 = mem.ReadU32((c.SP + 0x74u));
        c.S2 = mem.ReadU32((c.SP + 0x70u));
        c.S1 = mem.ReadU32((c.SP + 0x6Cu));
        c.S0 = mem.ReadU32((c.SP + 0x68u));
        c.SP = c.SP + 0x88u;
        return;
    }

    public static void func_80025260(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x78u;
        mem.WriteU32((c.SP + 0x5Cu), c.S1);
       
        c.S1 = mem.ReadU32((c.GP + 0x460u));
        mem.WriteU32((c.SP + 0x70u), c.S6);
       
        mem.WriteU32((c.SP + 0x74u), c.RA);
       
        mem.WriteU32((c.SP + 0x6Cu), c.S5);
       
        mem.WriteU32((c.SP + 0x68u), c.S4);
       
        mem.WriteU32((c.SP + 0x64u), c.S3);
       
        mem.WriteU32((c.SP + 0x60u), c.S2);
       
        mem.WriteU32((c.SP + 0x58u), c.S0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.S4 = mem.ReadU32((c.S1 + 0x20u));
        c.T3 = c.V0 + 0xCu;
        c.V0 = mem.ReadU16((c.S1 + 0xAu));
        c.T0 = mem.ReadU32((c.S1 + 0x24u));
        if (c.V0 == 0u) {
            c.S6 = c.A0 + 0u;
            goto L80025528;
        }
        c.S6 = c.A0 + 0u;
        c.T2 = 0u + 0u;
        c.S2 = mem.ReadU32((c.S1 + 0x28u));
        c.S3 = mem.ReadU32((c.S1 + 0x2Cu));
        c.A3 = mem.ReadU16((c.S1 + 0x8u));
        c.V1 = mem.ReadU32((c.S1 + 0x34u));
        c.V0 = c.V0 - 0x1u;
        if ((int)c.V1 <= 0) {
            mem.WriteU16((c.S1 + 0xAu), (ushort)c.V0);
            goto L80025528;
        }
        mem.WriteU16((c.S1 + 0xAu), (ushort)c.V0);
        c.T1 = c.S2 + 0x4u;
        L800252C8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.S2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8(c.V0);
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V1 << 3;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0x8u;
        c.A2 = c.S2 + c.V0;
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0xCu), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.S2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x1u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0xDu), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.S2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0xEu), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8(c.V0);
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x14u), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x1u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x15u), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x16u), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8(c.V0);
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x1Cu), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x1u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x1Du), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x1Eu), (byte)c.V0);
        c.V0 = mem.ReadU8(c.T3);
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0x8u;
        c.A1 = c.S3 + c.V0;
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A1 + 0xCu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.T3 + 0x1u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A1 + 0xDu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.T3 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A1 + 0xEu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A1 + 0xCu));
        c.V1 = mem.ReadU8((c.A1 + 0xDu));
        c.A0 = mem.ReadU8((c.A1 + 0xEu));
        mem.WriteU8((c.A1 + 0x14u), (byte)c.V0);
        mem.WriteU8((c.A1 + 0x15u), (byte)c.V1);
        mem.WriteU8((c.A1 + 0x16u), (byte)c.A0);
        c.V0 = mem.ReadU8((c.A2 + 0x14u));
        mem.WriteU8((c.A1 + 0x1Cu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x15u));
        mem.WriteU8((c.A1 + 0x1Du), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x16u));
        mem.WriteU8((c.A1 + 0x1Eu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x1Cu));
        c.T2 = c.T2 + 0x1u;
        mem.WriteU8((c.A1 + 0x24u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x1Du));
        c.S2 = c.S2 + 0x50u;
        mem.WriteU8((c.A1 + 0x25u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x1Eu));
        c.T1 = c.T1 + 0x50u;
        mem.WriteU8((c.A1 + 0x26u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V0 = (int)c.T2 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S3 = c.S3 + 0x60u;
            goto L800252C8;
        }
        c.S3 = c.S3 + 0x60u;
        L80025528: ;
        c.S5 = 0x1F800000u;
        c.V0 = 0x00001000u;
        c.S2 = mem.ReadU32((c.S1 + 0x28u));
        c.S3 = mem.ReadU32((c.S1 + 0x2Cu));
        c.S0 = c.S5 + 0x60u;
        mem.WriteU32((c.S5 + 0x60u), c.V0);
       
        mem.WriteU32((c.S0 + 0x4u), 0u);
        mem.WriteU32((c.S0 + 0x8u), c.V0);
       
        mem.WriteU32((c.S0 + 0xCu), 0u);
        mem.WriteU16((c.S0 + 0x10u), (ushort)c.V0);
        c.V1 = mem.ReadU32((c.S1 + 0x4u));
        c.V0 = c.V1 & 0x0080u;
        if (c.V0 == 0u) {
            c.A0 = c.S6 + 0u;
            goto L800257B4;
        }
        c.A0 = c.S6 + 0u;
        c.V0 = mem.ReadU16((c.S6 + 0x14u));
        c.A1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S6 + 0x18u));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S6 + 0x1Cu));
        c.A2 = c.A2 + 0x14u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.RA = 0x8002559Cu;
        MediEvil_game.func_800A4880(c, m);
        c.T4 = mem.ReadU32(c.S6);
        c.T5 = mem.ReadU32((c.S6 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S6 + 0x8u));
        c.T5 = mem.ReadU32((c.S6 + 0xCu));
        c.T6 = mem.ReadU32((c.S6 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T8 = 0x1F800000u;
        c.T8 = mem.ReadU32((c.T8 + 0x34u));
        c.T4 = mem.ReadU32((c.T8 + 0x14u));
        c.T5 = mem.ReadU32((c.T8 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T8 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.A0 = mem.ReadU16((c.V0 + 0x14u));
        mem.WriteU16((c.SP + 0x18u), (ushort)c.A0);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.A1 = mem.ReadU16((c.V0 + 0x18u));
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.A1);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.A2 = mem.ReadU16((c.V0 + 0x1Cu));
        c.V0 = 0x800F0000u;
        c.V1 = mem.ReadU32((c.V0 - 0x10F0u));
        c.V1 = c.V1 + 0xCu;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.A2);
        c.A3 = (uint)(short)mem.ReadU16((c.S1 + 0x38u));
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.V0 = c.A0 + c.V0;
        mem.WriteU16((c.SP + 0x28u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0x8u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.V0 = c.A1 + c.V0;
        mem.WriteU16((c.SP + 0x2Au), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0xEu));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.V0 = c.A2 + c.V0;
        mem.WriteU16((c.SP + 0x2Cu), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0x4u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.A0 = c.A0 - c.V0;
        mem.WriteU16((c.SP + 0x20u), (ushort)c.A0);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0xAu));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.A1 = c.A1 - c.V0;
        mem.WriteU16((c.SP + 0x22u), (ushort)c.A1);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0x10u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A1 = c.SP + 0x20u;
        c.A3 = c.SP + 0x18u;
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.A2 = c.A2 - c.V0;
        mem.WriteU16((c.SP + 0x24u), (ushort)c.A2);
        c.A2 = c.SP + 0x28u;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.A3));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.A3 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.A2));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.A2 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.A1));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.A1 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.V0 = c.SP + 0x50u;
        mem.WriteU32(c.V0, RecompOne.Runtime.Gte.Read(19));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x50u));
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V1 = (uint)((int)c.V1 >> (int)(c.A0 & 31u));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x50u), c.V1);
           
            goto L80025BB0;
        }
        mem.WriteU32((c.SP + 0x50u), c.V1);
       
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L80025BB0;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.A3; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.A2; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); var _ad = c.A1; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(14, _ad, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.SP + 0x2Au));
        c.V1 = (uint)(short)mem.ReadU16((c.SP + 0x1Au));
        c.V0 = c.V0 - c.V1;
        c.V1 = (uint)(short)mem.ReadU16((c.S1 + 0x38u));
        c.V0 = c.V0 << 12;
        if (c.V1 != 0u) { if ((int)c.V0 == int.MinValue && (int)c.V1 == -1) { c.LO = 0x80000000u; c.HI = 0u; } else { c.LO = (uint)((int)c.V0 / (int)c.V1); c.HI = (uint)((int)c.V0 % (int)c.V1); } }
        c.A3 = c.LO;
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        { var _r = (long)(int)c.A3 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.S5 + 0x60u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V0 = (uint)(short)mem.ReadU16((c.V0 + 0x8u));
        { var _r = (long)(int)c.A3 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.S0 + 0x8u), (ushort)c.V0);
        goto L800257F4;
        L800257B4: ;
        c.V0 = c.V1 & 0x0100u;
        if (c.V0 == 0u) {
            goto L800257D8;
        }
        c.V0 = mem.ReadU16((c.S1 + 0x3Cu));
        mem.WriteU16((c.S5 + 0x60u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S1 + 0x3Eu));
        mem.WriteU16((c.S0 + 0x8u), (ushort)c.V0);
        L800257D8: ;
        c.V0 = mem.ReadU16((c.S1 + 0x18u));
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V1 = mem.ReadU16((c.S1 + 0x1Au));
        c.V0 = 0x00000005u;
        mem.WriteU32((c.SP + 0x50u), c.V0);
       
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V1);
        L800257F4: ;
        c.V0 = mem.ReadU32((c.S1 + 0x4u));
        c.V0 = c.V0 & 0x0200u;
        if (c.V0 == 0u) {
            c.V0 = 0x00001000u;
            goto L8002583C;
        }
        c.V0 = 0x00001000u;
        c.S0 = c.SP + 0x30u;
        mem.WriteU32((c.SP + 0x30u), c.V0);
       
        mem.WriteU32((c.SP + 0x34u), 0u);
        mem.WriteU32((c.SP + 0x38u), c.V0);
       
        mem.WriteU32((c.SP + 0x3Cu), 0u);
        mem.WriteU16((c.SP + 0x40u), (ushort)c.V0);
        c.A0 = (uint)(short)mem.ReadU16((c.S1 + 0x3Au));
        c.A1 = c.S0 + 0u;
        c.RA = 0x8002582Cu;
        MediEvil_game.func_800A43E8(c, m);
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x60u;
        c.A1 = c.S0 + 0u;
        c.RA = 0x8002583Cu;
        MediEvil_game.func_800A4CFC(c, m);
        L8002583C: ;
        c.A1 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.GP + 0x5E0u));
        c.A1 = c.A1 + 0x60u;
        c.A0 = c.A0 + 0x80u;
        c.RA = 0x80025850u;
        MediEvil_game.func_800A4F4C(c, m);
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 + 0x60u;
        c.T4 = mem.ReadU32((c.T8 + 0x14u));
        c.T5 = mem.ReadU32((c.T8 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T8 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 + 0x60u;
        c.T4 = mem.ReadU32(c.T8);
        c.T5 = mem.ReadU32((c.T8 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T8 + 0x8u));
        c.T5 = mem.ReadU32((c.T8 + 0xCu));
        c.T6 = mem.ReadU32((c.T8 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T8 = 0u + 0u;
        c.V1 = (uint)(short)mem.ReadU16((c.SP + 0x18u));
        c.V0 = (uint)(short)mem.ReadU16((c.SP + 0x1Au));
        RecompOne.Runtime.Gte.WriteControl(5, c.V1);
        RecompOne.Runtime.Gte.WriteControl(6, c.V0);
        RecompOne.Runtime.Gte.WriteControl(7, c.T8);
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        if ((int)c.V0 <= 0) {
            c.T2 = c.T8 + 0u;
            goto L80025BB0;
        }
        c.T2 = c.T8 + 0u;
        c.T1 = c.SP + 0x10u;
        c.A3 = 0xFF000000u;
        c.A3 = c.A3 | CullMaskX;
        c.T0 = 0x00FF0000u;
        c.T0 = c.T0 | 0xFFFFu;
        c.T7 = 0xFF000000u;
        c.T3 = c.S2 + 0x4u;
        L800258E4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V1 << 3;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0x8u;
        c.A2 = c.S2 + c.V0;
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0x8u;
        c.V1 = (uint)(short)mem.ReadU16(c.S2);
        c.A1 = c.S3 + c.V0;
        c.V1 = c.V1 << 3;
        c.V1 = c.S4 + c.V1;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V1));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V1 + 0x4u)));
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 0);
        c.V0 = (uint)(short)mem.ReadU16((c.T3 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.V0 + 0x4u)));
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A2 + 0x10u), c.V0);
       
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 0);
        c.V0 = (uint)(short)mem.ReadU16(c.T3);
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.V0 + 0x4u)));
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A2 + 0x18u), c.V0);
       
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 2, 0);
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x10u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            mem.WriteU32((c.A2 + 0x20u), c.V1);
           
            goto L80025A24;
        }
        mem.WriteU32((c.A2 + 0x20u), c.V1);
       
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x18u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            goto L80025A24;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x20u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 != 0u) {
            goto L80025A64;
        }
        L80025A24: ;
        c.A0 = mem.ReadU32((c.SP + 0x50u));
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.V1 = mem.ReadU32(c.A2);
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.V0;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.V1 & c.T7;
        c.V0 = c.V0 & c.T0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A2, c.V1);
       
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.A2 & c.T0;
        c.V0 = c.V0 & c.T7;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
       
        L80025A64: ;
        c.V0 = (uint)(short)mem.ReadU16(c.S3);
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V0 + 0x4u)));
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 0);
        c.V0 = (uint)(short)mem.ReadU16((c.S3 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.V0 + 0x4u)));
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A1 + 0x10u), c.V0);
       
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 0);
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A1 + 0x18u), c.V0);
       
        c.V0 = mem.ReadU32((c.A2 + 0x18u));
        mem.WriteU32((c.A1 + 0x20u), c.V0);
       
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x10u));
        c.V1 = mem.ReadU32((c.A2 + 0x20u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            mem.WriteU32((c.A1 + 0x28u), c.V1);
           
            goto L80025B50;
        }
        mem.WriteU32((c.A1 + 0x28u), c.V1);
       
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x18u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            goto L80025B50;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x20u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            goto L80025B50;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x28u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 != 0u) {
            goto L80025B90;
        }
        L80025B50: ;
        c.A0 = mem.ReadU32((c.SP + 0x50u));
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.V1 = mem.ReadU32(c.A1);
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.V0;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.V1 & c.T7;
        c.V0 = c.V0 & c.T0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A1, c.V1);
       
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.A1 & c.T0;
        c.V0 = c.V0 & c.T7;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
       
        L80025B90: ;
        c.T2 = c.T2 + 0x1u;
        c.T3 = c.T3 + 0x50u;
        c.S2 = c.S2 + 0x50u;
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V0 = (int)c.T2 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S3 = c.S3 + 0x60u;
            goto L800258E4;
        }
        c.S3 = c.S3 + 0x60u;
        L80025BB0: ;
        c.RA = mem.ReadU32((c.SP + 0x74u));
        c.S6 = mem.ReadU32((c.SP + 0x70u));
        c.S5 = mem.ReadU32((c.SP + 0x6Cu));
        c.S4 = mem.ReadU32((c.SP + 0x68u));
        c.S3 = mem.ReadU32((c.SP + 0x64u));
        c.S2 = mem.ReadU32((c.SP + 0x60u));
        c.S1 = mem.ReadU32((c.SP + 0x5Cu));
        c.S0 = mem.ReadU32((c.SP + 0x58u));
        c.SP = c.SP + 0x78u;
        return;
    }
    
    public static void func_80010808_tl(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x148u;
        mem.WriteU32((c.SP + 0x130u), c.S4);
       
        c.S4 = c.A0 + 0u;
        mem.WriteU32((c.SP + 0x124u), c.S1);
       
        c.S1 = 0x1F800000u;
        mem.WriteU32((c.SP + 0x120u), c.S0);
       
        c.S0 = 0x1F800000u;
        mem.WriteU32((c.SP + 0x144u), c.RA);
       
        mem.WriteU32((c.SP + 0x140u), c.FP);
       
        mem.WriteU32((c.SP + 0x13Cu), c.S7);
       
        mem.WriteU32((c.SP + 0x138u), c.S6);
       
        mem.WriteU32((c.SP + 0x134u), c.S5);
       
        mem.WriteU32((c.SP + 0x12Cu), c.S3);
       
        mem.WriteU32((c.SP + 0x128u), c.S2);
       
        c.A1 = mem.ReadU32((c.S4 + 0x4u));
        c.A0 = mem.ReadU32((c.S1 + 0x88u));
        c.A2 = mem.ReadU32((c.S0 + 0x34u));
        c.A0 = c.A0 + 0xA0u;
        c.RA = 0x80010854u;
        MediEvil_game.func_800A4E3C(c, m);
        c.A1 = c.SP + 0x10u;
        c.V0 = mem.ReadU32((c.S4 + 0x4u));
        c.A3 = mem.ReadU32((c.S1 + 0x88u));
        c.V0 = mem.ReadU16((c.V0 + 0x14u));
        c.V1 = mem.ReadU16((c.A3 + 0xB4u));
        c.A2 = mem.ReadU32((c.S0 + 0x34u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S4 + 0x4u));
        c.V1 = mem.ReadU16((c.A3 + 0xB8u));
        c.V0 = mem.ReadU16((c.V0 + 0x18u));
        c.A0 = c.A3 + 0xA0u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S4 + 0x4u));
        c.V1 = mem.ReadU16((c.A3 + 0xBCu));
        c.V0 = mem.ReadU16((c.V0 + 0x1Cu));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.RA = 0x800108A8u;
        MediEvil_game.func_800A4880(c, m);
        c.S0 = c.S0 + 0x34u;
        c.T7 = mem.ReadU32(c.S0);
        c.T4 = mem.ReadU32(c.T7);
        c.T5 = mem.ReadU32((c.T7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T7 + 0x8u));
        c.T5 = mem.ReadU32((c.T7 + 0xCu));
        c.T6 = mem.ReadU32((c.T7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T7 + 0x14u));
        c.T5 = mem.ReadU32((c.T7 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T7 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.V0 = mem.ReadU32((c.S4 + 0x30u));
        c.T8 = mem.ReadU32((c.S4 + 0x30u));
        c.V1 = c.V0 + 0x8u;
        c.V0 = c.V0 + 0x10u;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T8));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T8 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.V1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.V1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.V0 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.A1 = c.SP + 0x18u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.A1; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw); }
        c.A0 = c.SP + 0x20u;
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.A0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw); }
        c.V0 = c.SP + 0x28u;
        { var _sw = RecompOne.Runtime.Gte.Read(14); var _ad = c.V0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(14, _ad, _sw); }
        c.V1 = 0xFF000000u;
        c.V0 = mem.ReadU32((c.SP + 0x18u));
        c.V1 = c.V1 | CullMaskX;
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 == 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L800109EC;
        }
        c.V0 = c.SP + 0xE8u;
        c.V0 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 == 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L800109EC;
        }
        c.V0 = c.SP + 0xE8u;
        c.V0 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 == 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L800109EC;
        }
        c.V0 = c.SP + 0xE8u;
        c.T7 = 0x800F0000u;
        c.T7 = c.T7 - 0x263Cu;
        c.T8 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.S4 + 0x30u));
        c.T8 = c.T8 - 0x263Cu;
        c.V0 = c.V0 + 0x18u;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T7));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T7 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T8));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T8 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.A1; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.A0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw); }
        c.V0 = mem.ReadU32((c.SP + 0x18u));
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 == 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L800109EC;
        }
        c.V0 = c.SP + 0xE8u;
        c.V0 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 != 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L8001107C;
        }
        c.V0 = c.SP + 0xE8u;
        L800109EC: ;
        mem.WriteU32(c.V0, RecompOne.Runtime.Gte.Read(19));
        c.V0 = 0x800F0000u;
        c.A0 = (uint)(short)mem.ReadU16((c.V0 - 0x121Cu));
        c.A2 = mem.ReadU32((c.SP + 0xE8u));
        c.V0 = (int)c.A0 < (int)c.A2 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = c.A0 - c.A2;
            goto L8001107C;
        }
        c.V1 = c.A0 - c.A2;
        c.V1 = c.V1 << 16;
        if (c.A0 != 0u) { if ((int)c.V1 == int.MinValue && (int)c.A0 == -1) { c.LO = 0x80000000u; c.HI = 0u; } else { c.LO = (uint)((int)c.V1 / (int)c.A0); c.HI = (uint)((int)c.V1 % (int)c.A0); } }
        c.V1 = c.LO;
        c.V0 = 0x1F800000u;
        c.A0 = 0x80020000u;
        c.A1 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = mem.ReadU32((c.A0 - 0x7EE0u));
        c.A0 = (uint)((int)c.A2 >> (int)(c.A1 & 31u));
        c.V0 = (int)c.A0 < (int)c.V0 ? 1u : 0u;
        mem.WriteU32((c.SP + 0xE8u), c.A0);
       
        if (c.V0 != 0u) {
            mem.WriteU32((c.S4 + 0x9Cu), c.V1);
           
            goto L8001107C;
        }
        mem.WriteU32((c.S4 + 0x9Cu), c.V1);
       
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x88u));
        c.V0 = mem.ReadU16((c.V0 + 0x68u));
        c.V1 = c.A0 + 0u;
        c.A0 = c.V0 - 0x1u;
        c.V0 = (int)c.A0 < (int)c.V1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L80010A64;
        }
        c.V1 = c.A0 + 0u;
        L80010A64: ;
        c.V0 = mem.ReadU32(c.S4);
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.S4 + 0x98u), c.V1);
           
            goto L80010A80;
        }
        mem.WriteU32((c.S4 + 0x98u), c.V1);
       
        c.A0 = c.S4 + 0u;
        c.RA = 0x80010A80u;
        MediEvil_tl.func_800104C4(c, m);
        L80010A80: ;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x1190u));
        if (c.V0 != 0u) {
            goto L80010B40;
        }
        c.A0 = mem.ReadU32(c.S4);
        c.V0 = c.A0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L80010B40;
        }
        c.V1 = mem.ReadU32((c.S4 + 0xACu));
        if (c.V1 == 0u) {
            c.V0 = c.A0 & 0x0004u;
            goto L80010B14;
        }
        c.V0 = c.A0 & 0x0004u;
        if (c.V0 == 0u) {
            goto L80010AF4;
        }
        c.V0 = mem.ReadU32((c.S4 + 0xA8u));
        c.A0 = mem.ReadU32((c.S4 + 0xA8u));
        c.V0 = (uint)((int)c.V0 >> 5);
        c.V0 = c.V1 + c.V0;
        mem.WriteU32((c.S4 + 0xACu), c.V0);
       
        c.V0 = (int)c.V0 < (int)c.A0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = 0xFFFFFFFBu;
            goto L80010B40;
        }
        c.V1 = 0xFFFFFFFBu;
        c.V0 = mem.ReadU32(c.S4);
        mem.WriteU32((c.S4 + 0xACu), c.A0);
       
        c.V0 = CullBias(c.V0) & c.V1;
        mem.WriteU32(c.S4, c.V0);
       
        goto L80010B40;
        L80010AF4: ;
        c.V0 = mem.ReadU32((c.S4 + 0xA8u));
        c.V0 = (uint)((int)c.V0 >> 5);
        c.V0 = c.V1 - c.V0;
        if ((int)c.V0 > 0) {
            mem.WriteU32((c.S4 + 0xACu), c.V0);
           
            goto L80010B40;
        }
        mem.WriteU32((c.S4 + 0xACu), c.V0);
       
        mem.WriteU32((c.S4 + 0xACu), 0u);
        goto L80010B40;
        L80010B14: ;
        c.RA = 0x80010B1Cu;
        MediEvil_game.func_800A43B8(c, m);
        c.V0 = c.V0 & 0x003Fu;
        if (c.V0 != 0u) {
            goto L80010B40;
        }
        c.V0 = mem.ReadU32((c.S4 + 0xA8u));
        c.V1 = mem.ReadU32(c.S4);
        c.V0 = (uint)((int)c.V0 >> 5);
        c.V1 = c.V1 | 0x0004u;
        mem.WriteU32((c.S4 + 0xACu), c.V0);
       
        mem.WriteU32(c.S4, c.V1);
       
        L80010B40: ;
        c.V1 = mem.ReadU32((c.S4 + 0x8Cu));
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T0 = 0x00FF0000u;
        c.T0 = c.T0 | 0xFFFFu;
        c.V0 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.V0 + 0x4u));
        c.A3 = 0xFF000000u;
        c.A2 = c.A1 << 2;
        c.A2 = c.S4 + c.A2;
        c.A1 = c.A1 << 5;
        c.A1 = c.A1 + 0x44u;
        c.A1 = c.S4 + c.A1;
        c.V0 = mem.ReadU32((c.A2 + 0x34u));
        c.V1 = c.LO;
        c.A0 = c.V1 << 1;
        c.A0 = c.A0 + c.V1;
        c.A0 = c.A0 << 5;
        c.A0 = c.A0 + c.V0;
        c.V0 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.V0 + 0x88u));
        c.V0 = mem.ReadU32((c.S4 + 0x98u));
        c.V1 = mem.ReadU32((c.T1 + 0x78u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = mem.ReadU32((c.A0 - 0x20u));
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.A3;
        c.V0 = c.V0 & c.T0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32((c.A0 - 0x20u), c.V1);
       
        c.V1 = mem.ReadU32((c.S4 + 0x98u));
        c.V0 = mem.ReadU32((c.T1 + 0x78u));
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        c.A1 = c.A1 & c.T0;
        c.V0 = c.V0 & c.A3;
        c.V0 = c.V0 | c.A1;
        mem.WriteU32(c.V1, c.V0);
       
        c.T4 = mem.ReadU32((c.S4 + 0x28u));
        c.S7 = mem.ReadU32((c.S4 + 0x2Cu));
        c.T7 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = c.T4 + 0x8u;
        mem.WriteU32((c.SP + 0x118u), c.T7);
       
        c.V0 = mem.ReadU32((c.S4 + 0x94u));
        c.T3 = mem.ReadU32((c.A2 + 0x3Cu));
        c.V0 = (int)c.V0 < 4096 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.FP = c.S7 + 0x8u;
            goto L80010C20;
        }
        c.FP = c.S7 + 0x8u;
        c.V0 = mem.ReadU32((c.S4 + 0x8Cu));
        c.V0 = c.V0 + 0x1u;
        mem.WriteU32((c.SP + 0x114u), c.V0);
       
        goto L80010C2C;
        L80010C20: ;
        c.T8 = mem.ReadU32((c.S4 + 0x8Cu));
        mem.WriteU32((c.SP + 0x114u), c.T8);
       
        L80010C2C: ;
        c.T7 = mem.ReadU32((c.SP + 0x114u));
        if ((int)c.T7 <= 0) {
            c.T5 = 0u + 0u;
            goto L8001107C;
        }
        c.T5 = 0u + 0u;
        c.T9 = 0x800F0000u;
        c.V0 = 0x800E0000u;
        c.T6 = c.V0 + 0x11E0u;
        c.T2 = c.SP + 0xC0u;
        c.T1 = c.T4 + 0x4u;
        L80010C50: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S5 = c.SP + 0x38u;
        c.V1 = mem.ReadU32((c.S4 + 0x90u));
        c.S0 = 0x00000001u;
        c.V0 = c.V1 + c.S0;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.S3 = c.SP + 0x80u;
            goto L80010F54;
        }
        c.S3 = c.SP + 0x80u;
        c.S6 = 0x00000050u;
        c.S2 = c.SP + 0x84u;
        c.S1 = c.SP + 0x3Cu;
        L80010C78: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.A0 = c.S0 << 12;
        if (c.V1 != 0u) { if ((int)c.A0 == int.MinValue && (int)c.V1 == -1) { c.LO = 0x80000000u; c.HI = 0u; } else { c.LO = (uint)((int)c.A0 / (int)c.V1); c.HI = (uint)((int)c.A0 % (int)c.V1); } }
        c.A0 = c.LO;
        c.V0 = (uint)(short)mem.ReadU16(c.S7);
        c.V1 = (uint)(short)mem.ReadU16(c.T4);
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.T4);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S5, (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.S7 + 0x2u));
        c.V1 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16((c.T1 - 0x2u));
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.S1 - 0x2u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.S7 + 0x4u));
        c.V1 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.T1);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S1, (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.FP);
        c.V1 = (uint)(short)mem.ReadU16(c.T0);
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.T0);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S3, (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.FP + 0x2u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x2u));
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16((c.T0 + 0x2u));
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.S2 - 0x2u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.FP + 0x4u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x4u));
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16((c.T0 + 0x4u));
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S2, (ushort)c.V0);
        c.V0 = mem.ReadU32(c.S4);
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L80010E98;
        }
        c.A1 = mem.ReadU32((c.S4 + 0xACu));
        if (c.A1 == 0u) {
            goto L80010E98;
        }
        c.V1 = mem.ReadU32((c.T9 - 0x11E4u));
        c.V0 = mem.ReadU32((c.S4 + 0xA0u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T7 = c.LO;
        mem.WriteU32((c.SP + 0x11Cu), c.T7);
       
        c.V0 = mem.ReadU32((c.S4 + 0xA4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = (uint)(short)mem.ReadU16(c.S5);
        c.V1 = (uint)(short)mem.ReadU16((c.S1 - 0x2u));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.T7;
        c.V1 = c.V1 << 4;
        c.A0 = c.LO;
        c.A0 = c.V1 + c.A0;
        c.V1 = c.V0 & 0x0FFFu;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T6;
        c.V0 = c.A0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T6;
        c.V1 = (uint)(short)mem.ReadU16(c.V1);
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        c.V1 = c.V1 + c.V0;
        { var _r = (long)(int)c.V1 * (int)c.A1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.S1);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S1, (ushort)c.V0);
        c.V1 = mem.ReadU32((c.T9 - 0x11E4u));
        c.V0 = mem.ReadU32((c.S4 + 0xA0u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T7 = c.LO;
        mem.WriteU32((c.SP + 0x11Cu), c.T7);
       
        c.V0 = mem.ReadU32((c.S4 + 0xA4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = (uint)(short)mem.ReadU16(c.S3);
        c.V1 = (uint)(short)mem.ReadU16((c.S2 - 0x2u));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.T7;
        c.V1 = c.V1 << 4;
        c.A0 = c.LO;
        c.A0 = c.V1 + c.A0;
        c.V1 = c.V0 & 0x0FFFu;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T6;
        c.V0 = c.A0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T6;
        c.V1 = (uint)(short)mem.ReadU16(c.V1);
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        c.A0 = mem.ReadU32((c.S4 + 0xACu));
        c.V1 = c.V1 + c.V0;
        { var _r = (long)(int)c.V1 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.S2);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S2, (ushort)c.V0);
        L80010E98: ;
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        if (c.V0 != 0u) { if ((int)c.S6 == int.MinValue && (int)c.V0 == -1) { c.LO = 0x80000000u; c.HI = 0u; } else { c.LO = (uint)((int)c.S6 / (int)c.V0); c.HI = (uint)((int)c.S6 % (int)c.V0); } }
        c.V0 = c.LO;
        c.V1 = mem.ReadU32((c.S4 + 0x9Cu));
        c.V0 = c.V0 + 0x8u;
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.S0 << 2;
        c.V0 = c.T2 + c.V0;
        c.V1 = c.LO;
        c.A3 = (uint)((int)c.V1 >> 16);
        mem.WriteU32(c.V0, c.A3);
       
        c.V0 = mem.ReadU32((c.S4 + 0x8Cu));
        c.V0 = (int)c.T5 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.S5 + 0u;
            goto L80010F2C;
        }
        c.A0 = c.S5 + 0u;
        c.A2 = mem.ReadU32((c.SP + 0x118u));
        c.A1 = c.S3 + 0u;
        mem.WriteU32((c.SP + 0xF0u), c.T0);
       
        mem.WriteU32((c.SP + 0xF4u), c.T1);
       
        mem.WriteU32((c.SP + 0xF8u), c.T2);
       
        mem.WriteU32((c.SP + 0xFCu), c.T3);
       
        mem.WriteU32((c.SP + 0x100u), c.T4);
       
        mem.WriteU32((c.SP + 0x104u), c.T5);
       
        mem.WriteU32((c.SP + 0x108u), c.T6);
       
        mem.WriteU32((c.SP + 0x110u), c.T9);
       
        c.RA = 0x80010F08u;
        MediEvil_tl.func_800105C0(c, m);
        mem.WriteU32((c.SP + 0x118u), c.V0);
       
        c.T9 = mem.ReadU32((c.SP + 0x110u));
        c.T6 = mem.ReadU32((c.SP + 0x108u));
        c.T5 = mem.ReadU32((c.SP + 0x104u));
        c.T4 = mem.ReadU32((c.SP + 0x100u));
        c.T3 = mem.ReadU32((c.SP + 0xFCu));
        c.T2 = mem.ReadU32((c.SP + 0xF8u));
        c.T1 = mem.ReadU32((c.SP + 0xF4u));
        c.T0 = mem.ReadU32((c.SP + 0xF0u));
        L80010F2C: ;
        c.S1 = c.S1 + 0x8u;
        c.S5 = c.S5 + 0x8u;
        c.S2 = c.S2 + 0x8u;
        c.S3 = c.S3 + 0x8u;
        c.V1 = mem.ReadU32((c.S4 + 0x90u));
        c.S0 = c.S0 + 0x1u;
        c.V0 = c.V1 + 0x1u;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S6 = c.S6 + 0x50u;
            goto L80010C78;
        }
        c.S6 = c.S6 + 0x50u;
        L80010F54: ;
        c.V0 = mem.ReadU32(c.T4);
        c.S5 = c.SP + 0x30u;
        mem.WriteU32((c.SP + 0x30u), c.V0);
       
        c.V0 = mem.ReadU16(c.T1);
        mem.WriteU16((c.S5 + 0x4u), (ushort)c.V0);
        c.V0 = 0x52000000u;
        mem.WriteU32((c.SP + 0xC0u), c.V0);
       
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        c.S0 = 0x00000001u;
        c.V0 = c.V0 + c.S0;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.S3 = c.SP + 0x38u;
            goto L80010FCC;
        }
        c.S3 = c.SP + 0x38u;
        c.A1 = 0x52000000u;
        c.A0 = c.T2 + 0x4u;
        L80010F94: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32(c.A0);
        c.S0 = c.S0 + 0x1u;
        c.V1 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        c.V1 = c.V1 << 8;
        c.V1 = c.V1 + c.V0;
        c.V1 = c.V1 + c.A1;
        mem.WriteU32(c.A0, c.V1);
       
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        c.V0 = c.V0 + 0x1u;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.A0 = c.A0 + 0x4u;
            goto L80010F94;
        }
        c.A0 = c.A0 + 0x4u;
        L80010FCC: ;
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        if ((int)c.V0 <= 0) {
            c.S0 = 0u + 0u;
            goto L80011058;
        }
        c.S0 = 0u + 0u;
        c.A0 = c.T3 + 0x10u;
        L80010FE0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.S5));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.S5 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.S3));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.S3 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.S3));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.S3 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.V0 = c.S0 << 2;
        c.V0 = c.T2 + c.V0;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.S0 + 0x1u;
        mem.WriteU32((c.A0 - 0xCu), c.V0);
       
        c.V0 = c.V1 << 2;
        c.V0 = c.T2 + c.V0;
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.A0 - 0x4u), c.V0);
       
        c.V0 = c.T3 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.V0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.A0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw); }
        c.A0 = c.A0 + 0x14u;
        c.T3 = c.T3 + 0x14u;
        c.S5 = c.S5 + 0x8u;
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        c.S0 = c.V1 + 0u;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S3 = c.S3 + 0x8u;
            goto L80010FE0;
        }
        c.S3 = c.S3 + 0x8u;
        L80011058: ;
        c.T1 = c.T1 + 0x8u;
        c.T4 = c.T4 + 0x8u;
        c.T0 = c.T0 + 0x8u;
        c.S7 = c.S7 + 0x8u;
        c.T7 = mem.ReadU32((c.SP + 0x114u));
        c.T5 = c.T5 + 0x1u;
        c.V0 = (int)c.T5 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.FP = c.FP + 0x8u;
            goto L80010C50;
        }
        c.FP = c.FP + 0x8u;
        L8001107C: ;
        c.RA = mem.ReadU32((c.SP + 0x144u));
        c.FP = mem.ReadU32((c.SP + 0x140u));
        c.S7 = mem.ReadU32((c.SP + 0x13Cu));
        c.S6 = mem.ReadU32((c.SP + 0x138u));
        c.S5 = mem.ReadU32((c.SP + 0x134u));
        c.S4 = mem.ReadU32((c.SP + 0x130u));
        c.S3 = mem.ReadU32((c.SP + 0x12Cu));
        c.S2 = mem.ReadU32((c.SP + 0x128u));
        c.S1 = mem.ReadU32((c.SP + 0x124u));
        c.S0 = mem.ReadU32((c.SP + 0x120u));
        c.SP = c.SP + 0x148u;
        return;
    }

    public static void func_8009A4C8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x18u;
        c.A3 = c.A0 + 0u;
        c.T4 = 0u + 0u;
        c.T3 = 0x00000001u;
        c.T2 = 0x00000002u;
        L8009A4DC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.A0 = c.A0 + 0x8u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        if (c.T2 != 0u) {
            c.A0 = c.A0 + 0x8u;
            goto L8009A554;
        }
        c.A0 = c.A0 + 0x8u;
        c.V0 = (uint)(short)mem.ReadU16(c.A3);
        c.V1 = (uint)(short)mem.ReadU16((c.A3 + 0x38u));
        c.V0 = c.V0 + c.V1;
        c.V0 = (uint)((int)c.V0 >> 1);
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x2u));
        c.V1 = (uint)(short)mem.ReadU16((c.A3 + 0x3Au));
        c.V0 = c.V0 + c.V1;
        c.V0 = (uint)((int)c.V0 >> 1);
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x4u));
        c.V1 = (uint)(short)mem.ReadU16((c.A3 + 0x3Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = (uint)((int)c.V0 >> 1);
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = c.SP + 0x10u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L8009A560;
        L8009A554: ;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.A0 = c.A0 + 0x8u;
        L8009A560: ;
        TerrainFloatProjection.Rtpt(12, false);
        c.A2 = c.SP + 0u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.SP, _sw); TerrainFloatProjection.Store(12, c.SP, _sw); }
        c.V0 = c.SP + 0x4u;
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V0, _sw); TerrainFloatProjection.Store(13, c.V0, _sw); }
        if (c.T2 != 0u) {
            c.V0 = c.SP + 0x8u;
            goto L8009A58C;
        }
        c.V0 = c.SP + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw); }
        c.V0 = c.SP + 0x8u;
        L8009A58C: ;
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); TerrainFloatProjection.Store(14, c.V0, _sw); }
        c.V1 = 0x00000002u;
        c.T1 = 0x1F800000u;
        c.T1 = mem.ReadU16((c.T1 + 0x98u));
        c.T1 = (uint)((int)c.T1 + CullEdgeX(mem));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU16((c.T0 + 0x9Au));
        L8009A5A4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.A2);
        if ((int)c.V0 < -CullEdgeX(mem)) {
            c.V0 = (int)c.V0 < (int)c.T1 ? 1u : 0u;
            goto L8009A5D8;
        }
        c.V0 = (int)c.V0 < (int)c.T1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L8009A5D8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        if ((int)c.V0 < 0) {
            c.V0 = (int)c.V0 < (int)c.T0 ? 1u : 0u;
            goto L8009A5D8;
        }
        c.V0 = (int)c.V0 < (int)c.T0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L8009A5D8;
        }
        c.T4 = c.T4 | c.T3;
        L8009A5D8: ;
        c.T3 = c.T3 << 1;
        c.A2 = c.A2 + 0x4u;
        c.V0 = c.V1 + 0u;
        if (c.V0 != 0u) {
            c.V1 = c.V1 - 0x1u;
            goto L8009A5A4;
        }
        c.V1 = c.V1 - 0x1u;
        c.V0 = c.T2 + 0u;
        if (c.V0 != 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L8009A4DC;
        }
        c.T2 = c.T2 - 0x1u;
        if (c.T4 == 0u) {
            c.V1 = 0x000000FFu;
            goto L8009A610;
        }
        c.V1 = 0x000000FFu;
        if (c.T4 == c.V1) {
            c.V0 = 0x00000002u;
            goto L8009A614;
        }
        c.V0 = 0x00000002u;
        c.V0 = 0x00000001u;
        goto L8009A614;
        L8009A610: ;
        c.V0 = 0u + 0u;
        L8009A614: ;
        c.SP = c.SP + 0x18u;
        return;
    }

    public static void func_8009A61C(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x20u;
        c.V1 = c.SP + 0u;
        c.A2 = 0x00000002u;
        L8009A628: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.A0 = c.A0 + 0x8u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        if (c.A2 != 0u) {
            c.A0 = c.A0 + 0x8u;
            goto L8009A65C;
        }
        c.A0 = c.A0 + 0x8u;
        c.T0 = 0x800F0000u;
        c.T0 = c.T0 - 0x263Cu;
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L8009A668;
        L8009A65C: ;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.A0 = c.A0 + 0x8u;
        L8009A668: ;
        TerrainFloatProjection.Rtpt(12, false);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V1, _sw); TerrainFloatProjection.Store(12, c.V1, _sw); }
        c.V1 = c.V1 + 0x4u;
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); TerrainFloatProjection.Store(13, c.V1, _sw); }
        if (c.A2 != 0u) {
            c.V1 = c.V1 + 0x4u;
            goto L8009A694;
        }
        c.V1 = c.V1 + 0x4u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw); }
        c.V0 = c.A2 + 0u;
        goto L8009A6A0;
        L8009A694: ;
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V1, _sw); TerrainFloatProjection.Store(14, c.V1, _sw); }
        c.V1 = c.V1 + 0x4u;
        c.V0 = c.A2 + 0u;
        L8009A6A0: ;
        if (c.V0 != 0u) {
            c.A2 = c.A2 - 0x1u;
            goto L8009A628;
        }
        c.A2 = c.A2 - 0x1u;
        c.A2 = 0u + 0u;
        c.A1 = c.A2 + 0u;
        c.V1 = c.SP + 0u;
        c.A0 = 0x00000008u;
        L8009A6B8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.V1);
        c.V1 = c.V1 + 0x4u;
        c.A0 = c.A0 - 0x1u;
        c.V0 = (int)c.V0 >= -CullEdgeX(mem) ? 1u : 0u;
        if (c.A0 != 0u) {
            c.A1 = c.A1 + c.V0;
            goto L8009A6B8;
        }
        c.A1 = c.A1 + c.V0;
        if (c.A1 == 0u) {
            c.A2 = c.A2 + c.A1;
            goto L8009A78C;
        }
        c.A2 = c.A2 + c.A1;
        c.A1 = 0u + 0u;
        c.V1 = c.SP + 0u;
        c.A0 = 0x00000008u;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU16((c.A3 + 0x98u));
        c.A3 = (uint)((int)c.A3 + CullEdgeX(mem));
        L8009A6F0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.V1);
        c.V1 = c.V1 + 0x4u;
        c.A0 = c.A0 - 0x1u;
        c.V0 = (int)c.V0 < (int)c.A3 ? 1u : 0u;
        if (c.A0 != 0u) {
            c.A1 = c.A1 + c.V0;
            goto L8009A6F0;
        }
        c.A1 = c.A1 + c.V0;
        if (c.A1 == 0u) {
            c.A2 = c.A2 + c.A1;
            goto L8009A78C;
        }
        c.A2 = c.A2 + c.A1;
        c.A1 = 0u + 0u;
        c.V1 = c.SP + 0x2u;
        c.A0 = 0x00000008u;
        L8009A71C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.V1);
        c.V1 = c.V1 + 0x4u;
        c.A0 = c.A0 - 0x1u;
        c.V0 = ~(0u | c.V0);
        c.V0 = c.V0 >> 31;
        if (c.A0 != 0u) {
            c.A1 = c.A1 + c.V0;
            goto L8009A71C;
        }
        c.A1 = c.A1 + c.V0;
        if (c.A1 == 0u) {
            c.A2 = c.A2 + c.A1;
            goto L8009A78C;
        }
        c.A2 = c.A2 + c.A1;
        c.A1 = 0u + 0u;
        c.V1 = c.SP + 0x2u;
        c.A0 = 0x00000008u;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU16((c.A3 + 0x9Au));
        L8009A754: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.V1);
        c.V1 = c.V1 + 0x4u;
        c.A0 = c.A0 - 0x1u;
        c.V0 = (int)c.A3 < (int)c.V0 ? 1u : 0u;
        c.V0 = c.V0 ^ 0x0001u;
        if (c.A0 != 0u) {
            c.A1 = c.A1 + c.V0;
            goto L8009A754;
        }
        c.A1 = c.A1 + c.V0;
        if (c.A1 == 0u) {
            c.A2 = c.A2 + c.A1;
            goto L8009A78C;
        }
        c.A2 = c.A2 + c.A1;
        c.V1 = 0x00000020u;
        if (c.A2 == c.V1) {
            c.V0 = 0x00000002u;
            goto L8009A790;
        }
        c.V0 = 0x00000002u;
        c.V0 = 0x00000001u;
        goto L8009A790;
        L8009A78C: ;
        c.V0 = 0u + 0u;
        L8009A790: ;
        c.SP = c.SP + 0x20u;
        return;
    }


    public static void func_800B36CC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x70u;
        c.T6 = c.A0;
        c.T4 = c.T6 + 0x10u;
        c.T5 = 0u;
        c.V0 = mem.ReadU32((c.T6 + 0x8u));
        c.V1 = mem.ReadU32((c.T6 + 0xCu));
        c.V0 = c.V0 | 0x0004u;
        if (c.V1 == 0u) {
            mem.WriteU32((c.T6 + 0x8u), c.V0);
            goto L800B3FA4;
        }
        mem.WriteU32((c.T6 + 0x8u), c.V0);
        c.T3 = 0u | 0xFFFFu;
        c.A3 = 0x80800000u;
        c.A3 = c.A3 | 0x8081u;
        L800B36FC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T2 = mem.ReadU16((c.T4 + 0x8u));
        c.T1 = mem.ReadU32((c.T4 + 0x10u));
        if (c.T2 == 0u) {
            goto L800B3F8C;
        }
        L800B370C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T0 = mem.ReadU16((c.T1 + 0x2u));
        c.V0 = mem.ReadU16(c.T1);
        c.V1 = c.V0 & 0xFFFFu;
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.T1 = c.T1 + 0x4u;
            goto L800B3F80;
        }
        c.T1 = c.T1 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + 0x77D0u;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800B3748u: goto L800B3748;
            case 0x800B3758u: goto L800B3758;
            case 0x800B3764u: goto L800B3764;
            case 0x800B3920u: goto L800B3920;
            case 0x800B3B44u: goto L800B3B44;
            case 0x800B3B50u: goto L800B3B50;
            case 0x800B3B60u: goto L800B3B60;
            case 0x800B3D1Cu: goto L800B3D1C;
            case 0x800B3F80u: goto L800B3F80;
            case 0x800B3F40u: goto L800B3F40;
            case 0x800B3F54u: goto L800B3F54;
            case 0x800B3F68u: goto L800B3F68;
            case 0x800B4038u: goto L800B4038;
            case 0x800B4078u: goto L800B4078;
            case 0x800B4040u: goto L800B4040;
            case 0x800B405Cu: goto L800B405C;
            case 0x800B4080u: goto L800B4080;
            case 0x800B4088u: goto L800B4088;
            case 0x800B40A0u: goto L800B40A0;
            case 0x800B40C4u: goto L800B40C4;
            case 0x800B4054u: goto L800B4054;
            case 0x800B40C0u: goto L800B40C0;
            case 0x800B4970u: goto L800B4970;
            case 0x800B49C4u: goto L800B49C4;
            case 0x800B4A18u: goto L800B4A18;
            case 0x800B4A6Cu: goto L800B4A6C;
            case 0x800B4AC0u: goto L800B4AC0;
            case 0x800B4B14u: goto L800B4B14;
            case 0x800B4B68u: goto L800B4B68;
            case 0x800B4BBCu: goto L800B4BBC;
            case 0x800B4CB8u: goto L800B4CB8;
            case 0x800B4C10u: goto L800B4C10;
            case 0x800B4C64u: goto L800B4C64;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B3748: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        goto L800B3F74;
        L800B3758: ;
        c.V0 = c.T0 & 0xFFFFu;
        c.V0 = c.V0 << 4;
        goto L800B3F78;
        L800B3764: ;
        c.V0 = c.T0 - 0x1u;
        c.T0 = c.V0;
        c.V0 = c.V0 & 0xFFFFu;
        if (c.V0 == c.T3) {
            c.V0 = c.T2 & 0xFFFFu;
            goto L800B3F84;
        }
        c.V0 = c.T2 & 0xFFFFu;
        c.A2 = c.T1 + 0x11u;
        L800B377C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 + 0x1u));
        c.V1 = mem.ReadU32((c.GP + 0x7C0u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.A1 = mem.ReadU32(c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0xAu));
        mem.WriteU16((c.A2 - 0x3u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0x6u));
        mem.WriteU16((c.A2 - 0x7u), (ushort)c.V0);
        c.V0 = mem.ReadU8((c.A2 - 0x9u));
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x8u));
        mem.WriteU8((c.A2 - 0x9u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x5u));
        mem.WriteU8((c.A2 - 0x8u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x4u));
        mem.WriteU8((c.A2 - 0x5u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x1u));
        mem.WriteU8((c.A2 - 0x4u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        mem.WriteU8((c.A2 - 0x1u), (byte)c.V1);
        c.V1 = mem.ReadU8(c.A2);
        c.V0 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        { var _r = (long)(int)c.V1 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T1 = c.T1 + 0x18u;
        c.T2 = c.T2 - 0x1u;
        c.A0 = c.T0 - 0x1u;
        c.T0 = c.A0;
        c.A0 = c.A0 & 0xFFFFu;
        c.V0 = c.HI;
        c.V1 = c.V0 + c.V1;
        c.V0 = mem.ReadU8((c.A1 + 0x5u));
        c.V1 = (uint)((int)c.V1 >> 7);
        c.V0 = c.V0 + c.V1;
        mem.WriteU8(c.A2, (byte)c.V0);
        if (c.A0 != c.T3) {
            c.A2 = c.A2 + 0x18u;
            goto L800B377C;
        }
        c.A2 = c.A2 + 0x18u;
        c.V0 = c.T2 & 0xFFFFu;
        goto L800B3F84;
        L800B3920: ;
        c.V0 = c.T0 - 0x1u;
        c.T0 = c.V0;
        c.V0 = c.V0 & 0xFFFFu;
        if (c.V0 == c.T3) {
            c.V0 = c.T2 & 0xFFFFu;
            goto L800B3F84;
        }
        c.V0 = c.T2 & 0xFFFFu;
        c.A2 = c.T1 + 0x17u;
        L800B3938: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 - 0xDu));
        c.V1 = mem.ReadU32((c.GP + 0x7C0u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.A1 = mem.ReadU32(c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0xAu));
        mem.WriteU16((c.A2 - 0x5u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0x6u));
        mem.WriteU16((c.A2 - 0x9u), (ushort)c.V0);
        c.V0 = mem.ReadU8((c.A2 - 0xBu));
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0xAu));
        mem.WriteU8((c.A2 - 0xBu), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x7u));
        mem.WriteU8((c.A2 - 0xAu), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x6u));
        mem.WriteU8((c.A2 - 0x7u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x3u));
        mem.WriteU8((c.A2 - 0x6u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x2u));
        mem.WriteU8((c.A2 - 0x3u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x1u));
        mem.WriteU8((c.A2 - 0x2u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        mem.WriteU8((c.A2 - 0x1u), (byte)c.V1);
        c.V1 = mem.ReadU8(c.A2);
        c.V0 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.T1 = c.T1 + 0x1Cu;
        c.T2 = c.T2 - 0x1u;
        { var _r = (long)(int)c.V1 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A0 = c.T0 - 0x1u;
        c.T0 = c.A0;
        c.A0 = c.A0 & 0xFFFFu;
        c.V0 = c.HI;
        c.V1 = c.V0 + c.V1;
        c.V0 = mem.ReadU8((c.A1 + 0x5u));
        c.V1 = (uint)((int)c.V1 >> 7);
        c.V0 = c.V0 + c.V1;
        mem.WriteU8(c.A2, (byte)c.V0);
        if (c.A0 != c.T3) {
            c.A2 = c.A2 + 0x1Cu;
            goto L800B3938;
        }
        c.A2 = c.A2 + 0x1Cu;
        c.V0 = c.T2 & 0xFFFFu;
        goto L800B3F84;
        L800B3B44: ;
        c.V0 = c.T0 & 0xFFFFu;
        c.V0 = c.V0 << 4;
        goto L800B3F78;
        L800B3B50: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        goto L800B3F74;
        L800B3B60: ;
        c.V0 = c.T0 - 0x1u;
        c.T0 = c.V0;
        c.V0 = c.V0 & 0xFFFFu;
        if (c.V0 == c.T3) {
            c.V0 = c.T2 & 0xFFFFu;
            goto L800B3F84;
        }
        c.V0 = c.T2 & 0xFFFFu;
        c.A2 = c.T1 + 0x15u;
        L800B3B78: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 + 0x1u));
        c.V1 = mem.ReadU32((c.GP + 0x7C0u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.A1 = mem.ReadU32(c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0xAu));
        mem.WriteU16((c.A2 - 0x3u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0x6u));
        mem.WriteU16((c.A2 - 0x7u), (ushort)c.V0);
        c.V0 = mem.ReadU8((c.A2 - 0x9u));
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x8u));
        mem.WriteU8((c.A2 - 0x9u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x5u));
        mem.WriteU8((c.A2 - 0x8u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x4u));
        mem.WriteU8((c.A2 - 0x5u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x1u));
        mem.WriteU8((c.A2 - 0x4u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        mem.WriteU8((c.A2 - 0x1u), (byte)c.V1);
        c.V1 = mem.ReadU8(c.A2);
        c.V0 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        { var _r = (long)(int)c.V1 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T1 = c.T1 + 0x1Cu;
        c.T2 = c.T2 - 0x1u;
        c.A0 = c.T0 - 0x1u;
        c.T0 = c.A0;
        c.A0 = c.A0 & 0xFFFFu;
        c.V0 = c.HI;
        c.V1 = c.V0 + c.V1;
        c.V0 = mem.ReadU8((c.A1 + 0x5u));
        c.V1 = (uint)((int)c.V1 >> 7);
        c.V0 = c.V0 + c.V1;
        mem.WriteU8(c.A2, (byte)c.V0);
        if (c.A0 != c.T3) {
            c.A2 = c.A2 + 0x1Cu;
            goto L800B3B78;
        }
        c.A2 = c.A2 + 0x1Cu;
        c.V0 = c.T2 & 0xFFFFu;
        goto L800B3F84;
        L800B3D1C: ;
        c.V0 = c.T0 - 0x1u;
        c.T0 = c.V0;
        c.V0 = c.V0 & 0xFFFFu;
        if (c.V0 == c.T3) {
            c.V0 = c.T2 & 0xFFFFu;
            goto L800B3F84;
        }
        c.V0 = c.T2 & 0xFFFFu;
        c.A2 = c.T1 + 0x1Bu;
        L800B3D34: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 + 0x1u));
        c.V1 = mem.ReadU32((c.GP + 0x7C0u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.A1 = mem.ReadU32(c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0xAu));
        mem.WriteU16((c.A2 - 0x5u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0x6u));
        mem.WriteU16((c.A2 - 0x9u), (ushort)c.V0);
        c.V0 = mem.ReadU8((c.A2 - 0xBu));
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0xAu));
        mem.WriteU8((c.A2 - 0xBu), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x7u));
        mem.WriteU8((c.A2 - 0xAu), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x6u));
        mem.WriteU8((c.A2 - 0x7u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x3u));
        mem.WriteU8((c.A2 - 0x6u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x2u));
        mem.WriteU8((c.A2 - 0x3u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x1u));
        mem.WriteU8((c.A2 - 0x2u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        mem.WriteU8((c.A2 - 0x1u), (byte)c.V1);
        c.V1 = mem.ReadU8(c.A2);
        c.V0 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.T1 = c.T1 + 0x24u;
        c.T2 = c.T2 - 0x1u;
        { var _r = (long)(int)c.V1 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A0 = c.T0 - 0x1u;
        c.T0 = c.A0;
        c.A0 = c.A0 & 0xFFFFu;
        c.V0 = c.HI;
        c.V1 = c.V0 + c.V1;
        c.V0 = mem.ReadU8((c.A1 + 0x5u));
        c.V1 = (uint)((int)c.V1 >> 7);
        c.V0 = c.V0 + c.V1;
        mem.WriteU8(c.A2, (byte)c.V0);
        if (c.A0 != c.T3) {
            c.A2 = c.A2 + 0x24u;
            goto L800B3D34;
        }
        c.A2 = c.A2 + 0x24u;
        c.V0 = c.T2 & 0xFFFFu;
        goto L800B3F84;
        L800B3F40: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 3;
        goto L800B3F78;
        L800B3F54: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 3;
        goto L800B3F78;
        L800B3F68: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 3;
        c.V0 = c.V0 - c.V1;
        L800B3F74: ;
        c.V0 = c.V0 << 2;
        L800B3F78: ;
        c.T1 = c.T1 + c.V0;
        c.T2 = c.T2 - c.T0;
        L800B3F80: ;
        c.V0 = c.T2 & 0xFFFFu;
        L800B3F84: ;
        if (c.V0 != 0u) {
            goto L800B370C;
        }
        L800B3F8C: ;
        c.T5 = c.T5 + 0x1u;
        c.V1 = mem.ReadU32((c.T6 + 0xCu));
        c.V0 = c.T5 & 0xFFFFu;
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.T4 = c.T4 + 0x2Cu;
            goto L800B36FC;
        }
        c.T4 = c.T4 + 0x2Cu;
        L800B3FA4: ;
        c.SP = c.SP + 0x70u;
        return;
        c.SP = c.SP - 0x38u;
        mem.WriteU32((c.SP + 0x28u), c.S2);
        c.S2 = c.A0;
        mem.WriteU32((c.SP + 0x24u), c.S1);
        c.S1 = 0u;
        c.V0 = 0xFFFFFFFBu;
        mem.WriteU32((c.SP + 0x30u), c.S4);
        mem.WriteU32((c.SP + 0x2Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S0);
        c.V1 = mem.ReadU32((c.S2 + 0x8u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        c.V1 = c.V1 & c.V0;
        if (c.A0 == 0u) {
            mem.WriteU32((c.S2 + 0x8u), c.V1);
            goto L800B41AC;
        }
        mem.WriteU32((c.S2 + 0x8u), c.V1);
        c.V0 = 0x800C0000u;
        c.S3 = c.V0 + 0x7810u;
        c.T3 = 0u;
        c.S0 = c.S2 + 0x20u;
        L800B3FF4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T9 = mem.ReadU16((c.S0 - 0x8u));
        c.A0 = mem.ReadU32(c.S0);
        if (c.T9 == 0u) {
            goto L800B4194;
        }
        L800B4004: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T5 = 0u;
        c.V0 = mem.ReadU16(c.A0);
        c.T8 = mem.ReadU16((c.A0 + 0x2u));
        c.V1 = c.V0 & 0xFFFFu;
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.A0 + 0x4u;
            goto L800B40C4;
        }
        c.A0 = c.A0 + 0x4u;
        c.V0 = c.V1 << 2;
        c.V0 = c.V0 + c.S3;
        c.V0 = mem.ReadU32(c.V0);
        switch (c.V0)
        {
            case 0x800B4038u: goto L800B4038;
            case 0x800B4078u: goto L800B4078;
            case 0x800B4040u: goto L800B4040;
            case 0x800B405Cu: goto L800B405C;
            case 0x800B4080u: goto L800B4080;
            case 0x800B4088u: goto L800B4088;
            case 0x800B40A0u: goto L800B40A0;
            case 0x800B40C4u: goto L800B40C4;
            case 0x800B4054u: goto L800B4054;
            case 0x800B40C0u: goto L800B40C0;
            case 0x800B4970u: goto L800B4970;
            case 0x800B49C4u: goto L800B49C4;
            case 0x800B4A18u: goto L800B4A18;
            case 0x800B4A6Cu: goto L800B4A6C;
            case 0x800B4AC0u: goto L800B4AC0;
            case 0x800B4B14u: goto L800B4B14;
            case 0x800B4B68u: goto L800B4B68;
            case 0x800B4BBCu: goto L800B4BBC;
            case 0x800B4CB8u: goto L800B4CB8;
            case 0x800B4C10u: goto L800B4C10;
            case 0x800B4C64u: goto L800B4C64;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B4038: ;
        c.A2 = 0x0000000Cu;
        goto L800B40C4;
        L800B4040: ;
        c.T5 = 0x00000003u;
        c.T6 = c.A0 + 0x12u;
        c.T1 = c.A0 + 0x8u;
        c.T0 = c.A0 + 0xCu;
        c.A3 = c.A0 + 0x10u;
        L800B4054: ;
        c.A2 = 0x00000018u;
        goto L800B40C4;
        L800B405C: ;
        c.T5 = 0x00000004u;
        c.T6 = c.A0 + 0xAu;
        c.T1 = c.A0 + 0xCu;
        c.T0 = c.A0 + 0x10u;
        c.A3 = c.A0 + 0x14u;
        c.T4 = c.A0 + 0x16u;
        goto L800B40C0;
        L800B4078: ;
        c.A2 = 0x00000010u;
        goto L800B40C4;
        L800B4080: ;
        c.A2 = 0x00000014u;
        goto L800B40C4;
        L800B4088: ;
        c.T5 = 0x00000003u;
        c.T6 = c.A0 + 0x16u;
        c.T1 = c.A0 + 0xCu;
        c.T0 = c.A0 + 0x10u;
        c.A3 = c.A0 + 0x14u;
        goto L800B40C0;
        L800B40A0: ;
        c.T5 = 0x00000004u;
        c.T6 = c.A0 + 0x1Cu;
        c.T1 = c.A0 + 0x10u;
        c.T0 = c.A0 + 0x14u;
        c.A3 = c.A0 + 0x18u;
        c.T4 = c.A0 + 0x1Au;
        c.A2 = 0x00000024u;
        goto L800B40C4;
        L800B40C0: ;
        c.A2 = 0x0000001Cu;
        L800B40C4: ;
        if ((int)c.T5 <= 0) {
            c.V0 = c.T8 & 0xFFFFu;
            goto L800B4170;
        }
        c.V0 = c.T8 & 0xFFFFu;
        c.T2 = c.T8 & 0xFFFFu;
        if ((int)c.T2 < 0) {
            c.V0 = c.A2 << 1;
            goto L800B416C;
        }
        c.V0 = c.A2 << 1;
        c.T7 = c.V0 - 0x2u;
        c.A1 = c.A2 - 0x1u;
        L800B40E0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.T6);
        c.V1 = mem.ReadU32((c.GP + 0x7C0u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = mem.ReadU32(c.V0);
        c.T6 = c.T6 + c.T7;
        mem.WriteU8(c.T1, (byte)0u);
        mem.WriteU8(c.T1, (byte)0u);
        c.T1 = c.T1 + 0x1u;
        mem.WriteU8(c.T1, (byte)0u);
        mem.WriteU8(c.T1, (byte)0u);
        c.T1 = c.T1 + c.A1;
        mem.WriteU8(c.T0, (byte)0u);
        mem.WriteU8(c.T0, (byte)0u);
        c.T0 = c.T0 + 0x1u;
        mem.WriteU8(c.T0, (byte)0u);
        mem.WriteU8(c.T0, (byte)0u);
        c.T0 = c.T0 + c.A1;
        mem.WriteU8(c.A3, (byte)0u);
        mem.WriteU8(c.A3, (byte)0u);
        c.A3 = c.A3 + 0x1u;
        mem.WriteU8(c.A3, (byte)0u);
        mem.WriteU8(c.A3, (byte)0u);
        c.V0 = 0x00000004u;
        if (c.T5 != c.V0) {
            c.A3 = c.A3 + c.A1;
            goto L800B4160;
        }
        c.A3 = c.A3 + c.A1;
        mem.WriteU8(c.T4, (byte)0u);
        mem.WriteU8(c.T4, (byte)0u);
        c.T4 = c.T4 + 0x1u;
        mem.WriteU8(c.T4, (byte)0u);
        mem.WriteU8(c.T4, (byte)0u);
        c.T4 = c.T4 + c.A1;
        L800B4160: ;
        c.T2 = c.T2 - 0x1u;
        if ((int)c.T2 >= 0) {
            goto L800B40E0;
        }
        L800B416C: ;
        c.V0 = c.T8 & 0xFFFFu;
        L800B4170: ;
        { var _r = (long)(int)c.A2 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = c.T9 - c.T8;
        c.S4 = c.LO;
        c.V0 = c.S4 >> 2;
        c.V0 = c.V0 << 2;
        c.A0 = c.A0 + c.V0;
        c.V0 = c.T9 & 0xFFFFu;
        if (c.V0 != 0u) {
            goto L800B4004;
        }
        L800B4194: ;
        c.S1 = c.S1 + 0x1u;
        c.V1 = mem.ReadU32((c.S2 + 0xCu));
        c.V0 = c.S1 & 0xFFFFu;
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S0 = c.S0 + 0x2Cu;
            goto L800B3FF4;
        }
        c.S0 = c.S0 + 0x2Cu;
        L800B41AC: ;
        c.S4 = mem.ReadU32((c.SP + 0x30u));
        c.S3 = mem.ReadU32((c.SP + 0x2Cu));
        c.S2 = mem.ReadU32((c.SP + 0x28u));
        c.S1 = mem.ReadU32((c.SP + 0x24u));
        c.S0 = mem.ReadU32((c.SP + 0x20u));
        c.SP = c.SP + 0x38u;
        return;
        c.SP = c.SP - 0xB8u;
        mem.WriteU32((c.SP + 0xA4u), c.S5);
        c.S5 = c.A0;
        c.V1 = 0x800F0000u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 - 0x2204u;
        mem.WriteU32((c.SP + 0xB4u), c.RA);
        mem.WriteU32((c.SP + 0xB0u), c.FP);
        mem.WriteU32((c.SP + 0xACu), c.S7);
        mem.WriteU32((c.SP + 0xA8u), c.S6);
        mem.WriteU32((c.SP + 0xA0u), c.S4);
        mem.WriteU32((c.SP + 0x9Cu), c.S3);
        mem.WriteU32((c.SP + 0x98u), c.S2);
        mem.WriteU32((c.SP + 0x94u), c.S1);
        mem.WriteU32((c.SP + 0x90u), c.S0);
        mem.WriteU8((c.SP + 0x84u), (byte)c.A2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x10u));
        c.V0 = mem.ReadU32(c.V0);
        c.S7 = c.A1;
        c.V1 = c.V1 < c.V0 ? 1u : 0u;
        if (c.V1 == 0u) {
            mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
            goto L800B4D54;
        }
        mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
        c.S2 = mem.ReadU32((c.S5 + 0x8u));
        c.S4 = 0u;
        c.FP = mem.ReadU32((c.S2 + 0x2Cu));
        c.V1 = mem.ReadU16((c.S2 + 0xAu));
        c.T1 = mem.ReadU32((c.FP + 0x4u));
        c.V0 = c.V1 & 0x0020u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x88u), c.T1);
            goto L800B4260;
        }
        mem.WriteU32((c.SP + 0x88u), c.T1);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x90u));
        c.S4 = c.S4 < c.V0 ? 1u : 0u;
        L800B4260: ;
        c.V0 = mem.ReadU16((c.S5 + 0xEu));
        c.V0 = c.V0 & 0x0040u;
        if (c.V0 == 0u) {
            c.S1 = c.S7 + 0xA0u;
            goto L800B4278;
        }
        c.S1 = c.S7 + 0xA0u;
        c.S1 = mem.ReadU32((c.S5 + 0x18u));
        L800B4278: ;
        c.V0 = c.V1 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800B4290;
        }
        c.A1 = mem.ReadU32((c.S2 + 0xCu));
        goto L800B429C;
        L800B4290: ;
        c.V0 = mem.ReadU32((c.S2 + 0xCu));
        c.A1 = c.V0 + 0x60u;
        L800B429C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x38u));
        if (c.V0 == c.A1) {
            goto L800B42C8;
        }
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A1);
        c.A0 = c.S1;
        c.RA = 0x800B42C8u;
        MediEvil_game.func_800A4E3C(c, m);
        L800B42C8: ;
        c.A1 = c.S2 + 0x10u;
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.A0 + 0x38u));
        c.A2 = c.SP + 0x70u;
        c.RA = 0x800B42DCu;
        MediEvil_game.func_800A4880(c, m);
        c.A0 = c.S1;
        c.A1 = c.SP + 0x68u;
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU16((c.SP + 0x70u));
        c.A3 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.T0 + 0x14u));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x18u));
        c.V1 = mem.ReadU16((c.SP + 0x74u));
        c.A3 = mem.ReadU16((c.S1 + 0x18u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x1Cu));
        c.V1 = mem.ReadU16((c.SP + 0x78u));
        c.A3 = mem.ReadU16((c.S1 + 0x1Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B4344u;
        MediEvil_game.func_800A4880(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = mem.ReadU32((c.T2 + 0x34u));
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T2 + 0x14u));
        c.T5 = mem.ReadU32((c.T2 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T2 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T3 = 0x800F0000u;
        c.T3 = c.T3 - 0x263Cu;
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.A1 = c.SP + 0x80u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw);  }
        c.V0 = TerrainPatch.MeshClipDistance(mem.ReadU16((c.FP + 0x8u)), m);
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4D54;
        }
        c.V1 = mem.ReadU8((c.SP + 0x84u));
        c.T1 = mem.ReadU32((c.SP + 0x88u));
        c.A2 = mem.ReadU32(c.FP);
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V1 = mem.ReadU32((c.T1 + 0x4u));
        c.V0 = c.V0 + 0x10u;
        c.S3 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.V1 = c.A2 & 0x0008u;
        c.A0 = mem.ReadU32((c.V0 + 0x8u));
        if (c.V1 != 0u) {
            goto L800B446C;
        }
        if (c.A0 == 0u) {
            c.V0 = c.A2 & 0x0010u;
            goto L800B446C;
        }
        c.V0 = c.A2 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B4438;
        }
        c.RA = 0x800B4430u;
        MediEvil_game.func_8009A61C(c, m);
        goto L800B4440;
        L800B4438: ;
        c.RA = 0x800B4440u;
        MediEvil_game.func_8009A4C8(c, m);
        L800B4440: ;
        if (c.V0 == 0u) {
            goto L800B4D54;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            goto L800B4D54;
        }
        L800B446C: ;
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        if (c.V1 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4784;
        }
        c.V0 = 0x00000002u;
        c.V0 = mem.ReadU32((c.V1 + 0x8u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 != 0u) {
            c.V0 = c.V1 + 0x18u;
            goto L800B45D0;
        }
        c.V0 = c.V1 + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        c.A1 = mem.ReadU32((c.V1 + 0x14u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        if (c.A1 == c.A0) {
            goto L800B45A0;
        }
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            c.V0 = c.A1 + 0x60u;
            goto L800B44D0;
        }
        c.V0 = c.A1 + 0x60u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A0);
        c.A0 = c.S1;
        goto L800B44DC;
        L800B44D0: ;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.V0);
        c.A0 = c.S1;
        L800B44DC: ;
        c.S0 = 0x1F800000u;
        c.S0 = c.S0 + 0x60u;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.S0;
        c.RA = 0x800B44F4u;
        MediEvil_game.func_800A4E3C(c, m);
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x38u));
        c.V1 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.A2 + 0x14u));
        c.A0 = c.S1;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x18u));
        c.V1 = mem.ReadU16((c.A0 + 0x18u));
        c.A1 = c.SP + 0x68u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.A0 + 0x1Cu));
        c.A2 = c.S0 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B453Cu;
        MediEvil_game.func_800A4880(c, m);
        c.T4 = mem.ReadU32(c.S0);
        c.T5 = mem.ReadU32((c.S0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S0 + 0x8u));
        c.T5 = mem.ReadU32((c.S0 + 0xCu));
        c.T6 = mem.ReadU32((c.S0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S0 + 0x14u));
        c.T5 = mem.ReadU32((c.S0 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S0 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        goto L800B45B4;
        L800B45A0: ;
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = c.V1 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        L800B45B4: ;
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        c.A0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 + c.A0;
        mem.WriteU32((c.V0 + 0x10u), c.V1);
        L800B45D0: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU16((c.V0 + 0xEu));
        mem.WriteU16((c.SP + 0x44u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU16((c.V0 + 0xCu));
        c.V0 = 0x00000001u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        mem.WriteU32((c.SP + 0x48u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x50u), c.V1);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x8u));
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B465C;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A1 = c.V0 - 0x1u;
        goto L800B469C;
        L800B465C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = (uint)((int)c.V1 >> (int)(c.V0 & 31u));
        c.A1 = c.V0;
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4D54;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B4D54;
        }
        L800B469C: ;
        c.V0 = mem.ReadU32((c.SP + 0x50u));
        c.A0 = mem.ReadU16((c.SP + 0x44u));
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V1 = c.V1 >> 1;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.SP + 0x46u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.A2 = (uint)((int)c.V0 >> 16);
        c.V0 = 0u - c.A2;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.V0 = c.V0 << (int)(c.A0 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.V0 < 3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B46F8;
        }
        c.V0 = 0x00000002u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V0 = c.V0 + c.A2;
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        goto L800B46FC;
        L800B46F8: ;
        mem.WriteU32((c.SP + 0x4Cu), 0u);
        L800B46FC: ;
        c.A0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.A0 + 0x8u));
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            c.A3 = 0x00FF0000u;
            goto L800B47B4;
        }
        c.A3 = 0x00FF0000u;
        c.A3 = c.A3 | 0xFFFFu;
        c.V0 = c.V1 | 0x0001u;
        c.A1 = c.A1 << 16;
        c.A1 = (uint)((int)c.A1 >> 14);
        c.A2 = 0xFF000000u;
        mem.WriteU32((c.A0 + 0x8u), c.V0);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.A0 = mem.ReadU32((c.SP + 0x40u));
        c.A1 = c.A1 + c.V0;
        c.V1 = mem.ReadU32(c.A0);
        c.V0 = mem.ReadU32(c.A1);
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A3;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A0, c.V1);
        c.A0 = mem.ReadU32(c.A1);
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = mem.ReadU32((c.SP + 0x40u));
        c.A0 = c.A0 & c.A2;
        c.V1 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - 0x4u;
        c.V0 = c.V0 & c.A3;
        c.A0 = c.A0 | c.V0;
        mem.WriteU32(c.A1, c.A0);
        goto L800B47B4;
        L800B4784: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x9Cu));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU16((c.A1 + 0x8Eu));
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        mem.WriteU32((c.SP + 0x50u), 0u);
        mem.WriteU16((c.SP + 0x46u), (ushort)0u);
        mem.WriteU32((c.SP + 0x40u), c.V1);
        mem.WriteU16((c.SP + 0x44u), (ushort)c.A0);
        mem.WriteU32((c.SP + 0x48u), c.A1);
        L800B47B4: ;
        c.V0 = mem.ReadU16((c.S5 + 0x1Eu));
        c.V0 = c.V0 & 0x0003u;
        if (c.V0 != 0u) {
            c.A0 = c.S2;
            goto L800B4800;
        }
        c.A0 = c.S2;
        c.V0 = mem.ReadU32((c.S2 + 0x8u));
        c.V1 = 0x00030000u;
        c.V0 = c.V0 & c.V1;
        if (c.V0 != c.V1) {
            c.A2 = c.S5 + 0x10u;
            goto L800B4804;
        }
        c.A2 = c.S5 + 0x10u;
        c.V0 = mem.ReadU16((c.S7 + 0x11Eu));
        if (c.V0 == 0u) {
            c.A0 = c.S7 + 0x12Cu;
            goto L800B4868;
        }
        c.A0 = c.S7 + 0x12Cu;
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            c.A0 = c.S2;
            goto L800B4864;
        }
        c.A0 = c.S2;
        L800B4800: ;
        c.A2 = c.S5 + 0x10u;
        L800B4804: ;
        c.A1 = mem.ReadU16((c.S5 + 0x1Eu));
        c.A3 = c.S5 + 0x14u;
        c.RA = 0x800B4810u;
        MediEvil_game.func_800AB9BC(c, m);
        c.S6 = c.V0;
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x3Cu;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.A0;
        c.RA = 0x800B482Cu;
        MediEvil_game.func_800A4E3C(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = c.T2 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        goto L800B48B0;
        L800B4864: ;
        c.A0 = c.S7 + 0x12Cu;
        L800B4868: ;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = 0x1F800000u;
        c.A2 = c.A2 + 0x3Cu;
        c.RA = 0x800B487Cu;
        MediEvil_game.func_800A4E3C(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = c.T3 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        c.S6 = 0u;
        L800B48B0: ;
        c.T1 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.T1 + 0x34u));
        c.T4 = mem.ReadU32(c.T1);
        c.T5 = mem.ReadU32((c.T1 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T1 + 0x8u));
        c.T5 = mem.ReadU32((c.T1 + 0xCu));
        c.T6 = mem.ReadU32((c.T1 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T1 + 0x14u));
        c.T5 = mem.ReadU32((c.T1 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T1 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.A2 = mem.ReadU32((c.S3 + 0x10u));
        c.A0 = mem.ReadU16((c.S3 + 0x8u));
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.S3 = mem.ReadU32(c.V0);
        c.V1 = mem.ReadU16((c.S5 + 0xEu));
        c.S2 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 | 0x0001u;
        c.S1 = c.V1 & 0x003Eu;
        if (c.A0 == 0u) {
            mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
            goto L800B4CC4;
        }
        mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
        c.S0 = c.SP + 0x20u;
        L800B4934: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A2 = c.A2 + 0x4u;
            goto L800B4CB8;
        }
        c.A2 = c.A2 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + 0x7850u;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800B4970u: goto L800B4970;
            case 0x800B49C4u: goto L800B49C4;
            case 0x800B4A18u: goto L800B4A18;
            case 0x800B4A6Cu: goto L800B4A6C;
            case 0x800B4AC0u: goto L800B4AC0;
            case 0x800B4B14u: goto L800B4B14;
            case 0x800B4B68u: goto L800B4B68;
            case 0x800B4BBCu: goto L800B4BBC;
            case 0x800B4CB8u: goto L800B4CB8;
            case 0x800B4C10u: goto L800B4C10;
            case 0x800B4C64u: goto L800B4C64;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B4970: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B499C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4994u;
        MediEvil_game.func_800B0900(c, m);
        goto L800B4CB0;
        L800B499C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B49BCu;
        MediEvil_game.func_800B76BC(c, m);
        goto L800B4CB0;
        L800B49C4: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B49F0;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B49E8u;
        MediEvil_game.func_800B0AC0(c, m);
        goto L800B4CB0;
        L800B49F0: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A10u;
        MediEvil_game.func_800B7A04(c, m);
        goto L800B4CB0;
        L800B4A18: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A44;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A3Cu;
        MediEvil_game.func_800B0068(c, m);
        goto L800B4CB0;
        L800B4A44: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A64u;
        MediEvil_game.func_800B59C8(c, m);
        goto L800B4CB0;
        L800B4A6C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A98;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A90u;
        MediEvil_game.func_800B0240(c, m);
        goto L800B4CB0;
        L800B4A98: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4AB8u;
        MediEvil_game.func_800B5DA4(c, m);
        goto L800B4CB0;
        L800B4AC0: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4AEC;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4AE4u;
        MediEvil_game.func_800B0CB8(c, m);
        goto L800B4CB0;
        L800B4AEC: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B0Cu;
        MediEvil_game.func_800B7DBC(c, m);
        goto L800B4CB0;
        L800B4B14: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B40;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B38u;
        MediEvil_game.func_800B0EA4(c, m);
        goto L800B4CB0;
        L800B4B40: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B60u;
        MediEvil_game.func_800B8288(c, m);
        goto L800B4CB0;
        L800B4B68: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B94;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B8Cu;
        MediEvil_game.func_800B0458(c, m);
        goto L800B4CB0;
        L800B4B94: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4BB4u;
        MediEvil_game.func_800B6238(c, m);
        goto L800B4CB0;
        L800B4BBC: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4BE8;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4BE0u;
        MediEvil_game.func_800B065C(c, m);
        goto L800B4CB0;
        L800B4BE8: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C08u;
        MediEvil_game.func_800B6724(c, m);
        goto L800B4CB0;
        L800B4C10: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C3C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C34u;
        MediEvil_game.func_800B6E70(c, m);
        goto L800B4CB0;
        L800B4C3C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C5Cu;
        MediEvil_game.func_800B89A8(c, m);
        goto L800B4CB0;
        L800B4C64: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C90;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C88u;
        MediEvil_game.func_800B7224(c, m);
        goto L800B4CB0;
        L800B4C90: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4CB0u;
        MediEvil_game.func_800B906C(c, m);
        L800B4CB0: ;
        c.A2 = mem.ReadU32((c.SP + 0x60u));
        c.A0 = mem.ReadU32((c.SP + 0x64u));
        L800B4CB8: ;
        if (c.A0 != 0u) {
            goto L800B4934;
        }
        L800B4CC4: ;
        c.V0 = c.S6 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.S7 + 0x14Cu;
            goto L800B4CF8;
        }
        c.V0 = c.S7 + 0x14Cu;
        c.T4 = mem.ReadU32(c.V0);
        c.T5 = mem.ReadU32((c.V0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(16, c.T4);
        RecompOne.Runtime.Gte.WriteControl(17, c.T5);
        c.T4 = mem.ReadU32((c.V0 + 0x8u));
        c.T5 = mem.ReadU32((c.V0 + 0xCu));
        c.T6 = mem.ReadU32((c.V0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(18, c.T4);
        RecompOne.Runtime.Gte.WriteControl(19, c.T5);
        RecompOne.Runtime.Gte.WriteControl(20, c.T6);
        L800B4CF8: ;
        c.V0 = c.S6 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B4D28;
        }
        c.T2 = mem.ReadU8((c.S7 + 0x16Cu));
        c.T3 = mem.ReadU8((c.S7 + 0x16Du));
        c.T1 = mem.ReadU8((c.S7 + 0x16Eu));
        c.T4 = c.T2 << 4;
        c.T5 = c.T3 << 4;
        c.T6 = c.T1 << 4;
        RecompOne.Runtime.Gte.WriteControl(13, c.T4);
        RecompOne.Runtime.Gte.WriteControl(14, c.T5);
        RecompOne.Runtime.Gte.WriteControl(15, c.T6);
        L800B4D28: ;
        c.V0 = mem.ReadU32(c.FP);
        c.V0 = c.V0 & 0x2000u;
        if (c.V0 == 0u) {
            c.A2 = c.S5;
            goto L800B4D54;
        }
        c.A2 = c.S5;
        c.A3 = 0x00FF0000u;
        c.T2 = mem.ReadU32((c.SP + 0x88u));
        c.A1 = mem.ReadU8((c.SP + 0x84u));
        c.A0 = mem.ReadU32((c.T2 + 0x4u));
        c.A3 = c.A3 | 0xFFFFu;
        c.RA = 0x800B4D54u;
        MediEvil_game.func_800AF414(c, m);
        L800B4D54: ;
        c.RA = mem.ReadU32((c.SP + 0xB4u));
        c.FP = mem.ReadU32((c.SP + 0xB0u));
        c.S7 = mem.ReadU32((c.SP + 0xACu));
        c.S6 = mem.ReadU32((c.SP + 0xA8u));
        c.S5 = mem.ReadU32((c.SP + 0xA4u));
        c.S4 = mem.ReadU32((c.SP + 0xA0u));
        c.S3 = mem.ReadU32((c.SP + 0x9Cu));
        c.S2 = mem.ReadU32((c.SP + 0x98u));
        c.S1 = mem.ReadU32((c.SP + 0x94u));
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        c.SP = c.SP + 0xB8u;
        return;
    }

    public static void func_800B3FAC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x38u;
        mem.WriteU32((c.SP + 0x28u), c.S2);
        c.S2 = c.A0;
        mem.WriteU32((c.SP + 0x24u), c.S1);
        c.S1 = 0u;
        c.V0 = 0xFFFFFFFBu;
        mem.WriteU32((c.SP + 0x30u), c.S4);
        mem.WriteU32((c.SP + 0x2Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S0);
        c.V1 = mem.ReadU32((c.S2 + 0x8u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        c.V1 = c.V1 & c.V0;
        if (c.A0 == 0u) {
            mem.WriteU32((c.S2 + 0x8u), c.V1);
            goto L800B41AC;
        }
        mem.WriteU32((c.S2 + 0x8u), c.V1);
        c.V0 = 0x800C0000u;
        c.S3 = c.V0 + 0x7810u;
        c.T3 = 0u;
        c.S0 = c.S2 + 0x20u;
        L800B3FF4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T9 = mem.ReadU16((c.S0 - 0x8u));
        c.A0 = mem.ReadU32(c.S0);
        if (c.T9 == 0u) {
            goto L800B4194;
        }
        L800B4004: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T5 = 0u;
        c.V0 = mem.ReadU16(c.A0);
        c.T8 = mem.ReadU16((c.A0 + 0x2u));
        c.V1 = c.V0 & 0xFFFFu;
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.A0 + 0x4u;
            goto L800B40C4;
        }
        c.A0 = c.A0 + 0x4u;
        c.V0 = c.V1 << 2;
        c.V0 = c.V0 + c.S3;
        c.V0 = mem.ReadU32(c.V0);
        switch (c.V0)
        {
            case 0x800B4038u: goto L800B4038;
            case 0x800B4078u: goto L800B4078;
            case 0x800B4040u: goto L800B4040;
            case 0x800B405Cu: goto L800B405C;
            case 0x800B4080u: goto L800B4080;
            case 0x800B4088u: goto L800B4088;
            case 0x800B40A0u: goto L800B40A0;
            case 0x800B40C4u: goto L800B40C4;
            case 0x800B4054u: goto L800B4054;
            case 0x800B40C0u: goto L800B40C0;
            case 0x800B4970u: goto L800B4970;
            case 0x800B49C4u: goto L800B49C4;
            case 0x800B4A18u: goto L800B4A18;
            case 0x800B4A6Cu: goto L800B4A6C;
            case 0x800B4AC0u: goto L800B4AC0;
            case 0x800B4B14u: goto L800B4B14;
            case 0x800B4B68u: goto L800B4B68;
            case 0x800B4BBCu: goto L800B4BBC;
            case 0x800B4CB8u: goto L800B4CB8;
            case 0x800B4C10u: goto L800B4C10;
            case 0x800B4C64u: goto L800B4C64;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B4038: ;
        c.A2 = 0x0000000Cu;
        goto L800B40C4;
        L800B4040: ;
        c.T5 = 0x00000003u;
        c.T6 = c.A0 + 0x12u;
        c.T1 = c.A0 + 0x8u;
        c.T0 = c.A0 + 0xCu;
        c.A3 = c.A0 + 0x10u;
        L800B4054: ;
        c.A2 = 0x00000018u;
        goto L800B40C4;
        L800B405C: ;
        c.T5 = 0x00000004u;
        c.T6 = c.A0 + 0xAu;
        c.T1 = c.A0 + 0xCu;
        c.T0 = c.A0 + 0x10u;
        c.A3 = c.A0 + 0x14u;
        c.T4 = c.A0 + 0x16u;
        goto L800B40C0;
        L800B4078: ;
        c.A2 = 0x00000010u;
        goto L800B40C4;
        L800B4080: ;
        c.A2 = 0x00000014u;
        goto L800B40C4;
        L800B4088: ;
        c.T5 = 0x00000003u;
        c.T6 = c.A0 + 0x16u;
        c.T1 = c.A0 + 0xCu;
        c.T0 = c.A0 + 0x10u;
        c.A3 = c.A0 + 0x14u;
        goto L800B40C0;
        L800B40A0: ;
        c.T5 = 0x00000004u;
        c.T6 = c.A0 + 0x1Cu;
        c.T1 = c.A0 + 0x10u;
        c.T0 = c.A0 + 0x14u;
        c.A3 = c.A0 + 0x18u;
        c.T4 = c.A0 + 0x1Au;
        c.A2 = 0x00000024u;
        goto L800B40C4;
        L800B40C0: ;
        c.A2 = 0x0000001Cu;
        L800B40C4: ;
        if ((int)c.T5 <= 0) {
            c.V0 = c.T8 & 0xFFFFu;
            goto L800B4170;
        }
        c.V0 = c.T8 & 0xFFFFu;
        c.T2 = c.T8 & 0xFFFFu;
        if ((int)c.T2 < 0) {
            c.V0 = c.A2 << 1;
            goto L800B416C;
        }
        c.V0 = c.A2 << 1;
        c.T7 = c.V0 - 0x2u;
        c.A1 = c.A2 - 0x1u;
        L800B40E0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.T6);
        c.V1 = mem.ReadU32((c.GP + 0x7C0u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = mem.ReadU32(c.V0);
        c.T6 = c.T6 + c.T7;
        mem.WriteU8(c.T1, (byte)0u);
        mem.WriteU8(c.T1, (byte)0u);
        c.T1 = c.T1 + 0x1u;
        mem.WriteU8(c.T1, (byte)0u);
        mem.WriteU8(c.T1, (byte)0u);
        c.T1 = c.T1 + c.A1;
        mem.WriteU8(c.T0, (byte)0u);
        mem.WriteU8(c.T0, (byte)0u);
        c.T0 = c.T0 + 0x1u;
        mem.WriteU8(c.T0, (byte)0u);
        mem.WriteU8(c.T0, (byte)0u);
        c.T0 = c.T0 + c.A1;
        mem.WriteU8(c.A3, (byte)0u);
        mem.WriteU8(c.A3, (byte)0u);
        c.A3 = c.A3 + 0x1u;
        mem.WriteU8(c.A3, (byte)0u);
        mem.WriteU8(c.A3, (byte)0u);
        c.V0 = 0x00000004u;
        if (c.T5 != c.V0) {
            c.A3 = c.A3 + c.A1;
            goto L800B4160;
        }
        c.A3 = c.A3 + c.A1;
        mem.WriteU8(c.T4, (byte)0u);
        mem.WriteU8(c.T4, (byte)0u);
        c.T4 = c.T4 + 0x1u;
        mem.WriteU8(c.T4, (byte)0u);
        mem.WriteU8(c.T4, (byte)0u);
        c.T4 = c.T4 + c.A1;
        L800B4160: ;
        c.T2 = c.T2 - 0x1u;
        if ((int)c.T2 >= 0) {
            goto L800B40E0;
        }
        L800B416C: ;
        c.V0 = c.T8 & 0xFFFFu;
        L800B4170: ;
        { var _r = (long)(int)c.A2 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = c.T9 - c.T8;
        c.S4 = c.LO;
        c.V0 = c.S4 >> 2;
        c.V0 = c.V0 << 2;
        c.A0 = c.A0 + c.V0;
        c.V0 = c.T9 & 0xFFFFu;
        if (c.V0 != 0u) {
            goto L800B4004;
        }
        L800B4194: ;
        c.S1 = c.S1 + 0x1u;
        c.V1 = mem.ReadU32((c.S2 + 0xCu));
        c.V0 = c.S1 & 0xFFFFu;
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S0 = c.S0 + 0x2Cu;
            goto L800B3FF4;
        }
        c.S0 = c.S0 + 0x2Cu;
        L800B41AC: ;
        c.S4 = mem.ReadU32((c.SP + 0x30u));
        c.S3 = mem.ReadU32((c.SP + 0x2Cu));
        c.S2 = mem.ReadU32((c.SP + 0x28u));
        c.S1 = mem.ReadU32((c.SP + 0x24u));
        c.S0 = mem.ReadU32((c.SP + 0x20u));
        c.SP = c.SP + 0x38u;
        return;
        c.SP = c.SP - 0xB8u;
        mem.WriteU32((c.SP + 0xA4u), c.S5);
        c.S5 = c.A0;
        c.V1 = 0x800F0000u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 - 0x2204u;
        mem.WriteU32((c.SP + 0xB4u), c.RA);
        mem.WriteU32((c.SP + 0xB0u), c.FP);
        mem.WriteU32((c.SP + 0xACu), c.S7);
        mem.WriteU32((c.SP + 0xA8u), c.S6);
        mem.WriteU32((c.SP + 0xA0u), c.S4);
        mem.WriteU32((c.SP + 0x9Cu), c.S3);
        mem.WriteU32((c.SP + 0x98u), c.S2);
        mem.WriteU32((c.SP + 0x94u), c.S1);
        mem.WriteU32((c.SP + 0x90u), c.S0);
        mem.WriteU8((c.SP + 0x84u), (byte)c.A2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x10u));
        c.V0 = mem.ReadU32(c.V0);
        c.S7 = c.A1;
        c.V1 = c.V1 < c.V0 ? 1u : 0u;
        if (c.V1 == 0u) {
            mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
            goto L800B4D54;
        }
        mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
        c.S2 = mem.ReadU32((c.S5 + 0x8u));
        c.S4 = 0u;
        c.FP = mem.ReadU32((c.S2 + 0x2Cu));
        c.V1 = mem.ReadU16((c.S2 + 0xAu));
        c.T1 = mem.ReadU32((c.FP + 0x4u));
        c.V0 = c.V1 & 0x0020u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x88u), c.T1);
            goto L800B4260;
        }
        mem.WriteU32((c.SP + 0x88u), c.T1);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x90u));
        c.S4 = c.S4 < c.V0 ? 1u : 0u;
        L800B4260: ;
        c.V0 = mem.ReadU16((c.S5 + 0xEu));
        c.V0 = c.V0 & 0x0040u;
        if (c.V0 == 0u) {
            c.S1 = c.S7 + 0xA0u;
            goto L800B4278;
        }
        c.S1 = c.S7 + 0xA0u;
        c.S1 = mem.ReadU32((c.S5 + 0x18u));
        L800B4278: ;
        c.V0 = c.V1 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800B4290;
        }
        c.A1 = mem.ReadU32((c.S2 + 0xCu));
        goto L800B429C;
        L800B4290: ;
        c.V0 = mem.ReadU32((c.S2 + 0xCu));
        c.A1 = c.V0 + 0x60u;
        L800B429C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x38u));
        if (c.V0 == c.A1) {
            goto L800B42C8;
        }
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A1);
        c.A0 = c.S1;
        c.RA = 0x800B42C8u;
        MediEvil_game.func_800A4E3C(c, m);
        L800B42C8: ;
        c.A1 = c.S2 + 0x10u;
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.A0 + 0x38u));
        c.A2 = c.SP + 0x70u;
        c.RA = 0x800B42DCu;
        MediEvil_game.func_800A4880(c, m);
        c.A0 = c.S1;
        c.A1 = c.SP + 0x68u;
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU16((c.SP + 0x70u));
        c.A3 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.T0 + 0x14u));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x18u));
        c.V1 = mem.ReadU16((c.SP + 0x74u));
        c.A3 = mem.ReadU16((c.S1 + 0x18u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x1Cu));
        c.V1 = mem.ReadU16((c.SP + 0x78u));
        c.A3 = mem.ReadU16((c.S1 + 0x1Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B4344u;
        MediEvil_game.func_800A4880(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = mem.ReadU32((c.T2 + 0x34u));
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T2 + 0x14u));
        c.T5 = mem.ReadU32((c.T2 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T2 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T3 = 0x800F0000u;
        c.T3 = c.T3 - 0x263Cu;
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.A1 = c.SP + 0x80u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw);  }
        c.V0 = TerrainPatch.MeshClipDistance(mem.ReadU16((c.FP + 0x8u)), m);
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4D54;
        }
        c.V1 = mem.ReadU8((c.SP + 0x84u));
        c.T1 = mem.ReadU32((c.SP + 0x88u));
        c.A2 = mem.ReadU32(c.FP);
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V1 = mem.ReadU32((c.T1 + 0x4u));
        c.V0 = c.V0 + 0x10u;
        c.S3 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.V1 = c.A2 & 0x0008u;
        c.A0 = mem.ReadU32((c.V0 + 0x8u));
        if (c.V1 != 0u) {
            goto L800B446C;
        }
        if (c.A0 == 0u) {
            c.V0 = c.A2 & 0x0010u;
            goto L800B446C;
        }
        c.V0 = c.A2 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B4438;
        }
        c.RA = 0x800B4430u;
        MediEvil_game.func_8009A61C(c, m);
        goto L800B4440;
        L800B4438: ;
        c.RA = 0x800B4440u;
        MediEvil_game.func_8009A4C8(c, m);
        L800B4440: ;
        if (c.V0 == 0u) {
            goto L800B4D54;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            goto L800B4D54;
        }
        L800B446C: ;
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        if (c.V1 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4784;
        }
        c.V0 = 0x00000002u;
        c.V0 = mem.ReadU32((c.V1 + 0x8u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 != 0u) {
            c.V0 = c.V1 + 0x18u;
            goto L800B45D0;
        }
        c.V0 = c.V1 + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        c.A1 = mem.ReadU32((c.V1 + 0x14u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        if (c.A1 == c.A0) {
            goto L800B45A0;
        }
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            c.V0 = c.A1 + 0x60u;
            goto L800B44D0;
        }
        c.V0 = c.A1 + 0x60u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A0);
        c.A0 = c.S1;
        goto L800B44DC;
        L800B44D0: ;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.V0);
        c.A0 = c.S1;
        L800B44DC: ;
        c.S0 = 0x1F800000u;
        c.S0 = c.S0 + 0x60u;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.S0;
        c.RA = 0x800B44F4u;
        MediEvil_game.func_800A4E3C(c, m);
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x38u));
        c.V1 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.A2 + 0x14u));
        c.A0 = c.S1;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x18u));
        c.V1 = mem.ReadU16((c.A0 + 0x18u));
        c.A1 = c.SP + 0x68u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.A0 + 0x1Cu));
        c.A2 = c.S0 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B453Cu;
        MediEvil_game.func_800A4880(c, m);
        c.T4 = mem.ReadU32(c.S0);
        c.T5 = mem.ReadU32((c.S0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S0 + 0x8u));
        c.T5 = mem.ReadU32((c.S0 + 0xCu));
        c.T6 = mem.ReadU32((c.S0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S0 + 0x14u));
        c.T5 = mem.ReadU32((c.S0 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S0 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        goto L800B45B4;
        L800B45A0: ;
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = c.V1 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        L800B45B4: ;
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        c.A0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 + c.A0;
        mem.WriteU32((c.V0 + 0x10u), c.V1);
        L800B45D0: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU16((c.V0 + 0xEu));
        mem.WriteU16((c.SP + 0x44u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU16((c.V0 + 0xCu));
        c.V0 = 0x00000001u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        mem.WriteU32((c.SP + 0x48u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x50u), c.V1);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x8u));
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B465C;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A1 = c.V0 - 0x1u;
        goto L800B469C;
        L800B465C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = (uint)((int)c.V1 >> (int)(c.V0 & 31u));
        c.A1 = c.V0;
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4D54;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B4D54;
        }
        L800B469C: ;
        c.V0 = mem.ReadU32((c.SP + 0x50u));
        c.A0 = mem.ReadU16((c.SP + 0x44u));
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V1 = c.V1 >> 1;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.SP + 0x46u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.A2 = (uint)((int)c.V0 >> 16);
        c.V0 = 0u - c.A2;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.V0 = c.V0 << (int)(c.A0 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.V0 < 3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B46F8;
        }
        c.V0 = 0x00000002u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V0 = c.V0 + c.A2;
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        goto L800B46FC;
        L800B46F8: ;
        mem.WriteU32((c.SP + 0x4Cu), 0u);
        L800B46FC: ;
        c.A0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.A0 + 0x8u));
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            c.A3 = 0x00FF0000u;
            goto L800B47B4;
        }
        c.A3 = 0x00FF0000u;
        c.A3 = c.A3 | 0xFFFFu;
        c.V0 = c.V1 | 0x0001u;
        c.A1 = c.A1 << 16;
        c.A1 = (uint)((int)c.A1 >> 14);
        c.A2 = 0xFF000000u;
        mem.WriteU32((c.A0 + 0x8u), c.V0);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.A0 = mem.ReadU32((c.SP + 0x40u));
        c.A1 = c.A1 + c.V0;
        c.V1 = mem.ReadU32(c.A0);
        c.V0 = mem.ReadU32(c.A1);
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A3;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A0, c.V1);
        c.A0 = mem.ReadU32(c.A1);
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = mem.ReadU32((c.SP + 0x40u));
        c.A0 = c.A0 & c.A2;
        c.V1 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - 0x4u;
        c.V0 = c.V0 & c.A3;
        c.A0 = c.A0 | c.V0;
        mem.WriteU32(c.A1, c.A0);
        goto L800B47B4;
        L800B4784: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x9Cu));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU16((c.A1 + 0x8Eu));
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        mem.WriteU32((c.SP + 0x50u), 0u);
        mem.WriteU16((c.SP + 0x46u), (ushort)0u);
        mem.WriteU32((c.SP + 0x40u), c.V1);
        mem.WriteU16((c.SP + 0x44u), (ushort)c.A0);
        mem.WriteU32((c.SP + 0x48u), c.A1);
        L800B47B4: ;
        c.V0 = mem.ReadU16((c.S5 + 0x1Eu));
        c.V0 = c.V0 & 0x0003u;
        if (c.V0 != 0u) {
            c.A0 = c.S2;
            goto L800B4800;
        }
        c.A0 = c.S2;
        c.V0 = mem.ReadU32((c.S2 + 0x8u));
        c.V1 = 0x00030000u;
        c.V0 = c.V0 & c.V1;
        if (c.V0 != c.V1) {
            c.A2 = c.S5 + 0x10u;
            goto L800B4804;
        }
        c.A2 = c.S5 + 0x10u;
        c.V0 = mem.ReadU16((c.S7 + 0x11Eu));
        if (c.V0 == 0u) {
            c.A0 = c.S7 + 0x12Cu;
            goto L800B4868;
        }
        c.A0 = c.S7 + 0x12Cu;
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            c.A0 = c.S2;
            goto L800B4864;
        }
        c.A0 = c.S2;
        L800B4800: ;
        c.A2 = c.S5 + 0x10u;
        L800B4804: ;
        c.A1 = mem.ReadU16((c.S5 + 0x1Eu));
        c.A3 = c.S5 + 0x14u;
        c.RA = 0x800B4810u;
        MediEvil_game.func_800AB9BC(c, m);
        c.S6 = c.V0;
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x3Cu;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.A0;
        c.RA = 0x800B482Cu;
        MediEvil_game.func_800A4E3C(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = c.T2 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        goto L800B48B0;
        L800B4864: ;
        c.A0 = c.S7 + 0x12Cu;
        L800B4868: ;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = 0x1F800000u;
        c.A2 = c.A2 + 0x3Cu;
        c.RA = 0x800B487Cu;
        MediEvil_game.func_800A4E3C(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = c.T3 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        c.S6 = 0u;
        L800B48B0: ;
        c.T1 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.T1 + 0x34u));
        c.T4 = mem.ReadU32(c.T1);
        c.T5 = mem.ReadU32((c.T1 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T1 + 0x8u));
        c.T5 = mem.ReadU32((c.T1 + 0xCu));
        c.T6 = mem.ReadU32((c.T1 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T1 + 0x14u));
        c.T5 = mem.ReadU32((c.T1 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T1 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.A2 = mem.ReadU32((c.S3 + 0x10u));
        c.A0 = mem.ReadU16((c.S3 + 0x8u));
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.S3 = mem.ReadU32(c.V0);
        c.V1 = mem.ReadU16((c.S5 + 0xEu));
        c.S2 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 | 0x0001u;
        c.S1 = c.V1 & 0x003Eu;
        if (c.A0 == 0u) {
            mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
            goto L800B4CC4;
        }
        mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
        c.S0 = c.SP + 0x20u;
        L800B4934: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A2 = c.A2 + 0x4u;
            goto L800B4CB8;
        }
        c.A2 = c.A2 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + 0x7850u;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800B4970u: goto L800B4970;
            case 0x800B49C4u: goto L800B49C4;
            case 0x800B4A18u: goto L800B4A18;
            case 0x800B4A6Cu: goto L800B4A6C;
            case 0x800B4AC0u: goto L800B4AC0;
            case 0x800B4B14u: goto L800B4B14;
            case 0x800B4B68u: goto L800B4B68;
            case 0x800B4BBCu: goto L800B4BBC;
            case 0x800B4CB8u: goto L800B4CB8;
            case 0x800B4C10u: goto L800B4C10;
            case 0x800B4C64u: goto L800B4C64;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B4970: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B499C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4994u;
        MediEvil_game.func_800B0900(c, m);
        goto L800B4CB0;
        L800B499C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B49BCu;
        MediEvil_game.func_800B76BC(c, m);
        goto L800B4CB0;
        L800B49C4: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B49F0;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B49E8u;
        MediEvil_game.func_800B0AC0(c, m);
        goto L800B4CB0;
        L800B49F0: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A10u;
        MediEvil_game.func_800B7A04(c, m);
        goto L800B4CB0;
        L800B4A18: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A44;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A3Cu;
        MediEvil_game.func_800B0068(c, m);
        goto L800B4CB0;
        L800B4A44: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A64u;
        MediEvil_game.func_800B59C8(c, m);
        goto L800B4CB0;
        L800B4A6C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A98;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A90u;
        MediEvil_game.func_800B0240(c, m);
        goto L800B4CB0;
        L800B4A98: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4AB8u;
        MediEvil_game.func_800B5DA4(c, m);
        goto L800B4CB0;
        L800B4AC0: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4AEC;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4AE4u;
        MediEvil_game.func_800B0CB8(c, m);
        goto L800B4CB0;
        L800B4AEC: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B0Cu;
        MediEvil_game.func_800B7DBC(c, m);
        goto L800B4CB0;
        L800B4B14: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B40;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B38u;
        MediEvil_game.func_800B0EA4(c, m);
        goto L800B4CB0;
        L800B4B40: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B60u;
        MediEvil_game.func_800B8288(c, m);
        goto L800B4CB0;
        L800B4B68: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B94;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B8Cu;
        MediEvil_game.func_800B0458(c, m);
        goto L800B4CB0;
        L800B4B94: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4BB4u;
        MediEvil_game.func_800B6238(c, m);
        goto L800B4CB0;
        L800B4BBC: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4BE8;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4BE0u;
        MediEvil_game.func_800B065C(c, m);
        goto L800B4CB0;
        L800B4BE8: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C08u;
        MediEvil_game.func_800B6724(c, m);
        goto L800B4CB0;
        L800B4C10: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C3C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C34u;
        MediEvil_game.func_800B6E70(c, m);
        goto L800B4CB0;
        L800B4C3C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C5Cu;
        MediEvil_game.func_800B89A8(c, m);
        goto L800B4CB0;
        L800B4C64: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C90;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C88u;
        MediEvil_game.func_800B7224(c, m);
        goto L800B4CB0;
        L800B4C90: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4CB0u;
        MediEvil_game.func_800B906C(c, m);
        L800B4CB0: ;
        c.A2 = mem.ReadU32((c.SP + 0x60u));
        c.A0 = mem.ReadU32((c.SP + 0x64u));
        L800B4CB8: ;
        if (c.A0 != 0u) {
            goto L800B4934;
        }
        L800B4CC4: ;
        c.V0 = c.S6 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.S7 + 0x14Cu;
            goto L800B4CF8;
        }
        c.V0 = c.S7 + 0x14Cu;
        c.T4 = mem.ReadU32(c.V0);
        c.T5 = mem.ReadU32((c.V0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(16, c.T4);
        RecompOne.Runtime.Gte.WriteControl(17, c.T5);
        c.T4 = mem.ReadU32((c.V0 + 0x8u));
        c.T5 = mem.ReadU32((c.V0 + 0xCu));
        c.T6 = mem.ReadU32((c.V0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(18, c.T4);
        RecompOne.Runtime.Gte.WriteControl(19, c.T5);
        RecompOne.Runtime.Gte.WriteControl(20, c.T6);
        L800B4CF8: ;
        c.V0 = c.S6 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B4D28;
        }
        c.T2 = mem.ReadU8((c.S7 + 0x16Cu));
        c.T3 = mem.ReadU8((c.S7 + 0x16Du));
        c.T1 = mem.ReadU8((c.S7 + 0x16Eu));
        c.T4 = c.T2 << 4;
        c.T5 = c.T3 << 4;
        c.T6 = c.T1 << 4;
        RecompOne.Runtime.Gte.WriteControl(13, c.T4);
        RecompOne.Runtime.Gte.WriteControl(14, c.T5);
        RecompOne.Runtime.Gte.WriteControl(15, c.T6);
        L800B4D28: ;
        c.V0 = mem.ReadU32(c.FP);
        c.V0 = c.V0 & 0x2000u;
        if (c.V0 == 0u) {
            c.A2 = c.S5;
            goto L800B4D54;
        }
        c.A2 = c.S5;
        c.A3 = 0x00FF0000u;
        c.T2 = mem.ReadU32((c.SP + 0x88u));
        c.A1 = mem.ReadU8((c.SP + 0x84u));
        c.A0 = mem.ReadU32((c.T2 + 0x4u));
        c.A3 = c.A3 | 0xFFFFu;
        c.RA = 0x800B4D54u;
        MediEvil_game.func_800AF414(c, m);
        L800B4D54: ;
        c.RA = mem.ReadU32((c.SP + 0xB4u));
        c.FP = mem.ReadU32((c.SP + 0xB0u));
        c.S7 = mem.ReadU32((c.SP + 0xACu));
        c.S6 = mem.ReadU32((c.SP + 0xA8u));
        c.S5 = mem.ReadU32((c.SP + 0xA4u));
        c.S4 = mem.ReadU32((c.SP + 0xA0u));
        c.S3 = mem.ReadU32((c.SP + 0x9Cu));
        c.S2 = mem.ReadU32((c.SP + 0x98u));
        c.S1 = mem.ReadU32((c.SP + 0x94u));
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        c.SP = c.SP + 0xB8u;
        return;
    }

    public static void func_800B41C8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0xB8u;
        mem.WriteU32((c.SP + 0xA4u), c.S5);
        c.S5 = c.A0;
        c.V1 = 0x800F0000u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 - 0x2204u;
        mem.WriteU32((c.SP + 0xB4u), c.RA);
        mem.WriteU32((c.SP + 0xB0u), c.FP);
        mem.WriteU32((c.SP + 0xACu), c.S7);
        mem.WriteU32((c.SP + 0xA8u), c.S6);
        mem.WriteU32((c.SP + 0xA0u), c.S4);
        mem.WriteU32((c.SP + 0x9Cu), c.S3);
        mem.WriteU32((c.SP + 0x98u), c.S2);
        mem.WriteU32((c.SP + 0x94u), c.S1);
        mem.WriteU32((c.SP + 0x90u), c.S0);
        mem.WriteU8((c.SP + 0x84u), (byte)c.A2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x10u));
        c.V0 = mem.ReadU32(c.V0);
        c.S7 = c.A1;
        c.V1 = c.V1 < c.V0 ? 1u : 0u;
        if (c.V1 == 0u) {
            mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
            goto L800B4D54;
        }
        mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
        c.S2 = mem.ReadU32((c.S5 + 0x8u));
        c.S4 = 0u;
        c.FP = mem.ReadU32((c.S2 + 0x2Cu));
        c.V1 = mem.ReadU16((c.S2 + 0xAu));
        c.T1 = mem.ReadU32((c.FP + 0x4u));
        c.V0 = c.V1 & 0x0020u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x88u), c.T1);
            goto L800B4260;
        }
        mem.WriteU32((c.SP + 0x88u), c.T1);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x90u));
        c.S4 = c.S4 < c.V0 ? 1u : 0u;
        L800B4260: ;
        c.V0 = mem.ReadU16((c.S5 + 0xEu));
        c.V0 = c.V0 & 0x0040u;
        if (c.V0 == 0u) {
            c.S1 = c.S7 + 0xA0u;
            goto L800B4278;
        }
        c.S1 = c.S7 + 0xA0u;
        c.S1 = mem.ReadU32((c.S5 + 0x18u));
        L800B4278: ;
        c.V0 = c.V1 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800B4290;
        }
        c.A1 = mem.ReadU32((c.S2 + 0xCu));
        goto L800B429C;
        L800B4290: ;
        c.V0 = mem.ReadU32((c.S2 + 0xCu));
        c.A1 = c.V0 + 0x60u;
        L800B429C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x38u));
        if (c.V0 == c.A1) {
            goto L800B42C8;
        }
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A1);
        c.A0 = c.S1;
        c.RA = 0x800B42C8u;
        MediEvil_game.func_800A4E3C(c, m);
        L800B42C8: ;
        c.A1 = c.S2 + 0x10u;
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.A0 + 0x38u));
        c.A2 = c.SP + 0x70u;
        c.RA = 0x800B42DCu;
        MediEvil_game.func_800A4880(c, m);
        c.A0 = c.S1;
        c.A1 = c.SP + 0x68u;
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU16((c.SP + 0x70u));
        c.A3 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.T0 + 0x14u));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x18u));
        c.V1 = mem.ReadU16((c.SP + 0x74u));
        c.A3 = mem.ReadU16((c.S1 + 0x18u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x1Cu));
        c.V1 = mem.ReadU16((c.SP + 0x78u));
        c.A3 = mem.ReadU16((c.S1 + 0x1Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B4344u;
        MediEvil_game.func_800A4880(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = mem.ReadU32((c.T2 + 0x34u));
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T2 + 0x14u));
        c.T5 = mem.ReadU32((c.T2 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T2 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T3 = 0x800F0000u;
        c.T3 = c.T3 - 0x263Cu;
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.A1 = c.SP + 0x80u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw);  }
        c.V0 = TerrainPatch.MeshClipDistance(mem.ReadU16((c.FP + 0x8u)), m);
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4D54;
        }
        c.V1 = mem.ReadU8((c.SP + 0x84u));
        c.T1 = mem.ReadU32((c.SP + 0x88u));
        c.A2 = mem.ReadU32(c.FP);
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V1 = mem.ReadU32((c.T1 + 0x4u));
        c.V0 = c.V0 + 0x10u;
        c.S3 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.V1 = c.A2 & 0x0008u;
        c.A0 = mem.ReadU32((c.V0 + 0x8u));
        if (c.V1 != 0u) {
            goto L800B446C;
        }
        if (c.A0 == 0u) {
            c.V0 = c.A2 & 0x0010u;
            goto L800B446C;
        }
        c.V0 = c.A2 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B4438;
        }
        c.RA = 0x800B4430u;
        MediEvil_game.func_8009A61C(c, m);
        goto L800B4440;
        L800B4438: ;
        c.RA = 0x800B4440u;
        MediEvil_game.func_8009A4C8(c, m);
        L800B4440: ;
        if (c.V0 == 0u) {
            goto L800B4D54;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            goto L800B4D54;
        }
        L800B446C: ;
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        if (c.V1 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4784;
        }
        c.V0 = 0x00000002u;
        c.V0 = mem.ReadU32((c.V1 + 0x8u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 != 0u) {
            c.V0 = c.V1 + 0x18u;
            goto L800B45D0;
        }
        c.V0 = c.V1 + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        c.A1 = mem.ReadU32((c.V1 + 0x14u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        if (c.A1 == c.A0) {
            goto L800B45A0;
        }
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            c.V0 = c.A1 + 0x60u;
            goto L800B44D0;
        }
        c.V0 = c.A1 + 0x60u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A0);
        c.A0 = c.S1;
        goto L800B44DC;
        L800B44D0: ;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.V0);
        c.A0 = c.S1;
        L800B44DC: ;
        c.S0 = 0x1F800000u;
        c.S0 = c.S0 + 0x60u;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.S0;
        c.RA = 0x800B44F4u;
        MediEvil_game.func_800A4E3C(c, m);
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x38u));
        c.V1 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.A2 + 0x14u));
        c.A0 = c.S1;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x18u));
        c.V1 = mem.ReadU16((c.A0 + 0x18u));
        c.A1 = c.SP + 0x68u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.A0 + 0x1Cu));
        c.A2 = c.S0 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B453Cu;
        MediEvil_game.func_800A4880(c, m);
        c.T4 = mem.ReadU32(c.S0);
        c.T5 = mem.ReadU32((c.S0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S0 + 0x8u));
        c.T5 = mem.ReadU32((c.S0 + 0xCu));
        c.T6 = mem.ReadU32((c.S0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S0 + 0x14u));
        c.T5 = mem.ReadU32((c.S0 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S0 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        goto L800B45B4;
        L800B45A0: ;
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = c.V1 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        L800B45B4: ;
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        c.A0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 + c.A0;
        mem.WriteU32((c.V0 + 0x10u), c.V1);
        L800B45D0: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU16((c.V0 + 0xEu));
        mem.WriteU16((c.SP + 0x44u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU16((c.V0 + 0xCu));
        c.V0 = 0x00000001u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        mem.WriteU32((c.SP + 0x48u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x50u), c.V1);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x8u));
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B465C;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A1 = c.V0 - 0x1u;
        goto L800B469C;
        L800B465C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = (uint)((int)c.V1 >> (int)(c.V0 & 31u));
        c.A1 = c.V0;
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4D54;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B4D54;
        }
        L800B469C: ;
        c.V0 = mem.ReadU32((c.SP + 0x50u));
        c.A0 = mem.ReadU16((c.SP + 0x44u));
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V1 = c.V1 >> 1;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.SP + 0x46u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.A2 = (uint)((int)c.V0 >> 16);
        c.V0 = 0u - c.A2;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.V0 = c.V0 << (int)(c.A0 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.V0 < 3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B46F8;
        }
        c.V0 = 0x00000002u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V0 = c.V0 + c.A2;
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        goto L800B46FC;
        L800B46F8: ;
        mem.WriteU32((c.SP + 0x4Cu), 0u);
        L800B46FC: ;
        c.A0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.A0 + 0x8u));
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            c.A3 = 0x00FF0000u;
            goto L800B47B4;
        }
        c.A3 = 0x00FF0000u;
        c.A3 = c.A3 | 0xFFFFu;
        c.V0 = c.V1 | 0x0001u;
        c.A1 = c.A1 << 16;
        c.A1 = (uint)((int)c.A1 >> 14);
        c.A2 = 0xFF000000u;
        mem.WriteU32((c.A0 + 0x8u), c.V0);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.A0 = mem.ReadU32((c.SP + 0x40u));
        c.A1 = c.A1 + c.V0;
        c.V1 = mem.ReadU32(c.A0);
        c.V0 = mem.ReadU32(c.A1);
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A3;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A0, c.V1);
        c.A0 = mem.ReadU32(c.A1);
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = mem.ReadU32((c.SP + 0x40u));
        c.A0 = c.A0 & c.A2;
        c.V1 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - 0x4u;
        c.V0 = c.V0 & c.A3;
        c.A0 = c.A0 | c.V0;
        mem.WriteU32(c.A1, c.A0);
        goto L800B47B4;
        L800B4784: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x9Cu));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU16((c.A1 + 0x8Eu));
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        mem.WriteU32((c.SP + 0x50u), 0u);
        mem.WriteU16((c.SP + 0x46u), (ushort)0u);
        mem.WriteU32((c.SP + 0x40u), c.V1);
        mem.WriteU16((c.SP + 0x44u), (ushort)c.A0);
        mem.WriteU32((c.SP + 0x48u), c.A1);
        L800B47B4: ;
        c.V0 = mem.ReadU16((c.S5 + 0x1Eu));
        c.V0 = c.V0 & 0x0003u;
        if (c.V0 != 0u) {
            c.A0 = c.S2;
            goto L800B4800;
        }
        c.A0 = c.S2;
        c.V0 = mem.ReadU32((c.S2 + 0x8u));
        c.V1 = 0x00030000u;
        c.V0 = c.V0 & c.V1;
        if (c.V0 != c.V1) {
            c.A2 = c.S5 + 0x10u;
            goto L800B4804;
        }
        c.A2 = c.S5 + 0x10u;
        c.V0 = mem.ReadU16((c.S7 + 0x11Eu));
        if (c.V0 == 0u) {
            c.A0 = c.S7 + 0x12Cu;
            goto L800B4868;
        }
        c.A0 = c.S7 + 0x12Cu;
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            c.A0 = c.S2;
            goto L800B4864;
        }
        c.A0 = c.S2;
        L800B4800: ;
        c.A2 = c.S5 + 0x10u;
        L800B4804: ;
        c.A1 = mem.ReadU16((c.S5 + 0x1Eu));
        c.A3 = c.S5 + 0x14u;
        c.RA = 0x800B4810u;
        MediEvil_game.func_800AB9BC(c, m);
        c.S6 = c.V0;
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x3Cu;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.A0;
        c.RA = 0x800B482Cu;
        MediEvil_game.func_800A4E3C(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = c.T2 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        goto L800B48B0;
        L800B4864: ;
        c.A0 = c.S7 + 0x12Cu;
        L800B4868: ;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = 0x1F800000u;
        c.A2 = c.A2 + 0x3Cu;
        c.RA = 0x800B487Cu;
        MediEvil_game.func_800A4E3C(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = c.T3 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        c.S6 = 0u;
        L800B48B0: ;
        c.T1 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.T1 + 0x34u));
        c.T4 = mem.ReadU32(c.T1);
        c.T5 = mem.ReadU32((c.T1 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T1 + 0x8u));
        c.T5 = mem.ReadU32((c.T1 + 0xCu));
        c.T6 = mem.ReadU32((c.T1 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T1 + 0x14u));
        c.T5 = mem.ReadU32((c.T1 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T1 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.A2 = mem.ReadU32((c.S3 + 0x10u));
        c.A0 = mem.ReadU16((c.S3 + 0x8u));
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.S3 = mem.ReadU32(c.V0);
        c.V1 = mem.ReadU16((c.S5 + 0xEu));
        c.S2 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 | 0x0001u;
        c.S1 = c.V1 & 0x003Eu;
        if (c.A0 == 0u) {
            mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
            goto L800B4CC4;
        }
        mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
        c.S0 = c.SP + 0x20u;
        L800B4934: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A2 = c.A2 + 0x4u;
            goto L800B4CB8;
        }
        c.A2 = c.A2 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + 0x7850u;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800B4970u: goto L800B4970;
            case 0x800B49C4u: goto L800B49C4;
            case 0x800B4A18u: goto L800B4A18;
            case 0x800B4A6Cu: goto L800B4A6C;
            case 0x800B4AC0u: goto L800B4AC0;
            case 0x800B4B14u: goto L800B4B14;
            case 0x800B4B68u: goto L800B4B68;
            case 0x800B4BBCu: goto L800B4BBC;
            case 0x800B4CB8u: goto L800B4CB8;
            case 0x800B4C10u: goto L800B4C10;
            case 0x800B4C64u: goto L800B4C64;
            case 0x800B5648u: goto L800B5648;
            case 0x800B5654u: goto L800B5654;
            case 0x800B5660u: goto L800B5660;
            case 0x800B566Cu: goto L800B566C;
            case 0x800B5678u: goto L800B5678;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B4970: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B499C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4994u;
        MediEvil_game.func_800B0900(c, m);
        goto L800B4CB0;
        L800B499C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B49BCu;
        MediEvil_game.func_800B76BC(c, m);
        goto L800B4CB0;
        L800B49C4: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B49F0;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B49E8u;
        MediEvil_game.func_800B0AC0(c, m);
        goto L800B4CB0;
        L800B49F0: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A10u;
        MediEvil_game.func_800B7A04(c, m);
        goto L800B4CB0;
        L800B4A18: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A44;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A3Cu;
        MediEvil_game.func_800B0068(c, m);
        goto L800B4CB0;
        L800B4A44: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A64u;
        MediEvil_game.func_800B59C8(c, m);
        goto L800B4CB0;
        L800B4A6C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A98;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A90u;
        MediEvil_game.func_800B0240(c, m);
        goto L800B4CB0;
        L800B4A98: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4AB8u;
        MediEvil_game.func_800B5DA4(c, m);
        goto L800B4CB0;
        L800B4AC0: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4AEC;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4AE4u;
        MediEvil_game.func_800B0CB8(c, m);
        goto L800B4CB0;
        L800B4AEC: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B0Cu;
        MediEvil_game.func_800B7DBC(c, m);
        goto L800B4CB0;
        L800B4B14: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B40;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B38u;
        MediEvil_game.func_800B0EA4(c, m);
        goto L800B4CB0;
        L800B4B40: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B60u;
        MediEvil_game.func_800B8288(c, m);
        goto L800B4CB0;
        L800B4B68: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B94;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B8Cu;
        MediEvil_game.func_800B0458(c, m);
        goto L800B4CB0;
        L800B4B94: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4BB4u;
        MediEvil_game.func_800B6238(c, m);
        goto L800B4CB0;
        L800B4BBC: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4BE8;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4BE0u;
        MediEvil_game.func_800B065C(c, m);
        goto L800B4CB0;
        L800B4BE8: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C08u;
        MediEvil_game.func_800B6724(c, m);
        goto L800B4CB0;
        L800B4C10: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C3C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C34u;
        MediEvil_game.func_800B6E70(c, m);
        goto L800B4CB0;
        L800B4C3C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C5Cu;
        MediEvil_game.func_800B89A8(c, m);
        goto L800B4CB0;
        L800B4C64: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C90;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C88u;
        MediEvil_game.func_800B7224(c, m);
        goto L800B4CB0;
        L800B4C90: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4CB0u;
        MediEvil_game.func_800B906C(c, m);
        L800B4CB0: ;
        c.A2 = mem.ReadU32((c.SP + 0x60u));
        c.A0 = mem.ReadU32((c.SP + 0x64u));
        L800B4CB8: ;
        if (c.A0 != 0u) {
            goto L800B4934;
        }
        L800B4CC4: ;
        c.V0 = c.S6 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.S7 + 0x14Cu;
            goto L800B4CF8;
        }
        c.V0 = c.S7 + 0x14Cu;
        c.T4 = mem.ReadU32(c.V0);
        c.T5 = mem.ReadU32((c.V0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(16, c.T4);
        RecompOne.Runtime.Gte.WriteControl(17, c.T5);
        c.T4 = mem.ReadU32((c.V0 + 0x8u));
        c.T5 = mem.ReadU32((c.V0 + 0xCu));
        c.T6 = mem.ReadU32((c.V0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(18, c.T4);
        RecompOne.Runtime.Gte.WriteControl(19, c.T5);
        RecompOne.Runtime.Gte.WriteControl(20, c.T6);
        L800B4CF8: ;
        c.V0 = c.S6 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B4D28;
        }
        c.T2 = mem.ReadU8((c.S7 + 0x16Cu));
        c.T3 = mem.ReadU8((c.S7 + 0x16Du));
        c.T1 = mem.ReadU8((c.S7 + 0x16Eu));
        c.T4 = c.T2 << 4;
        c.T5 = c.T3 << 4;
        c.T6 = c.T1 << 4;
        RecompOne.Runtime.Gte.WriteControl(13, c.T4);
        RecompOne.Runtime.Gte.WriteControl(14, c.T5);
        RecompOne.Runtime.Gte.WriteControl(15, c.T6);
        L800B4D28: ;
        c.V0 = mem.ReadU32(c.FP);
        c.V0 = c.V0 & 0x2000u;
        if (c.V0 == 0u) {
            c.A2 = c.S5;
            goto L800B4D54;
        }
        c.A2 = c.S5;
        c.A3 = 0x00FF0000u;
        c.T2 = mem.ReadU32((c.SP + 0x88u));
        c.A1 = mem.ReadU8((c.SP + 0x84u));
        c.A0 = mem.ReadU32((c.T2 + 0x4u));
        c.A3 = c.A3 | 0xFFFFu;
        c.RA = 0x800B4D54u;
        MediEvil_game.func_800AF414(c, m);
        L800B4D54: ;
        c.RA = mem.ReadU32((c.SP + 0xB4u));
        c.FP = mem.ReadU32((c.SP + 0xB0u));
        c.S7 = mem.ReadU32((c.SP + 0xACu));
        c.S6 = mem.ReadU32((c.SP + 0xA8u));
        c.S5 = mem.ReadU32((c.SP + 0xA4u));
        c.S4 = mem.ReadU32((c.SP + 0xA0u));
        c.S3 = mem.ReadU32((c.SP + 0x9Cu));
        c.S2 = mem.ReadU32((c.SP + 0x98u));
        c.S1 = mem.ReadU32((c.SP + 0x94u));
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        c.SP = c.SP + 0xB8u;
        return;
        c.SP = c.SP - 0x20u;
        mem.WriteU32((c.SP + 0x14u), c.S1);
        c.S1 = c.A0;
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.RA);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F2Cu), 0u);
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F18u), 0u);
        c.S2 = c.A1;
        c.RA = 0x800B4DB4u;
        MediEvil_game.func_800C09E8(c, m);
        c.S0 = 0x800F0000u;
        c.S0 = c.S0 + 0x6AC0u;
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x4F4Cu;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F64u), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x4EE4u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F60u), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x5050u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F5Cu), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x510Cu;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F58u), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x5388u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F54u), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x53C0u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F50u), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x5040u;
        c.A0 = c.S0;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F30u), c.S0);
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F40u), c.V0);
        c.A1 = 0x000001E0u;
        c.RA = 0x800B4E40u;
        MediEvil_game.func_800C0EA8(c, m);
        c.T0 = 0u;
        c.T1 = 0x000000FFu;
        c.A0 = c.S0 + 0x40u;
        c.A3 = 0x800F0000u;
        c.A3 = c.A3 + 0x6A78u;
        c.A2 = 0x800F0000u;
        c.A2 = c.A2 + 0x6A30u;
        mem.WriteU32((c.S0 + 0x30u), c.S1);
        mem.WriteU32((c.S0 + 0x120u), c.S2);
        L800B4E64: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32((c.A0 - 0x10u));
        c.A1 = c.S0 + 0x5Du;
        mem.WriteU32((c.A0 - 0x34u), 0u);
        mem.WriteU32((c.A0 - 0x30u), c.S0);
        mem.WriteU8(c.V0, (byte)c.T1);
        c.V0 = mem.ReadU32((c.A0 - 0x10u));
        c.V1 = 0x00000005u;
        mem.WriteU8((c.V0 + 0x1u), (byte)0u);
        mem.WriteU32((c.A0 - 0x4u), c.A2);
        mem.WriteU32(c.A0, c.A3);
        L800B4E8C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        mem.WriteU8(c.A1, (byte)c.T1);
        c.V1 = c.V1 - 0x1u;
        if ((int)c.V1 >= 0) {
            c.A1 = c.A1 + 0x1u;
            goto L800B4E8C;
        }
        c.A1 = c.A1 + 0x1u;
        c.A3 = c.A3 + 0x23u;
        c.A2 = c.A2 + 0x23u;
        c.T0 = c.T0 + 0x1u;
        c.A0 = c.A0 + 0xF0u;
        c.V0 = (int)c.T0 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S0 = c.S0 + 0xF0u;
            goto L800B4E64;
        }
        c.S0 = c.S0 + 0xF0u;
        c.RA = 0x800B4EC0u;
        MediEvil_game.func_800BFD88(c, m);
        c.V0 = 0x00000001u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F2Cu), c.V0);
        c.RA = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x20u;
        return;
        c.V0 = mem.ReadU8((c.A0 + 0x49u));
        if (c.V0 == 0u) {
            goto L800B4F44;
        }
        c.V1 = c.A0 + 0x5Du;
        c.A1 = 0x000000FFu;
        c.V0 = 0x00000005u;
        mem.WriteU8((c.A0 + 0x49u), (byte)0u);
        mem.WriteU8((c.A0 + 0x46u), (byte)0u);
        mem.WriteU16((c.A0 + 0xE6u), (ushort)0u);
        mem.WriteU32((c.A0 + 0x14u), 0u);
        mem.WriteU32((c.A0 + 0x18u), 0u);
        mem.WriteU8((c.A0 + 0xE3u), (byte)0u);
        mem.WriteU8((c.A0 + 0xE4u), (byte)0u);
        mem.WriteU16((c.A0 + 0xE6u), (ushort)0u);
        mem.WriteU8((c.A0 + 0xE9u), (byte)0u);
        mem.WriteU8((c.A0 + 0xEAu), (byte)0u);
        mem.WriteU32(c.A0, 0u);
        mem.WriteU32((c.A0 + 0x4u), 0u);
        mem.WriteU32((c.A0 + 0x8u), 0u);
        L800B4F34: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        mem.WriteU8(c.V1, (byte)c.A1);
        c.V0 = c.V0 - 0x1u;
        if ((int)c.V0 >= 0) {
            c.V1 = c.V1 + 0x1u;
            goto L800B4F34;
        }
        c.V1 = c.V1 + 0x1u;
        L800B4F44: ;
        return;
        c.SP = c.SP - 0x28u;
        c.A1 = c.A0;
        mem.WriteU32((c.SP + 0x14u), c.S1);
        c.S1 = 0x800F0000u;
        c.S1 = c.S1 + 0x6AC0u;
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        c.S3 = 0xFFFFFFF7u;
        mem.WriteU32((c.SP + 0x18u), c.S2);
        c.S2 = 0x800F0000u;
        c.S2 = c.S2 - 0x2F0Cu;
        mem.WriteU32((c.SP + 0x20u), c.RA);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        L800B4F7C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V1 = 0x800F0000u;
        c.V1 = mem.ReadU32((c.V1 - 0x2F24u));
        c.V0 = c.V1 << 4;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 4;
        if (c.A1 == c.S3) {
            c.S0 = c.V0 + c.S1;
            goto L800B4FC0;
        }
        c.S0 = c.V0 + c.S1;
        if (c.A1 != 0u) {
            c.V0 = c.V1 << 2;
            goto L800B4FB0;
        }
        c.V0 = c.V1 << 2;
        c.V0 = c.V0 + c.S2;
        mem.WriteU32(c.V0, 0u);
        goto L800B4FC0;
        L800B4FB0: ;
        c.A0 = c.S0;
        c.RA = 0x800B4FB8u;
        MediEvil_game.func_800C0D90(c, m);
        c.A0 = c.S0;
        c.RA = 0x800B4FC0u;
        MediEvil_game.func_800B5040(c, m);
        L800B4FC0: ;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x389Cu));
        c.V1 = 0x800F0000u;
        c.V1 = mem.ReadU32((c.V1 - 0x2F24u));
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F20u), 0u);
        mem.WriteU16((c.V0 + 0xAu), (ushort)0u);
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x2F10u));
        c.V1 = c.V1 + 0x1u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F24u), c.V1);
        c.V0 = (int)c.V0 < (int)c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = 0x00000001u;
            goto L800B5018;
        }
        c.V0 = 0x00000001u;
        c.A0 = c.V1 << 4;
        c.A0 = c.A0 - c.V1;
        c.A0 = c.A0 << 4;
        c.A0 = c.A0 + c.S1;
        c.RA = 0x800B5010u;
        MediEvil_game.func_800C0078(c, m);
        c.A1 = 0u | 0xFFFFu;
        goto L800B501C;
        L800B5018: ;
        c.A1 = 0u | 0xFFFFu;
        L800B501C: ;
        if (c.V0 == 0u) {
            goto L800B4F7C;
        }
        c.RA = mem.ReadU32((c.SP + 0x20u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x28u;
        return;
        c.V0 = mem.ReadU8((c.A0 + 0x37u));
        mem.WriteU8((c.A0 + 0x37u), (byte)0u);
        mem.WriteU8((c.A0 + 0x38u), (byte)c.V0);
        return;
        c.V0 = mem.ReadU8((c.A0 + 0x45u));
        c.A1 = mem.ReadU8((c.A0 + 0x37u));
        if (c.A1 == 0u) {
            c.V1 = c.V0 - 0x3u;
            goto L800B5078;
        }
        c.V1 = c.V0 - 0x3u;
        c.V0 = 0x0000004Du;
        if (c.A1 == c.V0) {
            goto L800B50C0;
        }
        goto L800B50E0;
        L800B5078: ;
        c.V0 = (int)c.V1 < 6 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.A0 + c.V1;
            goto L800B5094;
        }
        c.V0 = c.A0 + c.V1;
        c.V0 = mem.ReadU8((c.V0 + 0x57u));
        if (c.V0 == 0u) {
            c.V0 = 0u;
            goto L800B5104;
        }
        c.V0 = 0u;
        L800B5094: ;
        c.V0 = mem.ReadU8((c.A0 + 0x34u));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0u;
            goto L800B5104;
        }
        c.V0 = 0u;
        c.V0 = mem.ReadU32((c.A0 + 0x28u));
        c.V0 = c.V0 + c.V1;
        L800B50B4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8(c.V0);
        goto L800B5104;
        L800B50C0: ;
        c.V0 = mem.ReadU8((c.A0 + 0x36u));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x000000FFu;
            goto L800B5104;
        }
        c.V0 = 0x000000FFu;
        c.V0 = mem.ReadU32((c.A0 + 0x2Cu));
        c.V0 = c.V0 + c.V1;
        goto L800B50B4;
        L800B50E0: ;
        c.V0 = mem.ReadU8((c.A0 + 0x36u));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0u;
            goto L800B5104;
        }
        c.V0 = 0u;
        c.V0 = mem.ReadU32((c.A0 + 0x2Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = mem.ReadU8(c.V0);
        L800B5104: ;
        return;
        c.SP = c.SP - 0x18u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.S0 = c.A0;
        c.A0 = c.S0 + 0x57u;
        mem.WriteU32((c.SP + 0x14u), c.RA);
        c.A1 = 0x00000006u;
        c.RA = 0x800B5128u;
        MediEvil_game.func_800C0EA8(c, m);
        c.V0 = mem.ReadU16((c.S0 + 0xE6u));
        if (c.V0 == 0u) {
            goto L800B5284;
        }
        c.V0 = mem.ReadU32((c.S0 + 0x28u));
        if (c.V0 == 0u) {
            goto L800B5284;
        }
        c.V0 = mem.ReadU8((c.S0 + 0x34u));
        c.V0 = c.V0 < 0x00000007u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.T1 = 0x00000006u;
            goto L800B5160;
        }
        c.T1 = 0x00000006u;
        c.T1 = mem.ReadU8((c.S0 + 0x34u));
        L800B5160: ;
        c.V0 = mem.ReadU8((c.S0 + 0xE9u));
        if (c.V0 == 0u) {
            c.T0 = 0u;
            goto L800B5378;
        }
        c.T0 = 0u;
        c.T3 = 0x00000001u;
        c.T2 = 0u;
        L800B5178: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32((c.S0 + 0x4u));
        c.A2 = 0u;
        c.V0 = c.T2 + c.V0;
        c.V0 = mem.ReadU8((c.V0 + 0x2u));
        if (c.V0 == 0u) {
            c.A3 = 0x00000001u;
            goto L800B5198;
        }
        c.A3 = 0x00000001u;
        c.A3 = 0x000000FFu;
        L800B5198: ;
        c.A1 = c.S0 + 0x5Du;
        c.A0 = mem.ReadU32((c.S0 + 0x28u));
        if (c.T1 == 0u) {
            c.V1 = 0u;
            goto L800B51E0;
        }
        c.V1 = 0u;
        L800B51A8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8(c.A1);
        if (c.V0 != c.T0) {
            goto L800B51CC;
        }
        c.V0 = mem.ReadU8(c.A0);
        c.V0 = c.V0 & c.A3;
        if (c.V0 != 0u) {
            goto L800B5220;
        }
        L800B51CC: ;
        c.A1 = c.A1 + 0x1u;
        c.V1 = c.V1 + 0x1u;
        c.V0 = (int)c.V1 < (int)c.T1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.A0 = c.A0 + 0x1u;
            goto L800B51A8;
        }
        c.A0 = c.A0 + 0x1u;
        L800B51E0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if (c.A2 == 0u) {
            goto L800B5268;
        }
        c.V0 = mem.ReadU32((c.S0 + 0x4u));
        c.V0 = c.T2 + c.V0;
        c.V1 = mem.ReadU8((c.V0 + 0x3u));
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x2F1Cu));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < 61 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B5228;
        }
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F1Cu), c.V1);
        goto L800B522C;
        L800B5220: ;
        c.A2 = 0x00000001u;
        goto L800B51E0;
        L800B5228: ;
        c.A2 = 0u;
        L800B522C: ;
        if (c.A2 == 0u) {
            goto L800B5268;
        }
        c.A1 = c.S0 + 0x5Du;
        c.A0 = c.S0 + 0x57u;
        if (c.T1 == 0u) {
            c.V1 = 0u;
            goto L800B5268;
        }
        c.V1 = 0u;
        L800B5244: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8(c.A1);
        if (c.V0 != c.T0) {
            c.A1 = c.A1 + 0x1u;
            goto L800B5258;
        }
        c.A1 = c.A1 + 0x1u;
        mem.WriteU8(c.A0, (byte)c.T3);
        L800B5258: ;
        c.V1 = c.V1 + 0x1u;
        c.V0 = (int)c.V1 < (int)c.T1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.A0 = c.A0 + 0x1u;
            goto L800B5244;
        }
        c.A0 = c.A0 + 0x1u;
        L800B5268: ;
        c.V0 = mem.ReadU8((c.S0 + 0xE9u));
        c.T0 = c.T0 + 0x1u;
        c.V0 = (int)c.T0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.T2 = c.T2 + 0x5u;
            goto L800B5178;
        }
        c.T2 = c.T2 + 0x5u;
        goto L800B5378;
        L800B5284: ;
        c.V1 = mem.ReadU8((c.S0 + 0xE8u));
        c.V0 = c.V1 - 0x4u;
        c.V0 = c.V0 < 0x00000002u ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = 0x00000007u;
            goto L800B52A4;
        }
        c.V0 = 0x00000007u;
        if (c.V1 != c.V0) {
            goto L800B5338;
        }
        L800B52A4: ;
        c.V0 = mem.ReadU16((c.S0 + 0xE6u));
        if (c.V0 != 0u) {
            goto L800B5338;
        }
        c.V0 = mem.ReadU8((c.S0 + 0x34u));
        c.V0 = c.V0 < 0x00000002u ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = 0x00000040u;
            goto L800B5338;
        }
        c.V1 = 0x00000040u;
        c.A0 = mem.ReadU32((c.S0 + 0x28u));
        c.V0 = mem.ReadU8(c.A0);
        c.V0 = c.V0 & 0x00C0u;
        if (c.V0 != c.V1) {
            goto L800B5378;
        }
        c.V0 = mem.ReadU8((c.A0 + 0x1u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            goto L800B5378;
        }
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x2F1Cu));
        c.V0 = c.V0 + 0xAu;
        c.V0 = (int)c.V0 < 61 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000001u;
            goto L800B5378;
        }
        c.V0 = 0x00000001u;
        mem.WriteU8((c.S0 + 0x58u), (byte)c.V0);
        mem.WriteU8((c.S0 + 0x57u), (byte)c.V0);
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x2F1Cu));
        c.V0 = c.V0 + 0xAu;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - 0x2F1Cu), c.V0);
        goto L800B5378;
        L800B5338: ;
        c.V1 = mem.ReadU8((c.S0 + 0xE8u));
        c.V0 = 0x00000003u;
        if (c.V1 != c.V0) {
            c.V0 = 0x00000001u;
            goto L800B5350;
        }
        c.V0 = 0x00000001u;
        mem.WriteU8((c.S0 + 0x57u), (byte)c.V0);
        goto L800B5378;
        L800B5350: ;
        c.V0 = mem.ReadU16((c.S0 + 0xE6u));
        if (c.V0 != 0u) {
            c.V0 = 0x00000001u;
            goto L800B5378;
        }
        c.V0 = 0x00000001u;
        c.V1 = 0x00000005u;
        c.A0 = c.S0 + 0x5u;
        L800B5368: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        mem.WriteU8((c.A0 + 0x57u), (byte)c.V0);
        c.V1 = c.V1 - 0x1u;
        if ((int)c.V1 >= 0) {
            c.A0 = c.A0 - 0x1u;
            goto L800B5368;
        }
        c.A0 = c.A0 - 0x1u;
        L800B5378: ;
        c.RA = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x18u;
        return;
        c.A1 = 0u;
        c.A2 = 0x00000010u;
        c.V1 = 0x800F0000u;
        c.V1 = c.V1 + 0x6AC0u;
        L800B5398: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if (c.A0 == c.V1) {
            c.V0 = c.A2;
            goto L800B53B8;
        }
        c.V0 = c.A2;
        c.A2 = c.A2 + 0x10u;
        c.A1 = c.A1 + 0x1u;
        c.V0 = (int)c.A1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = c.V1 + 0xF0u;
            goto L800B5398;
        }
        c.V1 = c.V1 + 0xF0u;
        c.V0 = 0x000000FFu;
        L800B53B8: ;
        return;
        c.V0 = 0x800F0000u;
        c.V0 = c.V0 + 0x6AC0u;
        c.A0 = c.A0 & 0x00F0u;
        if (c.A0 == 0u) {
            goto L800B53D8;
        }
        c.V0 = c.V0 + 0xF0u;
        L800B53D8: ;
        return;
        c.SP = c.SP - 0x18u;
        mem.WriteU32((c.SP + 0x10u), c.RA);
        c.RA = 0x800B53F4u;
        MediEvil_game.func_800BFFAC(c, m);
        c.RA = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x18u;
        return;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x2F50u));
        c.SP = c.SP - 0x18u;
        mem.WriteU32((c.SP + 0x10u), c.RA);
        c.RA = 0x800B541Cu;
        Dispatcher.Call(c, m, c.V0);
        c.V1 = c.V0;
        c.V0 = mem.ReadU8((c.V1 + 0x37u));
        if (c.V0 != 0u) {
            goto L800B5478;
        }
        c.V0 = mem.ReadU8((c.V1 + 0x38u));
        if (c.V0 != 0u) {
            goto L800B5478;
        }
        c.V0 = mem.ReadU32((c.V1 + 0x10u));
        if (c.V1 == c.V0) {
            goto L800B5460;
        }
        c.V0 = mem.ReadU8((c.V1 + 0x39u));
        if (c.V0 != 0u) {
            goto L800B5478;
        }
        L800B5460: ;
        c.V0 = mem.ReadU32((c.V1 + 0x30u));
        c.V0 = mem.ReadU8(c.V0);
        if (c.V0 == 0u) {
            goto L800B54BC;
        }
        L800B5478: ;
        c.A0 = mem.ReadU8((c.V1 + 0x49u));
        c.V0 = 0x00000003u;
        if (c.A0 == c.V0) {
            c.V0 = (int)c.A0 < 4 ? 1u : 0u;
            goto L800B54B4;
        }
        c.V0 = (int)c.A0 < 4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B54A0;
        }
        c.V0 = 0x00000002u;
        if (c.A0 == c.V0) {
            c.V0 = 0x00000001u;
            goto L800B54C0;
        }
        c.V0 = 0x00000001u;
        goto L800B54BC;
        L800B54A0: ;
        c.V0 = 0x00000006u;
        if (c.A0 == c.V0) {
            c.V0 = 0x00000004u;
            goto L800B54C0;
        }
        c.V0 = 0x00000004u;
        goto L800B54BC;
        L800B54B4: ;
        c.V0 = 0x00000001u;
        goto L800B54C0;
        L800B54BC: ;
        c.V0 = mem.ReadU8((c.V1 + 0x49u));
        L800B54C0: ;
        c.RA = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x18u;
        return;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x2F50u));
        c.SP = c.SP - 0x20u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.S0 = c.A1;
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.RA);
        c.S1 = c.A2;
        c.RA = 0x800B54F4u;
        Dispatcher.Call(c, m, c.V0);
        c.V1 = c.V0;
        c.V0 = 0x00000003u;
        if (c.S0 == c.V0) {
            c.V0 = (int)c.S0 < 4 ? 1u : 0u;
            goto L800B5558;
        }
        c.V0 = (int)c.S0 < 4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000001u;
            goto L800B5524;
        }
        c.V0 = 0x00000001u;
        if (c.S0 == c.V0) {
            c.V0 = 0x00000002u;
            goto L800B5540;
        }
        c.V0 = 0x00000002u;
        if (c.S0 == c.V0) {
            c.V0 = 0u;
            goto L800B554C;
        }
        c.V0 = 0u;
        goto L800B55B4;
        L800B5524: ;
        c.V0 = 0x00000004u;
        if (c.S0 == c.V0) {
            c.V0 = 0x00000064u;
            goto L800B5564;
        }
        c.V0 = 0x00000064u;
        if (c.S0 == c.V0) {
            c.V0 = 0u;
            goto L800B55A4;
        }
        c.V0 = 0u;
        goto L800B55B4;
        L800B5540: ;
        c.V0 = mem.ReadU8((c.V1 + 0xE8u));
        goto L800B55B4;
        L800B554C: ;
        c.V0 = mem.ReadU16((c.V1 + 0xE6u));
        goto L800B55B4;
        L800B5558: ;
        c.V0 = mem.ReadU8((c.V1 + 0xE4u));
        goto L800B55B4;
        L800B5564: ;
        if ((int)c.S1 >= 0) {
            goto L800B5578;
        }
        c.V0 = mem.ReadU8((c.V1 + 0xE3u));
        goto L800B55B4;
        L800B5578: ;
        c.V0 = mem.ReadU8((c.V1 + 0xE3u));
        c.V0 = (int)c.S1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.S1 << 1;
            goto L800B55B0;
        }
        c.V0 = c.S1 << 1;
        c.V1 = mem.ReadU32(c.V1);
        c.V0 = c.V0 + c.V1;
        c.V0 = mem.ReadU16(c.V0);
        goto L800B55B4;
        L800B55A4: ;
        c.V0 = mem.ReadU32((c.V1 + 0x4Cu));
        goto L800B55B4;
        L800B55B0: ;
        c.V0 = 0u;
        L800B55B4: ;
        c.RA = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x20u;
        return;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x2F50u));
        c.SP = c.SP - 0x20u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.S0 = c.A1;
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.RA);
        c.S1 = c.A2;
        c.RA = 0x800B55ECu;
        Dispatcher.Call(c, m, c.V0);
        if ((int)c.S0 >= 0) {
            c.V1 = c.V0;
            goto L800B5600;
        }
        c.V1 = c.V0;
        c.V0 = mem.ReadU8((c.V1 + 0xE9u));
        goto L800B5688;
        L800B5600: ;
        c.V0 = mem.ReadU8((c.V1 + 0xE9u));
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.S0 << 2;
            goto L800B5684;
        }
        c.V0 = c.S0 << 2;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V0 + c.S0;
        c.A2 = c.S1 - 0x1u;
        c.V1 = c.V1 + c.V0;
        c.V0 = c.A2 < 0x00000005u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.A2 << 2;
            goto L800B5684;
        }
        c.V0 = c.A2 << 2;
        c.At = 0x800C0000u;
        c.At = c.At + c.V0;
        c.V0 = mem.ReadU32((c.At + 0x7890u));
        switch (c.V0)
        {
            case 0x800B5648u: goto L800B5648;
            case 0x800B5654u: goto L800B5654;
            case 0x800B5660u: goto L800B5660;
            case 0x800B566Cu: goto L800B566C;
            case 0x800B5678u: goto L800B5678;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B5648: ;
        c.V0 = mem.ReadU8(c.V1);
        goto L800B5688;
        L800B5654: ;
        c.V0 = mem.ReadU8((c.V1 + 0x1u));
        goto L800B5688;
        L800B5660: ;
        c.V0 = mem.ReadU8((c.V1 + 0x2u));
        goto L800B5688;
        L800B566C: ;
        c.V0 = mem.ReadU8((c.V1 + 0x3u));
        goto L800B5688;
        L800B5678: ;
        c.V0 = mem.ReadU8((c.V1 + 0x4u));
        goto L800B5688;
        L800B5684: ;
        c.V0 = 0u;
        L800B5688: ;
        c.RA = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x20u;
        return;
    }

    public static void func_800A105C(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x108u;
        c.V1 = 0x800F0000u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 - 0x2204u;
        mem.WriteU32((c.SP + 0x104u), c.RA);
        mem.WriteU32((c.SP + 0x100u), c.FP);
        mem.WriteU32((c.SP + 0xFCu), c.S7);
        mem.WriteU32((c.SP + 0xF8u), c.S6);
        mem.WriteU32((c.SP + 0xF4u), c.S5);
        mem.WriteU32((c.SP + 0xF0u), c.S4);
        mem.WriteU32((c.SP + 0xECu), c.S3);
        mem.WriteU32((c.SP + 0xE8u), c.S2);
        mem.WriteU32((c.SP + 0xE4u), c.S1);
        mem.WriteU32((c.SP + 0xE0u), c.S0);
        mem.WriteU32((c.SP + 0x108u), c.A0);
        mem.WriteU32((c.SP + 0x10Cu), c.A1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x10u));
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 < c.V0 ? 1u : 0u;
        if (c.V1 == 0u) {
            c.S5 = 0u;
            goto L800A2120;
        }
        c.S5 = 0u;
        c.T1 = mem.ReadU32((c.A0 + 0x8u));
        mem.WriteU32((c.SP + 0x94u), c.T1);
        c.T2 = mem.ReadU32((c.T1 + 0x2Cu));
        mem.WriteU32((c.SP + 0x98u), c.T2);
        c.V0 = mem.ReadU32((c.T2 + 0x4u));
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.SP + 0xA0u), c.V0);
        c.V1 = mem.ReadU16((c.V0 + 0x8u));
        c.V0 = c.V1 & 0x1000u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0xB8u), 0u);
            goto L800A2120;
        }
        mem.WriteU32((c.SP + 0xB8u), 0u);
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            goto L800A2120;
        }
        c.T3 = mem.ReadU32((c.SP + 0xA0u));
        c.S3 = mem.ReadU32((c.T3 + 0x20u));
        c.S4 = mem.ReadU32((c.T3 + 0x14u));
        c.A0 = (uint)(short)mem.ReadU16((c.S3 + 0x12u));
        c.S2 = mem.ReadU32(c.S3);
        if ((int)c.A0 < 0) {
            goto L800A2120;
        }
        c.V0 = mem.ReadU32((c.S2 + 0x8u));
        c.V1 = mem.ReadU32((c.V0 + 0x4u));
        c.V0 = c.A0 << 4;
        c.V1 = c.V1 + c.V0;
        mem.WriteU32((c.SP + 0x9Cu), c.V1);
        c.A0 = (uint)(short)mem.ReadU16((c.S3 + 0x10u));
        c.T7 = mem.ReadU16((c.S3 + 0x10u));
        if ((int)c.A0 < 0) {
            mem.WriteU16((c.SP + 0xA8u), (ushort)c.T7);
            goto L800A2120;
        }
        mem.WriteU16((c.SP + 0xA8u), (ushort)c.T7);
        c.V0 = mem.ReadU16((c.T1 + 0xAu));
        c.V0 = c.V0 & 0x0020u;
        if (c.V0 == 0u) {
            c.FP = c.S5;
            goto L800A117C;
        }
        c.FP = c.S5;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x90u));
        c.FP = c.S5 < c.V0 ? 1u : 0u;
        L800A117C: ;
        c.V1 = mem.ReadU8(c.S4);
        c.V0 = 0x00000031u;
        if (c.V1 != c.V0) {
            c.V1 = c.A0 << 1;
            goto L800A11AC;
        }
        c.V1 = c.A0 << 1;
        c.T1 = mem.ReadU32((c.SP + 0x9Cu));
        c.V1 = mem.ReadU32((c.T1 + 0x8u));
        c.V0 = c.A0 << 1;
        c.V0 = c.V0 + c.V1;
        c.S0 = mem.ReadU16(c.V0);
        goto L800A11CC;
        L800A11AC: ;
        c.T2 = mem.ReadU32((c.SP + 0x9Cu));
        c.V0 = mem.ReadU32((c.T2 + 0x8u));
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU16(c.V1);
        c.S0 = c.V0 - 0x1u;
        L800A11CC: ;
        c.T3 = mem.ReadU32((c.SP + 0xA0u));
        c.T7 = mem.ReadU32((c.SP + 0x9Cu));
        c.V0 = mem.ReadU16((c.T3 + 0xAu));
        c.T7 = mem.ReadU16((c.T7 + 0x2u));
        c.V0 = c.V0 & 0x0010u;
        if (c.V0 == 0u) {
            mem.WriteU16((c.SP + 0xB0u), (ushort)c.T7);
            goto L800A121C;
        }
        mem.WriteU16((c.SP + 0xB0u), (ushort)c.T7);
        c.V0 = mem.ReadU16((c.T3 + 0x8u));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            goto L800A1210;
        }
        c.V0 = mem.ReadU32(0x00000034u);
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.SP + 0xB8u), c.V0);
        goto L800A121C;
        L800A1210: ;
        c.T1 = mem.ReadU32((c.S3 + 0x1Cu));
        mem.WriteU32((c.SP + 0xB8u), c.T1);
        L800A121C: ;
        c.T2 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU16((c.T2 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800A1244;
        }
        c.A1 = mem.ReadU32((c.T2 + 0xCu));
        goto L800A1258;
        L800A1244: ;
        c.T3 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU32((c.T3 + 0xCu));
        c.A1 = c.V0 + 0x60u;
        L800A1258: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x38u));
        if (c.V0 == c.A1) {
            c.A2 = c.SP + 0x80u;
            goto L800A128C;
        }
        c.A2 = c.SP + 0x80u;
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A1);
        c.A0 = c.T7 + 0xA0u;
        c.RA = 0x800A1288u;
        MediEvil_game.func_800A4E3C(c, m);
        c.A2 = c.SP + 0x80u;
        L800A128C: ;
        c.T1 = mem.ReadU32((c.SP + 0x94u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.A0 + 0x38u));
        c.A1 = c.T1 + 0x10u;
        c.RA = 0x800A12A0u;
        MediEvil_game.func_800A4880(c, m);
        c.A1 = c.SP + 0x78u;
        c.T2 = mem.ReadU32((c.SP + 0x10Cu));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU16((c.SP + 0x80u));
        c.A0 = c.T2 + 0xA0u;
        c.A2 = c.A2 + 0x14u;
        c.V0 = mem.ReadU16((c.T0 + 0x14u));
        c.A3 = mem.ReadU16((c.T2 + 0xB4u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x78u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x18u));
        c.V1 = mem.ReadU16((c.SP + 0x84u));
        c.A3 = mem.ReadU16((c.T2 + 0xB8u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x7Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x1Cu));
        c.V1 = mem.ReadU16((c.SP + 0x88u));
        c.A3 = mem.ReadU16((c.T2 + 0xBCu));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x7Cu), (ushort)c.V0);
        c.RA = 0x800A130Cu;
        MediEvil_game.func_800A4880(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = mem.ReadU32((c.T3 + 0x34u));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T3 + 0x14u));
        c.T5 = mem.ReadU32((c.T3 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T3 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T7 = 0x800F0000u;
        c.T7 = c.T7 - 0x263Cu;
        { var _lw = mem.ReadU32(c.T7); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.T7 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.A1 = c.SP + 0x90u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw);  }
        c.T1 = mem.ReadU32((c.SP + 0x98u));
        c.V1 = mem.ReadU32((c.SP + 0x90u));
        c.V0 = TerrainPatch.MeshClipDistance(mem.ReadU16((c.T1 + 0x8u)), m);
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800A2120;
        }
        c.V1 = mem.ReadU16((c.S2 + 0x2u));
        c.V0 = c.V1 & 0x0002u;
        if (c.V0 == 0u) {
            c.A0 = c.S0 << 16;
            goto L800A13CC;
        }
        c.A0 = c.S0 << 16;
        c.V0 = mem.ReadU32((c.S2 + 0x10u));
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.A0 = (uint)((int)c.A0 >> 10);
        c.A0 = c.V0 + c.A0;
        goto L800A13DC;
        L800A13CC: ;
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 == 0u) {
            goto L800A1410;
        }
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        L800A13DC: ;
        c.RA = 0x800A13E4u;
        MediEvil_game.func_8009A4C8(c, m);
        if (c.V0 == 0u) {
            goto L800A2120;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x90u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            goto L800A2120;
        }
        L800A1410: ;
        c.T2 = mem.ReadU32((c.SP + 0x108u));
        c.V1 = mem.ReadU32((c.T2 + 0x20u));
        if (c.V1 == 0u) {
            c.V0 = 0x00000002u;
            goto L800A175C;
        }
        c.V0 = 0x00000002u;
        c.V0 = mem.ReadU32((c.V1 + 0x8u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 != 0u) {
            c.V0 = c.V1 + 0x18u;
            goto L800A159C;
        }
        c.V0 = c.V1 + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        c.T3 = mem.ReadU32((c.SP + 0x94u));
        c.A1 = mem.ReadU32((c.V1 + 0x14u));
        c.A0 = mem.ReadU32((c.T3 + 0xCu));
        if (c.A1 == c.A0) {
            goto L800A1564;
        }
        c.V0 = mem.ReadU16((c.T3 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            c.S0 = 0x1F800000u;
            goto L800A1480;
        }
        c.S0 = 0x1F800000u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A0);
        c.S0 = c.S0 + 0x60u;
        goto L800A1490;
        L800A1480: ;
        c.V0 = c.A1 + 0x60u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.V0);
        c.S0 = c.S0 + 0x60u;
        L800A1490: ;
        c.A2 = c.S0;
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.S1 = c.T7 + 0xA0u;
        c.A0 = c.S1;
        c.RA = 0x800A14ACu;
        MediEvil_game.func_800A4E3C(c, m);
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x38u));
        c.T1 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = mem.ReadU16((c.A2 + 0x14u));
        c.V1 = mem.ReadU16((c.T1 + 0xB4u));
        c.A0 = c.S1;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x78u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x18u));
        c.V1 = mem.ReadU16((c.T1 + 0xB8u));
        c.A1 = c.SP + 0x78u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.T1 + 0xBCu));
        c.A2 = c.S0 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Cu), (ushort)c.V0);
        c.RA = 0x800A14F8u;
        MediEvil_game.func_800A4880(c, m);
        c.T4 = mem.ReadU32(c.S0);
        c.T5 = mem.ReadU32((c.S0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S0 + 0x8u));
        c.T5 = mem.ReadU32((c.S0 + 0xCu));
        c.T6 = mem.ReadU32((c.S0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S0 + 0x14u));
        c.T5 = mem.ReadU32((c.S0 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S0 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x108u));
        c.V0 = mem.ReadU32((c.T2 + 0x20u));
        c.V0 = c.V0 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        goto L800A1578;
        L800A1564: ;
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = c.V1 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        L800A1578: ;
        c.T3 = mem.ReadU32((c.SP + 0x108u));
        c.V0 = mem.ReadU32((c.T3 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        c.A0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 + c.A0;
        mem.WriteU32((c.V0 + 0x10u), c.V1);
        L800A159C: ;
        c.T7 = mem.ReadU32((c.SP + 0x108u));
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.T7 + 0x20u));
        c.V0 = c.V0 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        mem.WriteU32((c.SP + 0x50u), c.V0);
        c.V0 = mem.ReadU32((c.T7 + 0x20u));
        c.V0 = mem.ReadU16((c.V0 + 0xEu));
        mem.WriteU16((c.SP + 0x54u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.T7 + 0x20u));
        c.V1 = mem.ReadU16((c.V0 + 0xCu));
        c.V0 = 0x00000001u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        mem.WriteU32((c.SP + 0x58u), c.V0);
        c.V0 = mem.ReadU32((c.T7 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x60u), c.V1);
        c.V0 = mem.ReadU32((c.T7 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x8u));
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800A162C;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A1 = c.V0 - 0x1u;
        goto L800A166C;
        L800A162C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = (uint)((int)c.V1 >> (int)(c.V0 & 31u));
        c.A1 = c.V0;
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800A2120;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800A2120;
        }
        L800A166C: ;
        c.V0 = mem.ReadU32((c.SP + 0x60u));
        c.A0 = mem.ReadU16((c.SP + 0x54u));
        c.V1 = mem.ReadU32((c.SP + 0x58u));
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V1 = c.V1 >> 1;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.SP + 0x56u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.A2 = (uint)((int)c.V0 >> 16);
        c.V0 = 0u - c.A2;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.V0 = c.V0 << (int)(c.A0 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.V0 < 3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800A16C8;
        }
        c.V0 = 0x00000002u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V0 = c.V0 + c.A2;
        mem.WriteU32((c.SP + 0x5Cu), c.V0);
        goto L800A16CC;
        L800A16C8: ;
        mem.WriteU32((c.SP + 0x5Cu), 0u);
        L800A16CC: ;
        c.T1 = mem.ReadU32((c.SP + 0x108u));
        c.A0 = mem.ReadU32((c.T1 + 0x20u));
        c.V1 = mem.ReadU32((c.A0 + 0x8u));
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            c.A3 = 0x00FF0000u;
            goto L800A178C;
        }
        c.A3 = 0x00FF0000u;
        c.A3 = c.A3 | 0xFFFFu;
        c.V0 = c.V1 | 0x0001u;
        c.A1 = c.A1 << 16;
        c.A1 = (uint)((int)c.A1 >> 14);
        c.A2 = 0xFF000000u;
        mem.WriteU32((c.A0 + 0x8u), c.V0);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.A0 = mem.ReadU32((c.SP + 0x50u));
        c.A1 = c.A1 + c.V0;
        c.V1 = mem.ReadU32(c.A0);
        c.V0 = mem.ReadU32(c.A1);
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A3;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A0, c.V1);
        c.A0 = mem.ReadU32(c.A1);
        c.V1 = mem.ReadU32((c.SP + 0x58u));
        c.V0 = mem.ReadU32((c.SP + 0x50u));
        c.A0 = c.A0 & c.A2;
        c.V1 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - 0x4u;
        c.V0 = c.V0 & c.A3;
        c.A0 = c.A0 | c.V0;
        mem.WriteU32(c.A1, c.A0);
        goto L800A178C;
        L800A175C: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x9Cu));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU16((c.A1 + 0x8Eu));
        mem.WriteU32((c.SP + 0x5Cu), c.V0);
        mem.WriteU32((c.SP + 0x60u), 0u);
        mem.WriteU16((c.SP + 0x56u), (ushort)0u);
        mem.WriteU32((c.SP + 0x50u), c.V1);
        mem.WriteU16((c.SP + 0x54u), (ushort)c.A0);
        mem.WriteU32((c.SP + 0x58u), c.A1);
        L800A178C: ;
        c.V0 = mem.ReadU16((c.S2 + 0x6u));
        c.V1 = mem.ReadU32((c.S4 + 0x18u));
        c.T2 = mem.ReadU32((c.SP + 0x108u));
        c.T3 = mem.ReadU32((c.SP + 0xA0u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.SP + 0xA4u), 0u);
        c.V0 = c.V0 + 0x10u;
        mem.WriteU32((c.SP + 0xBCu), c.V0);
        c.V1 = mem.ReadU16((c.T2 + 0xEu));
        c.V0 = mem.ReadU16((c.T3 + 0x8u));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            c.S7 = c.V1 & 0x003Eu;
            goto L800A17E8;
        }
        c.S7 = c.V1 & 0x003Eu;
        c.V0 = mem.ReadU32((c.S5 + 0x2Cu));
        if (c.V0 == 0u) {
            goto L800A17F4;
        }
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.SP + 0xA4u), c.V0);
        goto L800A17F4;
        L800A17E8: ;
        c.S3 = mem.ReadU32((c.S3 + 0x14u));
        mem.WriteU32((c.SP + 0xA4u), c.S3);
        L800A17F4: ;
        c.T7 = mem.ReadU32((c.SP + 0x108u));
        c.V0 = mem.ReadU16((c.T7 + 0x1Eu));
        c.V0 = c.V0 & 0x0003u;
        if (c.V0 != 0u) {
            c.V1 = 0x00030000u;
            goto L800A1858;
        }
        c.V1 = 0x00030000u;
        c.T1 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU32((c.T1 + 0x8u));
        c.V0 = c.V0 & c.V1;
        if (c.V0 != c.V1) {
            goto L800A1858;
        }
        c.T2 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = mem.ReadU16((c.T2 + 0x11Eu));
        if (c.V0 == 0u) {
            goto L800A1878;
        }
        c.V0 = mem.ReadU16((c.T1 + 0xAu));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            goto L800A1878;
        }
        L800A1858: ;
        c.T3 = mem.ReadU32((c.SP + 0x108u));
        c.A0 = mem.ReadU32((c.SP + 0x94u));
        c.A2 = c.T3 + 0x10u;
        c.A1 = mem.ReadU16((c.T3 + 0x1Eu));
        c.A3 = c.T3 + 0x14u;
        c.RA = 0x800A1870u;
        MediEvil_game.func_800AB9BC(c, m);
        mem.WriteU32((c.SP + 0xD0u), c.V0);
        goto L800A18BC;
        L800A1878: ;
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.V1 = mem.ReadU32((c.T7 + 0x12Cu));
        c.V0 = 0x1F800000u;
        mem.WriteU32((c.V0 + 0x3Cu), c.V1);
        c.V1 = mem.ReadU32((c.T7 + 0x130u));
        c.V0 = c.V0 + 0x3Cu;
        mem.WriteU32((c.V0 + 0x4u), c.V1);
        c.V1 = mem.ReadU32((c.T7 + 0x134u));
        mem.WriteU32((c.V0 + 0x8u), c.V1);
        c.V1 = mem.ReadU32((c.T7 + 0x138u));
        mem.WriteU32((c.V0 + 0xCu), c.V1);
        c.V1 = mem.ReadU16((c.T7 + 0x13Cu));
        mem.WriteU32((c.SP + 0xD0u), 0u);
        mem.WriteU16((c.V0 + 0x10u), (ushort)c.V1);
        L800A18BC: ;
        c.T1 = mem.ReadU16((c.SP + 0xB0u));
        c.V0 = c.T1 << 16;
        if ((int)c.V0 <= 0) {
            mem.WriteU16((c.SP + 0xC8u), (ushort)0u);
            goto L800A2098;
        }
        mem.WriteU16((c.SP + 0xC8u), (ushort)0u);
        c.S6 = c.SP + 0x30u;
        L800A18D4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T2 = mem.ReadU32((c.SP + 0xA4u));
        if (c.T2 == 0u) {
            goto L800A1930;
        }
        c.T3 = mem.ReadU16((c.SP + 0xC8u));
        c.V0 = c.T3 << 16;
        c.V0 = (uint)((int)c.V0 >> 16);
        c.V0 = c.T2 + c.V0;
        c.V1 = mem.ReadU8(c.V0);
        c.V0 = c.V1 & 0x0008u;
        if (c.V0 != 0u) {
            goto L800A2048;
        }
        c.T7 = mem.ReadU32((c.SP + 0xA0u));
        c.V0 = mem.ReadU16((c.T7 + 0xAu));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.V1 & 0x0001u;
            goto L800A1930;
        }
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 == 0u) {
            goto L800A2048;
        }
        L800A1930: ;
        c.T1 = mem.ReadU32((c.SP + 0xBCu));
        c.T2 = mem.ReadU32((c.SP + 0xA0u));
        c.S1 = mem.ReadU32((c.T1 + 0x10u));
        c.S0 = mem.ReadU16((c.T1 + 0x8u));
        c.V0 = mem.ReadU32((c.T1 + 0xCu));
        c.V1 = mem.ReadU16((c.T2 + 0xAu));
        c.S5 = mem.ReadU32(c.V0);
        c.T3 = mem.ReadU32((c.V0 + 0x8u));
        c.V1 = c.V1 & 0x0010u;
        mem.WriteU32((c.SP + 0xC0u), c.T3);
        c.S4 = mem.ReadU32((c.V0 + 0x4u));
        if (c.V1 == 0u) {
            goto L800A19BC;
        }
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.A1 = mem.ReadU32((c.SP + 0xB8u));
        c.A0 = c.T7 + 0xA0u;
        c.RA = 0x800A197Cu;
        MediEvil_game.func_800A4E3C(c, m);
        c.T1 = mem.ReadU32((c.SP + 0xB8u));
        c.T2 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = mem.ReadU16((c.T1 + 0x14u));
        c.V1 = mem.ReadU16((c.T2 + 0xB4u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x78u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T1 + 0x18u));
        c.V1 = mem.ReadU16((c.T2 + 0xB8u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T1 + 0x1Cu));
        c.V1 = mem.ReadU16((c.T2 + 0xBCu));
        c.S2 = c.T1;
        goto L800A1AC8;
        L800A19BC: ;
        c.T3 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU16((c.T3 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800A19E4;
        }
        c.S3 = mem.ReadU32((c.T3 + 0xCu));
        goto L800A19F8;
        L800A19E4: ;
        c.T7 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU32((c.T7 + 0xCu));
        c.S3 = c.V0 + 0x60u;
        L800A19F8: ;
        c.A0 = mem.ReadU32((c.SP + 0xA0u));
        c.T1 = mem.ReadU32((c.SP + 0x9Cu));
        c.T2 = mem.ReadU16((c.SP + 0xC8u));
        c.T3 = mem.ReadU16((c.SP + 0xA8u));
        c.A1 = c.SP + 0x20u;
        mem.WriteU16((c.SP + 0x24u), (ushort)0u);
        mem.WriteU32((c.SP + 0x20u), c.T1);
        mem.WriteU16((c.SP + 0x26u), (ushort)c.T2);
        mem.WriteU16((c.SP + 0x28u), (ushort)c.T3);
        c.RA = 0x800A1A20u;
        MediEvil_game.func_800A2E14(c, m);
        c.V1 = 0x800F0000u;
        c.S2 = c.V1 + 0x3854u;
        c.A0 = c.S3;
        c.A1 = c.V0;
        c.A2 = c.S2;
        c.RA = 0x800A1A38u;
        MediEvil_game.func_800A4E3C(c, m);
        c.A0 = c.S3;
        c.A1 = 0x1F800000u;
        c.A1 = c.A1 + 0x80u;
        c.A2 = c.S2 + 0x14u;
        c.RA = 0x800A1A4Cu;
        MediEvil_game.func_800A4880(c, m);
        c.A1 = c.S2;
        c.V0 = mem.ReadU32((c.S2 + 0x14u));
        c.V1 = mem.ReadU32((c.S3 + 0x14u));
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = c.V0 + c.V1;
        mem.WriteU32((c.S2 + 0x14u), c.V0);
        c.V0 = mem.ReadU32((c.S2 + 0x18u));
        c.V1 = mem.ReadU32((c.S3 + 0x18u));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.V0 = c.V0 + c.V1;
        mem.WriteU32((c.S2 + 0x18u), c.V0);
        c.V0 = mem.ReadU32((c.S2 + 0x1Cu));
        c.V1 = mem.ReadU32((c.S3 + 0x1Cu));
        c.A0 = c.T7 + 0xA0u;
        c.V0 = c.V0 + c.V1;
        mem.WriteU32((c.S2 + 0x1Cu), c.V0);
        c.RA = 0x800A1A94u;
        MediEvil_game.func_800A4E3C(c, m);
        c.T1 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = mem.ReadU16((c.S2 + 0x14u));
        c.V1 = mem.ReadU16((c.T1 + 0xB4u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x78u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S2 + 0x18u));
        c.V1 = mem.ReadU16((c.T1 + 0xB8u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.T1 + 0xBCu));
        L800A1AC8: ;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Cu), (ushort)c.V0);
        c.T2 = mem.ReadU32((c.SP + 0x10Cu));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.A1 = c.SP + 0x78u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.S2);
        c.A0 = c.T2 + 0xA0u;
        c.A2 = c.A2 + 0x14u;
        c.RA = 0x800A1AF8u;
        MediEvil_game.func_800A4880(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = mem.ReadU32((c.T3 + 0x34u));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T3 + 0x14u));
        c.T5 = mem.ReadU32((c.T3 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T3 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T7 = mem.ReadU32((c.SP + 0x98u));
        c.V1 = mem.ReadU32(c.T7);
        c.V0 = c.V1 & 0x0008u;
        if (c.V0 != 0u) {
            c.A0 = 0x1F800000u;
            goto L800A1BC0;
        }
        c.A0 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.SP + 0xC0u));
        if (c.T1 == 0u) {
            c.V0 = c.V1 & 0x0010u;
            goto L800A1BC0;
        }
        c.V0 = c.V1 & 0x0010u;
        if (c.V0 == 0u) {
            c.A0 = c.T1;
            goto L800A1B88;
        }
        c.A0 = c.T1;
        c.A1 = c.SP + 0x90u;
        c.RA = 0x800A1B80u;
        MediEvil_game.func_8009A61C(c, m);
        goto L800A1B94;
        L800A1B88: ;
        c.A0 = mem.ReadU32((c.SP + 0xC0u));
        c.A1 = c.SP + 0x90u;
        c.RA = 0x800A1B94u;
        MediEvil_game.func_8009A4C8(c, m);
        L800A1B94: ;
        if (c.V0 == 0u) {
            goto L800A2048;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x90u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            c.A0 = 0x1F800000u;
            goto L800A2048;
        }
        c.A0 = 0x1F800000u;
        L800A1BC0: ;
        c.A0 = c.A0 + 0x3Cu;
        c.A1 = c.S2;
        c.T2 = mem.ReadU32((c.SP + 0x108u));
        c.A2 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.T2 + 0xEu));
        c.A2 = c.A2 + 0x60u;
        c.V0 = c.V0 | 0x0001u;
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.V0);
        c.RA = 0x800A1BE4u;
        MediEvil_game.func_800A4E3C(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = c.T3 + 0x60u;
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        c.T7 = 0x1F800000u;
        c.T7 = mem.ReadU32((c.T7 + 0x34u));
        c.T4 = mem.ReadU32(c.T7);
        c.T5 = mem.ReadU32((c.T7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T7 + 0x8u));
        c.T5 = mem.ReadU32((c.T7 + 0xCu));
        c.T6 = mem.ReadU32((c.T7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T7 + 0x14u));
        c.T5 = mem.ReadU32((c.T7 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T7 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        if (c.S0 == 0u) {
            goto L800A2048;
        }
        L800A1C68: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.S1);
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.S1 = c.S1 + 0x4u;
            goto L800A203C;
        }
        c.S1 = c.S1 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + 0x6CD0u;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800A1CA4u: goto L800A1CA4;
            case 0x800A1D00u: goto L800A1D00;
            case 0x800A1D5Cu: goto L800A1D5C;
            case 0x800A1DB8u: goto L800A1DB8;
            case 0x800A1E14u: goto L800A1E14;
            case 0x800A1E70u: goto L800A1E70;
            case 0x800A1ECCu: goto L800A1ECC;
            case 0x800A1F28u: goto L800A1F28;
            case 0x800A203Cu: goto L800A203C;
            case 0x800A1F84u: goto L800A1F84;
            case 0x800A1FE0u: goto L800A1FE0;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800A1CA4: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1CD4;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1CCCu;
        MediEvil_game.func_800B0900(c, m);
        goto L800A2034;
        L800A1CD4: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1CF8u;
        MediEvil_game.func_800B76BC(c, m);
        goto L800A2034;
        L800A1D00: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1D30;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1D28u;
        MediEvil_game.func_800B0AC0(c, m);
        goto L800A2034;
        L800A1D30: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1D54u;
        MediEvil_game.func_800B7A04(c, m);
        goto L800A2034;
        L800A1D5C: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1D8C;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1D84u;
        MediEvil_game.func_800B0068(c, m);
        goto L800A2034;
        L800A1D8C: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1DB0u;
        MediEvil_game.func_800B59C8(c, m);
        goto L800A2034;
        L800A1DB8: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1DE8;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1DE0u;
        MediEvil_game.func_800B0240(c, m);
        goto L800A2034;
        L800A1DE8: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1E0Cu;
        MediEvil_game.func_800B5DA4(c, m);
        goto L800A2034;
        L800A1E14: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1E44;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1E3Cu;
        MediEvil_game.func_800B0CB8(c, m);
        goto L800A2034;
        L800A1E44: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1E68u;
        MediEvil_game.func_800B7DBC(c, m);
        goto L800A2034;
        L800A1E70: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1EA0;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1E98u;
        MediEvil_game.func_800B0EA4(c, m);
        goto L800A2034;
        L800A1EA0: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1EC4u;
        MediEvil_game.func_800B8288(c, m);
        goto L800A2034;
        L800A1ECC: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1EFC;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1EF4u;
        MediEvil_game.func_800B0458(c, m);
        goto L800A2034;
        L800A1EFC: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1F20u;
        MediEvil_game.func_800B6238(c, m);
        goto L800A2034;
        L800A1F28: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1F58;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1F50u;
        MediEvil_game.func_800B065C(c, m);
        goto L800A2034;
        L800A1F58: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1F7Cu;
        MediEvil_game.func_800B6724(c, m);
        goto L800A2034;
        L800A1F84: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1FB4;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1FACu;
        MediEvil_game.func_800B6E70(c, m);
        goto L800A2034;
        L800A1FB4: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1FD8u;
        MediEvil_game.func_800B89A8(c, m);
        goto L800A2034;
        L800A1FE0: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A2010;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A2008u;
        MediEvil_game.func_800B7224(c, m);
        goto L800A2034;
        L800A2010: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A2034u;
        MediEvil_game.func_800B906C(c, m);
        L800A2034: ;
        c.S1 = mem.ReadU32((c.SP + 0x70u));
        c.S0 = mem.ReadU32((c.SP + 0x74u));
        L800A203C: ;
        if (c.S0 != 0u) {
            goto L800A1C68;
        }
        L800A2048: ;
        c.T1 = mem.ReadU32((c.SP + 0xA0u));
        c.T2 = mem.ReadU32((c.SP + 0xBCu));
        c.V0 = mem.ReadU16((c.T1 + 0xAu));
        c.T2 = c.T2 + 0x2Cu;
        c.V0 = c.V0 & 0x0010u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0xBCu), c.T2);
            goto L800A2074;
        }
        mem.WriteU32((c.SP + 0xBCu), c.T2);
        c.T3 = mem.ReadU32((c.SP + 0xB8u));
        c.T3 = c.T3 + 0x20u;
        mem.WriteU32((c.SP + 0xB8u), c.T3);
        L800A2074: ;
        c.T7 = mem.ReadU16((c.SP + 0xC8u));
        c.T1 = mem.ReadU16((c.SP + 0xB0u));
        c.V0 = c.T7 + 0x1u;
        mem.WriteU16((c.SP + 0xC8u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.V1 = c.T1 << 16;
        c.V0 = (int)c.V0 < (int)c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800A18D4;
        }
        L800A2098: ;
        c.T2 = mem.ReadU32((c.SP + 0xD0u));
        c.V0 = c.T2 & 0x0001u;
        if (c.V0 == 0u) {
            goto L800A20E0;
        }
        c.T3 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = c.T3 + 0x14Cu;
        c.T4 = mem.ReadU32(c.V0);
        c.T5 = mem.ReadU32((c.V0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(16, c.T4);
        RecompOne.Runtime.Gte.WriteControl(17, c.T5);
        c.T4 = mem.ReadU32((c.V0 + 0x8u));
        c.T5 = mem.ReadU32((c.V0 + 0xCu));
        c.T6 = mem.ReadU32((c.V0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(18, c.T4);
        RecompOne.Runtime.Gte.WriteControl(19, c.T5);
        RecompOne.Runtime.Gte.WriteControl(20, c.T6);
        L800A20E0: ;
        c.T7 = mem.ReadU32((c.SP + 0xD0u));
        c.V0 = c.T7 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800A2120;
        }
        c.T1 = mem.ReadU32((c.SP + 0x10Cu));
        c.T2 = mem.ReadU8((c.T1 + 0x16Cu));
        c.T3 = mem.ReadU8((c.T1 + 0x16Du));
        c.T1 = mem.ReadU8((c.T1 + 0x16Eu));
        c.T4 = c.T2 << 4;
        c.T5 = c.T3 << 4;
        c.T6 = c.T1 << 4;
        RecompOne.Runtime.Gte.WriteControl(13, c.T4);
        RecompOne.Runtime.Gte.WriteControl(14, c.T5);
        RecompOne.Runtime.Gte.WriteControl(15, c.T6);
        L800A2120: ;
        c.RA = mem.ReadU32((c.SP + 0x104u));
        c.FP = mem.ReadU32((c.SP + 0x100u));
        c.S7 = mem.ReadU32((c.SP + 0xFCu));
        c.S6 = mem.ReadU32((c.SP + 0xF8u));
        c.S5 = mem.ReadU32((c.SP + 0xF4u));
        c.S4 = mem.ReadU32((c.SP + 0xF0u));
        c.S3 = mem.ReadU32((c.SP + 0xECu));
        c.S2 = mem.ReadU32((c.SP + 0xE8u));
        c.S1 = mem.ReadU32((c.SP + 0xE4u));
        c.S0 = mem.ReadU32((c.SP + 0xE0u));
        c.SP = c.SP + 0x108u;
        return;
    }
}
