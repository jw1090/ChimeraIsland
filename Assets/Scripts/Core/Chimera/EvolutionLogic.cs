using System.Collections.Generic;
using UnityEngine;

public class EvolutionLogic : MonoBehaviour
{
    [SerializeField] private List<EvolutionLogic> _evolutionPaths = null;
    [SerializeField] private ChimeraType _evolutionType = ChimeraType.None;
    [SerializeField] private int _reqEndurance = 0;
    [SerializeField] private int _reqIntelligence = 0;
    [SerializeField] private int _reqStrength = 0;
    private Chimera _chimeraBrain = null;
    private Sprite _icon = null;

    public ChimeraType Type { get => _evolutionType; }
    public Chimera ChimeraBrain { get => _chimeraBrain; }
    public Sprite Icon { get => _icon; }
    public int ReqEndurance { get => _reqEndurance; }
    public int ReqIntelligence { get => _reqIntelligence; }
    public int ReqStrength { get => _reqStrength; }

    public void Initialize(Chimera chimera)
    {
        _icon = ServiceLocator.Get<ResourceManager>().GetChimeraSprite(_evolutionType);

        _chimeraBrain = chimera;
    }

    public bool CheckEvolution(int endurance, int intelligence, int strength, out EvolutionLogic newEvolution)
    {
        newEvolution = null;

        if (_evolutionPaths == null)
        {
            return false;
        }

        foreach (var possibleEvolution in _evolutionPaths)
        {
            if (endurance < possibleEvolution.ReqEndurance)
            {
                continue;
            }
            else if (intelligence < possibleEvolution.ReqIntelligence)
            {
                continue;
            }
            if (strength < possibleEvolution.ReqStrength)
            {
                continue;
            }

            newEvolution = possibleEvolution;
            return true;
        }

        return false;
    }
}