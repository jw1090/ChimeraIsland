using System;
using System.Collections.Generic;

[Serializable]
 public class SaveJsonList
 {
     int currentChimeraCapacity;
     List<ChimeraJson> chimeraList;
     public SaveJsonList() 
     {
         chimeraList = new List<ChimeraJson> { };
     }
     public void setChimeraCapacity(int ChimeraCapacity) { currentChimeraCapacity = ChimeraCapacity; }
     public void setChimeraList(List<ChimeraJson> ChimeraList) { chimeraList = ChimeraList; }
     public void addToChimeraList(ChimeraJson chimera) { chimeraList.Add(chimera); }

    public List<ChimeraJson> getChimeraList() { return chimeraList; }
    public int getChimeraCapacity() { return currentChimeraCapacity; }
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


