using System;

[Serializable]
public class HabitatData
{
    public int _expeditionProgress = 0;
    public int _currentTier = 1;

    public HabitatData(int Tier = 1, int ExpeditionProgress = 0)
    {
        _expeditionProgress = ExpeditionProgress;
        _currentTier = Tier;
    }
}