using RecompOne.Runtime.Context;
using RecompOne.Runtime.Dispatch;
using RecompOne.Runtime.Memory;

namespace Recompiled;

//this is a mess
public static class EntityFloatFunctions
{
    public static void func_800B0068(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x07000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B00F0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x18u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0x14u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B01F0;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B01F0;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B01F0;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x18u), _sw); }
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        if (0u == c.V1) {
            goto L800B01A8;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B01AC;
        L800B01A8: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B01AC: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x4u), _sw); }
        c.T8 = 0u | 0x0007u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.T8 = mem.ReadU32((c.A2 + 0x8u));
        c.T9 = mem.ReadU32((c.A2 + 0xCu));
        c.T7 = mem.ReadU16((c.A2 + 0x10u));
        mem.WriteU32((c.A3 + 0xCu), c.T8);
        mem.WriteU32((c.A3 + 0x14u), c.T9);
        mem.WriteU16((c.A3 + 0x1Cu), (ushort)c.T7);
        c.A3 = c.A3 + 0x20u;
        L800B01F0: ;
        c.A2 = c.A2 + 0x18u;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B00F0;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0240(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x09000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B02D4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x1Cu;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0x18u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B0360;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B0408;
        }
        L800B0360: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0408;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B0408;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x18u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x20u), _sw); }
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        if (0u == c.V1) {
            goto L800B03B8;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B03BC;
        L800B03B8: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B03BC: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x4u), _sw); }
        c.T8 = 0u | 0x0009u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.T8 = mem.ReadU32((c.A2 + 0xCu));
        c.T9 = mem.ReadU32((c.A2 + 0x10u));
        c.T4 = mem.ReadU16((c.A2 + 0x14u));
        c.T5 = mem.ReadU16((c.A2 + 0x16u));
        mem.WriteU32((c.A3 + 0xCu), c.T8);
        mem.WriteU32((c.A3 + 0x14u), c.T9);
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T4);
        mem.WriteU16((c.A3 + 0x1Cu), (ushort)c.T5);
        c.A3 = c.A3 + 0x28u;
        L800B0408: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x1Cu;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B02D4;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0458(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x09000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B04E0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x1Cu;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0x18u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B060C;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B060C;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
            goto L800B060C;
        }
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x20u), _sw); }
        c.T5 = c.T5 << 3;
        c.T6 = c.T6 << 3;
        c.T5 = c.T5 + c.A1;
        c.T6 = c.T6 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B05BC;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B05C0;
        L800B05BC: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B05C0: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        c.T8 = 0u | 0x0009u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.T8 = mem.ReadU32((c.A2 + 0xCu));
        c.T9 = mem.ReadU32((c.A2 + 0x10u));
        c.T7 = mem.ReadU16((c.A2 + 0x14u));
        mem.WriteU32((c.A3 + 0xCu), c.T8);
        mem.WriteU32((c.A3 + 0x18u), c.T9);
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T7);
        c.A3 = c.A3 + 0x28u;
        L800B060C: ;
        c.A2 = c.A2 + 0x1Cu;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B04E0;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B065C(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x0C000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B06F0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x24u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0x20u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B077C;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B08B0;
        }
        L800B077C: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B08B0;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
            goto L800B08B0;
        }
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T7 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x20u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x2Cu), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x2Cu), _sw); }
        c.T5 = c.T5 << 3;
        c.T7 = c.T7 << 3;
        c.T5 = c.T5 + c.A1;
        c.T7 = c.T7 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B0838;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B0880;
        L800B0838: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B0880: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x28u), _sw); }
        c.T8 = 0u | 0x000Cu;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.T8 = mem.ReadU32((c.A2 + 0x10u));
        c.T9 = mem.ReadU32((c.A2 + 0x14u));
        c.T4 = mem.ReadU16((c.A2 + 0x18u));
        c.T5 = mem.ReadU16((c.A2 + 0x1Au));
        mem.WriteU32((c.A3 + 0xCu), c.T8);
        mem.WriteU32((c.A3 + 0x18u), c.T9);
        mem.WriteU16((c.A3 + 0x30u), (ushort)c.T4);
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T5);
        c.A3 = c.A3 + 0x34u;
        L800B08B0: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x24u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B06F0;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0900(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x04000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B0988: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0xCu;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0x8u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B0A70;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0A70;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B0A70;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0xCu), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x10u), _sw); }
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        if (0u == c.V1) {
            goto L800B0A40;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B0A44;
        L800B0A40: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B0A44: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x4u), _sw); }
        c.T8 = 0u | 0x0004u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.A3 = c.A3 + 0x14u;
        L800B0A70: ;
        c.A2 = c.A2 + 0xCu;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0988;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0AC0(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x05000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B0B54: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x10u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0xCu)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B0BE0;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B0C68;
        }
        L800B0BE0: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0C68;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B0C68;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0xCu), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x14u), _sw); }
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        if (0u == c.V1) {
            goto L800B0C38;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B0C3C;
        L800B0C38: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B0C3C: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x4u), _sw); }
        c.T8 = 0u | 0x0005u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.A3 = c.A3 + 0x18u;
        L800B0C68: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x10u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0B54;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0CB8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x06000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B0D40: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x10u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0xCu)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B0E54;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0E54;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
            goto L800B0E54;
        }
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x18u), _sw); }
        c.T5 = c.T5 << 3;
        c.T6 = c.T6 << 3;
        c.T5 = c.T5 + c.A1;
        c.T6 = c.T6 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B0E1C;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B0E20;
        L800B0E1C: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B0E20: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x14u), _sw); }
        c.T8 = 0u | 0x0006u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.A3 = c.A3 + 0x1Cu;
        L800B0E54: ;
        c.A2 = c.A2 + 0x10u;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0D40;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0EA4(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x08000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B0F38: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x14u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0x10u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B0FC4;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B10D8;
        }
        L800B0FC4: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B10D8;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
            goto L800B10D8;
        }
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T7 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x18u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x20u), _sw); }
        c.T5 = c.T5 << 3;
        c.T7 = c.T7 << 3;
        c.T5 = c.T5 + c.A1;
        c.T7 = c.T7 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B1080;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x14u), _sw); }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B10C8;
        L800B1080: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x14u), _sw); }
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B10C8: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        c.T8 = 0u | 0x0008u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.A3 = c.A3 + 0x24u;
        L800B10D8: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x14u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0F38;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B59C8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x30u;
        c.T7 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T2 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x40u));
        c.T5 = 0x1F800000u;
        c.T5 = mem.ReadU16((c.T5 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        c.V1 = c.T2;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T8 = mem.ReadU32((c.T0 + 0x20u));
        c.S2 = mem.ReadU16((c.T0 + 0x24u));
        c.S1 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S0 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S3 = mem.ReadU32((c.SP + 0x44u));
        c.T6 = mem.ReadU32((c.SP + 0x48u));
        if (c.V1 == 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L800B5D74;
        }
        c.T2 = c.T2 - 0x1u;
        c.T4 = 0x00FF0000u;
        c.T4 = c.T4 | 0xFFFFu;
        c.T9 = 0xFF000000u;
        c.A2 = c.A2 + 0x10u;
        c.A3 = c.A3 + 0x1Cu;
        L800B5A6C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T0);
        c.S5 = mem.ReadU32((c.T0 + 0x4u));
        c.S6 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T6 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B5B0C;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B5B0C: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.T6 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B5B34;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B5D54;
        }
        L800B5B34: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S2 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S0 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B5D54;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B5D54;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x18u), _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x12u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
            goto L800B5BB8;
        }
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xAu));
        if ((int)c.V0 >= 0) {
            goto L800B5BAC;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        if ((int)c.V0 < 0) {
            goto L800B5D54;
        }
        L800B5BAC: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x12u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        L800B5BB8: ;
        if (c.V0 != 0u) {
            c.V0 = c.T6 & 0x0020u;
            goto L800B5BE8;
        }
        c.V0 = c.T6 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xAu));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T6 & 0x0020u;
            goto L800B5BE8;
        }
        c.V0 = c.T6 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.T6 & 0x0020u;
            goto L800B5D54;
        }
        c.V0 = c.T6 & 0x0020u;
        L800B5BE8: ;
        if (c.V0 != 0u) {
            c.V1 = c.T6 & 0x0006u;
            goto L800B5CD0;
        }
        c.V1 = c.T6 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B5C80;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            goto L800B5C80;
        }
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = c.SP + 0x8u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B5C98;
        L800B5C80: ;
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B5C98: ;
        if (c.S3 == 0u) {
            goto L800B5CB4;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        goto L800B5CC4;
        L800B5CB4: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        L800B5CC4: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        goto L800B5CDC;
        L800B5CD0: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x18u), c.V0);
        L800B5CDC: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T8;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T9;
        c.V0 = c.V0 & c.T4;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T8;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.T4;
        c.V0 = c.V0 & c.T9;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000007u;
        mem.WriteU8((c.A3 - 0x19u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x8u));
        mem.WriteU32((c.A3 - 0x10u), c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x4u));
        mem.WriteU32((c.A3 - 0x8u), c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.T1 = c.T1 + 0x20u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x20u;
        L800B5D54: ;
        c.A2 = c.A2 + 0x18u;
        c.T3 = c.T3 + 0x18u;
        c.V1 = c.T2;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T2 = c.T2 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B5A6C;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B5D74: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x30u;
        return;
    }
    public static void func_800B5DA4(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x38u;
        c.T6 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x34u), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T5 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x48u));
        c.T4 = 0x1F800000u;
        c.T4 = mem.ReadU16((c.T4 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        c.V1 = c.T5;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.S0 = mem.ReadU32((c.T0 + 0x20u));
        c.S4 = mem.ReadU16((c.T0 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S3 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x6u));
        c.S2 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        c.S5 = mem.ReadU32((c.SP + 0x4Cu));
        c.T8 = mem.ReadU32((c.SP + 0x50u));
        c.T5 = c.T5 - 0x1u;
        if (c.V1 == 0u) {
            mem.WriteU32((c.SP + 0x10u), c.S2);
            goto L800B6204;
        }
        mem.WriteU32((c.SP + 0x10u), c.S2);
        c.T9 = c.T0 + 0x34u;
        c.T7 = 0x00FF0000u;
        c.T7 = c.T7 | 0xFFFFu;
        c.S1 = 0xFF000000u;
        c.A2 = c.A2 + 0x16u;
        c.A3 = c.A3 + 0x1Cu;
        L800B5E64: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S6 = mem.ReadU32(c.T0);
        c.S7 = mem.ReadU32((c.T0 + 0x4u));
        c.S2 = mem.ReadU32((c.T0 + 0xCu));
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = 0u;
        c.V0 = c.T1 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        c.S6 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.T9, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.V0 = mem.ReadU32((c.A2 + 0x2u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T8 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B5F44;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B5F44: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            goto L800B5F8C;
        }
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.T9, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 < 0) {
            c.V0 = c.T8 & 0x0002u;
            goto L800B5F7C;
        }
        c.V0 = c.T8 & 0x0002u;
        c.T2 = 0x00000001u;
        L800B5F7C: ;
        if (c.V0 != 0u) {
            goto L800B5F8C;
        }
        if (c.T2 != 0u) {
            goto L800B61E4;
        }
        L800B5F8C: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S4 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.S7 = mem.ReadU32((c.SP + 0x10u));
        c.V0 = (int)c.V1 < (int)c.S7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = (int)c.V1 < (int)c.S3 ? 1u : 0u;
            goto L800B61E4;
        }
        c.V0 = (int)c.V1 < (int)c.S3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T1 + 0x10u;
            goto L800B61E4;
        }
        c.A0 = c.T1 + 0x10u;
        c.V1 = c.T1 + 0x18u;
        c.V0 = c.T1 + 0x20u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x12u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
            goto L800B6030;
        }
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xAu));
        if ((int)c.V0 >= 0) {
            goto L800B6024;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        if ((int)c.V0 >= 0) {
            goto L800B6024;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x6u));
        if ((int)c.V0 < 0) {
            goto L800B61E4;
        }
        L800B6024: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x12u));
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        L800B6030: ;
        if (c.V0 != 0u) {
            c.V0 = c.T8 & 0x0020u;
            goto L800B6074;
        }
        c.V0 = c.T8 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xAu));
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T8 & 0x0020u;
            goto L800B6074;
        }
        c.V0 = c.T8 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T8 & 0x0020u;
            goto L800B6074;
        }
        c.V0 = c.T8 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x6u));
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.T8 & 0x0020u;
            goto L800B61E4;
        }
        c.V0 = c.T8 & 0x0020u;
        L800B6074: ;
        if (c.V0 != 0u) {
            c.V1 = c.T8 & 0x0006u;
            goto L800B6154;
        }
        c.V1 = c.T8 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B6104;
        }
        if (c.T2 == 0u) {
            goto L800B6104;
        }
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = c.SP + 0x8u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B611C;
        L800B6104: ;
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B611C: ;
        if (c.S5 == 0u) {
            goto L800B6138;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        goto L800B6148;
        L800B6138: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        L800B6148: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        goto L800B6160;
        L800B6154: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x18u), c.V0);
        L800B6160: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.S0;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S1;
        c.V0 = c.V0 & c.T7;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.S0;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.T7;
        c.V0 = c.V0 & c.S1;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000009u;
        mem.WriteU8((c.A3 - 0x19u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0xAu));
        mem.WriteU32((c.A3 - 0x10u), c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x6u));
        mem.WriteU32((c.A3 - 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        mem.WriteU16((c.A3 + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.T1 = c.T1 + 0x28u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x28u;
        L800B61E4: ;
        c.A2 = c.A2 + 0x1Cu;
        c.T3 = c.T3 + 0x1Cu;
        c.V1 = c.T5;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T5 = c.T5 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B5E64;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B6204: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S7 = mem.ReadU32((c.SP + 0x34u));
        c.S6 = mem.ReadU32((c.SP + 0x30u));
        c.S5 = mem.ReadU32((c.SP + 0x2Cu));
        c.S4 = mem.ReadU32((c.SP + 0x28u));
        c.S3 = mem.ReadU32((c.SP + 0x24u));
        c.S2 = mem.ReadU32((c.SP + 0x20u));
        c.S1 = mem.ReadU32((c.SP + 0x1Cu));
        c.S0 = mem.ReadU32((c.SP + 0x18u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x38u;
        return;
    }
    public static void func_800B6238(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x40u;
        c.T6 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x38u), c.S6);
        mem.WriteU32((c.SP + 0x34u), c.S5);
        mem.WriteU32((c.SP + 0x30u), c.S4);
        mem.WriteU32((c.SP + 0x2Cu), c.S3);
        mem.WriteU32((c.SP + 0x28u), c.S2);
        mem.WriteU32((c.SP + 0x24u), c.S1);
        mem.WriteU32((c.SP + 0x20u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T2 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x50u));
        c.T5 = 0x1F800000u;
        c.T5 = mem.ReadU16((c.T5 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        c.V1 = c.T2;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T8 = mem.ReadU32((c.T0 + 0x20u));
        c.S2 = mem.ReadU16((c.T0 + 0x24u));
        c.S1 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S0 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S3 = mem.ReadU32((c.SP + 0x54u));
        c.T7 = mem.ReadU32((c.SP + 0x58u));
        if (c.V1 == 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L800B66F4;
        }
        c.T2 = c.T2 - 0x1u;
        c.T4 = 0x00FF0000u;
        c.T4 = c.T4 | 0xFFFFu;
        c.T9 = 0xFF000000u;
        c.A2 = c.A2 + 0x14u;
        c.A3 = c.A3 + 0x24u;
        L800B62DC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T0);
        c.S5 = mem.ReadU32((c.T0 + 0x4u));
        c.S6 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T7 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B637C;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B637C: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.T7 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B63A4;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B66D4;
        }
        L800B63A4: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S2 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S0 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B66D4;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B66D4;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x20u), _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x1Au));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
            goto L800B6428;
        }
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xEu));
        if ((int)c.V0 >= 0) {
            goto L800B641C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        if ((int)c.V0 < 0) {
            goto L800B66D4;
        }
        L800B641C: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x1Au));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        L800B6428: ;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B6458;
        }
        c.V0 = c.T7 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xEu));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B6458;
        }
        c.V0 = c.T7 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B66D4;
        }
        c.V0 = c.T7 & 0x0020u;
        L800B6458: ;
        if (c.V0 != 0u) {
            c.V1 = c.T7 & 0x0006u;
            goto L800B6638;
        }
        c.V1 = c.T7 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B65C0;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            c.A0 = c.SP + 0x8u;
            goto L800B65C0;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B65FC;
        L800B65C0: ;
        c.A0 = mem.ReadU16((c.A2 - 0xEu));
        c.V1 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.A0 = c.A0 << 3;
        c.A0 = c.A1 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.A1 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B65FC: ;
        if (c.S3 == 0u) {
            goto L800B6618;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B6624;
        L800B6618: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B6624: ;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x1Cu), _sw); }
        goto L800B665C;
        L800B6638: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x20u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x14u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x8u), c.V0);
        L800B665C: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T8;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T9;
        c.V0 = c.V0 & c.T4;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T8;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.T4;
        c.V0 = c.V0 & c.T9;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000009u;
        mem.WriteU8((c.A3 - 0x21u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x8u));
        mem.WriteU32((c.A3 - 0x18u), c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x4u));
        mem.WriteU32((c.A3 - 0xCu), c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.T1 = c.T1 + 0x28u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x28u;
        L800B66D4: ;
        c.A2 = c.A2 + 0x1Cu;
        c.T3 = c.T3 + 0x1Cu;
        c.V1 = c.T2;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T2 = c.T2 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B62DC;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B66F4: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S6 = mem.ReadU32((c.SP + 0x38u));
        c.S5 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = mem.ReadU32((c.SP + 0x30u));
        c.S3 = mem.ReadU32((c.SP + 0x2Cu));
        c.S2 = mem.ReadU32((c.SP + 0x28u));
        c.S1 = mem.ReadU32((c.SP + 0x24u));
        c.S0 = mem.ReadU32((c.SP + 0x20u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x40u;
        return;
    }
    public static void func_800B6724(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x58u;
        c.T9 = c.A0;
        c.T5 = c.A2;
        mem.WriteU32((c.SP + 0x54u), c.S7);
        mem.WriteU32((c.SP + 0x50u), c.S6);
        mem.WriteU32((c.SP + 0x4Cu), c.S5);
        mem.WriteU32((c.SP + 0x48u), c.S4);
        mem.WriteU32((c.SP + 0x44u), c.S3);
        mem.WriteU32((c.SP + 0x40u), c.S2);
        mem.WriteU32((c.SP + 0x3Cu), c.S1);
        mem.WriteU32((c.SP + 0x38u), c.S0);
        c.V0 = mem.ReadU16(c.T5);
        c.T6 = mem.ReadU16((c.T5 - 0x2u));
        c.T1 = mem.ReadU32((c.SP + 0x68u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32(c.T1, c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x2u));
        c.T7 = 0x1F800000u;
        c.T7 = mem.ReadU16((c.T7 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x4u));
        c.T4 = mem.ReadU32((c.T1 + 0x20u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x6u));
        c.S4 = mem.ReadU16((c.T1 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0xCu), c.V0);
        c.S7 = mem.ReadU32((c.SP + 0x6Cu));
        c.S1 = mem.ReadU32((c.SP + 0x70u));
        c.T0 = c.A3;
        mem.WriteU32((c.SP + 0x28u), c.S4);
        c.S5 = mem.ReadU32((c.T1 + 0x28u));
        c.V1 = c.T6;
        mem.WriteU32((c.SP + 0x2Cu), c.S5);
        c.S6 = mem.ReadU32((c.T1 + 0x2Cu));
        c.T6 = c.T6 - 0x1u;
        if (c.V1 == 0u) {
            mem.WriteU32((c.SP + 0x30u), c.S6);
            goto L800B6E3C;
        }
        mem.WriteU32((c.SP + 0x30u), c.S6);
        c.S2 = c.T1 + 0x34u;
        c.S0 = c.S1 & 0x0006u;
        c.S3 = 0x00000006u;
        c.T2 = 0x00FF0000u;
        c.T2 = c.T2 | 0xFFFFu;
        c.T8 = 0xFF000000u;
        c.A2 = c.A2 + 0x1Au;
        c.A3 = c.A3 + 0x24u;
        L800B67F4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T1);
        c.S5 = mem.ReadU32((c.T1 + 0x4u));
        c.S6 = mem.ReadU32((c.T1 + 0xCu));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32(c.T1, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x10u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T3 = 0u;
        c.S4 = mem.ReadU32((c.T1 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.S2, _sw); }
        c.V0 = c.T0 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU32((c.A2 + 0x6u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.S1 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B68C0;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B68C0: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU16((c.A2 + 0xEu));
        c.V1 = mem.ReadU32((c.T1 + 0x34u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.T1 + 0x8u), c.V0);
            goto L800B6910;
        }
        mem.WriteU32((c.T1 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.S2, _sw); }
        c.V0 = mem.ReadU32((c.T1 + 0x34u));
        if ((int)c.V0 < 0) {
            c.V0 = c.S1 & 0x0002u;
            goto L800B6900;
        }
        c.V0 = c.S1 & 0x0002u;
        c.T3 = 0x00000001u;
        L800B6900: ;
        if (c.V0 != 0u) {
            goto L800B6910;
        }
        if (c.T3 != 0u) {
            goto L800B6E1C;
        }
        L800B6910: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T1 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.S5 = mem.ReadU32((c.SP + 0x28u));
        c.V1 = (uint)(short)mem.ReadU16((c.T1 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S5 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T1 + 0x38u), c.V1);
        c.S6 = mem.ReadU32((c.SP + 0x30u));
        c.V0 = (int)c.V1 < (int)c.S6 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B6E1C;
        }
        c.S4 = mem.ReadU32((c.SP + 0x2Cu));
        c.V0 = (int)c.V1 < (int)c.S4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T0 + 0x14u;
            goto L800B6E1C;
        }
        c.A0 = c.T0 + 0x14u;
        c.V1 = c.T0 + 0x20u;
        c.V0 = c.T0 + 0x2Cu;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x1Au));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
            goto L800B69C4;
        }
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xEu));
        if ((int)c.V0 >= 0) {
            goto L800B69B8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        if ((int)c.V0 >= 0) {
            goto L800B69B8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xAu));
        if ((int)c.V0 < 0) {
            goto L800B6E1C;
        }
        L800B69B8: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x1Au));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        L800B69C4: ;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B6A08;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xEu));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B6A08;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B6A08;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xAu));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B6E1C;
        }
        c.V0 = c.S1 & 0x0020u;
        L800B6A08: ;
        if (c.V0 != 0u) {
            goto L800B6D68;
        }
        if (c.S0 != c.S3) {
            goto L800B6BC4;
        }
        if (c.T3 == 0u) {
            c.A0 = c.SP + 0x8u;
            goto L800B6BC4;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x12u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x12u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x12u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x10u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x10u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x10u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x20u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x22u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x24u), (ushort)c.V0);
        c.V0 = c.SP + 0x20u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B6C00;
        L800B6BC4: ;
        c.A0 = mem.ReadU16((c.A2 - 0x12u));
        c.V1 = mem.ReadU16((c.A2 - 0x10u));
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.A0 = c.A0 << 3;
        c.A0 = c.A1 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.A1 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B6C00: ;
        if (c.S7 == 0u) {
            goto L800B6CB4;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T0 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T0 + 0x1Cu), _sw); }
        if (c.S0 != c.S3) {
            goto L800B6C88;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B6C88;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B6CA0;
        L800B6C88: ;
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B6CA0: ;
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T0 + 0x28u;
        goto L800B6D5C;
        L800B6CB4: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T0 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T0 + 0x1Cu), _sw); }
        if (c.S0 != c.S3) {
            goto L800B6D34;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B6D34;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B6D4C;
        L800B6D34: ;
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B6D4C: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T0 + 0x28u;
        L800B6D5C: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        c.V0 = 0x0000000Cu;
        goto L800B6DE4;
        L800B6D68: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x20u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x14u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x8u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x4u), c.V0);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x0000000Cu;
        L800B6DE4: ;
        mem.WriteU8((c.A3 - 0x21u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0xAu));
        mem.WriteU32((c.A3 - 0x18u), c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x6u));
        mem.WriteU32((c.A3 - 0xCu), c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        mem.WriteU16((c.A3 + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.T0 = c.T0 + 0x34u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x34u;
        L800B6E1C: ;
        c.A2 = c.A2 + 0x24u;
        c.T5 = c.T5 + 0x24u;
        c.V1 = c.T6;
        c.V0 = mem.ReadU32((c.T1 + 0x44u));
        c.T6 = c.T6 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T1 + 0x44u), c.V0);
            goto L800B67F4;
        }
        mem.WriteU32((c.T1 + 0x44u), c.V0);
        L800B6E3C: ;
        mem.WriteU32((c.T1 + 0x40u), c.T5);
        c.S7 = mem.ReadU32((c.SP + 0x54u));
        c.S6 = mem.ReadU32((c.SP + 0x50u));
        c.S5 = mem.ReadU32((c.SP + 0x4Cu));
        c.S4 = mem.ReadU32((c.SP + 0x48u));
        c.S3 = mem.ReadU32((c.SP + 0x44u));
        c.S2 = mem.ReadU32((c.SP + 0x40u));
        c.S1 = mem.ReadU32((c.SP + 0x3Cu));
        c.S0 = mem.ReadU32((c.SP + 0x38u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T0);
        c.SP = c.SP + 0x58u;
        return;
    }
    public static void func_800B6E70(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x44u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x09000000u;
        mem.WriteU32((c.SP + 0x34u), c.S6);
        mem.WriteU32((c.SP + 0x38u), c.S7);
        c.T9 = 0x800E0000u;
        c.At = 0x00010000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At - 0x2268u));
        c.S6 = mem.ReadU8((c.T5 + 0x4u));
        c.S7 = mem.ReadU8((c.T5 + 0x5u));
        c.S6 = c.S6 + 0x40u;
        c.S7 = c.S7 + 0x40u;
        mem.WriteU32((c.SP + 0x3Cu), c.S6);
        mem.WriteU32((c.SP + 0x40u), c.S7);
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B6F2C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x18u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0x14u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B71D4;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B71D4;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B71D4;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x20u), _sw); }
        c.T9 = 0x1F800000u;
        c.At = 0x00000000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At + 0x38u));
        c.T6 = mem.ReadU32(c.T5);
        c.T7 = mem.ReadU32((c.T5 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T6);
        RecompOne.Runtime.Gte.WriteControl(1, c.T7);
        c.T6 = mem.ReadU32((c.T5 + 0x8u));
        c.T7 = mem.ReadU32((c.T5 + 0xCu));
        c.T9 = mem.ReadU32((c.T5 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T6);
        RecompOne.Runtime.Gte.WriteControl(3, c.T7);
        RecompOne.Runtime.Gte.WriteControl(4, c.T9);
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.S6 = mem.ReadU32((c.SP + 0x3Cu));
        c.S7 = mem.ReadU32((c.SP + 0x40u));
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0xCu), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x18u), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 2, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T5);
        c.T9 = 0x1F800000u;
        c.At = 0x00000000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At + 0x34u));
        c.T6 = mem.ReadU32(c.T5);
        c.T7 = mem.ReadU32((c.T5 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T6);
        RecompOne.Runtime.Gte.WriteControl(1, c.T7);
        c.T6 = mem.ReadU32((c.T5 + 0x8u));
        c.T7 = mem.ReadU32((c.T5 + 0xCu));
        c.T9 = mem.ReadU32((c.T5 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T6);
        RecompOne.Runtime.Gte.WriteControl(3, c.T7);
        RecompOne.Runtime.Gte.WriteControl(4, c.T9);
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0x10u));
        c.T5 = c.T5 << 3;
        c.T6 = c.T6 << 3;
        c.T5 = c.T5 + c.A1;
        c.T6 = c.T6 + c.A1;
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B7170;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B7174;
        L800B7170: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B7174: ;
        c.S6 = mem.ReadU32((c.SP + 0x34u));
        c.S7 = mem.ReadU32((c.SP + 0x38u));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        c.T9 = 0x800E0000u;
        c.T8 = 0u | 0x0009u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.At = 0x00010000u;
        c.At = c.At + c.T9;
        c.T9 = mem.ReadU32((c.At - 0x2268u));
        c.T7 = (uint)(short)mem.ReadU16((c.T9 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.T9 + 0xAu));
        mem.WriteU16((c.A3 + 0xEu), (ushort)c.T7);
        mem.WriteU16((c.A3 + 0x1Au), (ushort)c.T8);
        c.A3 = c.A3 + 0x28u;
        L800B71D4: ;
        c.A2 = c.A2 + 0x18u;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B6F2C;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x44u;
        return;
    }
    public static void func_800B7224(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x44u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x0C000000u;
        mem.WriteU32((c.SP + 0x34u), c.S6);
        mem.WriteU32((c.SP + 0x38u), c.S7);
        c.T9 = 0x800E0000u;
        c.At = 0x00010000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At - 0x2268u));
        c.S6 = mem.ReadU8((c.T5 + 0x4u));
        c.S7 = mem.ReadU8((c.T5 + 0x5u));
        c.S6 = c.S6 + 0x40u;
        c.S7 = c.S7 + 0x40u;
        mem.WriteU32((c.SP + 0x3Cu), c.S6);
        mem.WriteU32((c.SP + 0x40u), c.S7);
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B72EC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x1Cu;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0x18u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B7378;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B766C;
        }
        L800B7378: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B766C;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B766C;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x20u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x2Cu), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x2Cu), _sw); }
        c.T9 = 0x1F800000u;
        c.At = 0x00000000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At + 0x38u));
        c.T6 = mem.ReadU32(c.T5);
        c.T7 = mem.ReadU32((c.T5 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T6);
        RecompOne.Runtime.Gte.WriteControl(1, c.T7);
        c.T6 = mem.ReadU32((c.T5 + 0x8u));
        c.T7 = mem.ReadU32((c.T5 + 0xCu));
        c.T9 = mem.ReadU32((c.T5 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T6);
        RecompOne.Runtime.Gte.WriteControl(3, c.T7);
        RecompOne.Runtime.Gte.WriteControl(4, c.T9);
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.S6 = mem.ReadU32((c.SP + 0x3Cu));
        c.S7 = mem.ReadU32((c.SP + 0x40u));
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0xCu), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x18u), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x30u), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x10u));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T5);
        c.T9 = 0x1F800000u;
        c.At = 0x00000000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At + 0x34u));
        c.T6 = mem.ReadU32(c.T5);
        c.T7 = mem.ReadU32((c.T5 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T6);
        RecompOne.Runtime.Gte.WriteControl(1, c.T7);
        c.T6 = mem.ReadU32((c.T5 + 0x8u));
        c.T7 = mem.ReadU32((c.T5 + 0xCu));
        c.T9 = mem.ReadU32((c.T5 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T6);
        RecompOne.Runtime.Gte.WriteControl(3, c.T7);
        RecompOne.Runtime.Gte.WriteControl(4, c.T9);
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T7 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        c.T5 = c.T5 << 3;
        c.T7 = c.T7 << 3;
        c.T5 = c.T5 + c.A1;
        c.T7 = c.T7 + c.A1;
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.S6 = mem.ReadU32((c.SP + 0x34u));
        c.S7 = mem.ReadU32((c.SP + 0x38u));
        if (0u == c.V1) {
            goto L800B75F0;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0x14u));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B7638;
        L800B75F0: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0x14u));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B7638: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x28u), _sw); }
        c.T9 = 0x800E0000u;
        c.T8 = 0u | 0x000Cu;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.At = 0x00010000u;
        c.At = c.At + c.T9;
        c.T9 = mem.ReadU32((c.At - 0x2268u));
        c.T7 = (uint)(short)mem.ReadU16((c.T9 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.T9 + 0xAu));
        mem.WriteU16((c.A3 + 0xEu), (ushort)c.T7);
        mem.WriteU16((c.A3 + 0x1Au), (ushort)c.T8);
        c.A3 = c.A3 + 0x34u;
        L800B766C: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x1Cu;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B72EC;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x44u;
        return;
    }
    public static void func_800B76BC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x30u;
        c.T5 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T2 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        c.V1 = c.T2;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T7 = mem.ReadU32((c.T0 + 0x20u));
        c.S2 = mem.ReadU16((c.T0 + 0x24u));
        c.S1 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S0 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S3 = mem.ReadU32((c.SP + 0x44u));
        c.T6 = mem.ReadU32((c.SP + 0x48u));
        if (c.V1 == 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L800B79D4;
        }
        c.T2 = c.T2 - 0x1u;
        c.T9 = c.T6 & 0x0006u;
        c.T4 = 0x00FF0000u;
        c.T4 = c.T4 | 0xFFFFu;
        c.T8 = 0xFF000000u;
        c.A2 = c.A2 + 0x6u;
        c.A3 = c.A3 + 0x3u;
        L800B775C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T0);
        c.S5 = mem.ReadU32((c.T0 + 0x4u));
        c.S6 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x2u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T6 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B77FC;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B77FC: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.T6 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B7824;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B79B4;
        }
        L800B7824: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S2 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S0 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B79B4;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B79B4;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0xCu), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x10u), _sw); }
        c.V0 = c.T6 & 0x0020u;
        if (c.V0 != 0u) {
            c.V0 = 0x00000006u;
            goto L800B7954;
        }
        c.V0 = 0x00000006u;
        if (c.T9 != c.V0) {
            goto L800B7904;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            goto L800B7904;
        }
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = c.SP + 0x8u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B791C;
        L800B7904: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B791C: ;
        if (c.S3 == 0u) {
            goto L800B7938;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        goto L800B7948;
        L800B7938: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        L800B7948: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        goto L800B7960;
        L800B7954: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x1u), c.V0);
        L800B7960: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T7;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T4;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.V1 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.T1 & c.T4;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T7;
        c.V0 = mem.ReadU32(c.V1);
        c.T1 = c.T1 + 0x14u;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.A0;
        mem.WriteU32(c.V1, c.V0);
        c.V0 = 0x00000004u;
        mem.WriteU8(c.A3, (byte)c.V0);
        c.A3 = c.A3 + 0x14u;
        L800B79B4: ;
        c.A2 = c.A2 + 0xCu;
        c.T3 = c.T3 + 0xCu;
        c.V1 = c.T2;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T2 = c.T2 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B775C;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B79D4: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x30u;
        return;
    }
    public static void func_800B7A04(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x30u;
        c.T5 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T4 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        c.V1 = c.T4;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T9 = mem.ReadU32((c.T0 + 0x20u));
        c.S3 = mem.ReadU16((c.T0 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S2 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x6u));
        c.S1 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        c.S4 = mem.ReadU32((c.SP + 0x44u));
        c.T7 = mem.ReadU32((c.SP + 0x48u));
        if (c.V1 == 0u) {
            c.T4 = c.T4 - 0x1u;
            goto L800B7D88;
        }
        c.T4 = c.T4 - 0x1u;
        c.T8 = c.T0 + 0x34u;
        c.T6 = 0x00FF0000u;
        c.T6 = c.T6 | 0xFFFFu;
        c.S0 = 0xFF000000u;
        c.A2 = c.A2 + 0x8u;
        c.A3 = c.A3 + 0x3u;
        L800B7AB8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S5 = mem.ReadU32(c.T0);
        c.S6 = mem.ReadU32((c.T0 + 0x4u));
        c.S7 = mem.ReadU32((c.T0 + 0xCu));
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = 0u;
        c.S5 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.T8, _sw); }
        c.V0 = c.T1 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T7 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B7B84;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B7B84: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V1 = mem.ReadU32((c.T0 + 0x34u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.T0 + 0x8u), c.V0);
            goto L800B7BD4;
        }
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.T8, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 < 0) {
            c.V0 = c.T7 & 0x0002u;
            goto L800B7BC4;
        }
        c.V0 = c.T7 & 0x0002u;
        c.T2 = 0x00000001u;
        L800B7BC4: ;
        if (c.V0 != 0u) {
            goto L800B7BD4;
        }
        if (c.T2 != 0u) {
            goto L800B7D68;
        }
        L800B7BD4: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S3 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B7D68;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S2 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T1 + 0xCu;
            goto L800B7D68;
        }
        c.A0 = c.T1 + 0xCu;
        c.V1 = c.T1 + 0x10u;
        c.V0 = c.T1 + 0x14u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = c.T7 & 0x0020u;
        if (c.V0 != 0u) {
            c.V1 = c.T7 & 0x0006u;
            goto L800B7D08;
        }
        c.V1 = c.T7 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B7CB8;
        }
        if (c.T2 == 0u) {
            goto L800B7CB8;
        }
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = c.SP + 0x8u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B7CD0;
        L800B7CB8: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B7CD0: ;
        if (c.S4 == 0u) {
            goto L800B7CEC;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        goto L800B7CFC;
        L800B7CEC: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        L800B7CFC: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        goto L800B7D14;
        L800B7D08: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x1u), c.V0);
        L800B7D14: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T9;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S0;
        c.V0 = c.V0 & c.T6;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.V1 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.T1 & c.T6;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T9;
        c.V0 = mem.ReadU32(c.V1);
        c.T1 = c.T1 + 0x18u;
        c.V0 = c.V0 & c.S0;
        c.V0 = c.V0 | c.A0;
        mem.WriteU32(c.V1, c.V0);
        c.V0 = 0x00000005u;
        mem.WriteU8(c.A3, (byte)c.V0);
        c.A3 = c.A3 + 0x18u;
        L800B7D68: ;
        c.A2 = c.A2 + 0x10u;
        c.T3 = c.T3 + 0x10u;
        c.V1 = c.T4;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T4 = c.T4 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B7AB8;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B7D88: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x30u;
        return;
    }
    public static void func_800B7DBC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x40u;
        c.T6 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x3Cu), c.S7);
        mem.WriteU32((c.SP + 0x38u), c.S6);
        mem.WriteU32((c.SP + 0x34u), c.S5);
        mem.WriteU32((c.SP + 0x30u), c.S4);
        mem.WriteU32((c.SP + 0x2Cu), c.S3);
        mem.WriteU32((c.SP + 0x28u), c.S2);
        mem.WriteU32((c.SP + 0x24u), c.S1);
        mem.WriteU32((c.SP + 0x20u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T2 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x50u));
        c.T5 = 0x1F800000u;
        c.T5 = mem.ReadU16((c.T5 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        c.V1 = c.T2;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T8 = mem.ReadU32((c.T0 + 0x20u));
        c.S3 = mem.ReadU16((c.T0 + 0x24u));
        c.S2 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S1 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S4 = mem.ReadU32((c.SP + 0x54u));
        c.T7 = mem.ReadU32((c.SP + 0x58u));
        if (c.V1 == 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L800B8254;
        }
        c.T2 = c.T2 - 0x1u;
        c.S0 = 0x00000006u;
        c.T4 = 0x00FF0000u;
        c.T4 = c.T4 | 0xFFFFu;
        c.T9 = 0xFF000000u;
        c.A2 = c.A2 + 0xAu;
        c.A3 = c.A3 + 0x3u;
        L800B7E68: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S5 = mem.ReadU32(c.T0);
        c.S6 = mem.ReadU32((c.T0 + 0x4u));
        c.S7 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x2u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T7 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B7F08;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B7F08: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.T7 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B7F30;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B8234;
        }
        L800B7F30: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S3 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B8234;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S2 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B8234;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x18u), _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x7u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
            goto L800B7FB4;
        }
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xFu));
        if ((int)c.V0 >= 0) {
            goto L800B7FA8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x17u));
        if ((int)c.V0 < 0) {
            goto L800B8234;
        }
        L800B7FA8: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x7u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        L800B7FB4: ;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B7FE4;
        }
        c.V0 = c.T7 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xFu));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B7FE4;
        }
        c.V0 = c.T7 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x17u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B8234;
        }
        c.V0 = c.T7 & 0x0020u;
        L800B7FE4: ;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0006u;
            goto L800B81C0;
        }
        c.V0 = c.T7 & 0x0006u;
        if (c.V0 != c.S0) {
            goto L800B8148;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            c.A0 = c.SP + 0x8u;
            goto L800B8148;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B8184;
        L800B8148: ;
        c.A0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = mem.ReadU16(c.A2);
        c.A0 = c.A0 << 3;
        c.A0 = c.A1 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.A1 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B8184: ;
        if (c.S4 == 0u) {
            goto L800B81A0;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B81AC;
        L800B81A0: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B81AC: ;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x14u), _sw); }
        goto L800B81E4;
        L800B81C0: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x1u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x9u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x11u), c.V0);
        L800B81E4: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T8;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T9;
        c.V0 = c.V0 & c.T4;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.V1 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.T1 & c.T4;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T8;
        c.V0 = mem.ReadU32(c.V1);
        c.T1 = c.T1 + 0x1Cu;
        c.V0 = c.V0 & c.T9;
        c.V0 = c.V0 | c.A0;
        mem.WriteU32(c.V1, c.V0);
        mem.WriteU8(c.A3, (byte)c.S0);
        c.A3 = c.A3 + 0x1Cu;
        L800B8234: ;
        c.A2 = c.A2 + 0x10u;
        c.T3 = c.T3 + 0x10u;
        c.V1 = c.T2;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T2 = c.T2 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B7E68;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B8254: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S7 = mem.ReadU32((c.SP + 0x3Cu));
        c.S6 = mem.ReadU32((c.SP + 0x38u));
        c.S5 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = mem.ReadU32((c.SP + 0x30u));
        c.S3 = mem.ReadU32((c.SP + 0x2Cu));
        c.S2 = mem.ReadU32((c.SP + 0x28u));
        c.S1 = mem.ReadU32((c.SP + 0x24u));
        c.S0 = mem.ReadU32((c.SP + 0x20u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x40u;
        return;
    }
    public static void func_800B8288(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x58u;
        c.T9 = c.A0;
        c.T5 = c.A2;
        mem.WriteU32((c.SP + 0x54u), c.S7);
        mem.WriteU32((c.SP + 0x50u), c.S6);
        mem.WriteU32((c.SP + 0x4Cu), c.S5);
        mem.WriteU32((c.SP + 0x48u), c.S4);
        mem.WriteU32((c.SP + 0x44u), c.S3);
        mem.WriteU32((c.SP + 0x40u), c.S2);
        mem.WriteU32((c.SP + 0x3Cu), c.S1);
        mem.WriteU32((c.SP + 0x38u), c.S0);
        c.V0 = mem.ReadU16(c.T5);
        c.T6 = mem.ReadU16((c.T5 - 0x2u));
        c.T1 = mem.ReadU32((c.SP + 0x68u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32(c.T1, c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x2u));
        c.T7 = 0x1F800000u;
        c.T7 = mem.ReadU16((c.T7 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x4u));
        c.T4 = mem.ReadU32((c.T1 + 0x20u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x6u));
        c.S4 = mem.ReadU16((c.T1 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0xCu), c.V0);
        c.S7 = mem.ReadU32((c.SP + 0x6Cu));
        c.S1 = mem.ReadU32((c.SP + 0x70u));
        c.T0 = c.A3;
        mem.WriteU32((c.SP + 0x28u), c.S4);
        c.S5 = mem.ReadU32((c.T1 + 0x28u));
        c.V1 = c.T6;
        mem.WriteU32((c.SP + 0x2Cu), c.S5);
        c.S6 = mem.ReadU32((c.T1 + 0x2Cu));
        c.T6 = c.T6 - 0x1u;
        if (c.V1 == 0u) {
            mem.WriteU32((c.SP + 0x30u), c.S6);
            goto L800B8974;
        }
        mem.WriteU32((c.SP + 0x30u), c.S6);
        c.S2 = c.T1 + 0x34u;
        c.S0 = c.S1 & 0x0006u;
        c.S3 = 0x00000006u;
        c.T2 = 0x00FF0000u;
        c.T2 = c.T2 | 0xFFFFu;
        c.T8 = 0xFF000000u;
        c.A2 = c.A2 + 0xCu;
        c.A3 = c.A3 + 0x3u;
        L800B8358: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T1);
        c.S5 = mem.ReadU32((c.T1 + 0x4u));
        c.S6 = mem.ReadU32((c.T1 + 0xCu));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32(c.T1, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T3 = 0u;
        c.S4 = mem.ReadU32((c.T1 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.S2, _sw); }
        c.V0 = c.T0 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.S1 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B8424;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B8424: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V1 = mem.ReadU32((c.T1 + 0x34u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.T1 + 0x8u), c.V0);
            goto L800B8474;
        }
        mem.WriteU32((c.T1 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.S2, _sw); }
        c.V0 = mem.ReadU32((c.T1 + 0x34u));
        if ((int)c.V0 < 0) {
            c.V0 = c.S1 & 0x0002u;
            goto L800B8464;
        }
        c.V0 = c.S1 & 0x0002u;
        c.T3 = 0x00000001u;
        L800B8464: ;
        if (c.V0 != 0u) {
            goto L800B8474;
        }
        if (c.T3 != 0u) {
            goto L800B8954;
        }
        L800B8474: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T1 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.S5 = mem.ReadU32((c.SP + 0x28u));
        c.V1 = (uint)(short)mem.ReadU16((c.T1 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S5 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T1 + 0x38u), c.V1);
        c.S6 = mem.ReadU32((c.SP + 0x30u));
        c.V0 = (int)c.V1 < (int)c.S6 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B8954;
        }
        c.S4 = mem.ReadU32((c.SP + 0x2Cu));
        c.V0 = (int)c.V1 < (int)c.S4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T0 + 0x10u;
            goto L800B8954;
        }
        c.A0 = c.T0 + 0x10u;
        c.V1 = c.T0 + 0x18u;
        c.V0 = c.T0 + 0x20u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x7u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
            goto L800B8528;
        }
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xFu));
        if ((int)c.V0 >= 0) {
            goto L800B851C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x17u));
        if ((int)c.V0 >= 0) {
            goto L800B851C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x1Fu));
        if ((int)c.V0 < 0) {
            goto L800B8954;
        }
        L800B851C: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x7u));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        L800B8528: ;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B856C;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xFu));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B856C;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x17u));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B856C;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x1Fu));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B8954;
        }
        c.V0 = c.S1 & 0x0020u;
        L800B856C: ;
        if (c.V0 != 0u) {
            goto L800B88CC;
        }
        if (c.S0 != c.S3) {
            goto L800B8728;
        }
        if (c.T3 == 0u) {
            c.A0 = c.SP + 0x8u;
            goto L800B8728;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x20u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x22u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x24u), (ushort)c.V0);
        c.V0 = c.SP + 0x20u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B8764;
        L800B8728: ;
        c.A0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.A0 = c.A0 << 3;
        c.A0 = c.A1 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.A1 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B8764: ;
        if (c.S7 == 0u) {
            goto L800B8818;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T0 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T0 + 0x14u), _sw); }
        if (c.S0 != c.S3) {
            goto L800B87EC;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B87EC;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B8804;
        L800B87EC: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B8804: ;
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T0 + 0x1Cu;
        goto L800B88C0;
        L800B8818: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T0 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T0 + 0x14u), _sw); }
        if (c.S0 != c.S3) {
            goto L800B8898;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B8898;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B88B0;
        L800B8898: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B88B0: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T0 + 0x1Cu;
        L800B88C0: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        c.V0 = 0x00000008u;
        goto L800B8948;
        L800B88CC: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x1u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x9u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x11u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x19u), c.V0);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000008u;
        L800B8948: ;
        mem.WriteU8(c.A3, (byte)c.V0);
        c.A3 = c.A3 + 0x24u;
        c.T0 = c.T0 + 0x24u;
        L800B8954: ;
        c.A2 = c.A2 + 0x14u;
        c.T5 = c.T5 + 0x14u;
        c.V1 = c.T6;
        c.V0 = mem.ReadU32((c.T1 + 0x44u));
        c.T6 = c.T6 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T1 + 0x44u), c.V0);
            goto L800B8358;
        }
        mem.WriteU32((c.T1 + 0x44u), c.V0);
        L800B8974: ;
        mem.WriteU32((c.T1 + 0x40u), c.T5);
        c.S7 = mem.ReadU32((c.SP + 0x54u));
        c.S6 = mem.ReadU32((c.SP + 0x50u));
        c.S5 = mem.ReadU32((c.SP + 0x4Cu));
        c.S4 = mem.ReadU32((c.SP + 0x48u));
        c.S3 = mem.ReadU32((c.SP + 0x44u));
        c.S2 = mem.ReadU32((c.SP + 0x40u));
        c.S1 = mem.ReadU32((c.SP + 0x3Cu));
        c.S0 = mem.ReadU32((c.SP + 0x38u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T0);
        c.SP = c.SP + 0x58u;
        return;
    }
    public static void func_800B89A8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x68u;
        mem.WriteU32((c.SP + 0x40u), c.S0);
        c.S0 = c.A0;
        c.T2 = c.A1;
        c.T7 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x60u), c.FP);
        mem.WriteU32((c.SP + 0x5Cu), c.S7);
        mem.WriteU32((c.SP + 0x58u), c.S6);
        mem.WriteU32((c.SP + 0x54u), c.S5);
        mem.WriteU32((c.SP + 0x50u), c.S4);
        mem.WriteU32((c.SP + 0x4Cu), c.S3);
        mem.WriteU32((c.SP + 0x48u), c.S2);
        mem.WriteU32((c.SP + 0x44u), c.S1);
        c.T3 = mem.ReadU16((c.T7 - 0x2u));
        c.V0 = mem.ReadU16(c.T7);
        c.V1 = mem.ReadU32((c.GP + 0x7C4u));
        c.T0 = mem.ReadU32((c.SP + 0x78u));
        c.T9 = 0x1F800000u;
        c.T9 = mem.ReadU16((c.T9 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.A0 = mem.ReadU8((c.V1 + 0x4u));
        c.V1 = mem.ReadU8((c.V1 + 0x5u));
        c.V0 = c.S0 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T7 + 0x2u));
        c.A1 = c.T3;
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T7 + 0x4u));
        c.S4 = mem.ReadU32((c.T0 + 0x20u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S2 = mem.ReadU32((c.SP + 0x80u));
        c.T3 = c.T3 - 0x1u;
        mem.WriteU32((c.SP + 0x38u), c.S4);
        c.S6 = mem.ReadU16((c.T0 + 0x24u));
        c.S3 = c.A0 + 0x40u;
        mem.WriteU32((c.SP + 0x34u), c.S6);
        c.FP = mem.ReadU32((c.T0 + 0x28u));
        c.S7 = mem.ReadU32((c.T0 + 0x2Cu));
        c.S1 = c.V1 + 0x40u;
        if (c.A1 == 0u) {
            mem.WriteU32((c.SP + 0x30u), c.S7);
            goto L800B9034;
        }
        mem.WriteU32((c.SP + 0x30u), c.S7);
        c.T8 = c.SP + 0x20u;
        c.A1 = 0x00FF0000u;
        c.A1 = c.A1 | 0xFFFFu;
        c.S5 = 0xFF000000u;
        c.A2 = c.A2 + 0x10u;
        c.A3 = c.A3 + 0x1Au;
        L800B8A78: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T0);
        c.S6 = mem.ReadU32((c.T0 + 0x4u));
        c.S7 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.S2 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B8B18;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B8B18: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.S2 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B8B40;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B9014;
        }
        L800B8B40: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.S4 = mem.ReadU32((c.SP + 0x34u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S4 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.S6 = mem.ReadU32((c.SP + 0x30u));
        c.V0 = (int)c.V1 < (int)c.S6 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = (int)c.V1 < (int)c.FP ? 1u : 0u;
            goto L800B9014;
        }
        c.V0 = (int)c.V1 < (int)c.FP ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B9014;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x20u), _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x10u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
            goto L800B8BD0;
        }
        c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x4u));
        if ((int)c.V0 >= 0) {
            goto L800B8BC4;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x8u));
        if ((int)c.V0 < 0) {
            goto L800B9014;
        }
        L800B8BC4: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x10u));
        c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
        L800B8BD0: ;
        if (c.V0 != 0u) {
            goto L800B8C00;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x4u));
        c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B8C00;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x8u));
        c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B9014;
        }
        L800B8C00: ;
        c.S7 = 0x1F800000u;
        c.S7 = mem.ReadU32((c.S7 + 0x38u));
        c.T4 = mem.ReadU32(c.S7);
        c.T5 = mem.ReadU32((c.S7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S7 + 0x8u));
        c.T5 = mem.ReadU32((c.S7 + 0xCu));
        c.T6 = mem.ReadU32((c.S7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V0 = mem.ReadU16((c.A2 - 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.T8, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.T8 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.T8 + 0x8u), _sw); }
        c.S4 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = mem.ReadU32((c.SP + 0x24u));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S1 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 - 0xEu), (ushort)c.V1);
        c.V0 = mem.ReadU16((c.A2 - 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.T8, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.T8 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.T8 + 0x8u), _sw); }
        c.S6 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = mem.ReadU32((c.SP + 0x24u));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S1 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 - 0x2u), (ushort)c.V1);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.T8, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.T8 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.T8 + 0x8u), _sw); }
        c.V1 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = mem.ReadU32((c.SP + 0x24u));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S1 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 + 0xAu), (ushort)c.V1);
        c.S7 = 0x1F800000u;
        c.S7 = mem.ReadU32((c.S7 + 0x34u));
        c.T4 = mem.ReadU32(c.S7);
        c.T5 = mem.ReadU32((c.S7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S7 + 0x8u));
        c.T5 = mem.ReadU32((c.S7 + 0xCu));
        c.T6 = mem.ReadU32((c.S7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.V0 = c.S2 & 0x0020u;
        if (c.V0 != 0u) {
            c.V1 = c.S2 & 0x0006u;
            goto L800B8F78;
        }
        c.V1 = c.S2 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B8EF8;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            c.A0 = c.SP + 0x8u;
            goto L800B8EF8;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B8F34;
        L800B8EF8: ;
        c.A0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = mem.ReadU16(c.A2);
        c.A0 = c.A0 << 3;
        c.A0 = c.T2 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.T2 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B8F34: ;
        c.S4 = mem.ReadU32((c.SP + 0x7Cu));
        if (c.S4 == 0u) {
            goto L800B8F58;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B8F64;
        L800B8F58: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B8F64: ;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x1Cu), _sw); }
        goto L800B8F9C;
        L800B8F78: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x16u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0xAu), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x2u), c.V0);
        L800B8F9C: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.S6 = mem.ReadU32((c.SP + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.S6;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S5;
        c.V0 = c.V0 & c.A1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.S6;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.A1;
        c.V0 = c.V0 & c.S5;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000009u;
        mem.WriteU8((c.A3 - 0x17u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.GP + 0x7C4u));
        c.V0 = mem.ReadU16((c.V1 + 0x6u));
        mem.WriteU16((c.A3 - 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.V1 + 0xAu));
        c.T1 = c.T1 + 0x28u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x28u;
        L800B9014: ;
        c.A2 = c.A2 + 0x18u;
        c.T7 = c.T7 + 0x18u;
        c.V1 = c.T3;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T3 = c.T3 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B8A78;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B9034: ;
        mem.WriteU32((c.T0 + 0x40u), c.T7);
        c.FP = mem.ReadU32((c.SP + 0x60u));
        c.S7 = mem.ReadU32((c.SP + 0x5Cu));
        c.S6 = mem.ReadU32((c.SP + 0x58u));
        c.S5 = mem.ReadU32((c.SP + 0x54u));
        c.S4 = mem.ReadU32((c.SP + 0x50u));
        c.S3 = mem.ReadU32((c.SP + 0x4Cu));
        c.S2 = mem.ReadU32((c.SP + 0x48u));
        c.S1 = mem.ReadU32((c.SP + 0x44u));
        c.S0 = mem.ReadU32((c.SP + 0x40u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x68u;
        return;
    }
    public static void func_800B906C(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x78u;
        mem.WriteU32((c.SP + 0x64u), c.S5);
        c.S5 = c.A0;
        c.T2 = c.A1;
        c.T8 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x70u), c.FP);
        mem.WriteU32((c.SP + 0x6Cu), c.S7);
        mem.WriteU32((c.SP + 0x68u), c.S6);
        mem.WriteU32((c.SP + 0x60u), c.S4);
        mem.WriteU32((c.SP + 0x5Cu), c.S3);
        mem.WriteU32((c.SP + 0x58u), c.S2);
        mem.WriteU32((c.SP + 0x54u), c.S1);
        mem.WriteU32((c.SP + 0x50u), c.S0);
        c.V0 = mem.ReadU16(c.T8);
        c.T9 = mem.ReadU16((c.T8 - 0x2u));
        c.V1 = mem.ReadU32((c.GP + 0x7C4u));
        c.T0 = mem.ReadU32((c.SP + 0x88u));
        c.S1 = 0x1F800000u;
        c.S1 = mem.ReadU16((c.S1 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        c.A0 = mem.ReadU8((c.V1 + 0x4u));
        c.V1 = mem.ReadU8((c.V1 + 0x5u));
        c.A1 = c.T9;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T8 + 0x2u));
        c.T9 = c.T9 - 0x1u;
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T8 + 0x4u));
        c.T7 = mem.ReadU32((c.T0 + 0x20u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T8 + 0x6u));
        c.S0 = mem.ReadU16((c.T0 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        mem.WriteU32((c.SP + 0x38u), c.S0);
        c.S6 = mem.ReadU32((c.T0 + 0x28u));
        c.S3 = c.A0 + 0x40u;
        mem.WriteU32((c.SP + 0x3Cu), c.S6);
        c.S7 = mem.ReadU32((c.T0 + 0x2Cu));
        c.S2 = c.V1 + 0x40u;
        if (c.A1 == 0u) {
            mem.WriteU32((c.SP + 0x40u), c.S7);
            goto L800B99CC;
        }
        mem.WriteU32((c.SP + 0x40u), c.S7);
        c.FP = c.T0 + 0x34u;
        c.S0 = c.SP + 0x28u;
        c.A1 = 0x00FF0000u;
        c.A1 = c.A1 | 0xFFFFu;
        c.S4 = 0xFF000000u;
        c.A2 = c.A2 + 0x14u;
        c.S6 = mem.ReadU32((c.SP + 0x90u));
        c.A3 = c.A3 + 0x1Au;
        mem.WriteU32((c.SP + 0x4Cu), c.S0);
        c.S6 = c.S6 & 0x0006u;
        mem.WriteU32((c.SP + 0x48u), c.S6);
        L800B915C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S7 = mem.ReadU32(c.T0);
        c.S0 = mem.ReadU32((c.T0 + 0x4u));
        c.S6 = mem.ReadU32((c.T0 + 0xCu));
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S0); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S0 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T3 = 0u;
        c.S7 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.FP, _sw); }
        c.V0 = c.T1 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.S0 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B9228;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B9228: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V1 = mem.ReadU32((c.T0 + 0x34u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.T0 + 0x8u), c.V0);
            goto L800B9284;
        }
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.FP, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 < 0) {
            goto L800B9268;
        }
        c.T3 = 0x00000001u;
        L800B9268: ;
        c.S6 = mem.ReadU32((c.SP + 0x90u));
        c.V0 = c.S6 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B9284;
        }
        if (c.T3 != 0u) {
            goto L800B99AC;
        }
        L800B9284: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.S7 = mem.ReadU32((c.SP + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S7 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.S0 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = (int)c.V1 < (int)c.S0 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B99AC;
        }
        c.S6 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = (int)c.V1 < (int)c.S6 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T1 + 0x14u;
            goto L800B99AC;
        }
        c.A0 = c.T1 + 0x14u;
        c.V1 = c.T1 + 0x20u;
        c.V0 = c.T1 + 0x2Cu;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x10u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
            goto L800B9338;
        }
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x4u));
        if ((int)c.V0 >= 0) {
            goto L800B932C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x8u));
        if ((int)c.V0 >= 0) {
            goto L800B932C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x14u));
        if ((int)c.V0 < 0) {
            goto L800B99AC;
        }
        L800B932C: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x10u));
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        L800B9338: ;
        if (c.V0 != 0u) {
            goto L800B937C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x4u));
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B937C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x8u));
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B937C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x14u));
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B99AC;
        }
        L800B937C: ;
        c.S7 = 0x1F800000u;
        c.S7 = mem.ReadU32((c.S7 + 0x38u));
        c.T4 = mem.ReadU32(c.S7);
        c.T5 = mem.ReadU32((c.S7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S7 + 0x8u));
        c.T5 = mem.ReadU32((c.S7 + 0xCu));
        c.T6 = mem.ReadU32((c.S7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        c.S0 = mem.ReadU32((c.SP + 0x4Cu));
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.S0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.S0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.S0 + 0x8u), _sw); }
        c.S6 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = mem.ReadU32((c.SP + 0x2Cu));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S2 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 - 0xEu), (ushort)c.V1);
        c.V0 = mem.ReadU16((c.A2 - 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.S0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.S0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.S0 + 0x8u), _sw); }
        c.S7 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = mem.ReadU32((c.SP + 0x2Cu));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S2 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 - 0x2u), (ushort)c.V1);
        c.V0 = mem.ReadU16((c.A2 - 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.S0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.S0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.S0 + 0x8u), _sw); }
        c.S0 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = mem.ReadU32((c.SP + 0x2Cu));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S2 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 + 0x16u), (ushort)c.V1);
        c.S6 = mem.ReadU32((c.SP + 0x4Cu));
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.S6, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.S6 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.S6 + 0x8u), _sw); }
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = mem.ReadU32((c.SP + 0x2Cu));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S2 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 + 0xAu), (ushort)c.V1);
        c.S7 = 0x1F800000u;
        c.S7 = mem.ReadU32((c.S7 + 0x34u));
        c.T4 = mem.ReadU32(c.S7);
        c.T5 = mem.ReadU32((c.S7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S7 + 0x8u));
        c.T5 = mem.ReadU32((c.S7 + 0xCu));
        c.T6 = mem.ReadU32((c.S7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        c.V0 = c.S0 & 0x0020u;
        if (c.V0 != 0u) {
            c.S7 = 0x00000006u;
            goto L800B9908;
        }
        c.S7 = 0x00000006u;
        c.S6 = mem.ReadU32((c.SP + 0x48u));
        if (c.S6 != c.S7) {
            goto L800B974C;
        }
        if (c.T3 == 0u) {
            c.A0 = c.SP + 0x8u;
            goto L800B974C;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x20u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x22u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x24u), (ushort)c.V0);
        c.V0 = c.SP + 0x20u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B9788;
        L800B974C: ;
        c.A0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.A0 = c.A0 << 3;
        c.A0 = c.T2 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.T2 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B9788: ;
        c.S0 = mem.ReadU32((c.SP + 0x8Cu));
        if (c.S0 == 0u) {
            goto L800B984C;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T7;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S4;
        c.V0 = c.V0 & c.A1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T7;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.A1;
        c.V0 = c.V0 & c.S4;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x1Cu), _sw); }
        c.S6 = mem.ReadU32((c.SP + 0x48u));
        c.S7 = 0x00000006u;
        if (c.S6 != c.S7) {
            goto L800B9820;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B9820;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B9838;
        L800B9820: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B9838: ;
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x28u;
        goto L800B98FC;
        L800B984C: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T7;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S4;
        c.V0 = c.V0 & c.A1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T7;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.A1;
        c.V0 = c.V0 & c.S4;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x1Cu), _sw); }
        c.S0 = mem.ReadU32((c.SP + 0x48u));
        c.S6 = 0x00000006u;
        if (c.S0 != c.S6) {
            goto L800B98D4;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B98D4;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B98EC;
        L800B98D4: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B98EC: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x28u;
        L800B98FC: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        c.V0 = 0x0000000Cu;
        goto L800B9984;
        L800B9908: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x16u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0xAu), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x2u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0xEu), c.V0);
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T7;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S4;
        c.V0 = c.V0 & c.A1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T7;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.A1;
        c.V0 = c.V0 & c.S4;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x0000000Cu;
        L800B9984: ;
        mem.WriteU8((c.A3 - 0x17u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.GP + 0x7C4u));
        c.V0 = mem.ReadU16((c.V1 + 0x6u));
        mem.WriteU16((c.A3 - 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.V1 + 0xAu));
        c.T1 = c.T1 + 0x34u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x34u;
        L800B99AC: ;
        c.A2 = c.A2 + 0x1Cu;
        c.T8 = c.T8 + 0x1Cu;
        c.V1 = c.T9;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T9 = c.T9 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B915C;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B99CC: ;
        mem.WriteU32((c.T0 + 0x40u), c.T8);
        c.FP = mem.ReadU32((c.SP + 0x70u));
        c.S7 = mem.ReadU32((c.SP + 0x6Cu));
        c.S6 = mem.ReadU32((c.SP + 0x68u));
        c.S5 = mem.ReadU32((c.SP + 0x64u));
        c.S4 = mem.ReadU32((c.SP + 0x60u));
        c.S3 = mem.ReadU32((c.SP + 0x5Cu));
        c.S2 = mem.ReadU32((c.SP + 0x58u));
        c.S1 = mem.ReadU32((c.SP + 0x54u));
        c.S0 = mem.ReadU32((c.SP + 0x50u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x78u;
        return;
    }
}
