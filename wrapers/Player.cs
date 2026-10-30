using RecompOne.Runtime.Memory;

namespace MediEvil;

public static class Player
{
    public const uint Energy = 0x00u;
    public const uint StoredEnergy = 0x04u;
    public const uint MaxStoredEnergy = 0x08u;
    public const uint Weapon = 0x0Cu;
    public const uint Shield = 0x10u;
    public const uint Gold = 0x14u;
    public const uint Flags = 0x18u;
    public const uint HallOfHeroesGroup = 0x1Cu;
    public const uint HallOfHeroesFlags = 0x1Eu;
    public const uint NextLandMapStart = 0x2Cu;
    public const uint LandMapLinkStates = 0x30u;
    public const uint LevelFlags = 0x34u;
    public const uint SiteFlags = 0x50u;
    public const uint LinkFlags = 0x68u;
    public const uint Inventory = 0x84u;
    public const int InventoryCount = 42;
    public const uint StateSize = 0x530u;


    public const byte ChaliceFlag = 1 << 1;
    public const byte SiteMistRemoved = 1 << 0;
    public const byte SiteCompleted = 1 << 1;



    static IMemory M => Game.M;
    static uint P => Game.PlayerAddr;

    public static bool Available => Game.Available && Game.IsRam(P);

    public static int EnergyValue { get => (int)M.ReadU32(P + Energy); set => M.WriteU32(P + Energy, (uint)value); }
    public static int MaxEnergy { get => (int)M.ReadU32(P + MaxStoredEnergy); set => M.WriteU32(P + MaxStoredEnergy, (uint)value); }
    public static uint LinkStates { get => M.ReadU32(P + LandMapLinkStates); set => M.WriteU32(P + LandMapLinkStates, value); }

    public static byte Level(int level) => M.ReadU8(P + LevelFlags + (uint)level);
    public static bool HasChalice(int level) => (Level(level) & ChaliceFlag) != 0;
    public static byte Site(int site) => M.ReadU8(P + SiteFlags + (uint)site);

    public static (short Quantity, short SubType) Item(int index) => ((short)M.ReadU16(P + Inventory + (uint)index * 4u), (short)M.ReadU16(P + Inventory + (uint)index * 4u + 2u));
}
