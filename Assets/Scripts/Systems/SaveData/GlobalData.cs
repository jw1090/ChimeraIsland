using System;

[Serializable]
public class GlobalData
{
    public HabitatType lastSessionHabitat = HabitatType.None;
    public int currentEssence = 0;
    public int currentTutorial = 0;
    public GlobalData(HabitatType habitatToLoad, int essence, int tutorialToLoad)
    {
        lastSessionHabitat = habitatToLoad;
        currentEssence = essence;
        currentTutorial = tutorialToLoad;
    }
}