using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private EssenceManager _essenceManager = null;
    private GlobalData _globalSaveData = null;
    private HabitatManager _habitatManager = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private List<FacilityData> _facilitySaveData = null;

    public List<ChimeraData> ChimeraData { get => _chimeraSaveData; }
    public List<FacilityData> FacilityData { get => _facilitySaveData; }
    public int EssenceData { get => CurrentEssence(); }

    public void SetEssenceManager(EssenceManager essenceManager) { _essenceManager = essenceManager; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }

    public PersistentData Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");
        GameSaveData myData = FileHandler.ReadFromJSON<GameSaveData>(GameConsts.JsonSaveKeys.GAME_SAVE_DATA_FILE);
        if(myData == null)
        {
            Debug.Log($"No Save Data found");
            myData = new GameSaveData();
        }
        _globalSaveData = myData.globalData;
        _chimeraSaveData = myData.chimeras;
        _facilitySaveData = myData.facilities;
		
        return this;
    }

    private void SaveData()
    {
        List<ChimeraData> myChimeraData = ChimerasToJson();
        List<FacilityData> myFacilityData = FacilitiesToJson();
        GlobalData myGlobalData = new GlobalData(_globalSaveData.currentEssence);
        GameSaveData myData = new GameSaveData(myChimeraData, myFacilityData, myGlobalData);
        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.GAME_SAVE_DATA_FILE);
    }

    private void SaveEssence()
    {
        GlobalData data = new GlobalData(_essenceManager.CurrentEssence);
        FileHandler.SaveToJSON(data, GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);
    }

    private List<ChimeraData> ChimerasToJson()
    {
        List<ChimeraData> chimeraList = new List<ChimeraData>();
        Dictionary<HabitatType, List<ChimeraData>> chimerasByHabitat = _habitatManager.ChimerasDictionary;

        foreach (KeyValuePair<HabitatType, List<ChimeraData>> kvp in chimerasByHabitat)
        {
            chimeraList.AddRange(kvp.Value);
        }

        return chimeraList;
    }

    private List<FacilityData> FacilitiesToJson()
    {
        List<FacilityData> facilityList = new List<FacilityData>();
        Dictionary<HabitatType, List<FacilityData>> facilityByHabitat = _habitatManager.FacilityDictionary;

        foreach (KeyValuePair<HabitatType, List<FacilityData>> kvp in facilityByHabitat)
        {
            facilityList.AddRange(kvp.Value);
        }

        return facilityList;
    }

    private int CurrentEssence()
    {
        if (_globalSaveData == null)
        {
            return 100;
        }

        return _globalSaveData.currentEssence;
    }

    public void SaveSessionData()
    {
        SaveChimeras();
        SaveFacilities();
        SaveEssence();
    }

    public void OnApplicationQuit()
    {
        SaveSessionData();
    }
}