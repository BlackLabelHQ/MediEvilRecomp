using RecompOne.Runtime.Config;

namespace Recompiled;

public static class QualityOfLifeOptions
{
    public static readonly Option<bool> CameraControl = Options.Bool("qol.camera_control");
    public static readonly Option<bool> CameraRecenter = Options.Bool("qol.camera_recenter");
    public static readonly Option<bool> InvertCameraX = Options.Bool("qol.invert_camera_x");
    public static readonly Option<bool> InvertCameraY = Options.Bool("qol.invert_camera_y");
    public static readonly Option<bool> NoCameraCollision = Options.Bool("qol.no_camera_collision");

    public static IEnumerable<Option<bool>> All => [CameraControl, CameraRecenter, InvertCameraX, InvertCameraY, NoCameraCollision];
}

internal static class QualityOfLife
{
    public static bool CameraControl;
    public static bool CameraRecenter;
    public static bool InvertCameraX;
    public static bool InvertCameraY;
    public static bool NoCameraCollision;

    public static void Load()
    {
        CameraControl = QualityOfLifeOptions.CameraControl;
        CameraRecenter = QualityOfLifeOptions.CameraRecenter;
        InvertCameraX = QualityOfLifeOptions.InvertCameraX;
        InvertCameraY = QualityOfLifeOptions.InvertCameraY;
        NoCameraCollision = QualityOfLifeOptions.NoCameraCollision;
    }
}
