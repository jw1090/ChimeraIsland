using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private AudioManager _audioManager = null;
    private CurrencyManager _currencyManager = null;
    private GlobalData _globalSaveData = null;
    private HabitatManager _habitatManager = null;
    private TutorialManager _tutorialManager = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private List<FacilityData> _facilitySaveData = null;
    private List<HabitatData> _habitatSaveData = null;
    private List<float> _volumes = new List<float>();
    private ResourceManager _resourceManager = null;

    public HabitatType LastSessionHabitat { get => _globalSaveData.lastSessionHabitat; }
    public List<ChimeraData> ChimeraData { get => _chimeraSaveData; }
    public List<FacilityData> FacilityData { get => _facilitySaveData; }
    public List<HabitatData> HabitatData { get => _habitatSaveData; }
    public List<float> Volumes { get => _volumes; }
    public int EssenceData { get => _globalSaveData.lastSessionEssence; }
    public int FossilData { get => _globalSaveData.lastSessionFossils; }

    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }
    public void SetCurrencyManager(CurrencyManager currencyManager) { _currencyManager = currencyManager; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }
    public void SetTutorialManager(TutorialManager tutorialManager) { _tutorialManager = tutorialManager; }

    public PersistentData Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");
        LoadData();

        return this;
    }

    private void LoadData()
    {
        GameSaveData myData = FileHandler.ReadFromJSON<GameSaveData>(GameConsts.JsonSaveKeys.GAME_DATA);
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

        FileHandler.SaveToJSON(newData, GameConsts.JsonSaveKeys.GAME_DATA);
    }

    public void SaveSessionData(HabitatType habitatType = HabitatType.None)
    {
        _habitatManager.UpdateCurrentHabitatChimeras();
        _habitatManager.UpdateCurrentHabitatFacilities();

        GlobalData myGlobalData = new GlobalData(habitatType, _currencyManager.Essence, _currencyManager.Fossils);
        List<FacilityData> myFacilityData = FacilitiesToData();
        List<ChimeraData> myChimeraData = ChimerasToData();
        List<HabitatData> habitatData = _habitatManager.HabitatDataList;
        GameSaveData myData = new GameSaveData(myGlobalData, myChimeraData, myFacilityData, habitatData, _audioManager.Volumes);
        UpdateGameSaveData(myData);

        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.GAME_DATA);
    }

    private void UpdateGameSaveData(GameSaveData myData)
    {
        _globalSaveData = myData.globalData;
        _chimeraSaveData = myData.chimeras;
        _habitatSaveData = myData.habitats;
        _facilitySaveData = myData.facilities;
        _volumes = new List<float> { myData.masterVolume, myData.musicVolume, myData.sfxVolume, myData.ambientVolume, myData.uiSfxVolume };
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
        if(_habitatManager.CurrentHabitat == null)
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