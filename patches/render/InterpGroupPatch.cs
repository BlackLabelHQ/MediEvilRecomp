using RecompOne.Runtime;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class InterpGroupPatch
{
    private const uint ViewportRoot = 0x7F0;
    private const uint ViewportCamera = 0x7C;
    private const uint ViewportWidth = 0x10;
    private const uint ViewportHeight = 0x12;
    private const uint RenderMatrix = 0xA0;
    private const int MaxViewports = 16;

    private static readonly Stack<bool> _cameras = new();

    public static void UpdateCamera(CpuContext c, IMemory m)
    {
        var root = m.ReadU32(c.GP + ViewportRoot);
        if (root == 0) return;

        uint best = 0;
        var area = -1;
        var vp = m.ReadU32(root);
        for (var i = 0; vp != 0 && i < MaxViewports; i++, vp = m.ReadU32(vp))
        {
            if (m.ReadU32(vp + ViewportCamera) == 0) continue;

            var size = (short)m.ReadU16(vp + ViewportWidth) * (short)m.ReadU16(vp + ViewportHeight);
            if (size <= area) continue;

            area = size;
            best = vp;
        }

        if (best != 0) InterpCamera.Set(m, best + RenderMatrix);
    }

    public static void BeginViewport(CpuContext c, IMemory m)
    {
        ParticleIdentityPatch.Register(m, c.A0);
        var camera = m.ReadU32(c.A0 + ViewportCamera) != 0;
        _cameras.Push(camera);
        if (camera) InterpCamera.Push(m, c.A0 + RenderMatrix);
    }

    public static void EndViewport(CpuContext c, IMemory m)
    {
        if (_cameras.Count > 0 && _cameras.Pop()) InterpCamera.Pop();
    }

    public static void BeginAnimMesh(CpuContext c, IMemory m)
    {
        InterpGroups.Push(c.A0);
    }

    public static void BeginStaticMesh(CpuContext c, IMemory m)
    {
        InterpGroups.Push(c.A0, (c.A2 & 0xFFu) | (c.A3 & 0xFFu) << 8);
    }

    public static void EndMesh(CpuContext c, IMemory m)
    {
        InterpGroups.Pop();
    }
}
