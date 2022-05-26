using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private readonly Dictionary<HabitatType, List<Chimera>> _chimerasByHabitat = new Dictionary<HabitatType, List<Chimera>>();
    [SerializeField] private List<HabitatData> _displayDictionary = new List<HabitatData>();
    private PersistentData _persistentData = null;
    private List<ChimeraSaveData> _chimeraSaveData = null;

    [Serializable]
    public class HabitatData
    {
        public HabitatType key = HabitatType.None;
        public List<string> value = new List<string>();
        public HabitatData(HabitatType Key, List<string> Value)
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
        if (InitializeChimeraData())
        {
            HabitatDataDisplayInit();
        }

        return this;
    }

    public void HabitatDataDisplayInit()
    {
        foreach (var entry in _chimerasByHabitat)
        {
            var names = entry.Value.Select(c => c.name).ToList();
            _displayDictionary.Add(new HabitatData(entry.Key, names));
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
         _chimeraSaveData = _persistentData.GetChimeraList();

        if (_chimeraSaveData == null)
        {
            Debug.LogError("Save data is null!");
            return false;
        }
        return true;
    }
}