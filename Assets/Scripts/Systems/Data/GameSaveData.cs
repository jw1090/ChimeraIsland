using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public GlobalData globalData = new GlobalData(HabitatType.None, 0, 0);
    public List<HabitatData> habitatData = new List<HabitatData>{new HabitatData(), new HabitatData()};
    public List<ChimeraData> chimeraData = new List<ChimeraData>();
    public List<FacilityData> facilityData = new List<FacilityData>();
    public TutorialCompletionData tutorialCompletionData = new TutorialCompletionData();
    public float masterVolume = 0.0f;
    public float musicVolume = 0.0f;
    public float sfxVolume = 0.0f;
    public float ambientVolume = 0.0f;
    public float uiSfxVolume = 0.0f;
    public float cameraSpeed = 20.0f;
    public float spinSpeed = 0.8f;

    public GameSaveData(GlobalData globalData, List<HabitatData> habitatData, List<FacilityData> facilityData, List<ChimeraData> chimeraData, TutorialCompletionData tutorialCompletionData, List<float> volumes, float cameraSpeed, float spinSpeed)
    {
        this.globalData = globalData;
        this.habitatData = habitatData;
        this.facilityData = facilityData;
        this.chimeraData = chimeraData;
        this.tutorialCompletionData = tutorialCompletionData;
        masterVolume = volumes[0];
        musicVolume = volumes[1];
        sfxVolume = volumes[2];
        ambientVolume = volumes[3];
        uiSfxVolume = volumes[4];
        this.cameraSpeed = cameraSpeed;
        this.spinSpeed = spinSpeed;
    }

    public GameSaveData() { }
}