using System.Numerics;
using RecompOne.Runtime;
using RecompOne.Runtime.Hle;

namespace Recompiled;

public static class TerrainFloatProjection
{
    private static readonly NativeVertex[] Fifo = new NativeVertex[3];
    private static readonly short[] Rotation = new short[9];
    private static readonly int[] Translation = new int[3];
    private static readonly int[] View = new int[3];
    private static int _serial;
    private static NativeVertex _first;
    private static uint _firstWord;
    private static bool _firstValid;

    public static void Reset()
    {
        Array.Clear(Fifo);
        _serial = 0;
        _first = default;
        _firstWord = 0;
        _firstValid = false;
    }

    public static void Rtpt(int shift, bool lm)
    {
        _firstValid = false;
        if (NativeGeometry.Enabled && TerrainPatch.NoTriangleSubdivision && shift == 12)
        {
            Refresh();
            LoadInputs();
            Gte.RtptFloat();
            for (var i = 0; i < 3; i++) Fifo[i] = Project(i);
        }
        Gte.Rtpt(shift, lm);
    }

    public static void Rtps(int shift, bool lm)
    {
        if (NativeGeometry.Enabled && TerrainPatch.NoTriangleSubdivision && shift == 12)
        {
            Refresh();
            LoadInputs();
            _first = Fifo[0];
            _firstWord = Gte.Read(12);
            _firstValid = true;
            Gte.RtpsFloat();
            Fifo[0] = Fifo[1];
            Fifo[1] = Fifo[2];
            Fifo[2] = Project(2);
        }
        Gte.Rtps(shift, lm);
    }

    public static void Store(int register, uint address, uint word, bool ignoreDepth = false)
    {
        if (NativeGeometry.Enabled && TerrainPatch.NoTriangleSubdivision && register is >= 12 and <= 14)
        {
            var vertex = Fifo[register - 12] with { IgnoreDepth = ignoreDepth };
            NativeGeometry.Store(address, word, in vertex);
        }
    }

    public static void StoreFirst(uint address, uint word)
    {
        if (NativeGeometry.Enabled && TerrainPatch.NoTriangleSubdivision && _firstValid && word == _firstWord)
            NativeGeometry.Store(address, word, in _first);
    }

    private static void Refresh()
    {
        var serial = Gte.TransformSerial;
        Gte.Snapshot(serial, Rotation, Translation, View);
        Span<float> rotation = stackalloc float[9];
        Span<float> translation = stackalloc float[3];
        for (var i = 0; i < 9; i++) rotation[i] = Rotation[i] / 4096f;
        for (var i = 0; i < 3; i++) translation[i] = Translation[i];
        Gte.SetFloatTransform(rotation, translation, View[0], View[1] / 65536f, View[2] / 65536f);
        _serial = serial;
    }

    private static void LoadInputs()
    {
        for (var i = 0; i < 3; i++)
        {
            var xy = Gte.Read(i * 2);
            Gte.LoadVertexFloat(i, (short)xy, (short)(xy >> 16), (short)Gte.Read(i * 2 + 1));
        }
    }

    private static NativeVertex Project(int index)
    {
        var vertex = Gte.ReadFloatVertex(index);
        return new NativeVertex(
            new Vector3(vertex.X, vertex.Y, vertex.Z),
            new Vector2(vertex.ScreenX, vertex.ScreenY),
            _serial);
    }
}
