namespace MediEvil;

public static class Functions
{
    //stupid library i hate you
    public const uint MRAllocMem = 0x800A403Cu;
    public const uint MRFreeMem = 0x800A42B4u;
    public const uint MRInitialiseResources = 0x800A7E4Cu;
    public const uint MRLoadResource = 0x800A804Cu;
    public const uint MRProcessResource = 0x800A83E0u;
    public const uint MRUnloadResource = 0x800A8564u;
    public const uint PowerPackerDecrunch = 0x800BA4D4u;
    public const uint MRAllocateDynamicPolys = 0x800A47C0u;
    public const uint MRFreeDynamicPolys = 0x800A4830u;
    public const uint MRRenderViewport = 0x8009FD78u;
    public const uint MRKillViewport = 0x8009F2F4u;
    public const uint MRKillAllViewports = 0x8009BBFCu;
    public const uint MRUpdateViewportRenderMatrices = 0x800A0480u;

    //game stuffs
    public const uint CamUpdate = 0x8002D21Cu;
    public const uint FadeUpdate = 0x8004EAB8u;
    public const uint EntLibPointOccluded = 0x80049420u;
    public const uint LevelStartUp = 0x80065660u;
    public const uint LevelShutDown = 0x80065904u;
    public const uint FreeTerrShutDown = 0x800514CCu;
    public const uint TerrainFogShutdown = 0x8007A0C0u;
}
