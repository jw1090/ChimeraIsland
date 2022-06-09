using System;

[Serializable]
public class GlobalData
{
    public HabitatType lastSessionHabitat = HabitatType.None;
    public int lastSessionEssence = 0;
    public int lastSessionTutorial = 0;

    public GlobalData(HabitatType habitatToLoad, int essenceToLoad, int tutorialToLoad)
    {
        lastSessionHabitat = habitatToLoad;
        lastSessionEssence = essenceToLoad;
        lastSessionTutorial = tutorialToLoad;
    }
}