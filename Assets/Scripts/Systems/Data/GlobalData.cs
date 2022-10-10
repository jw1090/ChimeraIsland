using System;

[Serializable]
public class GlobalData
{
    public HabitatType lastSessionHabitat = HabitatType.None;
    public int lastSessionEssence = 0;
    public int lastSessionFossils = 0;

    public GlobalData(HabitatType habitatToLoad, int essenceToLoad, int fossilsToLoad)
    {
        lastSessionHabitat = habitatToLoad;
        lastSessionEssence = essenceToLoad;
        lastSessionFossils = fossilsToLoad;
    }
}