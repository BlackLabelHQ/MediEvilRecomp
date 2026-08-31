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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(16, (c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(17, (c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(18, (c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(19, (c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(20, (c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(21, (c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(22, (c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(23, (c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(30, (c.SP + 0x30u), c.FP);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x34u), c.T1);
        mem.WriteU32((c.SP + 0x40u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.SP + 0x40u), c.T4);
        mem.WriteU32((c.SP + 0x38u), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0x38u), c.T2);
        mem.WriteU32((c.SP + 0x3Cu), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.SP + 0x3Cu), c.T3);
        c.S6 = 0x09000000u;
        c.S7 = 0x0C000000u;
        c.FP = 0x00FF0000u;
        c.FP = c.FP | 0xFFFFu;
        c.T6 = mem.ReadU32((c.V0 + 0x14u));
        c.T7 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x58u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.SP + 0x58u), c.T6);
        mem.WriteU32((c.SP + 0x5Cu), c.T7);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(15, (c.SP + 0x5Cu), c.T7);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0x3Cu), c.T0);
        mem.WriteU32((c.SP + 0x38u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x38u), c.T1);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0x44u), c.T0);
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0x4Cu), c.T2);
        mem.WriteU32((c.SP + 0x48u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x48u), c.T1);
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
            RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
            goto L80022A80;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(24);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
        c.T5 = 0xFF000000u;
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        c.T5 = c.T5 | CullMaskX;
        if ((int)c.T0 <= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
            goto L80021DEC;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(12);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 12, c.T1);
        c.T2 = RecompOne.Runtime.Gte.Read(13);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 13, c.T2);
        c.T3 = RecompOne.Runtime.Gte.Read(14);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(11, 14, c.T3);
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T2) & c.T7;
        c.T7 = CullBias(c.T3) & c.T7;
        if (c.T7 != 0u) {
            goto L80021DEC;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.A0 + 0x8u), c.T1);
        mem.WriteU32((c.A0 + 0x14u), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.A0 + 0x14u), c.T2);
        mem.WriteU32((c.A0 + 0x20u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.A0 + 0x20u), c.T3);
        c.T0 = RecompOne.Runtime.Gte.Read(17);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 17, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 18, c.T1);
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021EFC;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021EFC: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 19, c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 17, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 18, c.T1);
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021F58;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021F58: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 19, c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 7, c.T0);
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L80021F90: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80021FA4;
        }
        c.T0 = c.T1 + 0u;
        L80021FA4: ;
        c.T1 = (int)c.T0 < 4 ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x5Cu));
            goto L80021DEC;
        }
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(5, c.T1, c.A1);
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x5Cu), c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 17, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 18, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xACu), c.T0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T0, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        goto L80021DEC;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T0, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        goto L80021DEC;
        L80022160: ;
        c.T0 = c.A0 + 0x28u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        mem.WriteU32((c.SP + 0x98u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0x98u), c.T0);
        mem.WriteU32((c.SP + 0x9Cu), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x9Cu), c.T1);
        mem.WriteU32((c.SP + 0xA0u), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0xA0u), c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x20u), c.T6);
        mem.WriteU32((c.T1 + 0x20u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x20u), c.T6);
        mem.WriteU32((c.T2 + 0x20u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T2 + 0x20u), c.T6);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x1Cu), c.T6);
        mem.WriteU32((c.T1 + 0x1Cu), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x1Cu), c.T6);
        mem.WriteU32((c.T2 + 0x1Cu), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T2 + 0x1Cu), c.T6);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.T0 + 0x8u), c.T3);
        mem.WriteU32((c.T0 + 0x14u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T0 + 0x14u), c.T4);
        mem.WriteU32((c.T0 + 0x4u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T0 + 0x4u), c.T5);
        mem.WriteU32((c.T0 + 0x10u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x10u), c.T6);
        mem.WriteU32((c.T1 + 0x8u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x8u), c.T4);
        mem.WriteU32((c.T1 + 0x4u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x4u), c.T6);
        mem.WriteU32((c.T2 + 0x14u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.T2 + 0x14u), c.T3);
        mem.WriteU32((c.T2 + 0x10u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T2 + 0x10u), c.T5);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.T1 + 0x14u), c.T3);
        mem.WriteU32((c.T1 + 0x10u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x10u), c.T4);
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T5);
        mem.WriteU32((c.T2 + 0x8u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.T2 + 0x8u), c.T3);
        mem.WriteU32((c.T2 + 0x4u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T2 + 0x4u), c.T4);
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T5);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0x44u), c.T0);
        mem.WriteU32((c.SP + 0x48u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x48u), c.T1);
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0x4Cu), c.T2);
        mem.WriteU32((c.SP + 0x50u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.SP + 0x50u), c.T3);
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        mem.WriteU32((c.SP + 0x54u), RecompOne.Runtime.Gte.Read(12));
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022A80;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
            goto L80022A80;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L800224B8;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
            goto L80021DEC;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
        L800224B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 16, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(17);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 17, c.T1);
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800224D4;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800224D4: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 18, c.T0);
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800224EC;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800224EC: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 16, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(17);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 17, c.T1);
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022548;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022548: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 18, c.T0);
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L80022560;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L80022560: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 7, c.T0);
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002259C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800225B0;
        }
        c.T0 = c.T1 + 0u;
        L800225B0: ;
        c.T1 = (int)c.T0 < 4 ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            goto L80021DEC;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(11, 12, c.T3);
        c.T4 = RecompOne.Runtime.Gte.Read(13);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(12, 13, c.T4);
        c.T6 = RecompOne.Runtime.Gte.Read(14);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(14, 14, c.T6);
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            goto L80021DEC;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.A0 + 0x8u), c.T1);
        mem.WriteU32((c.A0 + 0x14u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.A0 + 0x14u), c.T3);
        mem.WriteU32((c.A0 + 0x20u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.A0 + 0x20u), c.T4);
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.A0 + 0x2Cu), c.T6);
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(5, c.T1, c.A1);
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x5Cu), c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 16, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 17, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 18, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xACu), c.T0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T0, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T0 + 0x20u), c.T8);
        mem.WriteU32((c.T1 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T1 + 0x20u), c.T8);
        mem.WriteU32((c.T2 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T2 + 0x20u), c.T8);
        mem.WriteU32((c.T3 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T3 + 0x20u), c.T8);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T0 + 0x1Cu), c.T4);
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x1Cu), c.T4);
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T2 + 0x1Cu), c.T4);
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T3 + 0x1Cu), c.T4);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T0 + 0x8u), c.T9);
        mem.WriteU32((c.T0 + 0x14u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T0 + 0x14u), c.T4);
        mem.WriteU32((c.T0 + 0x4u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T0 + 0x4u), c.T5);
        mem.WriteU32((c.T0 + 0x10u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x10u), c.T6);
        mem.WriteU32((c.T1 + 0x8u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x8u), c.T4);
        mem.WriteU32((c.T1 + 0x4u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x4u), c.T6);
        mem.WriteU32((c.T3 + 0x14u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T3 + 0x14u), c.T9);
        mem.WriteU32((c.T3 + 0x10u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T3 + 0x10u), c.T5);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T1 + 0x14u), c.T5);
        mem.WriteU32((c.T1 + 0x10u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x10u), c.T6);
        mem.WriteU32((c.T2 + 0x8u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T2 + 0x8u), c.T5);
        mem.WriteU32((c.T2 + 0x14u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T2 + 0x14u), c.T9);
        mem.WriteU32((c.T2 + 0x4u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T2 + 0x4u), c.T6);
        mem.WriteU32((c.T2 + 0x10u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T2 + 0x10u), c.T4);
        mem.WriteU32((c.T3 + 0x4u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T3 + 0x4u), c.T4);
        mem.WriteU32((c.T3 + 0x8u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T3 + 0x8u), c.T9);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.A0 = c.T3 + 0x28u;
        goto L80021DEC;
        L80022A80: ;
        L80022A88: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(18, (c.V0 + 0x34u), c.S2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.V0 + 0x10u), c.T1);
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
            MediEvil.func_80021DC4(c, m);
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
            MediEvil.func_800223F0(c, m);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0x44u), c.T0);
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0x4Cu), c.T2);
        mem.WriteU32((c.SP + 0x48u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x48u), c.T1);
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            MediEvil.func_80022A80(c, m);
            return;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            c.T0 = RecompOne.Runtime.Gte.Read(24);
            RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
            MediEvil.func_80022A80(c, m);
            return;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(24);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
        c.T5 = 0xFF000000u;
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        c.T5 = c.T5 | CullMaskX;
        if ((int)c.T0 <= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
            goto L80021DEC;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(12);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 12, c.T1);
        c.T2 = RecompOne.Runtime.Gte.Read(13);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 13, c.T2);
        c.T3 = RecompOne.Runtime.Gte.Read(14);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(11, 14, c.T3);
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T2) & c.T7;
        c.T7 = CullBias(c.T3) & c.T7;
        if (c.T7 != 0u) {
            goto L80021DEC;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.A0 + 0x8u), c.T1);
        mem.WriteU32((c.A0 + 0x14u), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.A0 + 0x14u), c.T2);
        mem.WriteU32((c.A0 + 0x20u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.A0 + 0x20u), c.T3);
        c.T0 = RecompOne.Runtime.Gte.Read(17);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 17, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 18, c.T1);
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021EFC;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021EFC: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 19, c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 17, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 18, c.T1);
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021F58;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021F58: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 19, c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 7, c.T0);
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L80021F90: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80021FA4;
        }
        c.T0 = c.T1 + 0u;
        L80021FA4: ;
        c.T1 = (int)c.T0 < 4 ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x5Cu));
            goto L80021DEC;
        }
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(5, c.T1, c.A1);
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x5Cu), c.T1);
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
        MediEvil.func_80022000(c, m);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 17, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 18, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xACu), c.T0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T0, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        MediEvil.func_80021DEC(c, m);
        return;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T0, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        MediEvil.func_80021DEC(c, m);
        return;
        L80022160: ;
        c.T0 = c.A0 + 0x28u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        mem.WriteU32((c.SP + 0x98u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0x98u), c.T0);
        mem.WriteU32((c.SP + 0x9Cu), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x9Cu), c.T1);
        mem.WriteU32((c.SP + 0xA0u), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0xA0u), c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x20u), c.T6);
        mem.WriteU32((c.T1 + 0x20u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x20u), c.T6);
        mem.WriteU32((c.T2 + 0x20u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T2 + 0x20u), c.T6);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x1Cu), c.T6);
        mem.WriteU32((c.T1 + 0x1Cu), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x1Cu), c.T6);
        mem.WriteU32((c.T2 + 0x1Cu), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T2 + 0x1Cu), c.T6);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.T0 + 0x8u), c.T3);
        mem.WriteU32((c.T0 + 0x14u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T0 + 0x14u), c.T4);
        mem.WriteU32((c.T0 + 0x4u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T0 + 0x4u), c.T5);
        mem.WriteU32((c.T0 + 0x10u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x10u), c.T6);
        mem.WriteU32((c.T1 + 0x8u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x8u), c.T4);
        mem.WriteU32((c.T1 + 0x4u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x4u), c.T6);
        mem.WriteU32((c.T2 + 0x14u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.T2 + 0x14u), c.T3);
        mem.WriteU32((c.T2 + 0x10u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T2 + 0x10u), c.T5);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.T1 + 0x14u), c.T3);
        mem.WriteU32((c.T1 + 0x10u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x10u), c.T4);
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T5);
        mem.WriteU32((c.T2 + 0x8u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.T2 + 0x8u), c.T3);
        mem.WriteU32((c.T2 + 0x4u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T2 + 0x4u), c.T4);
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T5);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.A0 = c.T2 + 0x28u;
        MediEvil.func_80021DEC(c, m);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0x44u), c.T0);
        mem.WriteU32((c.SP + 0x48u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x48u), c.T1);
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0x4Cu), c.T2);
        mem.WriteU32((c.SP + 0x50u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.SP + 0x50u), c.T3);
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        mem.WriteU32((c.SP + 0x54u), RecompOne.Runtime.Gte.Read(12));
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022A80;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
            goto L80022A80;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L800224B8;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
            MediEvil.func_80021DEC(c, m);
            return;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
        L800224B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 16, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(17);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 17, c.T1);
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800224D4;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800224D4: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 18, c.T0);
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800224EC;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800224EC: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
            MediEvil.func_80021DEC(c, m);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 16, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(17);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 17, c.T1);
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022548;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022548: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 18, c.T0);
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L80022560;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L80022560: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 7, c.T0);
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002259C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800225B0;
        }
        c.T0 = c.T1 + 0u;
        L800225B0: ;
        c.T1 = (int)c.T0 < 4 ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            MediEvil.func_80021DEC(c, m);
            return;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(11, 12, c.T3);
        c.T4 = RecompOne.Runtime.Gte.Read(13);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(12, 13, c.T4);
        c.T6 = RecompOne.Runtime.Gte.Read(14);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(14, 14, c.T6);
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            MediEvil.func_80021DEC(c, m);
            return;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.A0 + 0x8u), c.T1);
        mem.WriteU32((c.A0 + 0x14u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.A0 + 0x14u), c.T3);
        mem.WriteU32((c.A0 + 0x20u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.A0 + 0x20u), c.T4);
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.A0 + 0x2Cu), c.T6);
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(5, c.T1, c.A1);
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x5Cu), c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 16, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 17, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 18, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xACu), c.T0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T0, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x34u;
        MediEvil.func_80021DEC(c, m);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T0 + 0x20u), c.T8);
        mem.WriteU32((c.T1 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T1 + 0x20u), c.T8);
        mem.WriteU32((c.T2 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T2 + 0x20u), c.T8);
        mem.WriteU32((c.T3 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T3 + 0x20u), c.T8);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T0 + 0x1Cu), c.T4);
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x1Cu), c.T4);
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T2 + 0x1Cu), c.T4);
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T3 + 0x1Cu), c.T4);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T0 + 0x8u), c.T9);
        mem.WriteU32((c.T0 + 0x14u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T0 + 0x14u), c.T4);
        mem.WriteU32((c.T0 + 0x4u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T0 + 0x4u), c.T5);
        mem.WriteU32((c.T0 + 0x10u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x10u), c.T6);
        mem.WriteU32((c.T1 + 0x8u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x8u), c.T4);
        mem.WriteU32((c.T1 + 0x4u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x4u), c.T6);
        mem.WriteU32((c.T3 + 0x14u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T3 + 0x14u), c.T9);
        mem.WriteU32((c.T3 + 0x10u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T3 + 0x10u), c.T5);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T1 + 0x14u), c.T5);
        mem.WriteU32((c.T1 + 0x10u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x10u), c.T6);
        mem.WriteU32((c.T2 + 0x8u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T2 + 0x8u), c.T5);
        mem.WriteU32((c.T2 + 0x14u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T2 + 0x14u), c.T9);
        mem.WriteU32((c.T2 + 0x4u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T2 + 0x4u), c.T6);
        mem.WriteU32((c.T2 + 0x10u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T2 + 0x10u), c.T4);
        mem.WriteU32((c.T3 + 0x4u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T3 + 0x4u), c.T4);
        mem.WriteU32((c.T3 + 0x8u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T3 + 0x8u), c.T9);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.A0 = c.T3 + 0x28u;
        MediEvil.func_80021DEC(c, m);
        return;
        L80022A80: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(18, (c.V0 + 0x34u), c.S2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.V0 + 0x10u), c.T1);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0x44u), c.T0);
        mem.WriteU32((c.SP + 0x48u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x48u), c.T1);
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0x4Cu), c.T2);
        mem.WriteU32((c.SP + 0x50u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.SP + 0x50u), c.T3);
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        mem.WriteU32((c.SP + 0x54u), RecompOne.Runtime.Gte.Read(12));
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022A80;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
            goto L80022A80;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L800224B8;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 24, c.T0);
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
            MediEvil.func_80021DEC(c, m);
            return;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xA8u), c.T0);
        L800224B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 16, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(17);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 17, c.T1);
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800224D4;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800224D4: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 18, c.T0);
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800224EC;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800224EC: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
            MediEvil.func_80021DEC(c, m);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 16, c.T0);
        c.T1 = RecompOne.Runtime.Gte.Read(17);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(9, 17, c.T1);
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022548;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022548: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 18, c.T0);
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L80022560;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L80022560: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(8, 7, c.T0);
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002259C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800225B0;
        }
        c.T0 = c.T1 + 0u;
        L800225B0: ;
        c.T1 = (int)c.T0 < 4 ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            MediEvil.func_80021DEC(c, m);
            return;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(11, 12, c.T3);
        c.T4 = RecompOne.Runtime.Gte.Read(13);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(12, 13, c.T4);
        c.T6 = RecompOne.Runtime.Gte.Read(14);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(14, 14, c.T6);
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            MediEvil.func_80021DEC(c, m);
            return;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.A0 + 0x8u), c.T1);
        mem.WriteU32((c.A0 + 0x14u), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.A0 + 0x14u), c.T3);
        mem.WriteU32((c.A0 + 0x20u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.A0 + 0x20u), c.T4);
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.A0 + 0x2Cu), c.T6);
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(5, c.T1, c.A1);
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0x5Cu), c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 16, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 17, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 18, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(10, 19, c.T2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xACu), c.T0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T0, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x34u;
        MediEvil.func_80021DEC(c, m);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T0 + 0x20u), c.T8);
        mem.WriteU32((c.T1 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T1 + 0x20u), c.T8);
        mem.WriteU32((c.T2 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T2 + 0x20u), c.T8);
        mem.WriteU32((c.T3 + 0x20u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.T3 + 0x20u), c.T8);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T0 + 0x1Cu), c.T4);
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x1Cu), c.T4);
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T2 + 0x1Cu), c.T4);
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T3 + 0x1Cu), c.T4);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T0 + 0x8u), c.T9);
        mem.WriteU32((c.T0 + 0x14u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T0 + 0x14u), c.T4);
        mem.WriteU32((c.T0 + 0x4u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T0 + 0x4u), c.T5);
        mem.WriteU32((c.T0 + 0x10u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T0 + 0x10u), c.T6);
        mem.WriteU32((c.T1 + 0x8u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T1 + 0x8u), c.T4);
        mem.WriteU32((c.T1 + 0x4u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x4u), c.T6);
        mem.WriteU32((c.T3 + 0x14u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T3 + 0x14u), c.T9);
        mem.WriteU32((c.T3 + 0x10u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T3 + 0x10u), c.T5);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T1 + 0x14u), c.T5);
        mem.WriteU32((c.T1 + 0x10u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T1 + 0x10u), c.T6);
        mem.WriteU32((c.T2 + 0x8u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.T2 + 0x8u), c.T5);
        mem.WriteU32((c.T2 + 0x14u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T2 + 0x14u), c.T9);
        mem.WriteU32((c.T2 + 0x4u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.T2 + 0x4u), c.T6);
        mem.WriteU32((c.T2 + 0x10u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T2 + 0x10u), c.T4);
        mem.WriteU32((c.T3 + 0x4u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.T3 + 0x4u), c.T4);
        mem.WriteU32((c.T3 + 0x8u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.T3 + 0x8u), c.T9);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, c.T8, c.T9);
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(1, c.T9, c.At);
        c.A0 = c.T3 + 0x28u;
        MediEvil.func_80021DEC(c, m);
        return;
        L80022A80: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(18, (c.V0 + 0x34u), c.S2);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.V0 + 0x10u), c.T1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(17, (c.SP + 0x6Cu), c.S1);
        c.S1 = mem.ReadU32((c.GP + 0x460u));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        mem.WriteU32((c.SP + 0x68u), c.S0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(16, (c.SP + 0x68u), c.S0);
        mem.WriteU32((c.SP + 0x84u), c.RA);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(31, (c.SP + 0x84u), c.RA);
        mem.WriteU32((c.SP + 0x80u), c.S6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(22, (c.SP + 0x80u), c.S6);
        mem.WriteU32((c.SP + 0x7Cu), c.S5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(21, (c.SP + 0x7Cu), c.S5);
        mem.WriteU32((c.SP + 0x78u), c.S4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(20, (c.SP + 0x78u), c.S4);
        mem.WriteU32((c.SP + 0x74u), c.S3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(19, (c.SP + 0x74u), c.S3);
        mem.WriteU32((c.SP + 0x70u), c.S2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(18, (c.SP + 0x70u), c.S2);
        c.A1 = mem.ReadU32((c.S1 + 0xCu));
        c.S0 = c.A0 + 0u;
        c.RA = 0x80024ACCu;
        MediEvil.func_800A4E3C(c, m);
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
        MediEvil.func_800A4880(c, m);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x48u), c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.V1 = mem.ReadU8((c.V0 + 0x1u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x4Cu), c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.V1 = mem.ReadU8((c.V0 + 0x2u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x50u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x50u), c.V0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x58u), c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V1 = mem.ReadU8((c.V0 + 0x1u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x5Cu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x5Cu), c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V1 = mem.ReadU8((c.V0 + 0x2u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x60u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x60u), c.V0);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x18u), c.V0);
        c.V0 = c.T0 + 0x14u;
        mem.WriteU32((c.SP + 0x1Cu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x1Cu), c.V0);
        c.V0 = c.T0 + 0x20u;
        mem.WriteU32((c.SP + 0x20u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x20u), c.V0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x18u), c.V0);
        c.V0 = c.T0 + 0x18u;
        mem.WriteU32((c.SP + 0x1Cu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x1Cu), c.V0);
        c.V0 = c.T0 + 0x20u;
        mem.WriteU32((c.SP + 0x20u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x20u), c.V0);
        c.V0 = c.T0 + 0x28u;
        L80024DAC: ;
        mem.WriteU32((c.SP + 0x24u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x24u), c.V0);
        c.S5 = mem.ReadU32((c.SP + 0x18u));
        mem.WriteU32(c.S5, RecompOne.Runtime.Gte.Read(12));
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V0 + 0x4u)));
        RecompOne.Runtime.Gte.Rtps(12, false);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x28u), c.V0);
        c.V0 = c.S3 + 0x10u;
        mem.WriteU32((c.SP + 0x2Cu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x2Cu), c.V0);
        c.V0 = c.S3 + 0x1Cu;
        mem.WriteU32((c.SP + 0x30u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x30u), c.V0);
        c.V0 = c.S3 + 0x28u;
        goto L80024E3C;
        L80024E20: ;
        c.V0 = c.S2 + 0xCu;
        mem.WriteU32((c.SP + 0x28u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x28u), c.V0);
        c.V0 = c.S2 + 0x14u;
        mem.WriteU32((c.SP + 0x2Cu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x2Cu), c.V0);
        c.V0 = c.S2 + 0x1Cu;
        mem.WriteU32((c.SP + 0x30u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x30u), c.V0);
        c.V0 = c.S2 + 0x24u;
        L80024E3C: ;
        mem.WriteU32((c.SP + 0x34u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x34u), c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V1 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x38u), c.V1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.SP + 0x38u), c.V1);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x3Cu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x3Cu), c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x40u), c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x44u), c.V0);
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
        mem.WriteU32(c.S5, RecompOne.Runtime.Gte.Read(12));
        mem.WriteU32(c.S6, RecompOne.Runtime.Gte.Read(13));
        mem.WriteU32(c.S4, RecompOne.Runtime.Gte.Read(14));
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, c.T0, c.V1);
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T7;
        c.V0 = c.V0 & c.T6;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, c.A0, c.V0);
        goto L80025214;
        L800251C0: ;
        c.A3 = 0x00000003u;
        c.A2 = c.T9 + 0xCu;
        L800251C8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32(c.A2);
        c.V1 = mem.ReadU32(c.V0);
        c.V0 = c.V1 & 0xFE00u;
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(17, (c.SP + 0x5Cu), c.S1);
        c.S1 = mem.ReadU32((c.GP + 0x460u));
        mem.WriteU32((c.SP + 0x70u), c.S6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(22, (c.SP + 0x70u), c.S6);
        mem.WriteU32((c.SP + 0x74u), c.RA);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(31, (c.SP + 0x74u), c.RA);
        mem.WriteU32((c.SP + 0x6Cu), c.S5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(21, (c.SP + 0x6Cu), c.S5);
        mem.WriteU32((c.SP + 0x68u), c.S4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(20, (c.SP + 0x68u), c.S4);
        mem.WriteU32((c.SP + 0x64u), c.S3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(19, (c.SP + 0x64u), c.S3);
        mem.WriteU32((c.SP + 0x60u), c.S2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(18, (c.SP + 0x60u), c.S2);
        mem.WriteU32((c.SP + 0x58u), c.S0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(16, (c.SP + 0x58u), c.S0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.S5 + 0x60u), c.V0);
        mem.WriteU32((c.S0 + 0x4u), 0u);
        mem.WriteU32((c.S0 + 0x8u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.S0 + 0x8u), c.V0);
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
        MediEvil.func_800A4880(c, m);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
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
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.SP + 0x50u), c.V1);
            goto L80025BB0;
        }
        mem.WriteU32((c.SP + 0x50u), c.V1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.SP + 0x50u), c.V1);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L80025BB0;
        }
        mem.WriteU32(c.A3, RecompOne.Runtime.Gte.Read(12));
        mem.WriteU32(c.A2, RecompOne.Runtime.Gte.Read(13));
        mem.WriteU32(c.A1, RecompOne.Runtime.Gte.Read(14));
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x50u), c.V0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x30u), c.V0);
        mem.WriteU32((c.SP + 0x34u), 0u);
        mem.WriteU32((c.SP + 0x38u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x38u), c.V0);
        mem.WriteU32((c.SP + 0x3Cu), 0u);
        mem.WriteU16((c.SP + 0x40u), (ushort)c.V0);
        c.A0 = (uint)(short)mem.ReadU16((c.S1 + 0x3Au));
        c.A1 = c.S0 + 0u;
        c.RA = 0x8002582Cu;
        MediEvil.func_800A43E8(c, m);
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x60u;
        c.A1 = c.S0 + 0u;
        c.RA = 0x8002583Cu;
        MediEvil.func_800A4CFC(c, m);
        L8002583C: ;
        c.A1 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.GP + 0x5E0u));
        c.A1 = c.A1 + 0x60u;
        c.A0 = c.A0 + 0x80u;
        c.RA = 0x80025850u;
        MediEvil.func_800A4F4C(c, m);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(12, 9, c.T4);
        c.T5 = RecompOne.Runtime.Gte.Read(10);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(13, 10, c.T5);
        c.T6 = RecompOne.Runtime.Gte.Read(11);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(14, 11, c.T6);
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A2 + 0x10u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.A2 + 0x10u), c.V0);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 0);
        c.V0 = (uint)(short)mem.ReadU16(c.T3);
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.V0 + 0x4u)));
        c.T4 = RecompOne.Runtime.Gte.Read(9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(12, 9, c.T4);
        c.T5 = RecompOne.Runtime.Gte.Read(10);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(13, 10, c.T5);
        c.T6 = RecompOne.Runtime.Gte.Read(11);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(14, 11, c.T6);
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A2 + 0x18u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.A2 + 0x18u), c.V0);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 2, 0);
        c.T4 = RecompOne.Runtime.Gte.Read(9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(12, 9, c.T4);
        c.T5 = RecompOne.Runtime.Gte.Read(10);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(13, 10, c.T5);
        c.T6 = RecompOne.Runtime.Gte.Read(11);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(14, 11, c.T6);
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x10u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            mem.WriteU32((c.A2 + 0x20u), c.V1);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.A2 + 0x20u), c.V1);
            goto L80025A24;
        }
        mem.WriteU32((c.A2 + 0x20u), c.V1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.A2 + 0x20u), c.V1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, c.A2, c.V1);
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.A2 & c.T0;
        c.V0 = c.V0 & c.T7;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, c.A0, c.V0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(12, 9, c.T4);
        c.T5 = RecompOne.Runtime.Gte.Read(10);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(13, 10, c.T5);
        c.T6 = RecompOne.Runtime.Gte.Read(11);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(14, 11, c.T6);
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A1 + 0x10u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.A1 + 0x10u), c.V0);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 0);
        c.T4 = RecompOne.Runtime.Gte.Read(9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(12, 9, c.T4);
        c.T5 = RecompOne.Runtime.Gte.Read(10);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(13, 10, c.T5);
        c.T6 = RecompOne.Runtime.Gte.Read(11);
        RecompOne.Runtime.Pgxp.PgxpCpu.Mfc2(14, 11, c.T6);
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A1 + 0x18u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.A1 + 0x18u), c.V0);
        c.V0 = mem.ReadU32((c.A2 + 0x18u));
        mem.WriteU32((c.A1 + 0x20u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.A1 + 0x20u), c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x10u));
        c.V1 = mem.ReadU32((c.A2 + 0x20u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            mem.WriteU32((c.A1 + 0x28u), c.V1);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.A1 + 0x28u), c.V1);
            goto L80025B50;
        }
        mem.WriteU32((c.A1 + 0x28u), c.V1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.A1 + 0x28u), c.V1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, c.A1, c.V1);
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.A1 & c.T0;
        c.V0 = c.V0 & c.T7;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, c.A0, c.V0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(20, (c.SP + 0x130u), c.S4);
        c.S4 = c.A0 + 0u;
        mem.WriteU32((c.SP + 0x124u), c.S1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(17, (c.SP + 0x124u), c.S1);
        c.S1 = 0x1F800000u;
        mem.WriteU32((c.SP + 0x120u), c.S0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(16, (c.SP + 0x120u), c.S0);
        c.S0 = 0x1F800000u;
        mem.WriteU32((c.SP + 0x144u), c.RA);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(31, (c.SP + 0x144u), c.RA);
        mem.WriteU32((c.SP + 0x140u), c.FP);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(30, (c.SP + 0x140u), c.FP);
        mem.WriteU32((c.SP + 0x13Cu), c.S7);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(23, (c.SP + 0x13Cu), c.S7);
        mem.WriteU32((c.SP + 0x138u), c.S6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(22, (c.SP + 0x138u), c.S6);
        mem.WriteU32((c.SP + 0x134u), c.S5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(21, (c.SP + 0x134u), c.S5);
        mem.WriteU32((c.SP + 0x12Cu), c.S3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(19, (c.SP + 0x12Cu), c.S3);
        mem.WriteU32((c.SP + 0x128u), c.S2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(18, (c.SP + 0x128u), c.S2);
        c.A1 = mem.ReadU32((c.S4 + 0x4u));
        c.A0 = mem.ReadU32((c.S1 + 0x88u));
        c.A2 = mem.ReadU32((c.S0 + 0x34u));
        c.A0 = c.A0 + 0xA0u;
        c.RA = 0x80010854u;
        MediEvil.func_800A4E3C(c, m);
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
        MediEvil.func_800A4880(c, m);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
        c.A1 = c.SP + 0x18u;
        mem.WriteU32(c.A1, RecompOne.Runtime.Gte.Read(12));
        c.A0 = c.SP + 0x20u;
        mem.WriteU32(c.A0, RecompOne.Runtime.Gte.Read(13));
        c.V0 = c.SP + 0x28u;
        mem.WriteU32(c.V0, RecompOne.Runtime.Gte.Read(14));
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
        mem.WriteU32(c.A1, RecompOne.Runtime.Gte.Read(12));
        mem.WriteU32(c.A0, RecompOne.Runtime.Gte.Read(13));
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(4, (c.SP + 0xE8u), c.A0);
        if (c.V0 != 0u) {
            mem.WriteU32((c.S4 + 0x9Cu), c.V1);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.S4 + 0x9Cu), c.V1);
            goto L8001107C;
        }
        mem.WriteU32((c.S4 + 0x9Cu), c.V1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.S4 + 0x9Cu), c.V1);
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
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.S4 + 0x98u), c.V1);
            goto L80010A80;
        }
        mem.WriteU32((c.S4 + 0x98u), c.V1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.S4 + 0x98u), c.V1);
        c.A0 = c.S4 + 0u;
        c.RA = 0x80010A80u;
        MediEvil.func_800104C4(c, m);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.S4 + 0xACu), c.V0);
        c.V0 = (int)c.V0 < (int)c.A0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = 0xFFFFFFFBu;
            goto L80010B40;
        }
        c.V1 = 0xFFFFFFFBu;
        c.V0 = mem.ReadU32(c.S4);
        mem.WriteU32((c.S4 + 0xACu), c.A0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(4, (c.S4 + 0xACu), c.A0);
        c.V0 = CullBias(c.V0) & c.V1;
        mem.WriteU32(c.S4, c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, c.S4, c.V0);
        goto L80010B40;
        L80010AF4: ;
        c.V0 = mem.ReadU32((c.S4 + 0xA8u));
        c.V0 = (uint)((int)c.V0 >> 5);
        c.V0 = c.V1 - c.V0;
        if ((int)c.V0 > 0) {
            mem.WriteU32((c.S4 + 0xACu), c.V0);
            RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.S4 + 0xACu), c.V0);
            goto L80010B40;
        }
        mem.WriteU32((c.S4 + 0xACu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.S4 + 0xACu), c.V0);
        mem.WriteU32((c.S4 + 0xACu), 0u);
        goto L80010B40;
        L80010B14: ;
        c.RA = 0x80010B1Cu;
        MediEvil.func_800A43B8(c, m);
        c.V0 = c.V0 & 0x003Fu;
        if (c.V0 != 0u) {
            goto L80010B40;
        }
        c.V0 = mem.ReadU32((c.S4 + 0xA8u));
        c.V1 = mem.ReadU32(c.S4);
        c.V0 = (uint)((int)c.V0 >> 5);
        c.V1 = c.V1 | 0x0004u;
        mem.WriteU32((c.S4 + 0xACu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.S4 + 0xACu), c.V0);
        mem.WriteU32(c.S4, c.V1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, c.S4, c.V1);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, (c.A0 - 0x20u), c.V1);
        c.V1 = mem.ReadU32((c.S4 + 0x98u));
        c.V0 = mem.ReadU32((c.T1 + 0x78u));
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        c.A1 = c.A1 & c.T0;
        c.V0 = c.V0 & c.A3;
        c.V0 = c.V0 | c.A1;
        mem.WriteU32(c.V1, c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, c.V1, c.V0);
        c.T4 = mem.ReadU32((c.S4 + 0x28u));
        c.S7 = mem.ReadU32((c.S4 + 0x2Cu));
        c.T7 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = c.T4 + 0x8u;
        mem.WriteU32((c.SP + 0x118u), c.T7);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(15, (c.SP + 0x118u), c.T7);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x114u), c.V0);
        goto L80010C2C;
        L80010C20: ;
        c.T8 = mem.ReadU32((c.S4 + 0x8Cu));
        mem.WriteU32((c.SP + 0x114u), c.T8);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(24, (c.SP + 0x114u), c.T8);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(15, (c.SP + 0x11Cu), c.T7);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(15, (c.SP + 0x11Cu), c.T7);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(7, c.V0, c.A3);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(8, (c.SP + 0xF0u), c.T0);
        mem.WriteU32((c.SP + 0xF4u), c.T1);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(9, (c.SP + 0xF4u), c.T1);
        mem.WriteU32((c.SP + 0xF8u), c.T2);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(10, (c.SP + 0xF8u), c.T2);
        mem.WriteU32((c.SP + 0xFCu), c.T3);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(11, (c.SP + 0xFCu), c.T3);
        mem.WriteU32((c.SP + 0x100u), c.T4);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(12, (c.SP + 0x100u), c.T4);
        mem.WriteU32((c.SP + 0x104u), c.T5);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(13, (c.SP + 0x104u), c.T5);
        mem.WriteU32((c.SP + 0x108u), c.T6);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(14, (c.SP + 0x108u), c.T6);
        mem.WriteU32((c.SP + 0x110u), c.T9);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(25, (c.SP + 0x110u), c.T9);
        c.RA = 0x80010F08u;
        MediEvil.func_800105C0(c, m);
        mem.WriteU32((c.SP + 0x118u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x118u), c.V0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0x30u), c.V0);
        c.V0 = mem.ReadU16(c.T1);
        mem.WriteU16((c.S5 + 0x4u), (ushort)c.V0);
        c.V0 = 0x52000000u;
        mem.WriteU32((c.SP + 0xC0u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.SP + 0xC0u), c.V0);
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
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(3, c.A0, c.V1);
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
        RecompOne.Runtime.Gte.Rtpt(12, false);
        c.V0 = c.S0 << 2;
        c.V0 = c.T2 + c.V0;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.S0 + 0x1u;
        mem.WriteU32((c.A0 - 0xCu), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.A0 - 0xCu), c.V0);
        c.V0 = c.V1 << 2;
        c.V0 = c.T2 + c.V0;
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.A0 - 0x4u), c.V0);
        RecompOne.Runtime.Pgxp.PgxpCpu.Sw(2, (c.A0 - 0x4u), c.V0);
        c.V0 = c.T3 + 0x8u;
        mem.WriteU32(c.V0, RecompOne.Runtime.Gte.Read(12));
        mem.WriteU32(c.A0, RecompOne.Runtime.Gte.Read(13));
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

}