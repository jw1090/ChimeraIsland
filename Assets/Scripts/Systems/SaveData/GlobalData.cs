using System;

[Serializable]
public class GlobalData
{
    public HabitatType lastSessionHabitat = HabitatType.None;
    public int currentEssence = 0;
    public GlobalData(HabitatType habitatToLoad, int essence)
    {
        lastSessionHabitat = habitatToLoad;
        currentEssence = essence;
    }
}