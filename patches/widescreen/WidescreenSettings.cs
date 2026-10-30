using ImGuiNET;
using RecompOne.Runtime.Config;
using RecompOne.Runtime.Host.Window;

namespace Recompiled;

public static class WidescreenOptions
{
    public static readonly Option<float> Aspect = Options.Float("widescreen.aspect", 16f / 9f)
        .Normalized(static v => Math.Clamp(v, 1f, 3f));
    public static readonly Option<bool> OriginalAspect = Options.Bool("widescreen.original_aspect");
}

public static class RenderOptions
{
    public static readonly Option<float> DrawDistance = Options.Float("render.draw_distance", 1f)
        .Normalized(static v => Math.Clamp(v, 1f, 3f));
    public static readonly Option<bool> BetterRendering = Options.Bool("render.better_rendering", true);
}

public static class WidescreenSettings
{
    static readonly (string Label, float Value)[] _presets =
    [
        ("4:3", 4f / 3f),
        ("16:10", 16f / 10f),
        ("16:9", 16f / 9f),
        ("21:9", 21f / 9f),
    ];
    
    public static void Register()
    {
        SettingsPages.Page(SettingsPages.Display)
            .Add("widescreen", DrawWidescreen, 150)
            .Add("rendering", DrawRendering, 160);
    }
    
    static void DrawWidescreen()
    {
        float aspect = WidescreenOptions.Aspect;
        bool original = WidescreenOptions.OriginalAspect;
        int selected = MatchPreset(aspect);
        string label = original ? Localization.T("widescreen.original") : selected >= 0 ? _presets[selected].Label : Localization.T("widescreen.custom");
        
        ImGui.TextUnformatted(Localization.T("widescreen.aspect"));
        if (ImGui.BeginCombo("##aspect-preset", label))
        {
            if (ImGui.Selectable(Localization.T("widescreen.original"), original)) WidescreenOptions.OriginalAspect.Value = true;
            for (int i = 0; i < _presets.Length; i++)
                if (ImGui.Selectable(_presets[i].Label, !original && selected == i))
                    Apply(_presets[i].Value);
            if (ImGui.Selectable(Localization.T("widescreen.custom"), !original && selected < 0))
                Apply(aspect);
            ImGui.EndCombo();
        }
        
        if (original) ImGui.BeginDisabled();
        float custom = aspect;
        if (ImGui.SliderFloat("##aspect-custom", ref custom, 1.0f, 2.5f, "%.3f : 1"))
            Apply(custom);
        if (original) ImGui.EndDisabled();
    }

    static void DrawRendering()
    {
        ImGui.TextUnformatted(Localization.T("render.draw_distance"));
        float distance = RenderOptions.DrawDistance;
        if (ImGui.SliderFloat("##draw-distance", ref distance, 1.0f, 3.0f, "%.2fx"))
            RenderOptions.DrawDistance.Value = distance;
        SettingsWidgets.Note("render.draw_distance_hint");
        
        ImGui.Spacing();
        
        SettingsWidgets.Toggle(RenderOptions.BetterRendering, "render.better_terrain");
        SettingsWidgets.Note("render.better_terrain_hint");
    }
    
    static int MatchPreset(float aspect)
    {
        for (int i = 0; i < _presets.Length; i++)
            if (MathF.Abs(_presets[i].Value - aspect) < 0.001f) return i;
        return -1;
    }
    
    static void Apply(float aspect)
    {
        WidescreenOptions.Aspect.Value = aspect;
        WidescreenOptions.OriginalAspect.Value = false;
    }
}
