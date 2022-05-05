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
     public int id = 0;
     public ChimeraType cType;
     public int level = 0;
     public int intelligence = 0;
     public int strength = 0;
     public int endurance = 0;
     public int happiness = 0;
     public HabitatType hType;
     public ChimeraSaveData(int Id, ChimeraType CType,int Level,int Intelligence, int Strength, int Endurance, int Happiness, HabitatType HType)
     {
         id = Id;
         cType = CType;
         level = Level;
         intelligence = Intelligence;
         strength = Strength;
         endurance = Endurance;
         happiness = Happiness;
         hType = HType;
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
