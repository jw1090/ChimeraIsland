using UnityEngine;
using System.Collections.Generic;

public class PersistentData : MonoBehaviour
{
    private EssenceManager _essenceManager = null;

    public void SetEssenceManager(EssenceManager essenceManager) { _essenceManager = essenceManager; }

    public List<ChimeraSaveData> GetChimeraList()
    {
        return FileHandler.ReadListFromJSON<ChimeraSaveData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
    }

    public PersistentData Initialize()
    {
        Debug.Log($"<color=Orange> {this.GetType()} Initialized!</color>");

        return this;
    }

    public bool LoadSavedData()
    {
        List<ChimeraSaveData> jList = FileHandler.ReadListFromJSON<ChimeraSaveData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);

        if (jList == null || jList.Count == 0)
        {
            Debug.Log("Chimera Save not found");
            return false;
        }

        return true;
    }

    public void LoadChimerasToDictionary(Dictionary<HabitatType, List<Chimera>> keyValuePairs)
    {
        // TODO: Get JSON data and add it to dictionary
    }

    public void SaveChimeras()
    {
        SaveJsonList myData = ChimerasToJson();
        FileHandler.SaveToJSON(myData.GetChimeraList(), GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
    }

    private SaveJsonList ChimerasToJson()
    {
        // TODO: Grab chimeras from each habitat key in dictionary and turn them into JSON
        return null;
    }

    public void OnApplicationQuit()
    {
        SaveChimeras();
        _essenceManager.SaveEssence();
    }
}
