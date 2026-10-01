using RecompOne.Runtime.Context;
using RecompOne.Runtime.Events;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;
using RecompOne.Runtime.Pgxp;

namespace Recompiled;

public static class WidescreenPatch
{
    public static bool OriginalAspect;
    public static float StageAspect = 16f / 9f;
    static uint _screenFadePacket;

    public static void Register() => Event.AddListener<RuntimeReadyEvent>(OnRuntimeReady);

    public static void BeginScreenFadeRender(CpuContext c, IMemory m)
    {
        _screenFadePacket = m.ReadU32(0x1F800010u);
    }

    public static void EndScreenFadeRender(CpuContext c, IMemory m)
    {
        uint packet = _screenFadePacket;
        _screenFadePacket = 0;
        if (packet == 0 || m.ReadU32(0x1F800010u) != packet + 0x38u) return;
        if (Display.WideAspect <= 0f) return;

        int width = m.ReadU16(packet + 0xCu);
        int height = m.ReadU16(packet + 0x12u);
        int margin = Display.WideMargin(width);
        if (margin <= 0) return;

        WriteFadeVertex(m, packet + 0x8u, -margin, 0);
        WriteFadeVertex(m, packet + 0xCu, width + margin, 0);
        WriteFadeVertex(m, packet + 0x10u, -margin, height);
        WriteFadeVertex(m, packet + 0x14u, width + margin, height);
    }

    static void WriteFadeVertex(IMemory m, uint address, int x, int y)
    {
        uint packed = unchecked((uint)(ushort)x | ((uint)(ushort)y << 16));
        m.WriteU32(address, packed);
        var vertex = new PgxpValue { X = x, Y = y, Flags = PgxpFlags.ValidLow };
        PgxpMemory.Store(address, in vertex, packed);
    }

    static void OnRuntimeReady(RuntimeReadyEvent e)
    {
        var view = RecompOne.Runtime.Runtime.View;
        StageAspect = view.GetFloat("WidescreenAspect", 16f / 9f);
        OriginalAspect = view.GetBool("WidescreenOriginalAspect", false);
        Display.TargetAspect = StageAspect;
        Apply();
    }

    public static void Refresh() => Apply();

    static void Apply()
    {
        StageAspect = Display.TargetAspect;
        Display.SourceAspect = 4f / 3f;
        Display.OutputAspect = 4f / 3f;
        Display.WideAspect = OriginalAspect ? 0f : StageAspect;
    }
}
