using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class ParticleIdentityPatch
{
    private const uint PgenRoot = 0x10C;
    private const uint SpriteRoot = 0x110;
    private const uint PgenPrims = 0x0C;
    private const uint SpritePolygons = 0x0C;
    private const uint PolyFt4Size = 0x28;
    private const int MaxNodes = 512;
    private const uint DynamicPools = 0x800EDDF4u;
    private const uint DynamicPoolEnds = 0x800EDDFCu;

    public static void Register(IMemory m, uint viewport)
    {
        var poolA = m.ReadU32(DynamicPools);
        var poolB = m.ReadU32(DynamicPools + 4);
        var endA = m.ReadU32(DynamicPoolEnds);
        if (IsRam(poolA) && IsRam(poolB) && endA > poolA && poolB != poolA)
            PrimIdentity.Alias(poolA, poolB, endA - poolA);

        var root = m.ReadU32(viewport + PgenRoot);
        var node = IsRam(root) ? m.ReadU32(root) : 0u;
        for (var i = 0; IsRam(node) && i < MaxNodes; i++, node = m.ReadU32(node))
        {
            var primary = m.ReadU32(node + PgenPrims);
            var secondary = m.ReadU32(node + PgenPrims + 4);
            if (IsRam(primary) && IsRam(secondary) && secondary > primary)
                PrimIdentity.Alias(primary, secondary, secondary - primary);
        }

        root = m.ReadU32(viewport + SpriteRoot);
        node = IsRam(root) ? m.ReadU32(root) : 0u;
        for (var i = 0; IsRam(node) && i < MaxNodes; i++, node = m.ReadU32(node))
        {
            var polygons = node + SpritePolygons;
            PrimIdentity.Alias(polygons, polygons + PolyFt4Size, PolyFt4Size);
        }
    }

    private static bool IsRam(uint address)
    {
        return (address & 0xFF000000u) == 0x80000000u && (address & 0x00FFFFFFu) < 0x00400000u;
    }
}
