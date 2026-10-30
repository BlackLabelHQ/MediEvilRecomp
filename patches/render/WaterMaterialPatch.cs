using MediEvil;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Events;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;

namespace Recompiled;

//specific for shaders, add others if needed
public static class WaterMaterialPatch
{
    private const string LakeOverlay = "la";
    private const float LakeWaterLevel = 0f;
    private const int ScanInterval = 15;

    private static readonly List<(int TPage, int Clut, int U0, int V0, int U1, int V1, byte Material)> Tags = [];
    private static int _frame;
    private static long _signature;
    private static bool _lake;
    private static bool _inLevel;

    public static void Register()
    {
        Event.AddListener<OverlayLoadedEvent>(e => _lake = e.Name == LakeOverlay);
    }

    public static void BeginLevel(CpuContext c, IMemory m)
    {
        Reset();
        _inLevel = true;
    }

    public static void EndLevel(CpuContext c, IMemory m)
    {
        _inLevel = false;
        Reset();
    }

    public static void Update(CpuContext c, IMemory m)
    {
        if (!_inLevel || _frame++ % ScanInterval != 0) return;

        if (_lake) ShaderScene.SetWaterLevel(LakeWaterLevel);
        else ShaderData.Remove("SceneWaterLevel");

        var water = 0;
        for (var i = 0; i < 8; i++)
            if (((TextureAttribute)Game.TextureAttributes(i) & TextureAttribute.Water) != 0)
                water |= 1 << i;

        Tags.Clear();
        var signature = (long)Game.TextureListAddr * 31 + water;
        if (water != 0)
        {
            for (var i = 0; i < MediEvil.Texture.MaxCount; i++)
            {
                if (MediEvil.Texture.At(i) is not { } texture) continue;
                if ((water & (1 << texture.UserField)) == 0) continue;

                var (u0, v0, u1, v1) = texture.Bounds;
                Tags.Add((texture.TPage, texture.Clut, u0, v0, u1, v1, ShaderMaterials.Water));
                signature = signature * 31 + ((long)texture.TPage << 48 | (long)texture.Clut << 32 | (long)u0 << 24 |
                                              (long)v0 << 16 | (long)u1 << 8 | (uint)v1);
            }
        }

        if (signature == _signature) return;
        _signature = signature;
        if (Tags.Count == 0) ShaderMaterials.Clear();
        else ShaderMaterials.Replace(Tags);
    }

    private static void Reset()
    {
        _frame = 0;
        _signature = 0;
        Tags.Clear();
        ShaderMaterials.Clear();
        ShaderData.Remove("SceneWaterLevel");
    }
}
