using RecompOne.Runtime.Memory;
using Recompiled;

var title = "MediEvilRecomp";

var asm = System.Reflection.Assembly.GetExecutingAssembly();

if (Array.Find(asm.GetManifestResourceNames(), n => n.EndsWith(".languages.json", StringComparison.OrdinalIgnoreCase)) is { } languagesRes)
    RecompOne.Runtime.Runtime.AddLanguages(asm, languagesRes);

RecompOne.Runtime.Runtime.Defaults(cfg =>
{
    cfg.Default(RecompOne.Runtime.Pgxp.Pgxp.KeyEnable, true);
    cfg.Default(RecompOne.Runtime.Pgxp.Pgxp.KeyCulling, true);
    cfg.Default(RecompOne.Runtime.Pgxp.Pgxp.KeyTextureCorrection, true);
    cfg.Default(RecompOne.Runtime.Pgxp.Pgxp.KeyCpu, true);
    cfg.Default(RecompOne.Runtime.Pgxp.Pgxp.KeyMemory, true);
    cfg.Default(RecompOne.Runtime.Pgxp.Pgxp.KeyVertexCache, false);
    cfg.Default(RecompOne.Runtime.Pgxp.Pgxp.KeyCacheW, false);
    cfg.Default(RecompOne.Runtime.Pgxp.Pgxp.KeyTolerance, RecompOne.Runtime.Pgxp.Pgxp.DefaultTolerance);
});

TerrainPatch.Register();
WidescreenPatch.Register();
WidescreenSettings.Register();

RecompOne.Runtime.Runtime.Run(() => Entry.Run(new PSMemory(0x00400000), args.Length > 0 ? args[0] : null, title));
return 0;
