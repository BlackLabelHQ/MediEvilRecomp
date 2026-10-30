using MediEvil;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class PolyPoolPatch
{
    const uint Pools = DynamicPolys.PoolsAddr;
    const uint PoolsEnd = DynamicPolys.PoolEndsAddr;
    const uint PoolSizeSlot = DynamicPolys.PoolSizeSlot;
    const uint BufferSizeSlot = DynamicPolys.BufferSizeSlot;

    const uint PoolBase = 0x80200000u;
    const uint PoolStride = 0x80000u;

    static uint _heapPool;
    static uint _heapTag;

    public static void Relocate(CpuContext c, IMemory m)
    {
        uint heap = m.ReadU32(Pools);
        if (heap == 0u) return;

        uint buffer = m.ReadU32(c.GP + BufferSizeSlot);
        if (buffer >= PoolStride) return;

        uint size = PoolStride - buffer;
        if (size <= m.ReadU32(c.GP + PoolSizeSlot)) return;

        _heapPool = heap;
        _heapTag = MrHeap.SizeTag(heap);
        m.WriteU32(Pools, PoolBase);
        m.WriteU32(Pools + 4u, PoolBase + PoolStride);
        m.WriteU32(PoolsEnd, PoolBase + size);
        m.WriteU32(PoolsEnd + 4u, PoolBase + PoolStride + size);
        m.WriteU32(c.GP + PoolSizeSlot, size);
        c.V0 = PoolBase;
    }

    public static bool Restore(CpuContext c, IMemory m)
    {
        if (m.ReadU32(Pools) != PoolBase) return true;

        uint heap = _heapPool;
        _heapPool = 0u;
        if (heap != 0u && Allocated(m, heap))
        {
            m.WriteU32(Pools, heap);
            return true;
        }

        m.WriteU32(Pools, 0u);
        m.WriteU32(Pools + 4u, 0u);
        m.WriteU32(PoolsEnd, 0u);
        m.WriteU32(PoolsEnd + 4u, 0u);
        m.WriteU32(c.GP + PoolSizeSlot, 0u);
        m.WriteU32(c.GP + BufferSizeSlot, 0u);
        return false;
    }

    static bool Allocated(IMemory m, uint block)
    {
        return MrHeap.IsAllocated(block) && MrHeap.SizeTag(block) == _heapTag;
    }
}
