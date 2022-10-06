﻿using System;

[Serializable]
public class HabitatData
{
    public int expeditionEssenceProgress = 0;
    public int expeditionFossilProgress = 0;
    public int expeditionHabitatProgress = 0;
    public int currentTier = 1;
    public bool aUnlocked = false;
    public bool bUnlocked = false;
    public bool cUnlocked = false;
    public bool caveUnlocked = false;
    public bool runeUnlocked = false;
    public bool waterfallUnlocked = false;

    public HabitatData()
    {

    }
}