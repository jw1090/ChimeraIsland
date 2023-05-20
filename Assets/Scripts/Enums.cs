public enum AnimationType
{
    None = -1,
    Idle,
    Walk,
    Held,
    Success,
    Fail,
}

public enum ChimeraOrderType
{
    AveragePower,
    Exploration,
    Stamina,
    Wisdom,
}

public enum ChimeraType
{
    None = -1,
    A,
    A1,
    A2,
    A3,
    B,
    B1,
    B2,
    B3,
    C,
    C1,
    C2,
    C3,
}

public enum CursorType
{
    Default,
    Clickable,
    Dragable,
    Dragging,
    Minable,
    Rotate,
}

public enum DayType
{
    None = -1,
    DayTime,
    NightTime,
}

public enum DetailsButtonType
{
    None = -1,
    Standard,
    ExpeditionParty,
}

public enum ElementType
{
    None = -1,
    Water,
    Grass,
    Fire,
}

public enum EnvironmentSFXType
{
    None = -1,
    Evolution1,
    Evolution2,
    FacilityBuildStart,
    FacilityBuildEnd,
    LevelUp,
    FacilityBuild,
    PortalClick,
    MiningTap,
    MiningHarvest,
    WaterHit,
    StoneHit,
    DirtHit,
    TreeHit,
    StoneClick,
}

public enum EvolutionVFXType
{
    None = -1,
    GrowingLight,
    WaterDrop,
}

public enum ExpeditionState
{
    None = -1,
    Selection,
    Setup,
    InProgress,
    Result,
}

public enum ExpeditionType
{
    None = -1,
    Essence,
    Fossils,
    HabitatUpgrade,
}

public enum FacilityType
{
    None = -1,
    Cave,
    RuneStone,
    Waterfall,
}

public enum HabitatRewardType
{
    None = -1,
    Random,
    Waterfall,
    CaveExploring,
    RuneStone,
    Habitat,
}

public enum ModifierType
{
    None = -1,
    Random,
    Water,
    Grass,
    Fire,
    Stamina,
    Wisdom,
    Exploration,
    Max,
}

public enum OutlineType
{
    None = -1,
    StarterChimeras,
    HabitatChimeras,
    Portal,
    Crystals,
    Temple,
    Pillars,
    Figurines,
    Upgrades,
}

public enum QuestType
{
    None = -1,
    FirstExpedition,
    TrainChimera,
    UpgradeHabitatT2,
    SummonChimera,
    UpgradeFacility,
    UpgradeHabitatT3,
    UnlockAllChimera,
}

public enum SceneType
{
    None = -1,
    MainMenu,
    Starting,
    Habitat,
    Temple,
}

public enum SFXUIType
{
    None = -1,
    StandardClick,
    ConfirmClick,
    PurchaseClick,
    PlaceChimera,
    RemoveChimera,
    ErrorClick,
    Completion,
    Failure,
    StoneDrag,
    Whoosh,
}

public enum StatPreferenceType
{
    None = -1,
    Dislike,
    Neutral,
    Like
}

public enum StatType
{
    None = -1,
    Exploration,
    Stamina,
    Wisdom,
}

public enum TapVFXType
{
    None = -1,
    Ground,
    Water,
    Stone,
    Tree,
}

public enum TempleSectionType
{
    None = -1,
    Buying,
    Chimera,
    Upgrades,
    Collection,
    Gallery,
    Habitat,
}

public enum TutorialStageType
{
    None = -1,
    Intro,
    ExpeditionSetup,
    Facilities,
    ReccomendedTraits,
    Temple,
    HabitatUpgrades,
    Failure,
    BuyChimera,
    Collections,
    Upgrade,
}

public enum UIElementType
{
    None = -1,
    All,
    OtherFacilityButtons,
    ExpeditionButton,
    MarketplaceChimeraTab,
    WorldMapButton,
    FossilsWallets,
    EssenceWallets,
}

public enum UITempleBackType
{
    None = -1,
    BackToHabitat,
    BackToTemple,
}
