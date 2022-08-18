using System.Collections.Generic;
using UnityEngine;

public class EvolutionLogic : MonoBehaviour
{
    [SerializeField] private List<EvolutionLogic> _evolutionPaths = null;
    [SerializeField] private ChimeraType _evolutionType = ChimeraType.None;
    [SerializeField] private string _chimeraName = "";
    [SerializeField] private StatType _statBonus = StatType.None;
    [SerializeField] private int _reqExploration = 0;
    [SerializeField] private int _reqStamina = 0;
    [SerializeField] private int _reqWisdom = 0;
    [SerializeField] private float _speed = 4.5f;
    private ResourceManager _resourceManager = null;
    private Chimera _chimeraBrain = null;
    private Sprite _chimeraIcon = null;

    public ChimeraType Type { get => _evolutionType; }
    public Animator Animator { get => GetComponent<Animator>(); }
    public Chimera ChimeraBrain { get => _chimeraBrain; }
    public Sprite ChimeraIcon { get => _chimeraIcon; }
    public StatType StatBonus { get => _statBonus; }
    public int ReqExploration { get => _reqExploration; }
    public int ReqStamina { get => _reqStamina; }
    public int ReqWisdom { get => _reqWisdom; }
    public string Name { get => _chimeraName; }

    public void Initialize(Chimera chimera)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();

        _chimeraIcon = _resourceManager.GetChimeraSprite(_evolutionType);

        _chimeraBrain = chimera;
        _chimeraBrain.Behavior.SetAgentSpeed(_speed);
    }

    public bool CheckEvolution(int exploration, int staminan, int wisdom, out EvolutionLogic newEvolution)
    {
        newEvolution = null;

        if (_evolutionPaths == null)
        {
            return false;
        }

        foreach (var possibleEvolution in _evolutionPaths)
        {
            if (staminan < possibleEvolution.ReqStamina)
            {
                continue;
            }
            else if (wisdom < possibleEvolution.ReqWisdom)
            {
                continue;
            }
            if (exploration < possibleEvolution.ReqExploration)
            {
                continue;
            }

            newEvolution = possibleEvolution;
            return true;
        }

        return false;
    }
}