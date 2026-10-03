using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Dispatch;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class LandMapPatch
{
    const int BaseWidth = 512;
    static int Margin => Display.WideMargin(BaseWidth);
    public static uint RightEdge(uint x) => unchecked(x + (uint)(BaseWidth + Margin));
    public static uint LeftEdge(uint x) => unchecked((uint)-Margin - x);

    public static void func_80012B38(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = 0x80010000u;
        c.V1 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.V0 + 0x4F98u));
        c.V0 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V0 << 2;
        c.V0 = c.A0 + c.V0;
        c.A3 = mem.ReadU32((c.V0 + 0x74u));
        c.V0 = 0x80010000u;
        c.T0 = mem.ReadU32((c.A0 + 0x8u));
        c.A1 = c.V0 + 0x45F0u;
        c.V1 = c.T0;
        if (c.V1 == 0u) {
            c.T0 = c.T0 - 0x1u;
            goto L80012C40;
        }
        c.T0 = c.T0 - 0x1u;
        c.T3 = c.A0;
        c.V0 = 0x1F800000u;
        c.T2 = mem.ReadU32((c.V0 + 0x88u));
        c.T1 = 0x00FF0000u;
        c.T1 = c.T1 | 0xFFFFu;
        c.T4 = 0xFF000000u;
        c.A2 = c.A1 + 0x8u;
        L80012B8C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16(c.T3);
        c.A1 = c.A3 + 0x20u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.A1 + 0x8u), (ushort)c.V0);
        c.V1 = mem.ReadU16(c.A2);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.A0 = (uint)(short)mem.ReadU16((c.A1 + 0x8u));
        c.V1 = c.V1 - c.V0;
        c.V0 = (int)c.A0 < (int)RightEdge(0) ? 1u : 0u;
        if (c.V0 == 0u) {
            mem.WriteU16((c.A1 + 0xAu), (ushort)c.V1);
            goto L80012C2C;
        }
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.V1);
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x10u));
        c.V0 = c.A0 + c.V0;
        if ((int)c.V0 < (int)LeftEdge(0)) {
            c.V0 = c.V1 << 16;
            goto L80012C2C;
        }
        c.V0 = c.V1 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 240 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L80012C2C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x12u));
        c.V0 = c.V1 + c.V0;
        if ((int)c.V0 < 0) {
            goto L80012C2C;
        }
        c.V0 = mem.ReadU32((c.T2 + 0x78u));
        c.V1 = mem.ReadU32((c.A3 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 & c.T4;
        c.V0 = c.V0 & c.T1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32((c.A3 + 0x20u), c.V1);
        c.A0 = mem.ReadU32((c.T2 + 0x78u));
        c.V0 = mem.ReadU32((c.A0 + 0x28u));
        c.V1 = c.A3 & c.T1;
        c.V0 = c.V0 & c.T4;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32((c.A0 + 0x28u), c.V0);
        L80012C2C: ;
        c.A3 = c.A1 + 0x14u;
        c.A2 = c.A2 + 0xCu;
        c.V0 = c.T0;
        if (c.V0 != 0u) {
            c.T0 = c.T0 - 0x1u;
            goto L80012B8C;
        }
        c.T0 = c.T0 - 0x1u;
        L80012C40: ;
        return;
    }

    public static void func_80012EEC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x168u;
        c.T8 = 0x00000014u;
        c.V0 = 0x1F800000u;
        c.V0 = c.V0 + 0x60u;
        c.T7 = c.V0;
        c.V0 = 0x800E0000u;
        c.T3 = c.V0 + 0x11E0u;
        mem.WriteU32((c.SP + 0x120u), c.T8);
        c.T8 = c.SP + 0x18u;
        c.V0 = 0x00001000u;
        mem.WriteU32((c.SP + 0x164u), c.RA);
        mem.WriteU32((c.SP + 0x160u), c.FP);
        mem.WriteU32((c.SP + 0x15Cu), c.S7);
        mem.WriteU32((c.SP + 0x158u), c.S6);
        mem.WriteU32((c.SP + 0x154u), c.S5);
        mem.WriteU32((c.SP + 0x150u), c.S4);
        mem.WriteU32((c.SP + 0x14Cu), c.S3);
        mem.WriteU32((c.SP + 0x148u), c.S2);
        mem.WriteU32((c.SP + 0x144u), c.S1);
        mem.WriteU32((c.SP + 0x140u), c.S0);
        mem.WriteU32((c.SP + 0x12Cu), c.T8);
        mem.WriteU16((c.T7 + 0xCu), (ushort)0u);
        mem.WriteU16((c.T7 + 0xEu), (ushort)0u);
        mem.WriteU16((c.T7 + 0x10u), (ushort)0u);
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = 0x80010000u;
        mem.WriteU32((c.V0 + 0x4F88u), 0u);
        L80012F58: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = 0x80010000u;
        c.T8 = mem.ReadU32((c.SP + 0x120u));
        c.V0 = mem.ReadU32((c.V0 + 0x4F98u));
        c.V1 = c.T8 << 2;
        c.V0 = c.V0 + c.V1;
        c.FP = mem.ReadU32((c.V0 + 0x7Cu));
        if (c.FP == 0u) {
            c.V0 = 0x1F800000u;
            goto L80013714;
        }
        c.V0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.V0 + 0x4u));
        c.V0 = c.A0 << 5;
        c.V0 = c.V0 + 0x1Cu;
        c.A1 = c.FP + c.V0;
        c.V1 = c.A0 << 1;
        c.V1 = c.V1 + c.A0;
        c.V0 = c.V1 << 4;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 5;
        c.V0 = c.V0 + 0x37Cu;
        c.T1 = c.FP + c.V0;
        c.V1 = c.V1 << 3;
        c.V1 = c.V1 + c.A0;
        c.V1 = c.V1 << 4;
        c.V1 = c.V1 + 0x5Cu;
        c.V0 = mem.ReadU32((c.FP + 0x14u));
        c.V0 = c.V0 & 0x0006u;
        if (c.V0 == 0u) {
            c.T2 = c.FP + c.V1;
            goto L800131DC;
        }
        c.T2 = c.FP + c.V1;
        c.V0 = mem.ReadU32((c.FP + 0x18u));
        if (c.V0 != 0u) {
            c.V0 = 0x1F800000u;
            goto L8001317C;
        }
        c.V0 = 0x1F800000u;
        c.V0 = 0x00000040u;
        mem.WriteU16((c.A1 + 0x16u), (ushort)c.V0);
        c.V1 = mem.ReadU32((c.FP + 0xF0Cu));
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 >> 3;
        mem.WriteU16((c.SP + 0x118u), (ushort)c.V0);
        c.V1 = mem.ReadU32((c.FP + 0xF0Cu));
        c.V0 = c.V1 << 8;
        c.V0 = c.V0 - c.V1;
        c.V0 = (uint)((int)c.V0 >> 8);
        mem.WriteU16((c.SP + 0x11Au), (ushort)c.V0);
        c.V1 = mem.ReadU32((c.FP + 0xF0Cu));
        c.V0 = 0x00000100u;
        if (c.V1 == c.V0) {
            c.S7 = 0x00000014u;
            goto L80013068;
        }
        c.S7 = 0x00000014u;
        c.V1 = c.T2 + 0x6u;
        L8001302C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8((c.V1 + 0x1u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x2u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x1u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        c.S7 = c.S7 - 0x1u;
        mem.WriteU8(c.V1, (byte)c.V0);
        if ((int)c.S7 > 0) {
            c.V1 = c.V1 + 0x14u;
            goto L8001302C;
        }
        c.V1 = c.V1 + 0x14u;
        L80013068: ;
        c.S7 = 0x00000014u;
        c.V1 = c.T1 + 0x1Eu;
        L80013070: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x1Au), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x19u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x18u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x12u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x11u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x10u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0xAu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x9u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x8u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x2u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        c.S7 = c.S7 - 0x1u;
        mem.WriteU8((c.V1 - 0x1u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        c.T1 = c.T1 + 0x24u;
        mem.WriteU8(c.V1, (byte)c.V0);
        if ((int)c.S7 > 0) {
            c.V1 = c.V1 + 0x24u;
            goto L80013070;
        }
        c.V1 = c.V1 + 0x24u;
        c.S7 = 0x00000014u;
        c.V1 = c.T1 + 0x1Eu;
        L80013110: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x1Au), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x19u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x18u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x12u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x11u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        c.S7 = c.S7 - 0x1u;
        mem.WriteU8((c.V1 - 0xAu), (byte)0u);
        mem.WriteU8((c.V1 - 0x9u), (byte)0u);
        mem.WriteU8((c.V1 - 0x8u), (byte)0u);
        mem.WriteU8((c.V1 - 0x2u), (byte)0u);
        mem.WriteU8((c.V1 - 0x1u), (byte)0u);
        mem.WriteU8(c.V1, (byte)0u);
        mem.WriteU8((c.V1 - 0x10u), (byte)c.V0);
        if ((int)c.S7 > 0) {
            c.V1 = c.V1 + 0x24u;
            goto L80013110;
        }
        c.V1 = c.V1 + 0x24u;
        c.V0 = 0x1F800000u;
        L8001317C: ;
        c.V1 = mem.ReadU32((c.V0 + 0x4u));
        c.A0 = c.V1 << 1;
        c.A0 = c.A0 + c.V1;
        c.V0 = c.A0 << 3;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + 0x5Cu;
        c.T2 = c.FP + c.V0;
        c.V0 = c.A0 << 4;
        c.V0 = c.V0 - c.A0;
        c.V0 = c.V0 << 5;
        c.V0 = c.V0 + 0x37Cu;
        c.V1 = mem.ReadU32((c.FP + 0x14u));
        c.T1 = c.FP + c.V0;
        c.V0 = c.V1 & 0x0002u;
        if (c.V0 == 0u) {
            c.V0 = 0xFFFFFFFDu;
            goto L800131D0;
        }
        c.V0 = 0xFFFFFFFDu;
        c.V0 = c.V1 & c.V0;
        c.V0 = c.V0 | 0x0004u;
        goto L800131D8;
        L800131D0: ;
        c.V0 = 0xFFFFFFFBu;
        c.V0 = c.V1 & c.V0;
        L800131D8: ;
        mem.WriteU32((c.FP + 0x14u), c.V0);
        L800131DC: ;
        c.V0 = mem.ReadU32((c.FP + 0x14u));
        c.V0 = c.V0 & 0x0008u;
        if (c.V0 == 0u) {
            c.V0 = 0x800F0000u;
            goto L80013248;
        }
        c.V0 = 0x800F0000u;
        c.V1 = (uint)(short)mem.ReadU16((c.V0 - 0x2418u));
        c.V0 = 0x80010000u;
        c.T8 = 0x00000001u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.V0 + 0x4F88u), c.T8);
            goto L80013248;
        }
        mem.WriteU32((c.V0 + 0x4F88u), c.T8);
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x248Cu));
        if (c.V0 != 0u) {
            c.V0 = 0x80010000u;
            goto L8001324C;
        }
        c.V0 = 0x80010000u;
        c.V0 = mem.ReadU32((c.FP + 0xF0Cu));
        c.V0 = c.V0 - 0x3u;
        mem.WriteU32((c.FP + 0xF0Cu), c.V0);
        c.V0 = mem.ReadU32((c.FP + 0x14u));
        c.V1 = mem.ReadU32((c.FP + 0xF0Cu));
        c.A0 = c.V0 | 0x0002u;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.FP + 0x14u), c.A0);
            goto L80013248;
        }
        mem.WriteU32((c.FP + 0x14u), c.A0);
        c.V0 = 0xFFFFFFF6u;
        c.V0 = c.A0 & c.V0;
        mem.WriteU32((c.FP + 0x14u), c.V0);
        L80013248: ;
        c.V0 = 0x80010000u;
        L8001324C: ;
        c.A0 = mem.ReadU32((c.V0 + 0x4F98u));
        c.V1 = mem.ReadU32(c.FP);
        c.V0 = mem.ReadU32(c.A0);
        c.S7 = mem.ReadU32((c.FP + 0xCu));
        c.V1 = c.V1 - c.V0;
        mem.WriteU32((c.SP + 0x124u), c.V1);
        c.V1 = mem.ReadU32((c.FP + 0x4u));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        c.A0 = mem.ReadU32((c.FP + 0x8u));
        c.V1 = c.V1 - c.V0;
        c.V0 = (int)c.S7 < (int)c.A0 ? 1u : 0u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x128u), c.V1);
            goto L80013284;
        }
        mem.WriteU32((c.SP + 0x128u), c.V1);
        c.S7 = c.A0;
        L80013284: ;
        c.V0 = mem.ReadU32((c.FP + 0x14u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = RightEdge(c.S7);
            goto L800136F4;
        }
        c.V0 = RightEdge(c.S7);
        c.T8 = mem.ReadU32((c.SP + 0x124u));
        c.V0 = (int)c.T8 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V1 = 0u - c.S7;
            goto L800136F4;
        }
        c.V1 = 0u - c.S7;
        c.V0 = (int)c.T8 < (int)LeftEdge(c.S7) ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S7 + 0xF0u;
            goto L800136F4;
        }
        c.V0 = c.S7 + 0xF0u;
        c.T8 = mem.ReadU32((c.SP + 0x128u));
        c.V0 = (int)c.T8 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = (int)c.T8 < (int)c.V1 ? 1u : 0u;
            goto L800136F4;
        }
        c.V0 = (int)c.T8 < (int)c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S6 = c.SP + 0x28u;
            goto L800136F4;
        }
        c.S6 = c.SP + 0x28u;
        c.V0 = mem.ReadU32((c.FP + 0x10u));
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V1 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0x1F800000u;
        mem.WriteU16((c.V0 + 0x60u), (ushort)c.V1);
        c.V0 = mem.ReadU32((c.FP + 0x10u));
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V0 = mem.ReadU16(c.V0);
        c.T8 = mem.ReadU16((c.SP + 0x124u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.T7 + 0x4u), (ushort)c.T8);
        mem.WriteU16((c.T7 + 0x2u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.FP + 0x10u));
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V0 = mem.ReadU16(c.V0);
        mem.WriteU16((c.T7 + 0x6u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.FP + 0x10u));
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.T8 = mem.ReadU16((c.SP + 0x128u));
        mem.WriteU16((c.T7 + 0xAu), (ushort)c.T8);
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 + 0x60u;
        mem.WriteU16((c.T7 + 0x8u), (ushort)c.V0);
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
        c.V0 = mem.ReadU32((c.FP + 0x18u));
        if (c.V0 != 0u) {
            c.V1 = c.SP + 0x34u;
            goto L8001361C;
        }
        c.V1 = c.SP + 0x34u;
        c.S7 = 0u;
        c.A2 = c.FP;
        L800133AC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = 0x800F0000u;
        c.V1 = (uint)(short)mem.ReadU16((c.A2 + 0xEE4u));
        c.V0 = mem.ReadU32((c.V0 - 0x11E4u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.V0 = c.V1 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V1 = (uint)(short)mem.ReadU16((c.A2 + 0xEBCu));
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S1 = mem.ReadU32((c.FP + 0x8u));
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + 0x400u;
        { var _r = (long)(int)c.S1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A0 = 0x66660000u;
        c.S1 = c.LO;
        c.A0 = c.A0 | 0x6667u;
        c.V1 = c.S7 << 12;
        { var _r = (long)(int)c.V1 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A3 = c.HI;
        c.S0 = mem.ReadU32((c.FP + 0xCu));
        { var _r = (long)(int)c.S0 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = (uint)((int)c.V1 >> 31);
        c.V0 = (uint)((int)c.A3 >> 3);
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.S0 = c.LO;
        c.S3 = (uint)(short)mem.ReadU16((c.V0 + 0x2u));
        c.V1 = (uint)((int)c.S1 >> 12);
        { var _r = (long)(int)c.S3 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.S2 = (uint)(short)mem.ReadU16(c.V0);
        c.V0 = (uint)((int)c.S0 >> 12);
        { var _r = (long)(int)c.S2 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A1 = mem.ReadU32((c.SP + 0x12Cu));
        c.A0 = c.SP + 0x10u;
        c.S4 = (uint)((int)c.S1 >> 10);
        c.S1 = (uint)((int)c.S1 >> 11);
        mem.WriteU32((c.SP + 0x130u), c.A2);
        mem.WriteU32((c.SP + 0x134u), c.T3);
        mem.WriteU32((c.SP + 0x138u), c.T7);
        c.S5 = (uint)((int)c.S0 >> 10);
        c.S0 = (uint)((int)c.S0 >> 11);
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.A3 = c.LO;
        c.V0 = (uint)((int)c.A3 >> 12);
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.RA = 0x8001348Cu;
        MediEvil_game.func_800A4E0C(c, m);
        { var _r = (long)(int)c.S3 * (int)c.S1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A3 = c.LO;
        { var _r = (long)(int)c.S2 * (int)c.S0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16((c.SP + 0x18u));
        mem.WriteU16(c.S6, (ushort)c.V0);
        c.V0 = mem.ReadU16((c.SP + 0x1Cu));
        c.A1 = mem.ReadU32((c.SP + 0x12Cu));
        c.A0 = c.SP + 0x10u;
        mem.WriteU16((c.S6 + 0x2u), (ushort)c.V0);
        c.V0 = (uint)((int)c.A3 >> 12);
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.RA = 0x800134D4u;
        MediEvil_game.func_800A4E0C(c, m);
        { var _r = (long)(int)c.S3 * (int)c.S4; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A3 = c.LO;
        c.S6 = c.S6 + 0x4u;
        { var _r = (long)(int)c.S2 * (int)c.S5; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A2 = mem.ReadU32((c.SP + 0x130u));
        c.V0 = mem.ReadU16((c.SP + 0x18u));
        c.A0 = c.SP + 0x10u;
        mem.WriteU16(c.S6, (ushort)c.V0);
        c.V0 = mem.ReadU16((c.SP + 0x1Cu));
        c.A1 = mem.ReadU32((c.SP + 0x12Cu));
        c.A2 = c.A2 + 0x2u;
        mem.WriteU16((c.S6 + 0x2u), (ushort)c.V0);
        mem.WriteU32((c.SP + 0x130u), c.A2);
        c.V0 = (uint)((int)c.A3 >> 12);
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.RA = 0x80013524u;
        MediEvil_game.func_800A4E0C(c, m);
        c.V0 = mem.ReadU16((c.SP + 0x18u));
        c.S6 = c.S6 + 0x4u;
        mem.WriteU16(c.S6, (ushort)c.V0);
        c.V0 = mem.ReadU16((c.SP + 0x1Cu));
        c.S7 = c.S7 + 0x1u;
        mem.WriteU16((c.S6 + 0x2u), (ushort)c.V0);
        c.V0 = (int)c.S7 < 20 ? 1u : 0u;
        c.A2 = mem.ReadU32((c.SP + 0x130u));
        c.T3 = mem.ReadU32((c.SP + 0x134u));
        c.T7 = mem.ReadU32((c.SP + 0x138u));
        if (c.V0 != 0u) {
            c.S6 = c.S6 + 0x4u;
            goto L800133AC;
        }
        c.S6 = c.S6 + 0x4u;
        c.A2 = 0x00FF0000u;
        c.V0 = 0x1F800000u;
        c.A2 = c.A2 | 0xFFFFu;
        c.A1 = mem.ReadU32((c.V0 + 0x4u));
        c.T0 = 0xFF000000u;
        c.A0 = c.A1 << 1;
        c.A0 = c.A0 + c.A1;
        c.V0 = c.A0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + 0x5Cu;
        c.T2 = c.FP + c.V0;
        c.V0 = 0x1F800000u;
        c.A1 = c.A1 << 5;
        c.A3 = mem.ReadU32((c.V0 + 0x88u));
        c.A1 = c.A1 + 0x1Cu;
        c.V0 = mem.ReadU32((c.A3 + 0x78u));
        c.V1 = mem.ReadU32((c.T2 + 0x17Cu));
        c.V0 = mem.ReadU32((c.V0 + 0x20u));
        c.V1 = c.V1 & c.T0;
        c.V0 = c.V0 & c.A2;
        c.V1 = c.V1 | c.V0;
        c.V0 = c.A0 << 4;
        c.V0 = c.V0 - c.A0;
        c.V0 = c.V0 << 5;
        c.V0 = c.V0 + 0x37Cu;
        mem.WriteU32((c.T2 + 0x17Cu), c.V1);
        c.A0 = mem.ReadU32((c.A3 + 0x78u));
        c.T1 = c.FP + c.V0;
        c.V1 = mem.ReadU32((c.A0 + 0x20u));
        c.V0 = c.T2 & c.A2;
        c.V1 = c.V1 & c.T0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32((c.A0 + 0x20u), c.V1);
        c.V0 = mem.ReadU32((c.A3 + 0x78u));
        c.V1 = mem.ReadU32((c.T1 + 0x57Cu));
        c.V0 = mem.ReadU32((c.V0 + 0x20u));
        c.V1 = c.V1 & c.T0;
        c.V0 = c.V0 & c.A2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32((c.T1 + 0x57Cu), c.V1);
        c.V1 = mem.ReadU32((c.A3 + 0x78u));
        c.A1 = c.FP + c.A1;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        c.A2 = c.A1 & c.A2;
        c.V0 = c.V0 & c.T0;
        c.V0 = c.V0 | c.A2;
        mem.WriteU32((c.V1 + 0x20u), c.V0);
        c.S6 = c.SP + 0x28u;
        c.V1 = c.SP + 0x34u;
        L8001361C: ;
        c.S7 = 0x00000013u;
        c.A2 = c.T1 + 0x2F0u;
        c.A1 = c.T1 + 0x10u;
        c.A0 = c.T2 + 0x10u;
        L8001362C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32((c.FP + 0x18u));
        if (c.V0 != 0u) {
            goto L80013670;
        }
        c.T8 = mem.ReadU16((c.SP + 0x124u));
        mem.WriteU16((c.A0 - 0x8u), (ushort)c.T8);
        c.T8 = mem.ReadU16((c.SP + 0x128u));
        mem.WriteU16((c.A0 - 0x6u), (ushort)c.T8);
        c.V0 = mem.ReadU32(c.S6);
        mem.WriteU32((c.A0 - 0x4u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        mem.WriteU32(c.A0, c.V0);
        c.A0 = c.A0 + 0x14u;
        L80013670: ;
        c.V0 = mem.ReadU32(c.S6);
        mem.WriteU32((c.A1 + 0x8u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        c.S6 = c.S6 + 0x4u;
        mem.WriteU32((c.A1 + 0x10u), c.V0);
        c.V0 = mem.ReadU32(c.S6);
        c.V1 = c.V1 + 0x4u;
        mem.WriteU32((c.A1 - 0x8u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        c.T8 = 0x00000001u;
        mem.WriteU32(c.A1, c.V0);
        c.V0 = mem.ReadU32(c.S6);
        c.S6 = c.S6 + 0x4u;
        c.A1 = c.A1 + 0x24u;
        mem.WriteU32((c.A2 - 0x18u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        c.V1 = c.V1 + 0x4u;
        mem.WriteU32((c.A2 - 0x10u), c.V0);
        c.V0 = mem.ReadU32(c.S6);
        c.S6 = c.S6 + 0x4u;
        mem.WriteU32((c.A2 - 0x8u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        c.V1 = c.V1 + 0x4u;
        mem.WriteU32(c.A2, c.V0);
        if (c.S7 != c.T8) {
            c.A2 = c.A2 + 0x24u;
            goto L800136E0;
        }
        c.A2 = c.A2 + 0x24u;
        c.V1 = c.SP + 0x28u;
        L800136E0: ;
        c.V0 = c.S7;
        if (c.V0 != 0u) {
            c.S7 = c.S7 - 0x1u;
            goto L8001362C;
        }
        c.S7 = c.S7 - 0x1u;
        goto L80013710;
        L800136F4: ;
        c.A0 = mem.ReadU32((c.FP + 0x14u));
        c.V0 = 0x00000009u;
        c.V1 = c.A0 & 0x0009u;
        if (c.V1 != c.V0) {
            c.V0 = 0xFFFFFFF6u;
            goto L80013710;
        }
        c.V0 = 0xFFFFFFF6u;
        c.V0 = c.A0 & c.V0;
        mem.WriteU32((c.FP + 0x14u), c.V0);
        L80013710: ;
        c.T8 = mem.ReadU32((c.SP + 0x120u));
        L80013714: ;
        c.T8 = c.T8 - 0x1u;
        if ((int)c.T8 >= 0) {
            mem.WriteU32((c.SP + 0x120u), c.T8);
            goto L80012F58;
        }
        mem.WriteU32((c.SP + 0x120u), c.T8);
        c.RA = mem.ReadU32((c.SP + 0x164u));
        c.FP = mem.ReadU32((c.SP + 0x160u));
        c.S7 = mem.ReadU32((c.SP + 0x15Cu));
        c.S6 = mem.ReadU32((c.SP + 0x158u));
        c.S5 = mem.ReadU32((c.SP + 0x154u));
        c.S4 = mem.ReadU32((c.SP + 0x150u));
        c.S3 = mem.ReadU32((c.SP + 0x14Cu));
        c.S2 = mem.ReadU32((c.SP + 0x148u));
        c.S1 = mem.ReadU32((c.SP + 0x144u));
        c.S0 = mem.ReadU32((c.SP + 0x140u));
        c.SP = c.SP + 0x168u;
        return;
    }

    public static void func_80012F58_landmap(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = 0x80010000u;
        c.T8 = mem.ReadU32((c.SP + 0x120u));
        c.V0 = mem.ReadU32((c.V0 + 0x4F98u));
        c.V1 = c.T8 << 2;
        c.V0 = c.V0 + c.V1;
        c.FP = mem.ReadU32((c.V0 + 0x7Cu));
        if (c.FP == 0u) {
            c.V0 = 0x1F800000u;
            MediEvil_landmap.func_80013714(c, m);
            return;
        }
        c.V0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.V0 + 0x4u));
        c.V0 = c.A0 << 5;
        c.V0 = c.V0 + 0x1Cu;
        c.A1 = c.FP + c.V0;
        c.V1 = c.A0 << 1;
        c.V1 = c.V1 + c.A0;
        c.V0 = c.V1 << 4;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 5;
        c.V0 = c.V0 + 0x37Cu;
        c.T1 = c.FP + c.V0;
        c.V1 = c.V1 << 3;
        c.V1 = c.V1 + c.A0;
        c.V1 = c.V1 << 4;
        c.V1 = c.V1 + 0x5Cu;
        c.V0 = mem.ReadU32((c.FP + 0x14u));
        c.V0 = c.V0 & 0x0006u;
        if (c.V0 == 0u) {
            c.T2 = c.FP + c.V1;
            goto L800131DC;
        }
        c.T2 = c.FP + c.V1;
        c.V0 = mem.ReadU32((c.FP + 0x18u));
        if (c.V0 != 0u) {
            c.V0 = 0x1F800000u;
            goto L8001317C;
        }
        c.V0 = 0x1F800000u;
        c.V0 = 0x00000040u;
        mem.WriteU16((c.A1 + 0x16u), (ushort)c.V0);
        c.V1 = mem.ReadU32((c.FP + 0xF0Cu));
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 >> 3;
        mem.WriteU16((c.SP + 0x118u), (ushort)c.V0);
        c.V1 = mem.ReadU32((c.FP + 0xF0Cu));
        c.V0 = c.V1 << 8;
        c.V0 = c.V0 - c.V1;
        c.V0 = (uint)((int)c.V0 >> 8);
        mem.WriteU16((c.SP + 0x11Au), (ushort)c.V0);
        c.V1 = mem.ReadU32((c.FP + 0xF0Cu));
        c.V0 = 0x00000100u;
        if (c.V1 == c.V0) {
            c.S7 = 0x00000014u;
            goto L80013068;
        }
        c.S7 = 0x00000014u;
        c.V1 = c.T2 + 0x6u;
        L8001302C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8((c.V1 + 0x1u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x2u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x1u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        c.S7 = c.S7 - 0x1u;
        mem.WriteU8(c.V1, (byte)c.V0);
        if ((int)c.S7 > 0) {
            c.V1 = c.V1 + 0x14u;
            goto L8001302C;
        }
        c.V1 = c.V1 + 0x14u;
        L80013068: ;
        c.S7 = 0x00000014u;
        c.V1 = c.T1 + 0x1Eu;
        L80013070: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x1Au), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x19u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x18u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x12u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x11u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x10u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0xAu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x9u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x8u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        mem.WriteU8((c.V1 - 0x2u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        c.S7 = c.S7 - 0x1u;
        mem.WriteU8((c.V1 - 0x1u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x11Au));
        c.T1 = c.T1 + 0x24u;
        mem.WriteU8(c.V1, (byte)c.V0);
        if ((int)c.S7 > 0) {
            c.V1 = c.V1 + 0x24u;
            goto L80013070;
        }
        c.V1 = c.V1 + 0x24u;
        c.S7 = 0x00000014u;
        c.V1 = c.T1 + 0x1Eu;
        L80013110: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x1Au), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x19u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x18u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x12u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        mem.WriteU8((c.V1 - 0x11u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.SP + 0x118u));
        c.S7 = c.S7 - 0x1u;
        mem.WriteU8((c.V1 - 0xAu), (byte)0u);
        mem.WriteU8((c.V1 - 0x9u), (byte)0u);
        mem.WriteU8((c.V1 - 0x8u), (byte)0u);
        mem.WriteU8((c.V1 - 0x2u), (byte)0u);
        mem.WriteU8((c.V1 - 0x1u), (byte)0u);
        mem.WriteU8(c.V1, (byte)0u);
        mem.WriteU8((c.V1 - 0x10u), (byte)c.V0);
        if ((int)c.S7 > 0) {
            c.V1 = c.V1 + 0x24u;
            goto L80013110;
        }
        c.V1 = c.V1 + 0x24u;
        c.V0 = 0x1F800000u;
        L8001317C: ;
        c.V1 = mem.ReadU32((c.V0 + 0x4u));
        c.A0 = c.V1 << 1;
        c.A0 = c.A0 + c.V1;
        c.V0 = c.A0 << 3;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + 0x5Cu;
        c.T2 = c.FP + c.V0;
        c.V0 = c.A0 << 4;
        c.V0 = c.V0 - c.A0;
        c.V0 = c.V0 << 5;
        c.V0 = c.V0 + 0x37Cu;
        c.V1 = mem.ReadU32((c.FP + 0x14u));
        c.T1 = c.FP + c.V0;
        c.V0 = c.V1 & 0x0002u;
        if (c.V0 == 0u) {
            c.V0 = 0xFFFFFFFDu;
            goto L800131D0;
        }
        c.V0 = 0xFFFFFFFDu;
        c.V0 = c.V1 & c.V0;
        c.V0 = c.V0 | 0x0004u;
        goto L800131D8;
        L800131D0: ;
        c.V0 = 0xFFFFFFFBu;
        c.V0 = c.V1 & c.V0;
        L800131D8: ;
        mem.WriteU32((c.FP + 0x14u), c.V0);
        L800131DC: ;
        c.V0 = mem.ReadU32((c.FP + 0x14u));
        c.V0 = c.V0 & 0x0008u;
        if (c.V0 == 0u) {
            c.V0 = 0x800F0000u;
            goto L80013248;
        }
        c.V0 = 0x800F0000u;
        c.V1 = (uint)(short)mem.ReadU16((c.V0 - 0x2418u));
        c.V0 = 0x80010000u;
        c.T8 = 0x00000001u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.V0 + 0x4F88u), c.T8);
            goto L80013248;
        }
        mem.WriteU32((c.V0 + 0x4F88u), c.T8);
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - 0x248Cu));
        if (c.V0 != 0u) {
            c.V0 = 0x80010000u;
            goto L8001324C;
        }
        c.V0 = 0x80010000u;
        c.V0 = mem.ReadU32((c.FP + 0xF0Cu));
        c.V0 = c.V0 - 0x3u;
        mem.WriteU32((c.FP + 0xF0Cu), c.V0);
        c.V0 = mem.ReadU32((c.FP + 0x14u));
        c.V1 = mem.ReadU32((c.FP + 0xF0Cu));
        c.A0 = c.V0 | 0x0002u;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.FP + 0x14u), c.A0);
            goto L80013248;
        }
        mem.WriteU32((c.FP + 0x14u), c.A0);
        c.V0 = 0xFFFFFFF6u;
        c.V0 = c.A0 & c.V0;
        mem.WriteU32((c.FP + 0x14u), c.V0);
        L80013248: ;
        c.V0 = 0x80010000u;
        L8001324C: ;
        c.A0 = mem.ReadU32((c.V0 + 0x4F98u));
        c.V1 = mem.ReadU32(c.FP);
        c.V0 = mem.ReadU32(c.A0);
        c.S7 = mem.ReadU32((c.FP + 0xCu));
        c.V1 = c.V1 - c.V0;
        mem.WriteU32((c.SP + 0x124u), c.V1);
        c.V1 = mem.ReadU32((c.FP + 0x4u));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        c.A0 = mem.ReadU32((c.FP + 0x8u));
        c.V1 = c.V1 - c.V0;
        c.V0 = (int)c.S7 < (int)c.A0 ? 1u : 0u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x128u), c.V1);
            goto L80013284;
        }
        mem.WriteU32((c.SP + 0x128u), c.V1);
        c.S7 = c.A0;
        L80013284: ;
        c.V0 = mem.ReadU32((c.FP + 0x14u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = RightEdge(c.S7);
            MediEvil_landmap.func_800136F4(c, m);
            return;
        }
        c.V0 = RightEdge(c.S7);
        c.T8 = mem.ReadU32((c.SP + 0x124u));
        c.V0 = (int)c.T8 < (int)c.V0 ? 1u : 0u;
        Dispatcher.Call(c, m, 0x800132A4u);
    }

    public static void func_800132A4_landmap(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        if (c.V0 == 0u) {
            c.V1 = 0u - c.S7;
            goto L800136F4;
        }
        c.V1 = 0u - c.S7;
        c.V0 = (int)c.T8 < (int)LeftEdge(c.S7) ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S7 + 0xF0u;
            goto L800136F4;
        }
        c.V0 = c.S7 + 0xF0u;
        c.T8 = mem.ReadU32((c.SP + 0x128u));
        c.V0 = (int)c.T8 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = (int)c.T8 < (int)c.V1 ? 1u : 0u;
            goto L800136F4;
        }
        c.V0 = (int)c.T8 < (int)c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S6 = c.SP + 0x28u;
            goto L800136F4;
        }
        c.S6 = c.SP + 0x28u;
        c.V0 = mem.ReadU32((c.FP + 0x10u));
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V1 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0x1F800000u;
        mem.WriteU16((c.V0 + 0x60u), (ushort)c.V1);
        c.V0 = mem.ReadU32((c.FP + 0x10u));
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V0 = mem.ReadU16(c.V0);
        c.T8 = mem.ReadU16((c.SP + 0x124u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.T7 + 0x4u), (ushort)c.T8);
        mem.WriteU16((c.T7 + 0x2u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.FP + 0x10u));
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V0 = mem.ReadU16(c.V0);
        mem.WriteU16((c.T7 + 0x6u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.FP + 0x10u));
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.T8 = mem.ReadU16((c.SP + 0x128u));
        mem.WriteU16((c.T7 + 0xAu), (ushort)c.T8);
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 + 0x60u;
        mem.WriteU16((c.T7 + 0x8u), (ushort)c.V0);
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
        c.V0 = mem.ReadU32((c.FP + 0x18u));
        if (c.V0 != 0u) {
            c.V1 = c.SP + 0x34u;
            goto L8001361C;
        }
        c.V1 = c.SP + 0x34u;
        c.S7 = 0u;
        c.A2 = c.FP;
        L800133AC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = 0x800F0000u;
        c.V1 = (uint)(short)mem.ReadU16((c.A2 + 0xEE4u));
        c.V0 = mem.ReadU32((c.V0 - 0x11E4u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.V0 = c.V1 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.V1 = (uint)(short)mem.ReadU16((c.A2 + 0xEBCu));
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S1 = mem.ReadU32((c.FP + 0x8u));
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + 0x400u;
        { var _r = (long)(int)c.S1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A0 = 0x66660000u;
        c.S1 = c.LO;
        c.A0 = c.A0 | 0x6667u;
        c.V1 = c.S7 << 12;
        { var _r = (long)(int)c.V1 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A3 = c.HI;
        c.S0 = mem.ReadU32((c.FP + 0xCu));
        { var _r = (long)(int)c.S0 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = (uint)((int)c.V1 >> 31);
        c.V0 = (uint)((int)c.A3 >> 3);
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T3;
        c.S0 = c.LO;
        c.S3 = (uint)(short)mem.ReadU16((c.V0 + 0x2u));
        c.V1 = (uint)((int)c.S1 >> 12);
        { var _r = (long)(int)c.S3 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.S2 = (uint)(short)mem.ReadU16(c.V0);
        c.V0 = (uint)((int)c.S0 >> 12);
        { var _r = (long)(int)c.S2 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A1 = mem.ReadU32((c.SP + 0x12Cu));
        c.A0 = c.SP + 0x10u;
        c.S4 = (uint)((int)c.S1 >> 10);
        c.S1 = (uint)((int)c.S1 >> 11);
        mem.WriteU32((c.SP + 0x130u), c.A2);
        mem.WriteU32((c.SP + 0x134u), c.T3);
        mem.WriteU32((c.SP + 0x138u), c.T7);
        c.S5 = (uint)((int)c.S0 >> 10);
        c.S0 = (uint)((int)c.S0 >> 11);
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.A3 = c.LO;
        c.V0 = (uint)((int)c.A3 >> 12);
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.RA = 0x8001348Cu;
        MediEvil_game.func_800A4E0C(c, m);
        { var _r = (long)(int)c.S3 * (int)c.S1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A3 = c.LO;
        { var _r = (long)(int)c.S2 * (int)c.S0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16((c.SP + 0x18u));
        mem.WriteU16(c.S6, (ushort)c.V0);
        c.V0 = mem.ReadU16((c.SP + 0x1Cu));
        c.A1 = mem.ReadU32((c.SP + 0x12Cu));
        c.A0 = c.SP + 0x10u;
        mem.WriteU16((c.S6 + 0x2u), (ushort)c.V0);
        c.V0 = (uint)((int)c.A3 >> 12);
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.RA = 0x800134D4u;
        MediEvil_game.func_800A4E0C(c, m);
        { var _r = (long)(int)c.S3 * (int)c.S4; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A3 = c.LO;
        c.S6 = c.S6 + 0x4u;
        { var _r = (long)(int)c.S2 * (int)c.S5; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A2 = mem.ReadU32((c.SP + 0x130u));
        c.V0 = mem.ReadU16((c.SP + 0x18u));
        c.A0 = c.SP + 0x10u;
        mem.WriteU16(c.S6, (ushort)c.V0);
        c.V0 = mem.ReadU16((c.SP + 0x1Cu));
        c.A1 = mem.ReadU32((c.SP + 0x12Cu));
        c.A2 = c.A2 + 0x2u;
        mem.WriteU16((c.S6 + 0x2u), (ushort)c.V0);
        mem.WriteU32((c.SP + 0x130u), c.A2);
        c.V0 = (uint)((int)c.A3 >> 12);
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.RA = 0x80013524u;
        MediEvil_game.func_800A4E0C(c, m);
        c.V0 = mem.ReadU16((c.SP + 0x18u));
        c.S6 = c.S6 + 0x4u;
        mem.WriteU16(c.S6, (ushort)c.V0);
        c.V0 = mem.ReadU16((c.SP + 0x1Cu));
        c.S7 = c.S7 + 0x1u;
        mem.WriteU16((c.S6 + 0x2u), (ushort)c.V0);
        c.V0 = (int)c.S7 < 20 ? 1u : 0u;
        c.A2 = mem.ReadU32((c.SP + 0x130u));
        c.T3 = mem.ReadU32((c.SP + 0x134u));
        c.T7 = mem.ReadU32((c.SP + 0x138u));
        if (c.V0 != 0u) {
            c.S6 = c.S6 + 0x4u;
            goto L800133AC;
        }
        c.S6 = c.S6 + 0x4u;
        c.A2 = 0x00FF0000u;
        c.V0 = 0x1F800000u;
        c.A2 = c.A2 | 0xFFFFu;
        c.A1 = mem.ReadU32((c.V0 + 0x4u));
        c.T0 = 0xFF000000u;
        c.A0 = c.A1 << 1;
        c.A0 = c.A0 + c.A1;
        c.V0 = c.A0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + 0x5Cu;
        c.T2 = c.FP + c.V0;
        c.V0 = 0x1F800000u;
        c.A1 = c.A1 << 5;
        c.A3 = mem.ReadU32((c.V0 + 0x88u));
        c.A1 = c.A1 + 0x1Cu;
        c.V0 = mem.ReadU32((c.A3 + 0x78u));
        c.V1 = mem.ReadU32((c.T2 + 0x17Cu));
        c.V0 = mem.ReadU32((c.V0 + 0x20u));
        c.V1 = c.V1 & c.T0;
        c.V0 = c.V0 & c.A2;
        c.V1 = c.V1 | c.V0;
        c.V0 = c.A0 << 4;
        c.V0 = c.V0 - c.A0;
        c.V0 = c.V0 << 5;
        c.V0 = c.V0 + 0x37Cu;
        mem.WriteU32((c.T2 + 0x17Cu), c.V1);
        c.A0 = mem.ReadU32((c.A3 + 0x78u));
        c.T1 = c.FP + c.V0;
        c.V1 = mem.ReadU32((c.A0 + 0x20u));
        c.V0 = c.T2 & c.A2;
        c.V1 = c.V1 & c.T0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32((c.A0 + 0x20u), c.V1);
        c.V0 = mem.ReadU32((c.A3 + 0x78u));
        c.V1 = mem.ReadU32((c.T1 + 0x57Cu));
        c.V0 = mem.ReadU32((c.V0 + 0x20u));
        c.V1 = c.V1 & c.T0;
        c.V0 = c.V0 & c.A2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32((c.T1 + 0x57Cu), c.V1);
        c.V1 = mem.ReadU32((c.A3 + 0x78u));
        c.A1 = c.FP + c.A1;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        c.A2 = c.A1 & c.A2;
        c.V0 = c.V0 & c.T0;
        c.V0 = c.V0 | c.A2;
        mem.WriteU32((c.V1 + 0x20u), c.V0);
        c.S6 = c.SP + 0x28u;
        c.V1 = c.SP + 0x34u;
        L8001361C: ;
        c.S7 = 0x00000013u;
        c.A2 = c.T1 + 0x2F0u;
        c.A1 = c.T1 + 0x10u;
        c.A0 = c.T2 + 0x10u;
        L8001362C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32((c.FP + 0x18u));
        if (c.V0 != 0u) {
            goto L80013670;
        }
        c.T8 = mem.ReadU16((c.SP + 0x124u));
        mem.WriteU16((c.A0 - 0x8u), (ushort)c.T8);
        c.T8 = mem.ReadU16((c.SP + 0x128u));
        mem.WriteU16((c.A0 - 0x6u), (ushort)c.T8);
        c.V0 = mem.ReadU32(c.S6);
        mem.WriteU32((c.A0 - 0x4u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        mem.WriteU32(c.A0, c.V0);
        c.A0 = c.A0 + 0x14u;
        L80013670: ;
        c.V0 = mem.ReadU32(c.S6);
        mem.WriteU32((c.A1 + 0x8u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        c.S6 = c.S6 + 0x4u;
        mem.WriteU32((c.A1 + 0x10u), c.V0);
        c.V0 = mem.ReadU32(c.S6);
        c.V1 = c.V1 + 0x4u;
        mem.WriteU32((c.A1 - 0x8u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        c.T8 = 0x00000001u;
        mem.WriteU32(c.A1, c.V0);
        c.V0 = mem.ReadU32(c.S6);
        c.S6 = c.S6 + 0x4u;
        c.A1 = c.A1 + 0x24u;
        mem.WriteU32((c.A2 - 0x18u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        c.V1 = c.V1 + 0x4u;
        mem.WriteU32((c.A2 - 0x10u), c.V0);
        c.V0 = mem.ReadU32(c.S6);
        c.S6 = c.S6 + 0x4u;
        mem.WriteU32((c.A2 - 0x8u), c.V0);
        c.V0 = mem.ReadU32(c.V1);
        c.V1 = c.V1 + 0x4u;
        mem.WriteU32(c.A2, c.V0);
        if (c.S7 != c.T8) {
            c.A2 = c.A2 + 0x24u;
            goto L800136E0;
        }
        c.A2 = c.A2 + 0x24u;
        c.V1 = c.SP + 0x28u;
        L800136E0: ;
        c.V0 = c.S7;
        if (c.V0 != 0u) {
            c.S7 = c.S7 - 0x1u;
            goto L8001362C;
        }
        c.S7 = c.S7 - 0x1u;
        goto L80013710;
        L800136F4: ;
        c.A0 = mem.ReadU32((c.FP + 0x14u));
        c.V0 = 0x00000009u;
        c.V1 = c.A0 & 0x0009u;
        if (c.V1 != c.V0) {
            c.V0 = 0xFFFFFFF6u;
            goto L80013710;
        }
        c.V0 = 0xFFFFFFF6u;
        c.V0 = c.A0 & c.V0;
        mem.WriteU32((c.FP + 0x14u), c.V0);
        L80013710: ;
        c.T8 = mem.ReadU32((c.SP + 0x120u));
        c.T8 = c.T8 - 0x1u;
        if ((int)c.T8 >= 0) {
            mem.WriteU32((c.SP + 0x120u), c.T8);
            Dispatcher.Call(c, m, 0x80012F58u);
            return;
        }
        mem.WriteU32((c.SP + 0x120u), c.T8);
        c.RA = mem.ReadU32((c.SP + 0x164u));
        c.FP = mem.ReadU32((c.SP + 0x160u));
        c.S7 = mem.ReadU32((c.SP + 0x15Cu));
        c.S6 = mem.ReadU32((c.SP + 0x158u));
        c.S5 = mem.ReadU32((c.SP + 0x154u));
        c.S4 = mem.ReadU32((c.SP + 0x150u));
        c.S3 = mem.ReadU32((c.SP + 0x14Cu));
        c.S2 = mem.ReadU32((c.SP + 0x148u));
        c.S1 = mem.ReadU32((c.SP + 0x144u));
        c.S0 = mem.ReadU32((c.SP + 0x140u));
        c.SP = c.SP + 0x168u;
        return;
    }

    public static void func_80013BB8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = 0x80010000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4F88u));
        c.SP = c.SP - 0x58u;
        mem.WriteU32((c.SP + 0x54u), c.RA);
        mem.WriteU32((c.SP + 0x50u), c.FP);
        mem.WriteU32((c.SP + 0x4Cu), c.S7);
        mem.WriteU32((c.SP + 0x48u), c.S6);
        mem.WriteU32((c.SP + 0x44u), c.S5);
        mem.WriteU32((c.SP + 0x40u), c.S4);
        mem.WriteU32((c.SP + 0x3Cu), c.S3);
        mem.WriteU32((c.SP + 0x38u), c.S2);
        mem.WriteU32((c.SP + 0x34u), c.S1);
        if (c.V0 != 0u) {
            mem.WriteU32((c.SP + 0x30u), c.S0);
            goto L80013F88;
        }
        mem.WriteU32((c.SP + 0x30u), c.S0);
        c.V0 = 0x80010000u;
        c.V1 = mem.ReadU32((c.V0 + 0x4F98u));
        c.T1 = 0x00000005u;
        mem.WriteU32((c.SP + 0x14u), c.T1);
        c.T0 = c.V1 + 0xD0u;
        c.S3 = c.V1 + 0xE4u;
        L80013C08: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = 0x80010000u;
        c.A2 = mem.ReadU32((c.V0 + 0x4F98u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = mem.ReadU32(c.A2);
        c.A1 = c.V1 - c.V0;
        c.V1 = mem.ReadU32((c.S3 - 0x10u));
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        c.A3 = mem.ReadU32(c.S3);
        c.A0 = c.V1 - c.V0;
        c.V0 = c.A3 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.A1 + 0x60u;
            goto L80013F6C;
        }
        c.V0 = c.A1 + 0x60u;
        c.V0 = unchecked(c.V0 + (uint)Margin) < (uint)(0x2C0 + 2 * Margin) ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.A0 + 0x60u;
            goto L80013F6C;
        }
        c.V0 = c.A0 + 0x60u;
        c.V0 = c.V0 < 0x000001B0u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x1F800000u;
            goto L80013F6C;
        }
        c.V0 = 0x1F800000u;
        c.S7 = c.T0 + 0x145Cu;
        c.S5 = c.T0 + 0x14FCu;
        c.V1 = 0x800F0000u;
        c.A0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.V1 - 0x11E4u));
        c.T1 = mem.ReadU32((c.SP + 0x14u));
        c.V0 = c.A0 << 2;
        c.V0 = c.V0 + c.A0;
        c.V0 = c.V0 << 9;
        c.V0 = c.V0 + 0x5Cu;
        c.S6 = c.T0 + c.V0;
        c.V1 = c.V1 << 2;
        c.V0 = c.T1 << 4;
        c.V1 = c.V1 + c.V0;
        c.FP = c.V1 & 0x00FFu;
        c.A0 = c.A0 << 5;
        c.A0 = c.A0 + 0x1Cu;
        c.A0 = c.T0 + c.A0;
        mem.WriteU32((c.SP + 0x10u), c.A0);
        if (c.T1 == 0u) {
            c.V0 = 0u | 0x8498u;
            goto L80013CB8;
        }
        c.V0 = 0u | 0x8498u;
        c.V0 = c.A3 & 0x0004u;
        if (c.V0 != 0u) {
            c.V0 = 0u | 0x8898u;
            goto L80013CB8;
        }
        c.V0 = 0u | 0x8898u;
        c.V0 = 0u | 0x8098u;
        L80013CB8: ;
        c.A2 = c.A2 + c.V0;
        mem.WriteU32((c.SP + 0x20u), c.A2);
        mem.WriteU32((c.SP + 0x18u), 0u);
        c.S4 = c.S7 + 0x2u;
        c.S2 = c.S6 + 0xCu;
        L80013CCC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.FP = c.FP & 0x00FFu;
        c.V0 = 0x00FF0000u;
        c.T1 = mem.ReadU32((c.SP + 0x20u));
        c.V1 = c.FP << 2;
        c.V1 = c.T1 + c.V1;
        mem.WriteU32((c.SP + 0x1Cu), c.V1);
        c.V1 = mem.ReadU32(c.V1);
        c.V0 = c.V0 | 0xFFFFu;
        c.V1 = c.V1 & c.V0;
        if (c.V1 == 0u) {
            goto L80013D0C;
        }
        c.V0 = mem.ReadU32(c.S3);
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            c.V0 = 0x80010000u;
            goto L80013E2C;
        }
        c.V0 = 0x80010000u;
        L80013D0C: ;
        mem.WriteU32((c.SP + 0x28u), c.T0);
        c.RA = 0x80013D14u;
        MediEvil_game.func_800A43B8(c, m);
        c.S0 = c.V0 & 0x0FFFu;
        c.RA = 0x80013D1Cu;
        MediEvil_game.func_800A43B8(c, m);
        c.V0 = c.V0 & 0x07FFu;
        c.V1 = mem.ReadU32((c.S3 - 0xCu));
        c.V0 = c.V0 + 0xCCCu;
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.S1 = (uint)((int)c.V1 >> 13);
        c.RA = 0x80013D38u;
        MediEvil_game.func_800A43B8(c, m);
        c.V0 = c.V0 & 0x07FFu;
        c.V1 = mem.ReadU32((c.S3 - 0x8u));
        c.V0 = c.V0 + 0x800u;
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S0 = c.S0 << 2;
        c.T1 = 0x800E0000u;
        c.T1 = c.T1 + 0x11E0u;
        c.S0 = c.S0 + c.T1;
        c.A1 = c.LO;
        c.V0 = (uint)(short)mem.ReadU16((c.S0 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.S1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A0 = c.LO;
        c.V1 = (uint)(short)mem.ReadU16(c.S0);
        c.V0 = (uint)((int)c.A1 >> 13);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T1 = 0x800E0000u;
        c.V0 = mem.ReadU32((c.S3 - 0x4u));
        c.T1 = c.T1 + 0x11E0u;
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T1;
        c.A2 = c.LO;
        c.V1 = (uint)(short)mem.ReadU16((c.V0 + 0x2u));
        c.A1 = (uint)((int)c.A0 >> 12);
        { var _r = (long)(int)c.V1 * (int)c.A1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        c.A0 = (uint)((int)c.A2 >> 12);
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T0 = mem.ReadU32((c.SP + 0x28u));
        c.T1 = 0x800E0000u;
        c.A2 = c.LO;
        c.V0 = c.V1 - c.A2;
        c.V1 = mem.ReadU16(c.T0);
        c.V0 = (uint)((int)c.V0 >> 12);
        c.V1 = c.V1 + c.V0;
        mem.WriteU16(c.S7, (ushort)c.V1);
        c.V0 = mem.ReadU32((c.S3 - 0x4u));
        c.T1 = c.T1 + 0x11E0u;
        c.V0 = c.V0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T1;
        c.V1 = (uint)(short)mem.ReadU16(c.V0);
        { var _r = (long)(int)c.V1 * (int)c.A1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.V0 = (uint)(short)mem.ReadU16((c.V0 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A0 = c.LO;
        c.V0 = c.V1 + c.A0;
        c.V1 = mem.ReadU16((c.S3 - 0x10u));
        c.V0 = (uint)((int)c.V0 >> 12);
        c.V1 = c.V1 + c.V0;
        mem.WriteU16(c.S4, (ushort)c.V1);
        c.V0 = mem.ReadU32(c.S3);
        c.V1 = 0xFFFFFFFDu;
        c.V0 = c.V0 & c.V1;
        mem.WriteU32(c.S3, c.V0);
        c.V0 = 0x80010000u;
        L80013E2C: ;
        c.A1 = mem.ReadU32((c.V0 + 0x4F98u));
        c.V0 = mem.ReadU16(c.S7);
        c.V1 = mem.ReadU16(c.A1);
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.S2 + 0x4u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.S4);
        c.V1 = mem.ReadU16((c.A1 + 0x4u));
        c.A0 = mem.ReadU16((c.S2 + 0x4u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.S2 + 0x6u), (ushort)c.V0);
        mem.WriteU16((c.S2 - 0x4u), (ushort)c.A0);
        c.V1 = mem.ReadU16(c.S5);
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.S2 - 0x2u), (ushort)c.V0);
        c.V0 = c.A0;
        c.A0 = mem.ReadU16(c.S5);
        c.V1 = mem.ReadU16((c.S2 + 0x6u));
        c.V0 = c.V0 + c.A0;
        mem.WriteU16((c.S2 + 0xEu), (ushort)c.V1);
        mem.WriteU16((c.S2 + 0xCu), (ushort)c.V0);
        c.T1 = mem.ReadU32((c.SP + 0x1Cu));
        c.S6 = c.S6 + 0x40u;
        c.V0 = mem.ReadU32(c.T1);
        c.T1 = mem.ReadU32((c.SP + 0x18u));
        c.FP = c.FP + 0x10u;
        c.T1 = c.T1 + 0x1u;
        mem.WriteU32((c.SP + 0x18u), c.T1);
        mem.WriteU32(c.S2, c.V0);
        c.V0 = mem.ReadU16(c.S7);
        c.V1 = mem.ReadU16(c.A1);
        c.S2 = c.S2 + 0x20u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.S2 + 0x4u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.S4);
        c.V1 = mem.ReadU16((c.A1 + 0x4u));
        c.A0 = mem.ReadU16((c.S2 + 0x4u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.S2 + 0x6u), (ushort)c.V0);
        mem.WriteU16((c.S2 - 0x4u), (ushort)c.A0);
        c.V1 = mem.ReadU16(c.S5);
        c.S7 = c.S7 + 0x4u;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.S2 - 0x2u), (ushort)c.V0);
        c.V0 = c.A0;
        c.V1 = mem.ReadU16(c.S5);
        c.A0 = mem.ReadU16((c.S2 + 0x6u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.S2 + 0xCu), (ushort)c.V0);
        mem.WriteU16((c.S2 + 0xEu), (ushort)c.A0);
        c.T1 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = c.S4 + 0x4u;
        c.V0 = mem.ReadU32(c.T1);
        c.S5 = c.S5 + 0x2u;
        mem.WriteU32(c.S2, c.V0);
        c.T1 = mem.ReadU32((c.SP + 0x18u));
        c.V0 = (int)c.T1 < 40 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S2 = c.S2 + 0x20u;
            goto L80013CCC;
        }
        c.S2 = c.S2 + 0x20u;
        c.A0 = 0x00FF0000u;
        c.A0 = c.A0 | 0xFFFFu;
        c.V0 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.V0 + 0x88u));
        c.A2 = 0xFF000000u;
        c.V0 = mem.ReadU32((c.A1 + 0x78u));
        c.V1 = mem.ReadU32((c.S6 - 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x24u));
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32((c.S6 - 0x20u), c.V1);
        c.V1 = mem.ReadU32((c.A1 + 0x78u));
        c.T1 = mem.ReadU32((c.SP + 0x10u));
        c.V0 = mem.ReadU32((c.V1 + 0x24u));
        c.A0 = c.T1 & c.A0;
        c.V0 = c.V0 & c.A2;
        c.V0 = c.V0 | c.A0;
        mem.WriteU32((c.V1 + 0x24u), c.V0);
        L80013F6C: ;
        c.S3 = c.S3 + 0x154Cu;
        c.V0 = mem.ReadU32((c.SP + 0x14u));
        c.T0 = c.T0 + 0x154Cu;
        c.T1 = c.V0;
        c.T1 = c.T1 - 0x1u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.SP + 0x14u), c.T1);
            goto L80013C08;
        }
        mem.WriteU32((c.SP + 0x14u), c.T1);
        L80013F88: ;
        c.RA = mem.ReadU32((c.SP + 0x54u));
        c.FP = mem.ReadU32((c.SP + 0x50u));
        c.S7 = mem.ReadU32((c.SP + 0x4Cu));
        c.S6 = mem.ReadU32((c.SP + 0x48u));
        c.S5 = mem.ReadU32((c.SP + 0x44u));
        c.S4 = mem.ReadU32((c.SP + 0x40u));
        c.S3 = mem.ReadU32((c.SP + 0x3Cu));
        c.S2 = mem.ReadU32((c.SP + 0x38u));
        c.S1 = mem.ReadU32((c.SP + 0x34u));
        c.S0 = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x58u;
        return;
    }

    public static void func_80013C08(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = 0x80010000u;
        c.A2 = mem.ReadU32((c.V0 + 0x4F98u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = mem.ReadU32(c.A2);
        c.A1 = c.V1 - c.V0;
        c.V1 = mem.ReadU32((c.S3 - 0x10u));
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        c.A3 = mem.ReadU32(c.S3);
        c.A0 = c.V1 - c.V0;
        c.V0 = c.A3 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.A1 + 0x60u;
            MediEvil_landmap.func_80013F6C(c, m);
            return;
        }
        c.V0 = c.A1 + 0x60u;
        c.V0 = unchecked(c.V0 + (uint)Margin) < (uint)(0x2C0 + 2 * Margin) ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.A0 + 0x60u;
            MediEvil_landmap.func_80013F6C(c, m);
            return;
        }
        c.V0 = c.A0 + 0x60u;
        c.V0 = c.V0 < 0x000001B0u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x1F800000u;
            MediEvil_landmap.func_80013F6C(c, m);
            return;
        }
        c.V0 = 0x1F800000u;
        c.S7 = c.T0 + 0x145Cu;
        c.S5 = c.T0 + 0x14FCu;
        c.V1 = 0x800F0000u;
        c.A0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.V1 - 0x11E4u));
        c.T1 = mem.ReadU32((c.SP + 0x14u));
        c.V0 = c.A0 << 2;
        c.V0 = c.V0 + c.A0;
        c.V0 = c.V0 << 9;
        c.V0 = c.V0 + 0x5Cu;
        c.S6 = c.T0 + c.V0;
        c.V1 = c.V1 << 2;
        c.V0 = c.T1 << 4;
        c.V1 = c.V1 + c.V0;
        c.FP = c.V1 & 0x00FFu;
        c.A0 = c.A0 << 5;
        c.A0 = c.A0 + 0x1Cu;
        c.A0 = c.T0 + c.A0;
        mem.WriteU32((c.SP + 0x10u), c.A0);
        if (c.T1 == 0u) {
            c.V0 = 0u | 0x8498u;
            goto L80013CB8;
        }
        c.V0 = 0u | 0x8498u;
        c.V0 = c.A3 & 0x0004u;
        if (c.V0 != 0u) {
            c.V0 = 0u | 0x8898u;
            goto L80013CB8;
        }
        c.V0 = 0u | 0x8898u;
        c.V0 = 0u | 0x8098u;
        L80013CB8: ;
        c.A2 = c.A2 + c.V0;
        mem.WriteU32((c.SP + 0x20u), c.A2);
        mem.WriteU32((c.SP + 0x18u), 0u);
        c.S4 = c.S7 + 0x2u;
        c.S2 = c.S6 + 0xCu;
        c.FP = c.FP & 0x00FFu;
        c.V0 = 0x00FF0000u;
        c.T1 = mem.ReadU32((c.SP + 0x20u));
        c.V1 = c.FP << 2;
        c.V1 = c.T1 + c.V1;
        mem.WriteU32((c.SP + 0x1Cu), c.V1);
        c.V1 = mem.ReadU32(c.V1);
        c.V0 = c.V0 | 0xFFFFu;
        c.V1 = c.V1 & c.V0;
        if (c.V1 == 0u) {
            MediEvil_landmap.func_80013D0C(c, m);
            return;
        }
        c.V0 = mem.ReadU32(c.S3);
        c.V0 = c.V0 & 0x0002u;
        Dispatcher.Call(c, m, 0x80013D04u);
    }
}
