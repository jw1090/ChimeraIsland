using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        return this;
    }

    public void LoadChimerasToDictionary(Dictionary<HabitatType, List<Chimera>> keyValuePairs)
    {
        List<ChimeraSaveData> jList = FileHandler.ReadListFromJSON<ChimeraSaveData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
        var resourceManager = ServiceLocator.Get<ResourceManager>();
        List<Chimera> PlainsChimeras = new List<Chimera>();
        List<Chimera> IslandChimeras = new List<Chimera>();
        foreach (ChimeraSaveData data in jList)
        {
            GameObject chimeraGO = resourceManager.GetChimeraBasePrefab(data.chimeraType);
            Chimera chimera = chimeraGO.GetComponent<Chimera>();
            chimera.SetChimeraType(data.chimeraType);
            chimera.Level = data.level;
            chimera.Endurance = data.endurance;
            chimera.Intelligence = data.intelligence;
            chimera.Strength = data.strength;
            chimera.Happiness = data.happiness;
            if(data.habitatType.Equals(HabitatType.StonePlains))
            {
                PlainsChimeras.Add(chimera);
            }
            else if (data.habitatType.Equals(HabitatType.TreeOfLife))
            {
                IslandChimeras.Add(chimera);
            }
        }
        keyValuePairs.Add(HabitatType.StonePlains, PlainsChimeras);
        keyValuePairs.Add(HabitatType.TreeOfLife, IslandChimeras);
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
