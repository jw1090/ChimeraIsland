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
    public HabitatType LastSessionHabitat { get => _globalSaveData.lastSessionHabitat; }

    public void SetEssenceManager(EssenceManager essenceManager) { _essenceManager = essenceManager; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }

    public PersistentData Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

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

        _globalSaveData = myData.globalData;
        _facilitySaveData = myData.facilities;
        _chimeraSaveData = myData.chimeras;
    }

    public void LoadEssence()
    {
        _essenceManager.UpdateEssence(_globalSaveData.currentEssence);
    }

    public void SaveSessionData()
    {
        List<ChimeraData> myChimeraData = ChimerasToJson();
        List<FacilityData> myFacilityData = FacilitiesToJson();
        GlobalData myGlobalData = new GlobalData(HabitatType.None, _essenceManager.CurrentEssence, 0);

        GameSaveData myData = new GameSaveData(myGlobalData, myFacilityData, myChimeraData);

        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.GAME_SAVE_DATA_FILE);
    }

    public void SaveSessionDataOnQuit()
    {
        List<ChimeraData> myChimeraData = ChimerasToJson();
        List<FacilityData> myFacilityData = FacilitiesToJson();
        GlobalData myGlobalData = new GlobalData(_habitatManager.CurrentHabitat.Type, _essenceManager.CurrentEssence, 0);

        GameSaveData myData = new GameSaveData(myGlobalData, myFacilityData, myChimeraData);

        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.GAME_SAVE_DATA_FILE);
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

    public void OnApplicationQuit()
    {
        SaveSessionDataOnQuit();
    }
}