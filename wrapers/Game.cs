using RecompOne.Runtime.Memory;

namespace MediEvil;

public static class Game
{
    public const uint Gp = 0x800ED5D4u;

    public const uint CurrentLevelAddr = 0x800EEE2Cu;
    public const uint PlayerSlot = 0x568u;
    public const uint MainViewportSlot = 0x5E0u;
    public const uint ViewportRootSlot = 0x7F0u;
    public const uint CameraSlot = 0x470u;
    public const uint CameraEntitySlot = 0x474u;
    public const uint CameraPluginSlot = 0x484u;
    public const uint TextureListSlot = 0x7C0u;
    public const uint TextureAttributesAddr = 0x800ED7C4u;

    internal static IMemory M => RecompOne.Runtime.Runtime.Mem!;

    public static bool Available => RecompOne.Runtime.Runtime.Mem != null;

    public static uint GpWord(uint slot) => M.ReadU32(Gp + slot);

    public static int CurrentLevel => (int)M.ReadU32(CurrentLevelAddr);
    public static uint PlayerAddr => GpWord(PlayerSlot);
    public static uint MainViewportAddr => GpWord(MainViewportSlot);
    public static uint CameraAddr => GpWord(CameraSlot);
    public static uint CameraEntityAddr => GpWord(CameraEntitySlot);
    public static uint TextureListAddr => GpWord(TextureListSlot);

    public static byte TextureAttributes(int index) => M.ReadU8(TextureAttributesAddr + (uint)index);

    public static bool IsRam(uint address) => (address & 0xFF000000u) == 0x80000000u && (address & 0x00FFFFFFu) < RecompOne.Runtime.Runtime.RamSize;
}

[Flags]
public enum TextureAttribute : byte
{
    NotGround = 1 << 0,
    Water = 1 << 1,
    Mud = 1 << 2,
    DeadlyMud = 1 << 3,
    Corn = 1 << 4,
    Special1 = 1 << 5,
    Special2 = 1 << 6,
}
