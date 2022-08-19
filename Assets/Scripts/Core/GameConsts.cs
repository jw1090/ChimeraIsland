using UnityEngine;

public static class GameConsts
{
    public class LevelToLoadInts
    {
        public const int MAIN_MENU = 1;
        public const int STARTER_SELECT = 2;
        public const int STONE_PLANES = 3;
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
        public const string AMBIENT = "AmbientVolume";
        public const string UISFX = "UISFXVolume";
    }

    public class Colors
    {
        public static Color IDLE = new Color(255, 255, 255, 255);
        public static Color HOVER = new Color(222, 226, 178, 255);
        public static Color Active = new Color(218, 226, 129, 255);
    }
}