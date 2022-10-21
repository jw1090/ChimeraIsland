using System.Collections.Generic;
using UnityEngine;

public class EvolutionLogic : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private string _chimeraName = "";
    [SerializeField] private ElementType _elementType = ElementType.None;
    [SerializeField] private StatType _statBonus = StatType.None;
    [SerializeField][TextArea(3, 10)] private string _backgroundInfo = "";

    [Header("Evolution")]
    [SerializeField] private ChimeraType _evolutionType = ChimeraType.None;
    [SerializeField] private int _reqExploration = 0;
    [SerializeField] private int _reqStamina = 0;
    [SerializeField] private int _reqWisdom = 0;
    [SerializeField] private List<EvolutionLogic> _evolutionPaths = null;

    [Header("AI")]
    [SerializeField] private float _speed = 4.5f;

    [Header("Particles")]
    [SerializeField] private List<ParticleSystem> _idleParticles = null;
    [SerializeField] private List<ParticleSystem> _patrolParticles = null;

    private Animator _animator = null;
    private ResourceManager _resourceManager = null;
    private Chimera _chimeraBrain = null;
    private Sprite _chimeraIcon = null;

    public ChimeraType ChimeraType { get => _evolutionType; }
    public ElementType ElementType { get => _elementType; }
    public StatType StatBonus { get => _statBonus; }
    public Animator Animator { get => _animator; }
    public Chimera ChimeraBrain { get => _chimeraBrain; }
    public Sprite ChimeraIcon { get => _chimeraIcon; }
    public List<EvolutionLogic> PossibleEvolutions { get => _evolutionPaths; }
    public int ReqExploration { get => _reqExploration; }
    public int ReqStamina { get => _reqStamina; }
    public int ReqWisdom { get => _reqWisdom; }
    public string Name { get => _chimeraName; }
    public string BackgroundInfo { get => _backgroundInfo; }

    public void GetPreferredStat(ChimeraType chimeraType, out int explorationAmount, out int staminaAmount, out int wisdomAmount)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                explorationAmount = 4;
                staminaAmount = 3;
                wisdomAmount = 2;
                break;
            case ChimeraType.B:
                explorationAmount = 2;
                staminaAmount = 4;
                wisdomAmount = 3;
                break;
            case ChimeraType.C:
                explorationAmount = 3;
                staminaAmount = 4;
                wisdomAmount = 2;
                break;
            default:
                explorationAmount = 0;
                staminaAmount = 0;
                wisdomAmount = 0;
                break;
        }
    }

    public void Initialize(Chimera chimera)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _animator = GetComponent<Animator>();

        _chimeraIcon = _resourceManager.GetChimeraSprite(_evolutionType);

        _chimeraBrain = chimera;
        _chimeraBrain.Behavior.SetAgentSpeed(_speed);

        ToggleIdleParticles(false);
        TogglePatrolParticles(false);
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

    public void ToggleIdleParticles(bool toggle)
    {
        if (toggle == true &&_idleParticles != _patrolParticles)
        {
            ToggleParticles(false, _patrolParticles);
        }

        ToggleParticles(toggle, _idleParticles);
    }

    public void TogglePatrolParticles(bool toggle)
    {
        if (toggle == true && _idleParticles != _patrolParticles)
        {
            ToggleParticles(false, _idleParticles);
        }

        ToggleParticles(toggle, _patrolParticles);
    }

    private void ToggleParticles(bool toggle, List<ParticleSystem> particles)
    {
        if (particles.Count == 0)
        {
            return;
        }

        foreach (var particle in particles)
        {
            if (toggle == true)
            {
                if (particle.isPlaying == true)
                {
                    continue;
                }

                particle.Play();
            }
            else
            {
                particle.Stop();
            }
        }
    }
}