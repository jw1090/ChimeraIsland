using System;

[Serializable]
public class GlobalData
{
    public int lastSessionEssence = 0;
    public int lastSessionFossils = 0;

    public GlobalData(int essenceToLoad, int fossilsToLoad)
    {
        lastSessionEssence = essenceToLoad;
        lastSessionFossils = fossilsToLoad;
    }
}