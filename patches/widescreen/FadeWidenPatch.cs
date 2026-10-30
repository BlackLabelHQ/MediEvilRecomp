using RecompOne.Runtime.Context;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class FadeWidenPatch
{
    private const uint DynamicPolyPtr = 0x1F800010u;
    private const uint FadeType = 0x4;
    private const uint ScreenFade = 0;
    private const int ScreenWidth = 0x200;
    private const int MaxPrimWords = 16;

    private static readonly Stack<(uint Start, uint Type)> _updates = new();

    public static void BeginFadeUpdate(CpuContext c, IMemory m)
    {
        _updates.Push((m.ReadU32(DynamicPolyPtr), m.ReadU32(c.A0 + FadeType)));
    }

    public static void EndFadeUpdate(CpuContext c, IMemory m)
    {
        if (_updates.Count == 0) return;
        var (start, type) = _updates.Pop();
        if (type == ScreenFade || Display.WideAspect <= 0f) return;

        var end = m.ReadU32(DynamicPolyPtr);
        var margin = Display.WideMargin(ScreenWidth);
        if (margin <= 0 || end <= start || end - start > 0x10000) return;

        var scale = (ScreenWidth + margin * 2f) / ScreenWidth;
        for (var prim = start; prim < end;)
        {
            var words = (int)(m.ReadU32(prim) >> 24);
            if (words == 0)
            {
                prim += 4;
                continue;
            }

            if (words <= MaxPrimWords) Widen(m, prim + 4, words, scale, margin);
            prim += 4u + (uint)words * 4u;
        }
    }

    private static void Widen(IMemory m, uint body, int words, float scale, int margin)
    {
        var command = m.ReadU32(body) >> 24;
        if (command is >= 0x20 and <= 0x3F)
        {
            var quad = (command & 0x08) != 0;
            var gouraud = (command & 0x10) != 0;
            var textured = (command & 0x04) != 0;
            var vertices = quad ? 4 : 3;
            var index = 1;
            for (var v = 0; v < vertices; v++)
            {
                if (gouraud && v > 0) index++;
                if (index >= words) return;
                Stretch(m, body + (uint)index * 4u, scale, margin);
                index++;
                if (textured) index++;
            }

            return;
        }

        if (command is >= 0x60 and <= 0x7F && words >= 2)
        {
            Stretch(m, body + 4u, scale, margin);
            var sized = (command & 0x18) == 0;
            var sizeIndex = (command & 0x04) != 0 ? 3 : 2;
            if (!sized || sizeIndex >= words) return;

            var sizeAddress = body + (uint)sizeIndex * 4u;
            var size = m.ReadU32(sizeAddress);
            var width = (int)(size & 0xFFFF);
            var widened = (int)MathF.Round(width * scale);
            m.WriteU32(sizeAddress, (size & 0xFFFF0000u) | (uint)(ushort)widened);
        }
    }

    private static void Stretch(IMemory m, uint address, float scale, int margin)
    {
        var xy = m.ReadU32(address);
        var x = (short)(xy & 0xFFFF);
        var stretched = (int)MathF.Round(x * scale) - margin;
        m.WriteU32(address, (xy & 0xFFFF0000u) | (uint)(ushort)stretched);
    }
}
