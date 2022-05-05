using System.Collections.Generic;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private List<int> _essenceGainPerHabitat = new List<int>();
    [SerializeField] private Dictionary<HabitatType, List<Chimera>> _chimerasByHabitat = new Dictionary<HabitatType,List<Chimera>>();
    public HabitatManager Initialize()
    {
        InitializeChimeraData();
        List<Chimera> chimeras = GetChimerasForHabitat(HabitatType.StonePlains);
        return this;
    }

    public List<Chimera> GetChimerasForHabitat(HabitatType habitatType)
    {
        if (_chimerasByHabitat.ContainsKey(habitatType))
        {
            return _chimerasByHabitat[habitatType];
        }
        Debug.LogError($"No entry for habitat: {habitatType}");
        return null;
    }

    private void InitializeChimeraData()
    {
        // Get your data from the save system
        List<ChimeraSaveData> saveData = new List<ChimeraSaveData>();

        // Add chimeras to the dictionary
        foreach(var data in saveData)
        {
            if (_chimerasByHabitat.ContainsKey(data.habitatType) == false)
            {
                _chimerasByHabitat.Add(data.habitatType, new List<Chimera>());
            }

            Chimera chimera = AllocateStats(data);

            _chimerasByHabitat[data.habitatType].Add(chimera);
        }
    }

    private Chimera AllocateStats(ChimeraSaveData data)
    {
        Chimera chimera = new Chimera();

        chimera.SetChimeraType(data.chimeraType);
        chimera.Level = data.level;
        chimera.Endurance = data.endurance;
        chimera.Intelligence = data.intelligence;
        chimera.Strength = data.strength;
        chimera.Happiness = data.happiness;

        return chimera;
    }

}