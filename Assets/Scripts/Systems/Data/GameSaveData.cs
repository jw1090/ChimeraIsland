using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public GlobalData globalData = new GlobalData(HabitatType.None, 0, 0);
    public bool[] tutorialCompleted;
    public float masterVolume = 0.0f;
    public float musicVolume = 0.0f;
    public float sfxVolume = 0.0f;
    public float ambientVolume = 0.0f;
    public float uiSfxVolume = 0.0f;
    public List<HabitatData> habitats = new List<HabitatData>{new HabitatData(), new HabitatData()};
    public List<ChimeraData> chimeras = new List<ChimeraData>();
    public List<FacilityData> facilities = new List<FacilityData>();

    public GameSaveData(GlobalData newGlobalData, List<ChimeraData> newChimeraData, List<FacilityData> newFacilityData, List<HabitatData> newHabitatData, List<float> volumes, bool[] TutorialCompleted)
    {
        habitats = newHabitatData;
        globalData = newGlobalData;
        chimeras = newChimeraData;
        facilities = newFacilityData;
        masterVolume = volumes[0];
        musicVolume = volumes[1];
        sfxVolume = volumes[2];
        ambientVolume = volumes[3];
        uiSfxVolume = volumes[4];
        tutorialCompleted = TutorialCompleted;
    }

    public GameSaveData() { }
}