using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private List<HabitatInfo> _displayDictionary = new List<HabitatInfo>();
    [SerializeField] private int _chimeraCapacity = 5;
    [SerializeField]private float _tickTimer = 0.4f;
    private readonly Dictionary<HabitatType, List<ChimeraData>> _chimerasByHabitat = new Dictionary<HabitatType, List<ChimeraData>>();
    private readonly Dictionary<HabitatType, List<FacilityData>> _facilitiesByHabitat = new Dictionary<HabitatType, List<FacilityData>>();
    private List<HabitatData> _habitatData = new List<HabitatData>();
    private AudioManager _audioManager = null;
    private PersistentData _persistentData = null;
    private Habitat _currentHabitat = null;
    private List<ChimeraData> _chimeraSaveData = null;
    private List<FacilityData> _facilitySaveData = null;

    public Dictionary<HabitatType, List<ChimeraData>> ChimerasDictionary { get => _chimerasByHabitat; }
    public Dictionary<HabitatType, List<FacilityData>> FacilityDictionary { get => _facilitiesByHabitat; }
    public Habitat CurrentHabitat { get => _currentHabitat; }
    public int ChimeraCapacity { get => _chimeraCapacity; }
    public float TickTimer { get => _tickTimer; }
    public List<HabitatData> HabitatDataList { get => _habitatData; }

    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }

    private List<ChimeraData> GetChimerasForHabitat(HabitatType habitatType)
    {
        if (_chimerasByHabitat.ContainsKey(habitatType))
        {
            return _chimerasByHabitat[habitatType];
        }

        Debug.Log($"No Chimera entry for {habitatType}.");
        return new List<ChimeraData>();
    }

    private List<FacilityData> GetFaclitiesForHabitat(HabitatType habitatType)
    {
        if (_facilitiesByHabitat.ContainsKey(habitatType))
        {
            return _facilitiesByHabitat[habitatType];
        }

        Debug.Log($"No Facility entry for {habitatType}.");
        return new List<FacilityData>();
    }

    public void SetCurrentHabitat(Habitat habitat) 
    { 
        _currentHabitat = habitat;
    }

    public void SetExpeditionProgress(int essence, int fossil, int habitat)
    {
        int type = (int)CurrentHabitat.Type;
        while (type + 1 > _habitatData.Count()) _habitatData.Add(new HabitatData());
        _habitatData[type]._expeditionEssenceProgress = essence;
        _habitatData[type]._expeditionHabitatProgress = habitat;
        _habitatData[type]._expeditionFossilProgress = fossil;
    }
    public void SetHabitatUIProgress(bool a, bool b, bool c, bool cave, bool rune, bool waterfall)
    {
        int type = (int)CurrentHabitat.Type;
        while (type + 1 > _habitatData.Count()) _habitatData.Add(new HabitatData());
        _habitatData[type]._aUnlocked = a;
        _habitatData[type]._bUnlocked = b;
        _habitatData[type]._cUnlocked = c;
        _habitatData[type]._caveUnlocked = cave;
        _habitatData[type]._runeUnlocked = rune;
        _habitatData[type]._waterfallUnlocked = waterfall;
    }
    public void SetHabitatTier(int num, HabitatType type) 
    {
        while ((int)type + 1 > _habitatData.Count()) _habitatData.Add(new HabitatData());
        _habitatData[(int)type]._currentTier = num; 
    }

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

        LoadHabitatData();

        return this;
    }

    public void LoadHabitatData()
    {
        if (InitializeChimeraData())
        {
            StoreChimeraDataByHabitat();
            ChimeraDataDisplayInit();
        }

        if (InitializeFacilityData())
        {
            StoreFacilityDataByHabitat();
        }

        InitializeHabitatData();
    }

    public void ResetDictionaries()
    {
        _chimerasByHabitat.Clear();
        _facilitiesByHabitat.Clear();
    }

    private bool InitializeChimeraData()
    {
        // Get your data from the save system
        _chimeraSaveData = _persistentData.ChimeraData;

        if (_chimeraSaveData == null)
        {
            Debug.LogError("Chimera save data is null!");
            return false;
        }

        return true;
    }

    private bool InitializeFacilityData()
    {
        // Get your data from the save system
        _facilitySaveData = _persistentData.FacilityData;

        if (_facilitySaveData == null)
        {
            Debug.LogError("Facility save data is null!");
            return false;
        }

        return true;
    }

    private bool InitializeHabitatData()
    {
        // Get your data from the save system
        _habitatData = _persistentData.HabitatData;

        if (_habitatData == null)
        {
            Debug.LogError("Facility save data is null!");
            return false;
        }
        
        return true;
    }

    public void ChimeraDataDisplayInit()
    {
        foreach (var entry in _chimerasByHabitat)
        {
            var names = entry.Value.Select(chimera => chimera.chimeraType.ToString()).ToList();
            _displayDictionary.Add(new HabitatInfo(entry.Key, names));
        }
    }

    private void StoreChimeraDataByHabitat()
    {
        foreach (var chimera in _chimeraSaveData)
        {
            AddChimeraToHabitat(chimera, chimera.habitatType);
        }
    }

    private void StoreFacilityDataByHabitat()
    {
        foreach (var faciliy in _facilitySaveData)
        {
            AddFacilityToHabitat(faciliy, faciliy.habitatType);
        }
    }

    private bool AddChimeraToHabitat(ChimeraData chimeraToAdd, HabitatType habitat)
    {
        if (_chimerasByHabitat.ContainsKey(habitat) == false)
        {
            _chimerasByHabitat.Add(habitat, new List<ChimeraData>());
        }

        if(HabitatCapacityCheck(habitat) == false)
        {
            Debug.Log($"Cannot add {chimeraToAdd.chimeraType}, {habitat} is full.");
            return false;
        }

        _chimerasByHabitat[habitat].Add(chimeraToAdd);

        return true;
    }

    public bool HabitatCapacityCheck(HabitatType habitat)
    {
        if (_chimerasByHabitat[habitat].Count < _chimeraCapacity)
        {
            return true;
        }

        return false;
    }

    private void AddFacilityToHabitat(FacilityData facilityToAdd, HabitatType habitat)
    {
        if (_facilitiesByHabitat.ContainsKey(habitat) == false)
        {
            _facilitiesByHabitat.Add(habitat, new List<FacilityData>());
        }

        _facilitiesByHabitat[habitat].Add(facilityToAdd);
    }

    public void UpdateCurrentHabitatChimeras()
    {
        if (_chimerasByHabitat.ContainsKey(_currentHabitat.Type) == false)
        {
            Debug.Log($"Cannot update chimeras. Habitat key: {_currentHabitat.Type} not found");
            return;
        }

        _chimerasByHabitat.Remove(_currentHabitat.Type);

        foreach (Chimera chimera in _currentHabitat.ActiveChimeras)
        {
            AddNewChimera(chimera);
        }
    }

    public bool AddNewChimera(Chimera chimeraToSave)
    {
        ChimeraData chimeraSavedData = new ChimeraData(chimeraToSave);
        
        if(AddChimeraToHabitat(chimeraSavedData, chimeraSavedData.habitatType) == true)
        {
            return true;
        }

        return false;
    }

    public void AddNewFacility(Facility facilityToSave)
    {
        FacilityData facilitySavedData = new FacilityData(facilityToSave, _currentHabitat.Type);
        AddFacilityToHabitat(facilitySavedData, facilitySavedData.habitatType);
    }

    public void SpawnChimerasForHabitat()
    {
        var chimerasToSpawn = GetChimerasForHabitat(_currentHabitat.Type);
        _currentHabitat.CreateChimerasFromData(chimerasToSpawn);
    }

    public void PlayCurrentHabitatMusic()
    {
        _audioManager.PlayHabitatMusic(_currentHabitat.Type);
    }

    public void BuildFacilitiesForHabitat()
    {
        var facilitiesToBuild = GetFaclitiesForHabitat(_currentHabitat.Type);
        _currentHabitat.CreateFacilitiesFromData(facilitiesToBuild);
    }
}