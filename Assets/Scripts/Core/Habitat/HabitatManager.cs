using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private List<HabitatInfo> _displayDictionary = new List<HabitatInfo>();
    [SerializeField] private int _chimeraCapacity = 3;
    private readonly Dictionary<HabitatType, List<ChimeraData>> _chimerasByHabitat = new Dictionary<HabitatType, List<ChimeraData>>();
    private readonly List<HabitatData> _habitatList = new List<HabitatData>();
    private PersistentData _persistentData = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private List<HabitatData> _habitatSaveData = null;
    private Habitat _currentHabitat = null;

    public Habitat CurrentHabitat { get => _currentHabitat; }
    public int ChimeraCapacity { get => _chimeraCapacity; }
    public Dictionary<HabitatType, List<ChimeraData>> ChimerasDictionary { get => _chimerasByHabitat; }
    public List<HabitatData> HabitatList { get => _habitatList; }

    private List<ChimeraData> GetChimerasForHabitat(HabitatType habitatType)
    {
        if (_chimerasByHabitat.ContainsKey(habitatType))
        {
            return _chimerasByHabitat[habitatType];
        }

        Debug.Log($"No entry for habitat: {habitatType}");
        return new List<ChimeraData>();
    }
    private FacilityData[] GetFacilityData(HabitatType habitatType)
    {
        return _habitatList[(int)habitatType].facilities;
    }
    public void SetCurrentHabitat(Habitat habitat) { _currentHabitat = habitat; }

    [Serializable]
    public class HabitatInfo
    {
        public HabitatType key = HabitatType.None;
        public List<string> value = new List<string>();
        public HabitatInfo(HabitatType Key, List<string> Value)
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
        if(InitializeHabitatData())
        {
            StoreHabitatDataByHabitat();
        }
        return this;
    }

    private bool InitializeChimeraData()
    {
        // Get your data from the save system
        _chimeraSaveData = _persistentData.ChimeraData;

        if (_chimeraSaveData == null)
        {
            Debug.LogError("Chimera Save data is null!");
            return false;
        }

        return true;
    }
    private bool InitializeHabitatData()
    {
        for(int i = 1; i < Enum.GetValues(typeof(HabitatType)).Length; i++)
        {
            _habitatList.Add(new HabitatData());
        }
        //Initialize _habitatList
        foreach (HabitatType habitatType in (HabitatType[])Enum.GetValues(typeof(HabitatType)))
        {
            if (habitatType != HabitatType.None)
            {
                _habitatList[(int)habitatType].type = habitatType;
                FacilityData temp = new FacilityData();
                _habitatList[(int)habitatType].facilities = new FacilityData[]{temp,temp,temp};
            }
        }

        // Get your data from the save system
        _habitatSaveData = _persistentData.HabitatData;

        if (_habitatSaveData == null)
        {
            Debug.LogError("Habitat Save data is null!");
            return false;
        }
        return true;
    }
    private void StoreHabitatDataByHabitat()
    {
        foreach (var habitat in _habitatSaveData)
        {
            if (_chimerasByHabitat.ContainsKey(habitat.type) == false)
            {
                _chimerasByHabitat.Add(habitat.type, new List<ChimeraData>());
            }
            _habitatList[(int)habitat.type] = habitat;
        }
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
            _displayDictionary.Add(new HabitatInfo(entry.Key, names));
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

    public void UpdateCurrentHabitatFacilities(Facility facility)
    {
        int habitatType = (int)_currentHabitat.Type;
        int facilityType = (int)facility.GetFacilityType();

        _habitatSaveData[habitatType].facilities[facilityType].currentTier = facility.CurrentTier;
    }

    public void SpawnChimerasForHabitat()
    {
        var chimerasToSpawn = GetChimerasForHabitat(_currentHabitat.Type);
        _currentHabitat.SpawnChimeras(chimerasToSpawn);
    }

    public void SpawnFacilitiesForHabitat()
    {
        FacilityData[] facilitiesInfo = GetFacilityData(_currentHabitat.Type);
        _currentHabitat.SpawnFacilities(facilitiesInfo);
    }
}