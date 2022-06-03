using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public GlobalData globalData = new GlobalData(HabitatType.None, 100);
    public List<FacilityData> facilities = new List<FacilityData>();
    public List<ChimeraData> chimeras = new List<ChimeraData>();

    public GameSaveData(GlobalData newGlobalData, List<FacilityData> newFacilityData, List<ChimeraData> newChimeraData)
    {
        globalData = newGlobalData;
        facilities = newFacilityData;
        chimeras = newChimeraData;
    }

    public GameSaveData() { }
}

