using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public GlobalData globalData = new GlobalData( 0, 0);
    public HabitatData habitatData = new HabitatData();
    public List<FacilityData> facilityData = new List<FacilityData>();
    public List<ChimeraData> chimeraData = new List<ChimeraData>();
    public CollectionData collectionData = new CollectionData();
    public TutorialCompletionData tutorialCompletionData = new TutorialCompletionData();
    public SettingsData settingsData = new SettingsData();

    public GameSaveData(GlobalData globalData, HabitatData habitatData, List<FacilityData> facilityData, List<ChimeraData> chimeraData, CollectionData collectionData, TutorialCompletionData tutorialCompletionData, SettingsData settingsData)
    {
        this.globalData = globalData;
        this.habitatData = habitatData;
        this.facilityData = facilityData;
        this.chimeraData = chimeraData;
        this.collectionData = collectionData;
        this.tutorialCompletionData = tutorialCompletionData;
        this.settingsData = settingsData;
    }

    public GameSaveData() { }
}