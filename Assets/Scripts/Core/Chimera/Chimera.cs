using AI.Behavior;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private ElementType _elementalType = ElementType.None;
    [SerializeField] private StatType _statPreference = StatType.None;
    [SerializeField] private bool _inFacility = false;
    [SerializeField] private int _price = 50;

    [Header("Stats")]
    [SerializeField] private int _level = 0;
    [SerializeField] private int _levelCap = 99;
    [SerializeField] private int _agility = 1;
    [SerializeField] private int _intelligence = 1;
    [SerializeField] private int _strength = 1;

    [Header("Stat Growth")]
    [SerializeField] private int _agilityGrowth = 1;
    [SerializeField] private int _intelligenceGrowth = 1;
    [SerializeField] private int _strengthGrowth = 1;
    [SerializeField] private int _agilityExperience = 0;
    [SerializeField] private int _intelligenceExperience = 0;
    [SerializeField] private int _strengthExperience = 0;
    [SerializeField] private int _agilityThreshold = 5;
    [SerializeField] private int _intelligenceThreshold = 5;
    [SerializeField] private int _strengthThreshold = 5;
    [SerializeField] private int _levelUpTracker = 0;

    [Header("Essence")]
    [SerializeField] private const int _baseEssenceRate = 5; // Initial Essence gained per tick
    
    private BoxCollider _boxCollider = null;
    private EvolutionLogic _currentEvolution = null;
    private HabitatManager _habitatManager = null;
    private HabitatUI _habitatUI = null;
    private CurrencyManager _essenceManager = null;
    private ResourceManager _resourceManager = null;
    private Sprite _elementIcon = null;
    private HabitatType _habitatType = HabitatType.None;

    public ChimeraType ChimeraType { get => _chimeraType; }
    public ElementType ElementalType { get => _elementalType; }
    public HabitatType HabitatType { get => _habitatType; }
    public StatType StatPreference { get => _statPreference; }
    public Animator Animator { get => _currentEvolution.Animator; }
    public BoxCollider BoxCollider { get => _boxCollider; }
    public EvolutionLogic CurrentEvolution { get => _currentEvolution; }
    public Sprite ChimeraIcon { get => _currentEvolution.ChimeraIcon; }
    public Sprite ElementIcon { get => _elementIcon; }
    public bool InFacility { get => _inFacility; }
    public int Level { get => _level; }
    public int Agility { get => _agility; }
    public int Intelligence { get => _intelligence; }
    public int Strength { get => _strength; }
    public int Price { get => _price; }
    public string Name { get => GetName(); }

    public int GetAttribute(StatType type)
    {
        switch (type)
        {
            case StatType.Agility:
                return Agility;
            case StatType.Intelligence:
                return Intelligence;
            case StatType.Strength:
                return Strength;
            default:
                return -1;
        }
    }

    public int GetXPtoThreshold(StatType type, int level)
    {
        if (level <= _level || level > _levelCap) return -1;
        int threshold;
        int totalThreshold = 0;
        switch (type)
        {
            case StatType.Agility:
                threshold = _agilityThreshold;
                totalThreshold += threshold;
                for (int i = _level + 1; i < level; i++)
                {
                    threshold += (int)(Mathf.Sqrt(threshold) * 1.2f);
                    totalThreshold += threshold;
                }
                return totalThreshold - _agilityExperience;
            case StatType.Intelligence:
                threshold = _intelligenceThreshold;
                totalThreshold += threshold;
                for (int i = _level + 1; i < level; i++)
                {
                    threshold += (int)(Mathf.Sqrt(threshold) * 1.2f);
                    totalThreshold += threshold;
                }
                return totalThreshold - _intelligenceExperience;
            case StatType.Strength:
                threshold = _strengthThreshold;
                totalThreshold += threshold;
                for (int i = _level+1; i < level; i++)
                {
                    threshold += (int)(Mathf.Sqrt(threshold) * 1.2f);
                    totalThreshold += threshold;
                }
                return totalThreshold - _strengthExperience;
            default:
                return -1;
        }
    }

    public string GetName()
    {
        if(_currentEvolution == null)
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
            case StatType.Agility:
                amount = _agility;
                return true;
            case StatType.Intelligence:
                amount = _intelligence;
                return true;
            case StatType.Strength:
                amount = _strength;
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
    public void SetAgility(int agility) { _agility = agility; }
    public void SetIntelligence(int intelligence) { _intelligence = intelligence; }
    public void SetStrength(int strength) { _strength = strength; }

    public void Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing Chimera: {_chimeraType}</color>");

        _essenceManager = ServiceLocator.Get<CurrencyManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;

        _currentEvolution = GetComponentInChildren<EvolutionLogic>();
        _habitatType = _habitatManager.CurrentHabitat.Type;

        _elementIcon = _resourceManager.GetElementSprite(_elementalType);

        InitializeEvolution();
        GetComponent<ChimeraBehavior>().Initialize();
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
            case StatType.Agility:
                _agilityExperience += amount;
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

        int essenceGain = _baseEssenceRate + (int)Mathf.Sqrt(_level);
        _essenceManager.IncreaseEssence(essenceGain);
    }

    // Transfer experience stored by the chimera and see if each stat's threshold is met.
    // If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience()
    {
        bool levelUp = false;

        if (_agilityExperience >= _agilityThreshold)
        {
            _agilityExperience -= _agilityThreshold;
            levelUp = true;
            LevelUp(StatType.Agility);

            _agilityThreshold += (int)(Mathf.Sqrt(_agilityThreshold) * 1.2f);
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
            bool canEvolve = _currentEvolution.CheckEvolution(_agility, _intelligence, _strength, out EvolutionLogic evolution);

            if (canEvolve == true)
            {
                Evolve(evolution);
                _habitatUI.UpdateDetails();
            }
        }
    }

    // Increase stat at rate of the relevant statgrowth variable.
    private void LevelUp(StatType statType)
    {
        switch (statType)
        {
            case StatType.Agility:
                _agility += _agilityGrowth;
                Debug.Log($"{_currentEvolution} now has {_agility} {statType}");
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

        _habitatUI.UpdateDetails();

        ++_levelUpTracker;
        if (_levelUpTracker % 3 == 0)
        {
            ServiceLocator.Get<AudioManager>().PlayLevelUpSFX();
            ++_level;
            Debug.Log($"LEVEL UP! {_currentEvolution} is now level {_level} !");
        }
    }

    private void Evolve(EvolutionLogic evolution)
    {
        Debug.Log($"{_currentEvolution} is evolving into {evolution}!");

        EvolutionLogic newEvolution = Instantiate(evolution, transform);

        ServiceLocator.Get<AudioManager>().PlayEvolutionSFX();

        Destroy(_currentEvolution.gameObject);

        _currentEvolution = newEvolution;
        InitializeEvolution();
        _boxCollider.enabled = false;

        _habitatManager.UpdateCurrentHabitatChimeras();

        if(_inFacility == true)
        {
            _currentEvolution.gameObject.SetActive(false);
        }
    }
}