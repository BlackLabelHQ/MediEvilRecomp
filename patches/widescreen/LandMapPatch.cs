using RecompOne.Runtime.Hle;

namespace Recompiled;

//half baked, need to finish
public static class LandMapPatch
{
    const int BaseWidth = 512;

    static int Margin => Display.WideMargin(BaseWidth);

    public static uint RightEdge(uint x) => unchecked(x + (uint)(BaseWidth + Margin));

    public static uint LeftEdge(uint x) => unchecked((uint)-Margin - x);
}
