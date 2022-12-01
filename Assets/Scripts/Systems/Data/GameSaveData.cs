using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public GlobalData globalData = new GlobalData(HabitatType.None, 0, 0);
    public List<HabitatData> habitatData = new List<HabitatData>{new HabitatData(), new HabitatData()};
    public List<ChimeraData> chimeraData = new List<ChimeraData>();
    public List<FacilityData> facilityData = new List<FacilityData>();
    public List<CollectionsData> collectionData = new List<CollectionsData>();
    public TutorialCompletionData tutorialCompletionData = new TutorialCompletionData();
    public SettingsData settingsData = new SettingsData();

    public GameSaveData(GlobalData globalData, List<HabitatData> habitatData, List<FacilityData> facilityData, List<ChimeraData> chimeraData, List<CollectionsData> collectionsData, TutorialCompletionData tutorialCompletionData, SettingsData settingsData)
    {
        this.globalData = globalData;
        this.habitatData = habitatData;
        this.facilityData = facilityData;
        this.collectionData = collectionsData;
        this.chimeraData = chimeraData;
        this.tutorialCompletionData = tutorialCompletionData;
        this.settingsData = settingsData;
    }

    public GameSaveData() { }
}