public static class GameConsts
{
    public class LevelToLoadInts
    {
        public const int MAIN_MENU = 1;
        public const int STARTER_SELECT = 2;
        public const int WORLD_MAP = 3;
        public const int STONE_PLANES = 4;
        public const int TREE_OF_LIFE = 5;
        public const int ASHLANDS = 6;
    }

    public class JsonSaveKeys
    {
        public static readonly string GAME_DATA = "Saves/GameSaveData.json";
        public static readonly string TUTORIAL_DATA = "Tutorial/TutorialInfo.json";
    }

    public class AudioMixerKeys
    {
        public const string MASTER = "MasterVolume";
        public const string MUSIC = "MusicVolume";
        public const string SFX = "SFXVolume";
    }
}