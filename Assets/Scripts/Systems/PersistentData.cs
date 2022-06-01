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

        _globalSaveData = FileHandler.ReadFromJSON<GlobalData>(GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);
        _chimeraSaveData = FileHandler.ReadListFromJSON<ChimeraData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
        _facilitySaveData = FileHandler.ReadListFromJSON<FacilityData>(GameConsts.JsonSaveKeys.FACILITY_SAVE_DATA_FILE);

        return this;
    }

    private void SaveChimeras()
    {
        List<ChimeraData> myData = ChimerasToJson();
        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
    }

    private void SaveFacilities()
    {
        List<FacilityData> myData = FacilitiesToJson();
        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.FACILITY_SAVE_DATA_FILE);
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

    public void OnApplicationQuit()
    {
        SaveChimeras();
        SaveFacilities();
        _essenceManager.SaveEssence();
    }
}