using System;

[Serializable]
public class HabitatData
{
    public int ExpeditionEssenceProgress = 0;
    public int ExpeditionFossilProgress = 0;
    public int ExpeditionHabitatProgress = 0;
    public int CurrentTier = 1;
    public bool CaveUnlocked = false;
    public bool RuneUnlocked = false;
    public bool WaterfallUnlocked = false;

    public HabitatData() { }
}