using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private List<HabitatData> _displayDictionary = new List<HabitatData>();
    [SerializeField] private int _chimeraCapacity = 3;
    private readonly Dictionary<HabitatType, List<ChimeraData>> _chimerasByHabitat = new Dictionary<HabitatType, List<ChimeraData>>();
    private PersistentData _persistentData = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private Habitat _currentHabitat = null;

    public Habitat CurrentHabitat { get => _currentHabitat; }
    public int ChimeraCapacity { get => _chimeraCapacity; }
    public Dictionary<HabitatType, List<ChimeraData>> ChimerasDictionary { get => _chimerasByHabitat; }

    private List<ChimeraData> GetChimerasForHabitat(HabitatType habitatType)
    {
        if (_chimerasByHabitat.ContainsKey(habitatType))
        {
            return _chimerasByHabitat[habitatType];
        }

        Debug.Log($"No entry for habitat: {habitatType}");
        return new List<ChimeraData>();
    }

    public void SetCurrentHabitat(Habitat habitat) { _currentHabitat = habitat; }

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

    public HabitatManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();

        if (InitializeChimeraData())
        {
            StoreChimeraDataByHabitat();
            HabitatDataDisplayInit();
        }

        return this;
    }

    private bool InitializeChimeraData()
    {
        // Get your data from the save system
        _chimeraSaveData = _persistentData.ChimeraData;

        if (_chimeraSaveData == null)
        {
            Debug.LogError("Save data is null!");
            return false;
        }

        return true;
    }

    private void StoreChimeraDataByHabitat()
    {
        foreach (var chimera in _chimeraSaveData)
        {
            AddChimeraToHabitat(chimera, chimera.habitatType);
        }
    }

    public void HabitatDataDisplayInit()
    {
        foreach (var entry in _chimerasByHabitat)
        {
            var names = entry.Value.Select(chimera => chimera.chimeraType.ToString()).ToList();
            _displayDictionary.Add(new HabitatData(entry.Key, names));
        }
    }

    public void AddNewChimera(Chimera chimeraToSave)
    {
        ChimeraData chimeraSavedData = new ChimeraData(chimeraToSave);
        AddChimeraToHabitat(chimeraSavedData, chimeraSavedData.habitatType);
    }

    private void AddChimeraToHabitat(ChimeraData chimeraToAdd, HabitatType habitat)
    {
        if (_chimerasByHabitat.ContainsKey(habitat) == false)
        {
            _chimerasByHabitat.Add(habitat, new List<ChimeraData>());
        }

        _chimerasByHabitat[habitat].Add(chimeraToAdd);
    }

    public void UpdateCurrentHabitatChimeras()
    {
        if (_chimerasByHabitat.ContainsKey(_currentHabitat.Type) == false)
        {
            Debug.LogError("Cannot update chimeras. Habitat key not found");
            return;
        }

        _chimerasByHabitat.Remove(_currentHabitat.Type);

        foreach (Chimera chimera in _currentHabitat.ActiveChimeras)
        {
            AddNewChimera(chimera);
        }
    }

    public void SpawnChimerasForHabitat()
    {
        var chimerasToSpawn = GetChimerasForHabitat(_currentHabitat.Type);
        _currentHabitat.SpawnChimeras(chimerasToSpawn);
    }
}