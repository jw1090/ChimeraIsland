using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private EssenceManager _essenceManager = null;
    private GlobalData _globalSaveData = null;
    private HabitatManager _habitatManager = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private List<FacilityData> _facilitySaveData = null;

    public int lastSessionTutorial { get => _globalSaveData.lastSessionTutorial; }
    public HabitatType LastSessionHabitat { get => _globalSaveData.lastSessionHabitat; }
    public List<ChimeraData> ChimeraData { get => _chimeraSaveData; }
    public List<FacilityData> FacilityData { get => _facilitySaveData; }
    public int EssenceData { get => _globalSaveData.lastSessionEssence; }

    public void SetEssenceManager(EssenceManager essenceManager) { _essenceManager = essenceManager; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }
    public void SetLastSessionTutorial(int lst) { _globalSaveData.lastSessionTutorial = lst; }

    public PersistentData Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");
        LoadData();
        return this;
    }

    public void LoadData()
    {
        GameSaveData myData = FileHandler.ReadFromJSON<GameSaveData>(GameConsts.JsonSaveKeys.GAME_SAVE_DATA_FILE);
        if (myData == null)
        {
            Debug.Log($"No Save Data found");
            myData = new GameSaveData();
        }

        UpdateGameSaveData(myData);
    }

    public void SaveSessionData(HabitatType habitatType = HabitatType.None)
    {
        GlobalData myGlobalData = new GlobalData(habitatType, _essenceManager.CurrentEssence, _globalSaveData.lastSessionTutorial);
        List<FacilityData> myFacilityData = FacilitiesToData();
        List<ChimeraData> myChimeraData = ChimerasToData();

        GameSaveData myData = new GameSaveData(myGlobalData, myChimeraData, myFacilityData);
        UpdateGameSaveData(myData);

        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.GAME_SAVE_DATA_FILE);
    }

    private void UpdateGameSaveData(GameSaveData myData)
    {
        _globalSaveData = myData.globalData;
        _chimeraSaveData = myData.chimeras;
        _facilitySaveData = myData.facilities;
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
        SaveSessionData(_habitatManager.CurrentHabitat.Type);
    }
}