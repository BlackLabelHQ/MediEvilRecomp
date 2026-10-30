using RecompOne.Runtime.Memory;

namespace MediEvil;

//should the other stuff be here?
public static class MrHeap
{
    public const uint TagSize = 0x14u;
    public const uint SizeOffset = 0x10u;

    static IMemory M => Game.M;

    public static uint SizeTag(uint block) => M.ReadU32(block - TagSize + SizeOffset);

    public static bool IsAllocated(uint block) => Game.IsRam(block) && (int)SizeTag(block) < 0;
}

public static class DynamicPolys
{
    public const uint PoolsAddr = 0x800EDDF4u;
    public const uint PoolEndsAddr = 0x800EDDFCu;
    public const uint PoolSizeSlot = 0x81Cu;
    public const uint BufferSizeSlot = 0x818u;

    static IMemory M => Game.M;

    public static uint Pool(int index) => M.ReadU32(PoolsAddr + (uint)index * 4u);
    public static uint PoolEnd(int index) => M.ReadU32(PoolEndsAddr + (uint)index * 4u);
    public static uint PoolSize => Game.GpWord(PoolSizeSlot);
    public static uint BufferSize => Game.GpWord(BufferSizeSlot);
}

public readonly record struct ResourceInfo(uint Addr)
{
    public const uint TableAddr = 0x800CE508u;
    public const int Count = 0x316;
    public const uint Stride = 0x20u;
    public const uint Packed = 0x10u;

    static IMemory M => Game.M;

    public static ResourceInfo At(int id) => new(TableAddr + (uint)id * Stride);

    public uint Flags => M.ReadU32(Addr + 0x04u);
    public uint Type => M.ReadU32(Addr + 0x08u);
    public uint Sector => M.ReadU32(Addr + 0x0Cu);
    public uint Address => M.ReadU32(Addr + 0x10u);
    public uint DepackedAddress => M.ReadU32(Addr + 0x14u);
    public uint FileSize => M.ReadU32(Addr + 0x18u);
    public uint RealSize => M.ReadU32(Addr + 0x1Cu);
    public bool IsPacked => (Flags & Packed) != 0;
}

public readonly record struct Viewport(uint Addr)
{
    public const uint OtSize = 0x68u;
    public const uint OtSizeBits = 0x6Au;
    public const uint OtzShift = 0x6Cu;
    public const uint ViewDistance = 0x6Eu;
    public const uint Ot0 = 0x70u;
    public const uint Ot1 = 0x74u;
    public const uint WorkOt = 0x78u;
    public const uint Camera = 0x7Cu;
    public const uint AspectMatrix = 0x80u;
    public const uint RenderMatrix = 0xA0u;
    public const uint PgenRoot = 0x10Cu;
    public const uint SpriteRoot = 0x110u;

    static IMemory M => Game.M;

    public static Viewport Main => new(Game.MainViewportAddr);

    public bool IsValid => Game.IsRam(Addr);
    public uint Next => M.ReadU32(Addr);
    public short Width => (short)M.ReadU16(Addr + 0x10u);
    public short Height => (short)M.ReadU16(Addr + 0x12u);
    public uint CameraFrame => M.ReadU32(Addr + Camera);

    public static IEnumerable<Viewport> All()
    {
        var root = Game.GpWord(Game.ViewportRootSlot);
        if (!Game.IsRam(root)) yield break;
        var vp = M.ReadU32(root);
        for (var i = 0; Game.IsRam(vp) && i < 16; i++, vp = M.ReadU32(vp)) yield return new Viewport(vp);
    }
}

public readonly record struct Texture(uint Addr)
{
    public const int MaxCount = 4616;
    public const ushort UserMask = 0x7000;
    public const int UserShift = 12;

    static IMemory M => Game.M;

    public static Texture? At(int index)
    {
        var list = Game.TextureListAddr;
        if (!Game.IsRam(list)) return null;
        var texture = M.ReadU32(list + (uint)index * 4u);
        return Game.IsRam(texture) ? new Texture(texture) : null;
    }

    public ushort Flags => M.ReadU16(Addr);
    public int UserField => (Flags & UserMask) >> UserShift;
    public TextureAttribute Attributes => (TextureAttribute)Game.TextureAttributes(UserField);
    public ushort Clut => M.ReadU16(Addr + 0x06u);
    public ushort TPage => M.ReadU16(Addr + 0x0Au);

    public (int U0, int V0, int U1, int V1) Bounds
    {
        get
        {
            int u0 = M.ReadU8(Addr + 0x04u), v0 = M.ReadU8(Addr + 0x05u);
            int u1 = M.ReadU8(Addr + 0x08u), v1 = M.ReadU8(Addr + 0x09u);
            int u2 = M.ReadU8(Addr + 0x0Cu), v2 = M.ReadU8(Addr + 0x0Du);
            int u3 = M.ReadU8(Addr + 0x0Eu), v3 = M.ReadU8(Addr + 0x0Fu);
            return (Math.Min(Math.Min(u0, u1), Math.Min(u2, u3)), Math.Min(Math.Min(v0, v1), Math.Min(v2, v3)),
                Math.Max(Math.Max(u0, u1), Math.Max(u2, u3)), Math.Max(Math.Max(v0, v1), Math.Max(v2, v3)));
        }
    }
}
