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
    private Vector3 _volumes = Vector3.zero;

    public HabitatType LastSessionHabitat { get => _globalSaveData.lastSessionHabitat; }
    public List<ChimeraData> ChimeraData { get => _chimeraSaveData; }
    public List<FacilityData> FacilityData { get => _facilitySaveData; }
    public Vector3 Volumes { get => _volumes; }
    public int EssenceData { get => _globalSaveData.lastSessionEssence; }
    public int FossilData { get => _globalSaveData.lastSessionFossils; }

    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }
    public void SetEssenceManager(CurrencyManager essenceManager) { _currencyManager = essenceManager; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }
    public void SetTutorialManager(TutorialManager tutorialManager) { _tutorialManager = tutorialManager; }

    public PersistentData Initialize()
    {
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

        GlobalData myGlobalData = new GlobalData(habitatType, _currencyManager.Essence, _currencyManager.Fossils);
        List<FacilityData> myFacilityData = FacilitiesToData();
        List<ChimeraData> myChimeraData = ChimerasToData();

        GameSaveData myData = new GameSaveData(myGlobalData, myChimeraData, myFacilityData, _audioManager.Volumes);
        UpdateGameSaveData(myData);

        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.GAME_DATA);
    }

    private void UpdateGameSaveData(GameSaveData myData)
    {
        _globalSaveData = myData.globalData;
        _chimeraSaveData = myData.chimeras;
        _facilitySaveData = myData.facilities;
        _volumes = new Vector3(myData.masterVolume, myData.musicVolume, myData.sfxVolume);
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