using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private EssenceManager _essenceManager = null;
    private List<ChimeraSaveData> _chimeraSaveData = null;
    private GlobalSaveData _globalSaveData = null;

    public void SetEssenceManager(EssenceManager essenceManager) { _essenceManager = essenceManager; }

    public List<ChimeraSaveData> GetChimeraList()
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

        _chimeraSaveData = FileHandler.ReadListFromJSON<ChimeraSaveData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
        _globalSaveData = FileHandler.ReadFromJSON<GlobalSaveData>(GameConsts.JsonSaveKeys.GLOBAL_SAVE_DATA_FILE);

        return this;
    }

    public void SaveChimeras()
    {
        List<ChimeraSaveData> myData = ChimerasToJson();
        FileHandler.SaveToJSON(myData, GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
    }

    private List<ChimeraSaveData> ChimerasToJson()
    {
        List<ChimeraSaveData> chimeraList = new List<ChimeraSaveData> { };
        Dictionary<HabitatType, List<Chimera>> chimerasByHabitat = ServiceLocator.Get<HabitatManager>().GetChimerasDictionary();
        foreach (KeyValuePair<HabitatType, List<Chimera>> kvp in chimerasByHabitat)
        {
            foreach (var chimera in kvp.Value)
            {
                ChimeraSaveData temp = new ChimeraSaveData
                (
                    chimera.GetInstanceID(),
                    chimera.ChimeraType,
                    chimera.Level,
                    chimera.Endurance,
                    chimera.Intelligence,
                    chimera.Strength,
                    chimera.Happiness,
                    kvp.Key
                );

                chimeraList.Add(temp);
            }
        }
        return chimeraList;
    }

    public void OnApplicationQuit()
    {
        SaveChimeras();
        _essenceManager.SaveEssence();
    }
}
