using System;
using System.Collections.Generic;

[Serializable]
 public class SaveJsonList
 {
     int currentEssance;
     List<ChimeraJson> chimeraList;
     public SaveJsonList() 
     {
         chimeraList = new List<ChimeraJson> { };
     }
     public void setEssance(int essance) { currentEssance = essance; }
     public void setChimeraList(List<ChimeraJson> ChimeraList) { chimeraList = ChimeraList; }
     public void addToChimeraList(ChimeraJson chimera) { chimeraList.Add(chimera); }
 }

 [Serializable]
 public class ChimeraJson
 {
     int id = 0;
     ChimeraType cType;
     int level = 0;
     int intelligence = 0;
     int strength = 0;
     int endurance = 0;
     int happiness = 0;
     HabitatType hType;
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


