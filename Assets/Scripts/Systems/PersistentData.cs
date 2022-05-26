using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private EssenceManager _essenceManager = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private GlobalData _globalSaveData = null;

    public void SetEssenceManager(EssenceManager essenceManager) { _essenceManager = essenceManager; }

    public List<ChimeraData> GetChimeraList()
    {
        return _chimeraSaveData;
    }

    public int GetEssenceData()
    {
        return _globalSaveData.currentEssence;
    }

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
        List<ChimeraData> chimeraList = new List<ChimeraData> { };
        Dictionary<HabitatType, List<ChimeraData>> chimerasByHabitat = ServiceLocator.Get<HabitatManager>().GetChimerasDictionary();
        foreach (KeyValuePair<HabitatType, List<ChimeraData>> kvp in chimerasByHabitat)
        {
            chimeraList.AddRange(kvp.Value);
        }
        return chimeraList;
    }

    public void OnApplicationQuit()
    {
        SaveChimeras();
        _essenceManager.SaveEssence();
    }
}
