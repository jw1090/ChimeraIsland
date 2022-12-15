using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private CurrencyManager _currencyManager = null;
    private TutorialManager _tutorialManager = null;
    private GlobalData _globalSaveData = null;
    private HabitatManager _habitatManager = null;
    private HabitatData _habitatSaveData = null;
    private List<FacilityData> _facilitySaveData = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private CollectionData _collectionsData = null;
    private TutorialCompletionData _tutorialCompletionData = null;
    private SettingsData _settingsData = null;

    public HabitatData HabitatData { get => _habitatSaveData; }
    public List<FacilityData> FacilityData { get => _facilitySaveData; }
    public List<ChimeraData> ChimeraData { get => _chimeraSaveData; }
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
        newData.settingsData = _settingsData;
        UpdateGameSaveData(newData);

        _tutorialManager.ResetTutorialProgress();

        _currencyManager.ResetCurrency();
        _habitatManager.ResetHabitatData();
        _habitatManager.LoadHabitatData();

        FileHandler.SaveToJSON(newData, GameConsts.JsonSaveKeys.GAME_DATA, true);
    }

    public void SaveSessionData()
    {
        Debug.Log("Saving Data!");

        _habitatManager.UpdateCurrentChimeras();
        _habitatManager.UpdateCurrentFacilities();

        GlobalData globalData = new GlobalData(_currencyManager.Essence, _currencyManager.Fossils);
        CollectionData collectionData = new CollectionData(_habitatManager.Collections);

        GameSaveData data = new GameSaveData(globalData, _habitatManager.HabitatData, _habitatManager.FacilitiesInHabitat, _habitatManager.ChimerasInHabitat, collectionData, _tutorialCompletionData, _settingsData);
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

    public void QuitGameSave()
    {
        if (_habitatManager.CurrentHabitat == null)
        {
            return;
        }

        SaveSessionData();
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        QuitGameSave();
#endif
    }
}