using AI.Behavior;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private ElementalType _elementalType = ElementalType.None;
    [SerializeField] private StatType _statPreference = StatType.None;
    [SerializeField] private bool _inFacility = false;
    [SerializeField] private int _price = 50;

    [Header("Stats")]
    [SerializeField] private int _level = 0;
    [SerializeField] private int _levelCap = 99;
    [SerializeField] private int _endurance = 1;
    [SerializeField] private int _intelligence = 1;
    [SerializeField] private int _strength = 1;
    [SerializeField] private int _happiness = 0;

    [Header("Stat Growth")]
    [SerializeField] private int _enduranceGrowth = 1;
    [SerializeField] private int _intelligenceGrowth = 1;
    [SerializeField] private int _strengthGrowth = 1;
    [SerializeField] private int _enduranceExperience = 0;
    [SerializeField] private int _intelligenceExperience = 0;
    [SerializeField] private int _strengthExperience = 0;
    [SerializeField] private int _enduranceThreshold = 5;
    [SerializeField] private int _intelligenceThreshold = 5;
    [SerializeField] private int _strengthThreshold = 5;
    [SerializeField] private int _levelUpTracker = 0;

    [Header("Essence")]
    [SerializeField] private const int _baseEssenceRate = 5; // Initial Essence gained per tick
    
    private BoxCollider _boxCollider = null;
    private EvolutionLogic _currentEvolution = null;
    private HabitatManager _habitatManager = null;
    private UIManager _uiManager = null;
    private EssenceManager _essenceManager = null;
    private HabitatType _habitatType = HabitatType.None;

    public ChimeraType ChimeraType { get => _chimeraType; }
    public ElementalType ElementalType { get => _elementalType; }
    public HabitatType HabitatType { get => _habitatType; }
    public StatType StatPreference { get => _statPreference; }
    public Animator Animator { get => _currentEvolution.Animator; }
    public BoxCollider BoxCollider { get => _boxCollider; }
    public Sprite Icon { get => _currentEvolution.Icon; }
    public bool InFacility { get => _inFacility; }
    public int Level { get => _level; }
    public int Endurance { get => _endurance; }
    public int Intelligence { get => _intelligence; }
    public int Strength { get => _strength; }
    public int Happiness { get => _happiness; }
    public int Price { get => _price; }

    public bool GetStatByType(StatType statType, out int amount)
    {
        amount = 0;

        switch (statType)
        {
            case StatType.Endurance:
                amount = _endurance;
                return true;
            case StatType.Intelligence:
                amount = _intelligence;
                return true;
            case StatType.Strength:
                amount = _strength;
                return true;
            case StatType.Happiness:
                amount = _happiness;
                return true;
            default:
                Debug.LogError("Default StatType please change!");
                break;
        }

        return false;
    }

    public void SetHabitatType(HabitatType habitatType) { _habitatType = habitatType; }
    public void SetInFacility(bool inFacility) { _inFacility = inFacility; }
    public void SetLevel(int level) { _level = level; }
    public void SetEndurance(int endurance) { _endurance = endurance; }
    public void SetIntelligence(int intelligence) { _intelligence = intelligence; }
    public void SetStrength(int strength) { _strength = strength; }
    public void SetHappiness(int happiness) { _happiness = happiness; }

    public void Initialize()
    {
        _essenceManager = ServiceLocator.Get<EssenceManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _uiManager = ServiceLocator.Get<UIManager>();

        _currentEvolution = GetComponentInChildren<EvolutionLogic>();
        _habitatType = _habitatManager.CurrentHabitat.Type;

        InitializeEvolution();
        GetComponent<ChimeraBehavior>().Initialize();

        Debug.Log($"<color=Cyan> Initializing Chimera: {_chimeraType}</color>");
    }

    private void InitializeEvolution()
    {
        _boxCollider = _currentEvolution.GetComponent<BoxCollider>();
        _currentEvolution.Initialize(this);
        _chimeraType = _currentEvolution.Type;
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
            case StatType.Endurance:
                _enduranceExperience += amount;
                break;
            case StatType.Intelligence:
                _intelligenceExperience += amount;
                break;
            case StatType.Strength:
                _strengthExperience += amount;
                break;
            default:
                Debug.LogError("Default Experience Tick Please Change!");
                break;
        }

        AllocateExperience();
    }

    // Checks if stored experience is below cap and appropriately assigns.
    // The essence formula is located here.
    public void EssenceTick()
    {
        if (_essenceManager == null)
        {
            return;
        }

        if (_inFacility == true)
        {
            return;
        }

        int happinessModifier = HappinessModifierCalc(); // The happiness of a Chimera affects how much Essence it will generate.
        int essenceGain = (int)((happinessModifier * _baseEssenceRate) + Mathf.Sqrt(_level));
        _essenceManager.IncreaseEssence(essenceGain);
    }

    public void HappinessTick()
    {
        if (_inFacility == false)
        {
            int happinessAmount = -1;

            ChangeHappiness(happinessAmount);
            _uiManager.UpdateDetails();
        }
    }

    // Happiness can range between -100 and 100.
    // At -100, happinessMod is 0.3. At 0, it is 1. At 100 it is 3.
    private int HappinessModifierCalc()
    {
        if (_happiness == 0)
        {
            return 1;
        }
        else if (_happiness > 0)
        {
            int hapMod = (_happiness) / 50 + 1;
            return hapMod;
        }
        else
        {
            int hapMod = (1 * (int)Mathf.Sqrt(_happiness + 100) / 15) + (1 / 3);
            return hapMod;
        }
    }

    public void ChangeHappiness(int amount)
    {
        if (_happiness + amount >= 100)
        {
            _happiness = 100;
            return;
        }
        else if (_happiness + amount <= -100)
        {
            _happiness = -100;
            return;
        }

        _happiness += amount;
    }

    // Transfer experience stored by the chimera and see if each stat's threshold is met.
    // If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {
        bool levelUp = false;

        if (_enduranceExperience >= _enduranceThreshold)
        {
            _enduranceExperience -= _enduranceThreshold;
            levelUp = true;
            LevelUp(StatType.Endurance);

            _enduranceThreshold += (int)(Mathf.Sqrt(_enduranceThreshold) * 1.2f);
        }

        if (_intelligenceExperience >= _intelligenceThreshold)
        {
            _intelligenceExperience -= _intelligenceThreshold;
            levelUp = true;
            LevelUp(StatType.Intelligence);

            _intelligenceThreshold += (int)(Mathf.Sqrt(_intelligenceThreshold) * 1.2f);
        }

        if (_strengthExperience >= _strengthThreshold)
        {
            _strengthExperience -= _strengthThreshold;
            levelUp = true;
            LevelUp(StatType.Strength);

            _strengthThreshold += (int)(Mathf.Sqrt(_strengthThreshold) * 1.2f);
        }

        if (levelUp == true)
        {
            bool canEvolve = _currentEvolution.CheckEvolution(_endurance, _intelligence, _strength, out EvolutionLogic evolution);

            if (canEvolve == true)
            {
                Evolve(evolution);
                _uiManager.UpdateDetails();
            }
        }
    }

    // Increase stat at rate of the relevant statgrowth variable.
    private void LevelUp(StatType statType)
    {
        switch (statType)
        {
            case StatType.Endurance:
                _endurance += _enduranceGrowth;
                Debug.Log($"{_currentEvolution} now has {_endurance} {statType}");
                break;
            case StatType.Intelligence:
                _intelligence += _intelligenceGrowth;
                Debug.Log($"{_currentEvolution} now has {_intelligence} {statType}");
                break;
            case StatType.Strength:
                _strength += _strengthGrowth;
                Debug.Log($"{_currentEvolution} now has {_strength} {statType}");
                break;
            default:
                Debug.LogError("Default Level Up Please Change!");
                break;
        }

        _uiManager.UpdateDetails();

        ++_levelUpTracker;
        if (_levelUpTracker % 3 == 0)
        {
            ++_level;
            Debug.Log($"LEVEL UP! {_currentEvolution} is now level {_level} !");
        }
    }

    private void Evolve(EvolutionLogic evolution)
    {
        Debug.Log($"{_currentEvolution} is evolving into {evolution}!");

        EvolutionLogic newEvolution = Instantiate(evolution, transform);

        Destroy(_currentEvolution.gameObject);

        _currentEvolution = newEvolution;
        InitializeEvolution();
        _boxCollider.enabled = false;

        _habitatManager.UpdateCurrentHabitatChimeras();
    }
}