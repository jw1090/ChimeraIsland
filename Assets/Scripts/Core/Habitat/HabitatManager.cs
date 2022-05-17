using System;
using System.Collections.Generic;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private readonly Dictionary<HabitatType, List<Chimera>> _chimerasByHabitat = new Dictionary<HabitatType,List<Chimera>>();
    [SerializeField] private List<HabitatData> _displayDictionary = new List<HabitatData>();
    private ResourceManager _resourceManager = null;

    [Serializable]
    public class HabitatData
    {
        public HabitatType key = HabitatType.None;
        public List<Chimera> value = new List<Chimera>();
    }

    public HabitatManager Initialize()
    {
        Debug.Log($"<color=lime> {this.GetType()} Initialized!</color>");

        _resourceManager = ServiceLocator.Get<ResourceManager>();

        HabitatDataDisplayInit();
        InitializeChimeraData();

        return this;
    }

    public void HabitatDataDisplayInit()
    {
        foreach (var entry in _chimerasByHabitat)
        {
            _displayDictionary.Add(new HabitatData()
            {
                key = entry.Key,
                value = entry.Value
            });
        }
    }

    public List<Chimera> GetChimerasForHabitat(HabitatType habitatType)
    {
        if (_chimerasByHabitat.ContainsKey(habitatType))
        {
            return _chimerasByHabitat[habitatType];
        }
        Debug.Log($"No entry for habitat: {habitatType}");
        return new List<Chimera>();
    }

    private bool InitializeChimeraData()
    {
        // Get your data from the save system
        List<ChimeraSaveData> saveData = ServiceLocator.Get<PersistentData>().GetChimeraList();

        if (saveData == null)
        {
            Debug.LogError("Save data is null!");
            return false;
        }

        // Add chimeras to the dictionary
        foreach(var data in saveData)
        {
            if (_chimerasByHabitat.ContainsKey(data.habitatType) == false)
            {
                _chimerasByHabitat.Add(data.habitatType, new List<Chimera>());
            }

            Chimera chimera = LoadChimeraFromJson(data);

            _chimerasByHabitat[data.habitatType].Add(chimera);
        }

        return true;
    }

    private Chimera LoadChimeraFromJson(ChimeraSaveData data)
    {
        GameObject chimeraGO = _resourceManager.GetChimeraBasePrefab(data.chimeraType);
        Chimera chimera = chimeraGO.GetComponent<Chimera>();
        chimera.SetChimeraType(data.chimeraType);
        chimera.Level = data.level;
        chimera.Endurance = data.endurance;
        chimera.Intelligence = data.intelligence;
        chimera.Strength = data.strength;
        chimera.Happiness = data.happiness;

        return chimera;
    }
}