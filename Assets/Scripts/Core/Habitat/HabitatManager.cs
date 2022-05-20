using System;
using System.Collections.Generic;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private readonly Dictionary<HabitatType, List<Chimera>> _chimerasByHabitat = new Dictionary<HabitatType, List<Chimera>>();
    [SerializeField] private List<HabitatData> _displayDictionary = new List<HabitatData>();
    private PersistentData _persistentData = null;

    [Serializable]
    public class HabitatData
    {
        public HabitatType key = HabitatType.None;
        public List<Chimera> value = new List<Chimera>();
        public HabitatData(HabitatType Key, List<Chimera> Value)
        {
            key = Key;
            value = Value;
        }
    }

    public Dictionary<HabitatType, List<Chimera>> GetChimerasDictionary() { return _chimerasByHabitat; }

    public HabitatManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();

        HabitatDataDisplayInit();
        InitializeChimeraData();

        return this;
    }

    public void HabitatDataDisplayInit()
    {
        foreach (var entry in _chimerasByHabitat)
        {
            _displayDictionary.Add(new HabitatData(entry.Key, entry.Value));
        }
    }

    public void AddChimeraToHabitat(Chimera chimeraToAdd, HabitatType habitat)
    {
        if (_chimerasByHabitat.ContainsKey(habitat) == false)
        {
            _chimerasByHabitat.Add(habitat, new List<Chimera>());
        }
        _chimerasByHabitat[habitat].Add(chimeraToAdd);
    }

    public void RemoveChimeraFromHabitat(Chimera chimeraToRemove, HabitatType habitat)
    {
        if (!_chimerasByHabitat.ContainsKey(habitat))
        {
            Debug.LogError("Cannot remove chimera. Habitat key not found");
            return;
        }

        var chimeras = _chimerasByHabitat[habitat];
        if (chimeras.Remove(chimeraToRemove))
        {
            Debug.Log($"Successfully removed chimera {chimeraToRemove.name} from habitat {habitat}");
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
        foreach (KeyValuePair<HabitatType, List<Chimera>> kvp in _chimerasByHabitat)
        {
            _displayDictionary.Add(new HabitatData(kvp.Key, kvp.Value));
        }
        return true;
    }
}