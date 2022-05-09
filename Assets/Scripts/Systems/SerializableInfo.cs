using System;
using System.Collections.Generic;
[Serializable]
public class SaveJsonList
{
    private List<ChimeraSaveData> _chimeraList = new List<ChimeraSaveData>();

    public int CurrentChimeraCapacity { get; set; } = 3;

    public List<ChimeraSaveData> GetChimeraList() { return _chimeraList; }
    public void AddToChimeraList(ChimeraSaveData chimera) { _chimeraList.Add(chimera); }
 }

 [Serializable]
 public class ChimeraSaveData
 {
     public HabitatType habitatType;
     public ChimeraType chimeraType;
     public int id = 0;
     public int level = 0;
     public int intelligence = 0;
     public int strength = 0;
     public int endurance = 0;
     public int happiness = 0;


     public ChimeraSaveData(
         int newId, 
         ChimeraType newChimeraType, 
         int newLevel, 
         int newEndurance, 
         int newIntelligence, 
         int newSrength, 
         int newHappiness, 
         HabitatType newHabitatType)
     {
        id = newId;
        chimeraType = newChimeraType;
        level = newLevel;
        endurance = newEndurance;
        intelligence = newIntelligence;
        strength = newSrength;
        happiness = newHappiness;
        habitatType = newHabitatType;
     }
 }

//will fill in later
[Serializable]
public class HabitatSaveData
{
}

//will add more later
[Serializable]
public class GlobalSaveData
{
    public int currentEssence = 0;
    public GlobalSaveData(int essence) 
    {
        currentEssence = essence;
    }
}
