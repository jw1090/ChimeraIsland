using System.Collections.Generic;
using UnityEngine;

public class EvolutionLogic : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private string _chimeraName = "";
    [SerializeField] private ChimeraType _evolutionType = ChimeraType.None;
    [SerializeField] private ElementType _elementType = ElementType.None;
    [SerializeField][TextArea(3, 10)] private string _backgroundInfo = "";

    [Header("Stat Preferences")]
    [SerializeField] private StatPreferenceType _explorationPreference = StatPreferenceType.Neutral;
    [SerializeField] private StatPreferenceType _staminaPreference = StatPreferenceType.Neutral;
    [SerializeField] private StatPreferenceType _wisdomPreference = StatPreferenceType.Neutral;

    [Header("Evolution")]
    [SerializeField] private StatType _evolutionStat = StatType.None;
    [SerializeField] private List<EvolutionLogic> _evolutionPaths = null;
    private int _evolutionStatThreshold = 7;

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
    public StatType EvolutionStat { get => _evolutionStat; }
    public StatPreferenceType ExplorationPreference { get => _explorationPreference; }
    public StatPreferenceType StaminaPreference { get => _staminaPreference; }
    public StatPreferenceType WisdomPreference { get => _wisdomPreference; }
    public Animator Animator { get => _animator; }
    public Chimera ChimeraBrain { get => _chimeraBrain; }
    public Sprite ChimeraIcon { get => _chimeraIcon; }
    public List<EvolutionLogic> PossibleEvolutions { get => _evolutionPaths; }
    public string Name { get => _chimeraName; }
    public string BackgroundInfo { get => _backgroundInfo; }
    public int EvolutionStatThreshold { get => _evolutionStatThreshold; }

    public float GetPreferenceModifier(StatType trainingStat)
    {
        float amount = 0.0f;

        switch (trainingStat)
        {
            case StatType.Exploration:
                amount = DeterminePreferenceBonusAmount(_explorationPreference);
                break;
            case StatType.Stamina:
                amount = DeterminePreferenceBonusAmount(_staminaPreference);
                break;
            case StatType.Wisdom:
                amount = DeterminePreferenceBonusAmount(_wisdomPreference);
                break;
            default:
                Debug.LogError($"Invalid Training Stat Type: {trainingStat}");
                break;
        }

        return amount;
    }

    private float DeterminePreferenceBonusAmount(StatPreferenceType statPreferenceType)
    {
        float amount = 0.0f;

        switch (statPreferenceType)
        {
            case StatPreferenceType.Dislike:
                amount = -10.0f;
                break;
            case StatPreferenceType.Neutral:
                break;
            case StatPreferenceType.Like:
                amount = 20.0f;
                break;
            default:
                Debug.LogError($"Invalid Preference Stat Type: {statPreferenceType}");
                break;
        }

        return amount;
    }

    public float GetEvolutionBonusAmount(StatType trainingStat)
    {
        float amount = 0.0f;

        if (trainingStat == _evolutionStat)
        {
            amount = 50.0f;
        }

        return amount;
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

    public bool CheckEvolution(int exploration, int stamina, int wisdom, out EvolutionLogic newEvolution)
    {
        newEvolution = null;

        if (_evolutionPaths == null)
        {
            return false;
        }

        foreach (var possibleEvolution in _evolutionPaths)
        {
            switch (possibleEvolution.EvolutionStat)
            {
                case StatType.None: // No way to evolve into this
                    break;
                case StatType.Exploration:
                    if (exploration >= possibleEvolution.EvolutionStatThreshold)
                    {
                        newEvolution = possibleEvolution;
                        return true;
                    }
                    break;
                case StatType.Stamina:
                    if (stamina >= possibleEvolution.EvolutionStatThreshold)
                    {
                        newEvolution = possibleEvolution;
                        return true;
                    }
                    break;
                case StatType.Wisdom:
                    if (wisdom >= possibleEvolution.EvolutionStatThreshold)
                    {
                        newEvolution = possibleEvolution;
                        return true;
                    }
                    break;
                default:
                    Debug.LogError($"Invalid Stat Type: {possibleEvolution.EvolutionStatThreshold}");
                    break;
            }
        }

        return false;
    }

    public void ToggleIdleParticles(bool toggle)
    {
        if (toggle == true && _idleParticles != _patrolParticles)
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