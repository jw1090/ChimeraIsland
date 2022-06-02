using System;

[Serializable]
public class GlobalData
{
    public int currentEssence = 0;
    public GlobalData(int essence, Habitat LastUsedHabitat)
    {
        currentEssence = essence;
        lastUsedHabitat = LastUsedHabitat;
    }
    public Habitat lastUsedHabitat = null;
}