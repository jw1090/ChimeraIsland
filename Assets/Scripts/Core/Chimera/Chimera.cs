using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private ElementType _elementalType = ElementType.None;
    [SerializeField] private int _price = 5;

    [Header("Stat Growth")]
    [SerializeField] private int _explorationThreshold = 5;
    [SerializeField] private int _staminaThreshold = 5;
    [SerializeField] private int _wisdomThreshold = 5;

    [Header("References")]
    [SerializeField] private ChimeraInteractionIcon _interactionIcon = null;

    private AudioManager _audioManager = null;
    private BoxCollider _boxCollider = null;
    private ChimeraBehavior _chimeraBehavior = null;
    private EvolutionLogic _currentEvolution = null;
    private EvolutionLogic _chimeraToBecome = null;
    private HabitatManager _habitatManager = null;
    private HabitatUI _habitatUI = null;
    private ResourceManager _resourceManager = null;
    private Sprite _elementIcon = null;
    private HabitatType _habitatType = HabitatType.None;
    private bool _inFacility = false;
    private bool _onExpedition = false;
    private bool _readyToEvolve = false;
    private float _averagePower = 0;
    private int _uniqueId = 1;
    private int _exploration = 1;
    private int _stamina = 1;
    private int _wisdom = 1;
    private int _currentEnergy = 5;
    private int _maxEnergy = 5;
    private int _staminaExperience = 0;
    private int _wisdomExperience = 0;
    private int _explorationExperience = 0;
    private int _levelCap = 99;
    private int _energyTickCounter = 0;
    private const int _statBonusAmount = 3;

    public bool ReadyToEvolve { get => _readyToEvolve; }
    public ChimeraType ChimeraType { get => _chimeraType; }
    public ElementType ElementalType { get => _elementalType; }
    public HabitatType HabitatType { get => _habitatType; }
    public Animator Animator { get => _currentEvolution.Animator; }
    public BoxCollider BoxCollider { get => _boxCollider; }
    public ChimeraBehavior Behavior { get => _chimeraBehavior; }
    public EvolutionLogic CurrentEvolution { get => _currentEvolution; }
    public Sprite ChimeraIcon { get => _currentEvolution.ChimeraIcon; }
    public Sprite ElementIcon { get => _elementIcon; }
    public bool InFacility { get => _inFacility; }
    public bool OnExpedition { get => _onExpedition; }
    public float AveragePower { get => _averagePower; }
    public int Stamina { get => _stamina; }
    public int Wisdom { get => _wisdom; }
    public int Exploration { get => _exploration; }
    public int CurrentEnergy { get => _currentEnergy; }
    public int MaxEnergy { get => _maxEnergy; }
    public int Price { get => _price; }
    public string Name { get => GetName(); }
    public int UniqueID { get => _uniqueId; }

    public int GetStatThreshold(StatType statType)
    {
        switch (statType)
        {
            case StatType.Stamina:
                return _staminaThreshold;
            case StatType.Wisdom:
                return _wisdomThreshold;
            case StatType.Exploration:
                return _explorationThreshold;
            default:
                Debug.LogError($"Stat Type [{statType}] is invalid.");
                return -1;
        }
    }

    public int GetXP(StatType statType)
    {
        switch (statType)
        {
            case StatType.Stamina:
                return _staminaExperience;
            case StatType.Wisdom:
                return _wisdomExperience;
            case StatType.Exploration:
                return _explorationExperience;
            default:
                Debug.LogError($"Stat Type [{statType}] is invalid.");
                return -1;
        }
    }

    public int GetEXPThresholdDifference(StatType statType, int statLevelGoal)
    {
        GetStatByType(statType, out int currentStatAmount);
        int threshold = GetStatThreshold(statType);
        int totalThreshold = threshold;

        if (statLevelGoal <= currentStatAmount || statLevelGoal > _levelCap)
        {
            Debug.LogError($"Level Goal [{statLevelGoal}] is invalid.");
            return -1;
        }

        for (int i = currentStatAmount + 1; i < statLevelGoal; ++i)
        {
            threshold += (int)(Mathf.Sqrt(threshold) * 1.2f);
            totalThreshold += threshold;
        }

        switch (statType)
        {
            case StatType.Stamina:
                return totalThreshold - _staminaExperience;
            case StatType.Wisdom:
                return totalThreshold - _wisdomExperience;
            case StatType.Exploration:
                return totalThreshold - _explorationExperience;
            default:
                Debug.LogError($"StatType: [{statType}] is invalid, please change!");
                return -1;
        }
    }

    public string GetName()
    {
        if (_currentEvolution == null)
        {
            switch (_chimeraType)
            {
                case ChimeraType.A:
                case ChimeraType.A1:
                case ChimeraType.A2:
                case ChimeraType.A3:
                    return "Nauphant";
                case ChimeraType.B:
                case ChimeraType.B1:
                case ChimeraType.B2:
                case ChimeraType.B3:
                    return "Frolli";
                case ChimeraType.C:
                case ChimeraType.C1:
                case ChimeraType.C2:
                case ChimeraType.C3:
                    return "Patchero";
                default:
                    Debug.LogError($"{_chimeraType} is invalid, please change!");
                    return "";
            }
        }

        return _currentEvolution.Name;
    }

    public bool GetStatByType(StatType statType, out int amount)
    {
        amount = 0;

        switch (statType)
        {
            case StatType.Stamina:
                amount = _stamina;
                return true;
            case StatType.Wisdom:
                amount = _wisdom;
                return true;
            case StatType.Exploration:
                amount = _exploration;
                return true;
            default:
                Debug.LogError("Default StatType please change!");
                break;
        }

        return false;
    }

    public void SetEvolutionIconActive() { _interactionIcon.gameObject.SetActive(true); }
    public void SetUniqueID(int id) { _uniqueId = id; }
    public void SetHabitatType(HabitatType habitatType) { _habitatType = habitatType; }
    public void SetInFacility(bool inFacility) { _inFacility = inFacility; }
    public void SetOnExpedition(bool onExpedition) { _onExpedition = onExpedition; }
    public void SetStamina(int stamina) { _stamina = stamina; }
    public void SetWisdom(int wisdom) { _wisdom = wisdom; }
    public void SetExploration(int exploration) { _exploration = exploration; }
    public void SetCurrentEnergy(int currentEnergy) { _currentEnergy = currentEnergy; }

    public void SetXPByType(StatType statType, int amount)
    {
        switch (statType)
        {
            case StatType.Stamina:
                _staminaExperience = amount;
                break;
            case StatType.Wisdom:
                _wisdomExperience = amount;
                break;
            case StatType.Exploration:
                _explorationExperience = amount;
                break;
            default:
                Debug.LogError("Default StatType please change!");
                break;
        }
    }

    public void Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing Chimera: {_chimeraType}</color>");

        _audioManager = ServiceLocator.Get<AudioManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;

        _chimeraBehavior = GetComponent<ChimeraBehavior>();
        _currentEvolution = GetComponentInChildren<EvolutionLogic>();
        _habitatType = _habitatManager.CurrentHabitat.Type;

        _elementIcon = _resourceManager.GetElementSprite(_elementalType);

        if (_uniqueId == 1)
        {
            _uniqueId = gameObject.GetInstanceID();
        }

        InitializeStats();
        _chimeraBehavior.Initialize();
        InitializeEvolution();
        _interactionIcon.Initialize();

        _chimeraBehavior.StartAI();
    }

    private void InitializeStats()
    {
        for (int i = 1; i < _stamina; ++i)
        {
            _staminaThreshold += (int)(Mathf.Sqrt(_staminaThreshold) * 1.2f);
        }

        for (int i = 1; i < _wisdom; ++i)
        {
            _wisdomThreshold += (int)(Mathf.Sqrt(_wisdomThreshold) * 1.2f);
        }

        for (int i = 1; i < _exploration; ++i)
        {
            _explorationThreshold += (int)(Mathf.Sqrt(_explorationThreshold) * 1.2f);
        }

        _maxEnergy = (int)(_stamina * 0.5) + 5;

        LevelCalculation();
    }

    private void InitializeEvolution()
    {
        _boxCollider = _currentEvolution.GetComponent<BoxCollider>();
        _currentEvolution.Initialize(this);
        _chimeraType = _currentEvolution.Type;
    }

    public void EnergyTick()
    {
        ++_energyTickCounter;

        if (_energyTickCounter >= 20)
        {
            _energyTickCounter = 0;

            if (_currentEnergy < _maxEnergy)
            {
                ++_currentEnergy;
                _habitatUI.UpdateHabitatUI();
            }
        }
    }

    // Checks if stored experience is below cap and appropriately adds stat exp.
    public void ExperienceTick(StatType statType, int amount)
    {
        if (_averagePower >= _levelCap)
        {
            return;
        }

        switch (statType)
        {
            case StatType.Stamina:
                _staminaExperience += amount;
                break;
            case StatType.Wisdom:
                _wisdomExperience += amount;
                break;
            case StatType.Exploration:
                _explorationExperience += amount;
                break;
            default:
                Debug.LogError("Default Experience Tick Please Change!");
                break;
        }

        AllocateExperience();
    }

    // Transfer experience stored by the chimera and see if each stat's threshold is met.
    // If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {
        bool levelUp = false;
        _energyTickCounter++;

        if (_staminaExperience >= _staminaThreshold)
        {
            _staminaExperience = 0;
            levelUp = true;
            LevelUp(StatType.Stamina);

            _staminaThreshold += (int)(Mathf.Sqrt(_staminaThreshold) * 1.2f);
        }

        if (_wisdomExperience >= _wisdomThreshold)
        {
            _wisdomExperience = 0;
            levelUp = true;
            LevelUp(StatType.Wisdom);

            _wisdomThreshold += (int)(Mathf.Sqrt(_wisdomThreshold) * 1.2f);
        }

        if (_explorationExperience >= _explorationThreshold)
        {
            _explorationExperience = 0;
            levelUp = true;
            LevelUp(StatType.Exploration);

            _explorationThreshold += (int)(Mathf.Sqrt(_explorationThreshold) * 1.2f);
        }

        if (levelUp == true && _chimeraToBecome == null)
        {
            _readyToEvolve = _currentEvolution.CheckEvolution(_exploration, _stamina, _wisdom, out EvolutionLogic evolution);
            _chimeraToBecome = evolution;
        }
    }

    public void EvolveChimera()
    {
        if (_readyToEvolve == false)
        {
            Debug.LogError($"Not ready for evolution!");
            return;
        }

        Evolve(_chimeraToBecome);
        EvolveStatBonus();
        _chimeraBehavior.EvaluateParticlesOnEvolve();
        _habitatUI.UpdateHabitatUI();
    }

    private void EvolveStatBonus()
    {
        switch (_currentEvolution.StatBonus)
        {
            case StatType.None:
                break;
            case StatType.Exploration:
                _exploration += _statBonusAmount;
                break;
            case StatType.Stamina:
                _stamina += _statBonusAmount;
                break;
            case StatType.Wisdom:
                _wisdom += _statBonusAmount;
                break;
            default:
                Debug.LogError($"Unhandled stat type [{_currentEvolution.StatBonus}]");
                break;
        }
    }

    // Increase stat at rate of the relevant statgrowth variable.
    private void LevelUp(StatType statType)
    {
        switch (statType)
        {
            case StatType.Stamina:
                _stamina += 1;
                _maxEnergy = (int)(_stamina * 0.5) + 5;
                Debug.Log($"{_currentEvolution.name} now has {_stamina} {statType}");
                break;
            case StatType.Wisdom:
                _wisdom += 1;
                Debug.Log($"{_currentEvolution.name} now has {_wisdom} {statType}");
                break;
            case StatType.Exploration:
                _exploration += 1;
                Debug.Log($"{_currentEvolution.name} now has {_exploration} {statType}");
                break;
            default:
                Debug.LogError("Default Level Up Please Change!");
                break;
        }

        LevelCalculation();

        _habitatUI.UpdateHabitatUI();
    }

    private void LevelCalculation()
    {
        float power = (_stamina + _wisdom + _exploration) * 0.334f;
        _averagePower = Mathf.Round(power * 10.0f) * 0.1f; // Round to the tenth
        _habitatUI.UpdateHabitatUI();
    }

    private void Evolve(EvolutionLogic evolution)
    {
        _readyToEvolve = false;
        _interactionIcon.gameObject.SetActive(false);

        Debug.Log($"{_currentEvolution} is evolving into {evolution}!");

        EvolutionLogic newEvolution = Instantiate(evolution, transform);

        _audioManager.PlayUISFX(SFXUIType.Evolution);

        Destroy(_currentEvolution.gameObject);

        _currentEvolution = newEvolution;
        InitializeEvolution();
        InitializeStats();
        _currentEvolution.Animator.SetBool("Walk", true);

        _habitatManager.UpdateCurrentHabitatChimeras();

        LevelCalculation();

        _chimeraToBecome = null;
    }

    public void RevealChimera(bool reveal)
    {
        _currentEvolution.gameObject.SetActive(reveal);

        if (reveal == true)
        {
            _currentEvolution.Animator.SetBool("Walk", true);
            _currentEvolution.ToggleIdleParticles(false);
            _currentEvolution.TogglePatrolParticles(false);
        }

        _boxCollider.enabled = reveal;
    }
}