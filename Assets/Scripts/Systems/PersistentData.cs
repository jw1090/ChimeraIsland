using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private EssenceManager _essenceManager = null;
    private GlobalData _globalSaveData = null;
    private HabitatManager _habitatManager = null;
    private List<ChimeraData> _chimeraSaveData = null;

    public List<ChimeraData> ChimeraData { get => _chimeraSaveData; }
    public int EssenceData { get => CurrentEssence(); }

    public void SetEssenceManager(EssenceManager essenceManager) { _essenceManager = essenceManager; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }

    public PersistentData Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _chimeraSaveData = FileHandler.ReadListFromJSON<ChimeraData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
        _globalSaveData = FileHandler.ReadFromJSON<GlobalData>(GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);

        return this;
    }

    public void SaveChimeras()
    {
        List<ChimeraData> myData = ChimerasToJson();
        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
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
        _essenceManager.SaveEssence();
    }
}