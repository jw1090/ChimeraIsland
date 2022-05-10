using System;
using System.Collections.Generic;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private Chimera _chimeraBrainA = null;
    [SerializeField] private Chimera _chimeraBrainB = null;
    [SerializeField] private Chimera _chimeraBrainC = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabA = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabA1 = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabA2 = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabA3 = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabB = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabB1 = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabB2 = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabB3 = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabC = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabC1 = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabC2 = null;
    [SerializeField] private EvolutionLogic _chimeraPrefabC3 = null;
    [SerializeField] private readonly Dictionary<HabitatType, List<Chimera>> _chimerasByHabitat = new Dictionary<HabitatType,List<Chimera>>();
    [SerializeField] private List<HabitatData> displayDictionary = new List<HabitatData>();

    [Serializable]
    public class HabitatData
    {
        public HabitatType key = HabitatType.None;
        public List<Chimera> value = new List<Chimera>();
    }

    public HabitatManager Initialize()
    {
        _chimeraBrainA = Resources.Load<Chimera>("Chimera/ChimeraPrefabA");
        _chimeraBrainB = Resources.Load<Chimera>("Chimera/ChimeraPrefabB");
        _chimeraBrainC = Resources.Load<Chimera>("Chimera/ChimeraPrefabC");
        _chimeraPrefabA = Resources.Load<EvolutionLogic>("Chimera/Models/A Model");
        _chimeraPrefabA1 = Resources.Load<EvolutionLogic>("Chimera/Models/A1 Model");
        _chimeraPrefabA2 = Resources.Load<EvolutionLogic>("Chimera/Models/A2 Model");
        _chimeraPrefabA3 = Resources.Load<EvolutionLogic>("Chimera/Models/A3 Model");
        _chimeraPrefabB = Resources.Load<EvolutionLogic>("Chimera/Models/B Model");
        _chimeraPrefabB1 = Resources.Load<EvolutionLogic>("Chimera/Models/B1 Model");
        _chimeraPrefabB2 = Resources.Load<EvolutionLogic>("Chimera/Models/B2 Model");
        _chimeraPrefabB3 = Resources.Load<EvolutionLogic>("Chimera/Models/B3 Model");
        _chimeraPrefabC = Resources.Load<EvolutionLogic>("Chimera/Models/C Model");
        _chimeraPrefabC1 = Resources.Load<EvolutionLogic>("Chimera/Models/C1 Model");
        _chimeraPrefabC2 = Resources.Load<EvolutionLogic>("Chimera/Models/C2 Model");
        _chimeraPrefabC3 = Resources.Load<EvolutionLogic>("Chimera/Models/C3 Model");
        Debug.Log($"<color=lime> {this.GetType()} Initialized!</color>");

        HabitatDataDisplayInit();
        InitializeChimeraData();

        return this;
    }

    private EvolutionLogic GetEvolutionLogic(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
                return _chimeraPrefabA;
            case ChimeraType.A1:
                return _chimeraPrefabA1;
            case ChimeraType.A2:
                return _chimeraPrefabA2;
            case ChimeraType.A3:
                return _chimeraPrefabA3;
            case ChimeraType.B:
                return _chimeraPrefabB;
            case ChimeraType.B1:
                return _chimeraPrefabB1;
            case ChimeraType.B2:
                return _chimeraPrefabB2;
            case ChimeraType.B3:
                return _chimeraPrefabB3;
            case ChimeraType.C:
                return _chimeraPrefabC;
            case ChimeraType.C1:
                return _chimeraPrefabC1;
            case ChimeraType.C2:
                return _chimeraPrefabC2;
            case ChimeraType.C3:
                return _chimeraPrefabC3;
            default:
                Debug.LogWarning($"Unhandled prefab type {type}");
                return null;
        }
    }

    private Chimera GetChimeraBrain(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
            case ChimeraType.A1:
            case ChimeraType.A2:
            case ChimeraType.A3:
                return _chimeraBrainA;
            case ChimeraType.B:
            case ChimeraType.B1:
            case ChimeraType.B2:
            case ChimeraType.B3:
                return _chimeraBrainB;
            case ChimeraType.C:
            case ChimeraType.C1:
            case ChimeraType.C2:
            case ChimeraType.C3:
                return _chimeraBrainC;
            default:
                Debug.LogWarning($"Unhandled prefab type {type}");
                return null;
        }
    }

    public void HabitatDataDisplayInit()
    {
        foreach (var entry in _chimerasByHabitat)
        {
            displayDictionary.Add(new HabitatData()
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
        Chimera chimera = GetChimeraBrain(data.chimeraType);
        EvolutionLogic a = GetEvolutionLogic(data.chimeraType);
        chimera.SetModel(a);
        chimera.SetChimeraType(data.chimeraType);
        chimera.Level = data.level;
        chimera.Endurance = data.endurance;
        chimera.Intelligence = data.intelligence;
        chimera.Strength = data.strength;
        chimera.Happiness = data.happiness;

        return chimera;
    }
}