using RecompOne.Runtime.Events;
using RecompOne.Runtime.Hle;

namespace Recompiled;

public static class WidescreenPatch
{
    public static bool OriginalAspect;
    public static float StageAspect = 16f / 9f;
    
    public static void Register() => Event.AddListener<RuntimeReadyEvent>(OnRuntimeReady);
    
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
