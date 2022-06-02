using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public List<ChimeraData> chimeras = new List<ChimeraData>();
    public List<FacilityData> facilities = new List<FacilityData>();
    public GlobalData globalData = new GlobalData(100, null);

    public GameSaveData(List<ChimeraData> cd, List<FacilityData> fd, GlobalData gd)
    {
        chimeras = cd;
        facilities = fd;
        globalData = gd;
    }

    public GameSaveData() { }
}

