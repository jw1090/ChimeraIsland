using System;

[Serializable]
public class GlobalData
{
    public HabitatType lastSessionHabitat = HabitatType.None;
    public int lastSessionEssence = 0;

    public GlobalData(HabitatType habitatToLoad, int essenceToLoad)
    {
        lastSessionHabitat = habitatToLoad;
        lastSessionEssence = essenceToLoad;
    }
}