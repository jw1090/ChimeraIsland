using System;
using System.Collections.Generic;

[Serializable]
public class HabitatData
{
    public int ExpeditionEssenceProgress = 0;
    public int ExpeditionFossilProgress = 0;
    public int ExpeditionHabitatProgress = 0;
    public List<ModifierType> ExpeditionEssenceModifier = new List<ModifierType>();
    public List<ModifierType> ExpeditionFossilModifier = new List<ModifierType>();
    public List<ModifierType> ExpeditionHabitatModifier = new List<ModifierType>();
    public List<QuestData> questDataList = new List<QuestData>();
    public int CurrentTier = 1;
    public bool CaveUnlocked = false;
    public bool RuneUnlocked = false;
    public bool WaterfallUnlocked = false;

    public HabitatData() { }
}