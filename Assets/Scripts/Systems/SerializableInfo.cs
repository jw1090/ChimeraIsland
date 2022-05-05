using System;
using System.Collections.Generic;

[Serializable]
public class SaveJsonList
{
    private List<ChimeraJson> _chimeraList = new List<ChimeraJson>();

    public int CurrentChimeraCapacity { get; set; } = 3;

    public List<ChimeraJson> GetChimeraList() { return _chimeraList; }
    public void AddToChimeraList(ChimeraJson chimera) { _chimeraList.Add(chimera); }
 }

 [Serializable]
 public class ChimeraJson
 {
     public HabitatType habitatType;
     public ChimeraType chimeraType;
     public int id = 0;
     public int level = 0;
     public int intelligence = 0;
     public int strength = 0;
     public int endurance = 0;
     public int happiness = 0;

     public ChimeraJson(HabitatType newHabitatType, ChimeraType newChimeraType, int newId, int newLevel, int newIntelligence, int newSrength, int newEndurance, int newHappiness)
     {
        habitatType = newHabitatType;
        chimeraType = newChimeraType;
        id = newId;
        level = newLevel;
        intelligence = newIntelligence;
        strength = newSrength;
        endurance = newEndurance;
        happiness = newHappiness;
     }
 }
