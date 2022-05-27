using AI.Behavior;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private HabitatType _habitatType = HabitatType.None;
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
    [SerializeField] private int _happinessMod = 1;

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
    [SerializeField] private float _baseEssenceRate = 5;
    [SerializeField] private float _essenceModifier = 1.0f; // Tuning knob for essence gain

    private EvolutionLogic _currentEvolution = null;
    private HabitatManager _habitatManager = null;
    private ResourceManager _resourceManager = null;
    private UIManager _uiManager = null;
    private EssenceManager _essenceManager = null;

    public ChimeraType ChimeraType { get => _chimeraType; }
    public HabitatType HabitatType { get => _habitatType; }
    public ElementalType ElementalType { get => _elementalType; }
    public StatType StatPreference { get => _statPreference; }
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

    public Sprite GetIcon() 
    {
        if(_currentEvolution == null)
        {
            var defaultSprite = _resourceManager.GetDefaultChimeraSprite();
            return defaultSprite;
        }

        return _currentEvolution.Icon; 
    }

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
        _resourceManager = ServiceLocator.Get<ResourceManager>();

        _habitatType = _habitatManager.CurrentHabitat.Type;
        _currentEvolution = GetComponentInChildren<EvolutionLogic>();

        Debug.Log($"<color=Green> Initializing Chimera: {_currentEvolution}.</color>");

        GetComponent<ChimeraBehavior>().Initialize();
    }

    // Checks if stored experience is below cap and appropriately adds stat exp.
    public void ExperienceTick(StatType statType, int amount)
    {
        if (Level >= _levelCap)
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
        if(_essenceManager == null)
        {
            return;
        }

        _happinessMod = HappinessModifierCalc();

        if (_inFacility)
        {
            return;
        }

        // Sqrt is used to gain diminishing returns on levels.
        // EssenceModifier is used to tune the level scaling
        int essenceGain = (int)((_happinessMod * _baseEssenceRate) + Mathf.Sqrt(Level * _essenceModifier));
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
        if (Happiness == 0)
        {
            return 1;
        }
        else if (Happiness > 0)
        {
            int hapMod = (Happiness) / 50 + 1;
            return hapMod;
        }
        else
        {
            int hapMod = (1 * (int)Mathf.Sqrt(Happiness + 100) / 15) + (1 / 3);
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
        _chimeraType = newEvolution.Type;

        _habitatManager.UpdateCurrentHabitatChimeras();
    }
}