using RecompOne.Runtime.Memory;
using RecompOne.Runtime.Updates;
using Recompiled;
using RecompOne.Runtime.Dispatch;

if (AutoUpdater.HandleRelaunch(args)) return 0;
args = AutoUpdater.ConsumeArguments(args);

var title = "MediEvilRecomp";

RecompOne.Runtime.Runtime.UserInterface = RecompOne.UI.BigPicture.UserInterfaces.Create();

var asm = System.Reflection.Assembly.GetExecutingAssembly();

if (Array.Find(asm.GetManifestResourceNames(), n => n.EndsWith(".languages.json", StringComparison.OrdinalIgnoreCase)) is { } languagesRes)
    RecompOne.UI.Localization.MergeEmbedded(asm, languagesRes);

if (Array.Find(asm.GetManifestResourceNames(), n => n.EndsWith(".MediEvilRecomp.ico", StringComparison.OrdinalIgnoreCase)) is { } iconRes)
{
    using var iconStream = asm.GetManifestResourceStream(iconRes)!;
    using var iconMem = new MemoryStream();
    iconStream.CopyTo(iconMem);
    RecompOne.Runtime.Runtime.SetIcon(iconMem.ToArray());
}


RecompOne.Runtime.Hle.NativeGeometry.Enabled = false;

Dispatcher.Tolerant = true;

TerrainPatch.Register();
WaterMaterialPatch.Register();
WidescreenPatch.Register();
QualityOfLifeMenu.Register();
AutoUpdater.Configure("BlackLabelHQ/MediEvilRecomp");
RecompOne.Runtime.Hub.ModHub.Configure("BlackLabelHQ/MediEvilRecompModHub");

RecompOne.Runtime.Runtime.Run(() => Entry.Run(new PSMemory(0x00400000), args.Length > 0 ? args[0] : null, title));
return 0;
