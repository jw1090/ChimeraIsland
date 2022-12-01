using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private CurrencyManager _currencyManager = null;
    private GlobalData _globalSaveData = null;
    private HabitatManager _habitatManager = null;
    private TutorialManager _tutorialManager = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private List<FacilityData> _facilitySaveData = null;
    private List<HabitatData> _habitatSaveData = null;
    private CollectionData _collectionsData = null;
    private TutorialCompletionData _tutorialCompletionData = null;
    private SettingsData _settingsData = null;

    public HabitatType LastSessionHabitat { get => _globalSaveData.lastSessionHabitat; }
    public List<ChimeraData> ChimeraData { get => _chimeraSaveData; }
    public List<FacilityData> FacilityData { get => _facilitySaveData; }
    public List<HabitatData> HabitatData { get => _habitatSaveData; }
    public CollectionData CollectionData { get => _collectionsData; }
    public TutorialCompletionData MyTutorialCompletion { get => _tutorialCompletionData; }
    public SettingsData SettingsData { get => _settingsData; }
    public int EssenceData { get => _globalSaveData.lastSessionEssence; }
    public int FossilData { get => _globalSaveData.lastSessionFossils; }

    public void SetCurrencyManager(CurrencyManager currencyManager) { _currencyManager = currencyManager; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }
    public void SetTutorialManager(TutorialManager tutorialManager) { _tutorialManager = tutorialManager; }
    public void SetTutorialCompletion(TutorialCompletionData tutorialCompletion) { _tutorialCompletionData = tutorialCompletion; }
    public void SetVolume(List<float> volumes) { _settingsData.SetVolume(volumes); }
    public void SetSpeed(float speed) { _settingsData.cameraSpeed = speed; }
    public void SetSpinSpeed(float speed) { _settingsData.spinSpeed = speed; }

    public PersistentData Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");
        LoadData();

        return this;
    }

    private void LoadData()
    {
        GameSaveData myData = FileHandler.ReadFromJSON<GameSaveData>(GameConsts.JsonSaveKeys.GAME_DATA, true);
        if (myData == null)
        {
            Debug.Log($"No Save Data found");
            myData = new GameSaveData();
        }
        UpdateGameSaveData(myData);
    }

    public void NewSaveData()
    {
        GameSaveData newData = new GameSaveData();
        UpdateGameSaveData(newData);

        _tutorialManager.ResetTutorialProgress();

        _currencyManager.ResetCurrency();
        _habitatManager.ResetDictionaries();
        _habitatManager.LoadHabitatData();

        FileHandler.SaveToJSON(newData, GameConsts.JsonSaveKeys.GAME_DATA, true);
    }

    public void SaveSessionData(HabitatType habitatType = HabitatType.None)
    {
        _habitatManager.UpdateCurrentHabitatChimeras();
        _habitatManager.UpdateCurrentHabitatFacilities();

        GlobalData globalData = new GlobalData(habitatType, _currencyManager.Essence, _currencyManager.Fossils);
        List<HabitatData> habitatData = _habitatManager.HabitatDataList;
        List<FacilityData> facilityData = FacilitiesToData();
        List<ChimeraData> chimeraData = ChimerasToData();
        CollectionData collectionData = new CollectionData(_habitatManager.ChimeraCollections);

        GameSaveData data = new GameSaveData(globalData, habitatData, facilityData, chimeraData, collectionData, _tutorialCompletionData, _settingsData);
        UpdateGameSaveData(data);

        FileHandler.SaveToJSON(data, GameConsts.JsonSaveKeys.GAME_DATA, true);
    }

    private void UpdateGameSaveData(GameSaveData myData)
    {
        _globalSaveData = myData.globalData;
        _habitatSaveData = myData.habitatData;
        _facilitySaveData = myData.facilityData;
        _chimeraSaveData = myData.chimeraData;
        _collectionsData = myData.collectionData;
        _tutorialCompletionData = myData.tutorialCompletionData;
        _settingsData = myData.settingsData;
    }

    public void ResetLastSessionHabitat()
    {
        _globalSaveData.lastSessionHabitat = HabitatType.None;
    }

    private List<ChimeraData> ChimerasToData()
    {
        List<ChimeraData> chimeraList = new List<ChimeraData>();
        Dictionary<HabitatType, List<ChimeraData>> chimerasByHabitat = _habitatManager.ChimerasDictionary;

        foreach (KeyValuePair<HabitatType, List<ChimeraData>> kvp in chimerasByHabitat)
        {
            chimeraList.AddRange(kvp.Value);
        }

        return chimeraList;
    }

    private List<FacilityData> FacilitiesToData()
    {
        List<FacilityData> facilityList = new List<FacilityData>();
        Dictionary<HabitatType, List<FacilityData>> facilityByHabitat = _habitatManager.FacilityDictionary;

        foreach (KeyValuePair<HabitatType, List<FacilityData>> kvp in facilityByHabitat)
        {
            facilityList.AddRange(kvp.Value);
        }

        return facilityList;
    }

    public void QuitGameSave()
    {
        if (_habitatManager.CurrentHabitat == null)
        {
            return;
        }

        SaveSessionData(_habitatManager.CurrentHabitat.Type);
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        QuitGameSave();
#endif
    }
}