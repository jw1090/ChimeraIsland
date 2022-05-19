using System;
using System.Collections.Generic;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private readonly Dictionary<HabitatType, List<Chimera>> _chimerasByHabitat = new Dictionary<HabitatType,List<Chimera>>();
    [SerializeField] private List<HabitatData> _displayDictionary = new List<HabitatData>();
    private ResourceManager _resourceManager = null;
    private PersistentData _persistentData = null;

    [Serializable]
    public class HabitatData
    {
        public HabitatType key = HabitatType.None;
        public List<Chimera> value = new List<Chimera>();
    }
    public Dictionary<HabitatType, List<Chimera>> GetChimerasDictionary() { return _chimerasByHabitat; }
    public HabitatManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();

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

        _persistentData.LoadChimerasToDictionary(_chimerasByHabitat);
        return true;
    }

}