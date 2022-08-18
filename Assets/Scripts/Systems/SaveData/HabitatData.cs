using System;

[Serializable]
public class HabitatData
{
    public int _expeditionHabitatProgress = 0;
    public int _expeditionEssenceProgress = 0;
    public int _expeditionFossilProgress = 0;
    public int _currentTier = 1;
    public bool _aUnlocked = false;
    public bool _bUnlocked = false;
    public bool _cUnlocked = false;
    public bool _caveUnlocked = false;
    public bool _runeUnlocked = false;
    public bool _waterfallUnlocked = false;

    public HabitatData()
    {

    }
}