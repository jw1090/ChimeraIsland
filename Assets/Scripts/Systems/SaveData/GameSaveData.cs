using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public GlobalData globalData = new GlobalData(HabitatType.None, 100, 0);
    public List<ChimeraData> chimeras = new List<ChimeraData>();
    public List<FacilityData> facilities = new List<FacilityData>();

    public GameSaveData(GlobalData newGlobalData, List<ChimeraData> newChimeraData, List<FacilityData> newFacilityData)
    {
        globalData = newGlobalData;
        chimeras = newChimeraData;
        facilities = newFacilityData;
    }

    public GameSaveData() { }
}