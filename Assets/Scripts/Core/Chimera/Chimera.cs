using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private string _customName = null;
    [SerializeField] private int _price = 5;

    [Header("Stat Growth")]
    [SerializeField] private int _explorationThreshold = 5;
    [SerializeField] private int _staminaThreshold = 5;
    [SerializeField] private int _wisdomThreshold = 5;

    [Header("References")]
    [SerializeField] private ChimeraInteractionIcon _interactionIcon = null;

    private PersistentData _persistentData = null;
    private AudioManager _audioManager = null;
    private BoxCollider _boxCollider = null;
    private ChimeraBehavior _chimeraBehavior = null;
    private EvolutionLogic _currentEvolution = null;
    private EvolutionLogic _chimeraToBecome = null;
    private HabitatManager _habitatManager = null;
    private HabitatUI _habitatUI = null;
    private ResourceManager _resourceManager = null;
    private Sprite _elementIcon = null;
    private ElementType _elementalType = ElementType.None;
    private bool _inFacility = false;
    private bool _onExpedition = false;
    private bool _readyToEvolve = false;
    private bool _isFirstChimera = false;
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
    private const float _thresholdScaler = 1.2f;
    private const float _thresholdExponent = 0.35f;

    public bool FirstChimera { get => _isFirstChimera; }
    public bool ReadyToEvolve { get => _readyToEvolve; }
    public ChimeraType ChimeraType { get => _chimeraType; }
    public ElementType ElementalType { get => _elementalType; }
    public StatType PreferredStat { get => _currentEvolution.StatBonus; }
    public Animator Animator { get => _currentEvolution.Animator; }
    public BoxCollider BoxCollider { get => _boxCollider; }
    public ChimeraBehavior Behavior { get => _chimeraBehavior; }
    public EvolutionLogic CurrentEvolution { get => _currentEvolution; }
    public Sprite ChimeraIcon { get => _currentEvolution.ChimeraIcon; }
    public Sprite ElementIcon { get => _elementIcon; }
    public bool InFacility { get => _inFacility; }
    public bool OnExpedition { get => _onExpedition; }
    public int Stamina { get => _stamina; }
    public int Wisdom { get => _wisdom; }
    public int Exploration { get => _exploration; }
    public int CurrentEnergy { get => _currentEnergy; }
    public int MaxEnergy { get => _maxEnergy; }
    public int Price { get => _price; }
    public int UniqueID { get => _uniqueId; }
    public float AveragePower { get => _averagePower; }
    public string Name { get => GetName(); }
    public string CustomName { get => _customName; }
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

    public int GetEXP(StatType statType)
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
            CalculateThresholdGrowth(ref threshold);
            totalThreshold += threshold;
        }

        switch (statType)
        {
            case StatType.Exploration:
                return totalThreshold - _explorationExperience;
            case StatType.Stamina:
                return totalThreshold - _staminaExperience;
            case StatType.Wisdom:
                return totalThreshold - _wisdomExperience;
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
        
        if(CustomName != "")
        {
            return CustomName;
        }
        else
        {
            return _currentEvolution.Name;
        }
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

    public void SetIsFirstChimera(bool IsFirstChimera) { _isFirstChimera = IsFirstChimera; }
    public void SetEvolutionIconActive() { _interactionIcon.gameObject.SetActive(true); }
    public void SetUniqueID(int id) { _uniqueId = id; }
    public void SetInFacility(bool inFacility) { _inFacility = inFacility; }
    public void SetOnExpedition(bool onExpedition) { _onExpedition = onExpedition; }
    public void SetStamina(int stamina) { _stamina = stamina; }
    public void SetWisdom(int wisdom) { _wisdom = wisdom; }
    public void SetExploration(int exploration) { _exploration = exploration; }
    public void SetCurrentEnergy(int currentEnergy) { _currentEnergy = currentEnergy; }
    public void SetCustomName(string newName) { _customName = newName; }

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
        _persistentData = ServiceLocator.Get<PersistentData>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;

        _chimeraBehavior = GetComponent<ChimeraBehavior>();
        FindEvolution();

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

    public void FindEvolution()
    {
        _currentEvolution = GetComponentInChildren<EvolutionLogic>();
    }

    private void InitializeStats()
    {
        for (int i = 1; i < _exploration; ++i)
        {
            CalculateThresholdGrowth(ref _explorationThreshold);
        }

        for (int i = 1; i < _stamina; ++i)
        {
            CalculateThresholdGrowth(ref _staminaThreshold);
        }

        for (int i = 1; i < _wisdom; ++i)
        {
            CalculateThresholdGrowth(ref _wisdomThreshold);
        }

        _maxEnergy = (int)(_stamina * 0.5) + 5;

        LevelCalculation();
    }

    private void CalculateThresholdGrowth(ref int statThreshold)
    {
        float scalerNumber = statThreshold * _thresholdScaler;
        float powNumber = (Mathf.Pow(scalerNumber, _thresholdExponent));

        statThreshold += (int)powNumber;
    }

    private void InitializeEvolution()
    {
        _boxCollider = _currentEvolution.GetComponent<BoxCollider>();

        _currentEvolution.Initialize(this);

        _chimeraType = _currentEvolution.ChimeraType;
        _elementalType = _currentEvolution.ElementType;
        _elementIcon = _resourceManager.GetElementSimpleSprite(_elementalType);
    }

    public void EnergyTick()
    {
        if (_onExpedition == true) // Nn energy gain on expeditions.
        {
            return;
        }

        ++_energyTickCounter;

        if (_energyTickCounter >= DetermineTickRequired())
        {
            _energyTickCounter = 0;

            if (_currentEnergy < _maxEnergy)
            {
                ++_currentEnergy;
                _habitatUI.UpdateHabitatUI();
            }
        }
    }

    private int DetermineTickRequired()
    {
        float numerator = _stamina + 10.0f;
        float denominator = 100.0f;
        float exponent = -1.5f;
        float flatModifer = 15.0f;

        int amount = (int)(Mathf.Pow(numerator / denominator, exponent) + flatModifer);

        return amount;
    }

    public void AddEnergy(int energyAmount)
    {
        if (_currentEnergy + energyAmount > _maxEnergy)
        {
            _currentEnergy = _maxEnergy;
        }
        else
        {
            _currentEnergy += energyAmount;
        }

        _habitatUI.UpdateHabitatUI();
    }

    public void DrainEnergy(int drainAmount)
    {
        if (_currentEnergy - drainAmount < 0)
        {
            Debug.LogError($"Drain Amount [{drainAmount}] is causing Current Energy [{_currentEnergy}] to try to enter negatives. Please check.");
            _currentEnergy = 0;
        }
        else
        {
            _currentEnergy -= drainAmount;
        }

        _habitatUI.UpdateHabitatUI();
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

        if (_explorationExperience >= _explorationThreshold)
        {
            _explorationExperience = 0;
            levelUp = true;
            LevelUp(StatType.Exploration);

            CalculateThresholdGrowth(ref _explorationThreshold);
        }

        if (_staminaExperience >= _staminaThreshold)
        {
            _staminaExperience = 0;
            levelUp = true;
            LevelUp(StatType.Stamina);

            CalculateThresholdGrowth(ref _staminaThreshold);
        }


        if (_wisdomExperience >= _wisdomThreshold)
        {
            _wisdomExperience = 0;
            levelUp = true;
            LevelUp(StatType.Wisdom);

            CalculateThresholdGrowth(ref _wisdomThreshold);
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

        _habitatUI.UIManager.AlertText.CreateAlert($"{GetName()} has evolved to {_chimeraToBecome.Name}");

        Evolve(_chimeraToBecome);
        _chimeraBehavior.EvaluateParticlesOnEvolve();
        _habitatUI.DetailsManager.DetailsStatGlow();
        _habitatUI.UpdateHabitatUI();


        _habitatManager.ChimeraCollections.CollectChimera(_chimeraType);

        _persistentData.SaveSessionData();
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
        _chimeraBehavior.ChangeState(ChimeraBehaviorState.Patrol);
        _currentEvolution.Animator.Play("Walk");

        _habitatManager.UpdateCurrentChimeras();

        LevelCalculation();

        _chimeraToBecome = null;
    }

    public void RevealChimera(bool reveal)
    {
        _currentEvolution.gameObject.SetActive(reveal);

        _chimeraBehavior.enabled = reveal;
        _chimeraBehavior.Agent.enabled = reveal;
        _boxCollider.enabled = reveal;

        if (reveal == true)
        {
            _chimeraBehavior.ChangeState(ChimeraBehaviorState.Patrol);
            _currentEvolution.Animator.Play("Walk");
        }
    }
}