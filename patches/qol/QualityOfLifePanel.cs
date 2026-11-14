using System.Numerics;
using Hexa.NET.ImGui;
using RecompOne.Runtime.Config;
using RecompOne.UI;

namespace Recompiled;

public sealed class QualityOfLifePanel : IEmbeddedPanel
{
    public string Name => "Quality Of Life Options";
    public string TitleKey => "panel.qol";
    public bool IsOpen { get; set; }

    public void Draw()
    {
        ImGui.SetNextWindowSize(new Vector2(320, 260), ImGuiCond.FirstUseEver);
        var open = IsOpen;
        if (!ImGui.Begin(this.Title(), ref open))
        {
            IsOpen = open;
            ImGui.End();
            return;
        }

        DrawContent();

        IsOpen = open;
        ImGui.End();
    }

    public void DrawContent()
    {
        Ui.Section(Localization.T("qol.camera"));
        Toggle(QualityOfLifeOptions.CameraControl, "qol.camera_control");

        if (!QualityOfLife.CameraControl) ImGui.BeginDisabled();
        Toggle(QualityOfLifeOptions.CameraRecenter, "qol.camera_recenter");
        Toggle(QualityOfLifeOptions.InvertCameraX, "qol.invert_camera_x");
        Toggle(QualityOfLifeOptions.InvertCameraY, "qol.invert_camera_y");
        SettingsWidgets.Toggle(QualityOfLifeOptions.NoCameraCollision, "qol.no_camera_collision");
        if (!QualityOfLife.CameraControl) ImGui.EndDisabled();
    }

    private static void Toggle(Option<bool> option, string key)
    {
        SettingsWidgets.Toggle(option, key, key + ".hint");
    }
}
