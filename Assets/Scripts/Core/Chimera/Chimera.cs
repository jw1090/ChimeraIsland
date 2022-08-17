using AI.Behavior;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private ElementType _elementalType = ElementType.None;
    [SerializeField] private int _price = 5;

    [Header("Stats")]
    [SerializeField] private int _level = 0;
    [SerializeField] private int _exploration = 1;
    [SerializeField] private int _stamina = 1;
    [SerializeField] private int _wisdom = 1;
    [SerializeField] private int _currentEnergy = 1;
    [SerializeField] private int _maxEnergy = 1;

    [Header("Stat Growth")]
    [SerializeField] private int _explorationThreshold = 5;
    [SerializeField] private int _staminaThreshold = 5;
    [SerializeField] private int _wisdomThreshold = 5;

    private AudioManager _audioManager = null;
    private BoxCollider _boxCollider = null;
    private ChimeraBehavior _chimeraBehavior = null;
    private EvolutionLogic _currentEvolution = null;
    private HabitatManager _habitatManager = null;
    private HabitatUI _habitatUI = null;
    private ResourceManager _resourceManager = null;
    private Sprite _elementIcon = null;
    private HabitatType _habitatType = HabitatType.None;
    private bool _inFacility = false;
    private bool _onExpedition = false;
    private int _staminaExperience = 0;
    private int _wisdomExperience = 0;
    private int _explorationExperience = 0;
    private int _levelUpTracker = 0;
    private int _levelCap = 99;
    private int _tickCounter = 0;

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
    public int Level { get => _level; }
    public int Stamina { get => _stamina; }
    public int Wisdom { get => _wisdom; }
    public int Exploration { get => _exploration; }
    public int Energy { get => _maxEnergy; }
    public int Price { get => _price; }
    public string Name { get => GetName(); }

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
                    return "Elephanto";
                case ChimeraType.B:
                case ChimeraType.B1:
                case ChimeraType.B2:
                case ChimeraType.B3:
                    return "Waddlo";
                case ChimeraType.C:
                case ChimeraType.C1:
                case ChimeraType.C2:
                case ChimeraType.C3:
                    return "Leafo";
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

    public void SetHabitatType(HabitatType habitatType) { _habitatType = habitatType; }
    public void SetInFacility(bool inFacility) { _inFacility = inFacility; }
    public void SetOnExpedition(bool onExpedition) { _onExpedition = onExpedition; }
    public void SetLevel(int level) { _level = level; }
    public void SetStamina(int stamina) { _stamina = stamina; }
    public void SetWisdom(int wisdom) { _wisdom = wisdom; }
    public void SetExploration(int exploration) { _exploration = exploration; }
    public void SetEnergy(int energy) { _maxEnergy = energy; }

    public void Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing Chimera: {_chimeraType}</color>");

        _audioManager = ServiceLocator.Get<AudioManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;

        _currentEvolution = GetComponentInChildren<EvolutionLogic>();
        _habitatType = _habitatManager.CurrentHabitat.Type;

        _elementIcon = _resourceManager.GetElementSprite(_elementalType);

        InitializeStats();
        InitializeEvolution();
        GetComponent<ChimeraBehavior>().Initialize();
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

        _currentEnergy = _maxEnergy;
    }

    private void InitializeEvolution()
    {
        _boxCollider = _currentEvolution.GetComponent<BoxCollider>();
        _currentEvolution.Initialize(this);
        _chimeraType = _currentEvolution.Type;
    }

    public void EnergyTick()
    {
        ++_tickCounter;

        if (_tickCounter == 20)
        {
            _tickCounter = 0;

            if (_currentEnergy < _maxEnergy)
            {
                ++_currentEnergy;
            }
        }
    }

    // Checks if stored experience is below cap and appropriately adds stat exp.
    public void ExperienceTick(StatType statType, int amount)
    {
        if (_level >= _levelCap)
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
        _tickCounter++;

        if (_staminaExperience >= _staminaThreshold)
        {
            _staminaExperience -= _staminaThreshold;
            levelUp = true;
            LevelUp(StatType.Stamina);

            _staminaThreshold += (int)(Mathf.Sqrt(_staminaThreshold) * 1.2f);
        }

        if (_wisdomExperience >= _wisdomThreshold)
        {
            _wisdomExperience -= _wisdomThreshold;
            levelUp = true;
            LevelUp(StatType.Wisdom);

            _wisdomThreshold += (int)(Mathf.Sqrt(_wisdomThreshold) * 1.2f);
        }

        if (_explorationExperience >= _explorationThreshold)
        {
            _explorationExperience -= _explorationThreshold;
            levelUp = true;
            LevelUp(StatType.Exploration);

            _explorationThreshold += (int)(Mathf.Sqrt(_explorationThreshold) * 1.2f);
        }

        if (levelUp == true)
        {
            bool canEvolve = _currentEvolution.CheckEvolution(_exploration, _stamina, _wisdom, out EvolutionLogic evolution);

            if (canEvolve == true)
            {
                Evolve(evolution);
                _habitatUI.UpdateHabitatUI();
            }
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

        if (++_levelUpTracker % 2 == 0)
        {
            _audioManager.PlayUISFX(SFXUIType.LevelUp);
            ++_level;
            Debug.Log($"LEVEL UP! {_currentEvolution} is now level {_level} !");
        }

        _habitatUI.UpdateHabitatUI();
    }

    private void Evolve(EvolutionLogic evolution)
    {
        Debug.Log($"{_currentEvolution} is evolving into {evolution}!");

        EvolutionLogic newEvolution = Instantiate(evolution, transform);

        _audioManager.PlayUISFX(SFXUIType.Evolution);

        Destroy(_currentEvolution.gameObject);

        _currentEvolution = newEvolution;
        InitializeEvolution();
        _boxCollider.enabled = false;

        _habitatManager.UpdateCurrentHabitatChimeras();

        if (_inFacility == true)
        {
            _currentEvolution.gameObject.SetActive(false);
        }
    }

    public void RevealChimera(bool reveal)
    {
        _currentEvolution.gameObject.SetActive(reveal);

        if (reveal == true)
        {
            _currentEvolution.Animator.SetBool("Walk", true);
        }

        _boxCollider.enabled = reveal;
    }
}