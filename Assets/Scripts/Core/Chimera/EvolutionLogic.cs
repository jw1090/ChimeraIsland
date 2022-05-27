using System.Collections.Generic;
using UnityEngine;

public class EvolutionLogic : MonoBehaviour
{
    [Header("Evolution Info")]
    [SerializeField] private ChimeraType _evolutionType;
    [SerializeField] private Sprite _icon = null;
    [SerializeField] private List<EvolutionLogic> _evolutionPaths;
    [SerializeField] private int _reqEndurance = 0;
    [SerializeField] private int _reqIntelligence = 0;
    [SerializeField] private int _reqStrength = 0;

    public ChimeraType Type { get => _evolutionType; }
    public Sprite Icon { get => _icon; }
    public int ReqEndurance { get => _reqEndurance; }
    public int ReqIntelligence { get => _reqIntelligence; }
    public int ReqStrength { get => _reqStrength; }

    public bool CheckEvolution(int endurance, int intelligence, int strength, out EvolutionLogic newEvolution)
    {
        newEvolution = null;

        if (_evolutionPaths == null)
        {
            return false;
        }

        foreach(var possibleEvolution in _evolutionPaths)
        {
            if (endurance < possibleEvolution.ReqEndurance)
            {
                continue;
            }
            else if (intelligence < possibleEvolution.ReqIntelligence)
            {
                continue;
            }
            if(strength < possibleEvolution.ReqStrength)
            {
                continue;
            }

            newEvolution = possibleEvolution;
            return true;
        }

        return false;
    }
}