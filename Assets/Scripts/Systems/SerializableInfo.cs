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
     public int id = 0;
     public ChimeraType cType;
     public int level = 0;
     public int intelligence = 0;
     public int strength = 0;
     public int endurance = 0;
     public int happiness = 0;
     public HabitatType hType;
     public ChimeraJson(int Id, ChimeraType CType,int Level,int Intelligence, int Strength, int Endurance, int Happiness, HabitatType HType)
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
