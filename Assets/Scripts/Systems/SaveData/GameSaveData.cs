using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public GlobalData globalData = new GlobalData(HabitatType.None, 0, 0);
    public float masterVolume = 0.0f;
    public float musicVolume = 0.0f;
    public float sfxVolume = 0.0f;
    public List<ChimeraData> chimeras = new List<ChimeraData>();
    public List<FacilityData> facilities = new List<FacilityData>();

    public GameSaveData(GlobalData newGlobalData, List<ChimeraData> newChimeraData, List<FacilityData> newFacilityData, Vector3 volumes)
    {
        globalData = newGlobalData;
        chimeras = newChimeraData;
        facilities = newFacilityData;
        masterVolume = volumes.x;
        musicVolume = volumes.y;
        sfxVolume = volumes.z;
    }

    public GameSaveData() { }
}