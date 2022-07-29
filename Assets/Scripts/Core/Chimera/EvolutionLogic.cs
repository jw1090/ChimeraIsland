using System.Collections.Generic;
using UnityEngine;

public class EvolutionLogic : MonoBehaviour
{
    [SerializeField] private List<EvolutionLogic> _evolutionPaths = null;
    [SerializeField] private ChimeraType _evolutionType = ChimeraType.None;
    [SerializeField] private string _chimeraName = "";
    [SerializeField] private int _reqEndurance = 0;
    [SerializeField] private int _reqIntelligence = 0;
    [SerializeField] private int _reqStrength = 0;
    private ResourceManager _resourceManager = null;
    private Chimera _chimeraBrain = null;
    private Sprite _chimeraIcon = null;

    public ChimeraType Type { get => _evolutionType; }
    public Animator Animator { get => GetComponent<Animator>(); }
    public Chimera ChimeraBrain { get => _chimeraBrain; }
    public Sprite ChimeraIcon { get => _chimeraIcon; }
    public int ReqEndurance { get => _reqEndurance; }
    public int ReqIntelligence { get => _reqIntelligence; }
    public int ReqStrength { get => _reqStrength; }
    public string Name { get => _chimeraName; }

    public void Initialize(Chimera chimera)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();

        _chimeraIcon = _resourceManager.GetChimeraSprite(_evolutionType);

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