using System.Collections;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    [SerializeField] private string _customName = null;

    [Header("References")]
    [SerializeField] private ChimeraInteractionIcon _interactionIcon = null;

    private PersistentData _persistentData = null;
    private HabitatManager _habitatManager = null;
    private InputManager _inputManager = null;
    private ResourceManager _resourceManager = null;
    private AudioManager _audioManager = null;
    private UIManager _uiManager = null;
    private BoxCollider _boxCollider = null;
    private ChimeraBehavior _chimeraBehavior = null;
    private EvolutionLogic _currentEvolution = null;
    private EvolutionLogic _chimeraToBecome = null;
    private HabitatUI _habitatUI = null;
    private Sprite _elementIcon = null;
    private ElementType _elementalType = ElementType.None;
    private bool _inFacility = false;
    private bool _onExpedition = false;
    private bool _readyToEvolve = false;
    private bool _isFirstChimera = false;
    private bool _pauseEnergyRegen = false;
    private float _averagePower = 0;
    private int _uniqueId = 1;
    private int _exploration = 1;
    private int _stamina = 1;
    private int _wisdom = 1;
    private int _staminaExperience = 0;
    private int _wisdomExperience = 0;
    private int _explorationExperience = 0;
    private int _explorationThreshold = 0;
    private int _staminaThreshold = 0;
    private int _wisdomThreshold = 0;
    private int _currentEnergy = 5;
    private int _maxEnergy = 5;
    private int _levelCap = 99;
    private int _energyTickCounter = 0;

    public bool FirstChimera { get => _isFirstChimera; }
    public bool ReadyToEvolve { get => _readyToEvolve; }
    public ChimeraType ChimeraType { get => _chimeraType; }
    public ElementType ElementalType { get => _elementalType; }
    public StatType EvolutionBonusStat { get => _currentEvolution.EvolutionStat; }
    public BoxCollider BoxCollider { get => _boxCollider; }
    public ChimeraBehavior Behavior { get => _chimeraBehavior; }
    public EvolutionLogic CurrentEvolution { get => _currentEvolution; }
    public Sprite ChimeraIcon { get => _currentEvolution.ChimeraIcon; }
    public Sprite ElementIcon { get => _elementIcon; }
    public bool InFacility { get => _inFacility; }
    public bool OnExpedition { get => _onExpedition; }
    public float AveragePower { get => _averagePower; }
    public int Exploration { get => _exploration; }
    public int Stamina { get => _stamina; }
    public int Wisdom { get => _wisdom; }
    public int CurrentEnergy { get => _currentEnergy; }
    public int MaxEnergy { get => _maxEnergy; }
    public int UniqueID { get => _uniqueId; }
    public string Name { get => GetName(); }
    public string CustomName { get => _customName; }

    public int GetCurrentStatAmount(StatType statType)
    {
        switch (statType)
        {
            case StatType.Stamina:
                return _stamina;
            case StatType.Wisdom:
                return _wisdom;
            case StatType.Exploration:
                return _exploration;
            default:
                Debug.LogError($"Stat Type [{statType}] is invalid.");
                break;
        }

        return 0;
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

    public int ThresholdDifference(StatType statType, int statLevelGoal)
    {
        int currentAmount = GetCurrentStatAmount(statType);

        int goalThreshold = 0;

        for (int i = currentAmount; i < statLevelGoal; ++i)
        {
            goalThreshold += CalculateCurrentThreshold(i);
        }

        return goalThreshold;
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

        if (CustomName != "")
        {
            return CustomName;
        }
        else
        {
            return _currentEvolution.Name;
        }
    }

    public void SetOnExpedition(bool onExpedition)
    {
        _onExpedition = onExpedition;

        if (_onExpedition == false)
        {
            _pauseEnergyRegen = false;
        }
    }

    public void SetIsFirstChimera(bool isFirstChimera) { _isFirstChimera = isFirstChimera; }
    public void SetEvolutionIconActive() { _interactionIcon.gameObject.SetActive(true); }
    public void SetUniqueID(int id) { _uniqueId = id; }
    public void SetInFacility(bool inFacility) { _inFacility = inFacility; }
    public void SetStamina(int stamina) { _stamina = stamina; }
    public void SetWisdom(int wisdom) { _wisdom = wisdom; }
    public void SetExploration(int exploration) { _exploration = exploration; }
    public void SetCurrentEnergy(int currentEnergy) { _currentEnergy = currentEnergy; }
    public void SetCustomName(string newName) { _customName = newName; }

    public void SetEXPByType(StatType statType, int amount)
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
        _inputManager = ServiceLocator.Get<InputManager>();
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
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
        _explorationThreshold = CalculateCurrentThreshold(_exploration);
        _staminaThreshold = CalculateCurrentThreshold(_stamina);
        _wisdomThreshold = CalculateCurrentThreshold(_wisdom);

        MaxEnergyCalculation();
        LevelCalculation();
    }

    private void MaxEnergyCalculation()
    {
        _maxEnergy = (int)(_stamina * 0.5) + 5;
    }

    private int CalculateCurrentThreshold(float statAmount)
    {
        float thresholdGoal = ++statAmount;

        float frontFraction = 1.0f / 8.0f;
        float levelExponentiaCalc = Mathf.Pow(thresholdGoal, 2.0f) - thresholdGoal;
        float modifier = 150.0f;
        float numerator = Mathf.Pow(2.0f, thresholdGoal / 7.0f) - Mathf.Pow(2.0f, 1.0f / 7.0f);
        float denominator = Mathf.Pow(2.0f, 1.0f / 7.0f) - 1.0f;

        float threshold = frontFraction * (levelExponentiaCalc + modifier * (numerator / denominator));

        return Mathf.RoundToInt(threshold);
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
        if (_pauseEnergyRegen == true || _onExpedition == true) // No energy gain on expeditions.
        {
            return;
        }

        if (_currentEnergy == _maxEnergy) // Already full.
        {
            return;
        }

        ++_energyTickCounter;

        if (_energyTickCounter >= DetermineEnergyTickRequired())
        {
            _energyTickCounter = 0;

            if (_currentEnergy < _maxEnergy)
            {
                ++_currentEnergy;
                _habitatUI.UpdateHabitatUI();
            }
        }
    }

    private int DetermineEnergyTickRequired()
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

        _pauseEnergyRegen = false;

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

        _pauseEnergyRegen = true;

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

        AllocateExperience(statType);
    }

    // Transfer experience stored by the chimera and see if each stat's threshold is met.
    // If so, LevelUp is called with specific stat enumerator.
    private void AllocateExperience(StatType statType)
    {
        bool levelUp = false;
        _energyTickCounter++;

        switch (statType)
        {
            case StatType.Exploration:
                if (_explorationExperience >= _explorationThreshold)
                {
                    _explorationExperience = 0;
                    levelUp = true;
                }
                break;
            case StatType.Stamina:
                if (_staminaExperience >= _staminaThreshold)
                {
                    _staminaExperience = 0;
                    levelUp = true;
                }
                break;
            case StatType.Wisdom:
                if (_wisdomExperience >= _wisdomThreshold)
                {
                    _wisdomExperience = 0;
                    levelUp = true;
                }
                break;
            default:
                Debug.Log($"Stat Type is invalid: {statType}");
                break;
        }

        if (levelUp == true)
        {
            LevelUp(statType);
        }

        if (levelUp == true && _chimeraToBecome == null)
        {
            _readyToEvolve = _currentEvolution.CheckEvolution(_exploration, _stamina, _wisdom, out EvolutionLogic evolution);
            _chimeraToBecome = evolution;
        }
    }

    public IEnumerator EvolveChimera()
    {
        _interactionIcon.gameObject.SetActive(false);
        _chimeraBehavior.ChangeState(ChimeraBehaviorState.DoNothing);
        _habitatManager.CurrentHabitat.ChimeraEvolveCameraEnable(this);
        _uiManager.RevealCoreUI(false);
        _audioManager.StartFadeCoroutine(1.0f, 1.0f, 0.0f, 8.5f);
        _audioManager.PlaySFX(EnvironmentSFXType.Evolution1);

        yield return new WaitUntil(() => _habitatManager.CurrentHabitat.MovingAlternateCamera == false);

        _currentEvolution.FullBody.SetActive(false);
        _currentEvolution.EvolutionAnimation.SetActive(true);

        yield return new WaitForSeconds(5.5f);

        _uiManager.AlertText.CreateAlert($"{GetName()} Has Evolved To {_chimeraToBecome.Name}!");

        StartCoroutine(Evolve(_chimeraToBecome));
        _chimeraBehavior.EnterAnim(AnimationType.Walk);
        _chimeraBehavior.EvaluateParticlesOnEvolve();
        _habitatUI.DetailsManager.DetailsStatGlow();
        _habitatUI.UpdateHabitatUI();

        _habitatManager.Collections.CollectChimera(_chimeraType);

        yield return new WaitForSeconds(4f);
        _persistentData.SaveSessionData();
        StartCoroutine(_habitatManager.CurrentHabitat.ChimeraEvolveCameraDisable());

        _inputManager.DisableOutline(false);
        _uiManager.RevealCoreUI(true);
    }

    // Increase stat at rate of the relevant statgrowth variable.
    private void LevelUp(StatType statType)
    {
        int amount = GetCurrentStatAmount(statType);

        switch (statType)
        {
            case StatType.Exploration:
                ++_exploration;
                _explorationThreshold = CalculateCurrentThreshold(_exploration);
                break;
            case StatType.Stamina:
                ++_stamina;
                _staminaThreshold = CalculateCurrentThreshold(_stamina);

                MaxEnergyCalculation();
                break;
            case StatType.Wisdom:
                ++_wisdom;
                _wisdomThreshold = CalculateCurrentThreshold(_wisdom);
                break;
            default:
                Debug.LogError("Default Level Up Please Change!");
                break;
        }
        Debug.Log($"{_currentEvolution.name} now has {amount} {statType}");

        LevelCalculation();

        _habitatUI.UpdateHabitatUI();
    }

    private void LevelCalculation()
    {
        float power = (_stamina + _wisdom + _exploration) * 0.334f;
        _averagePower = Mathf.Round(power * 10.0f) * 0.1f; // Round to the tenth
        _habitatUI.UpdateHabitatUI();
    }

    private IEnumerator Evolve(EvolutionLogic evolution)
    {
        _readyToEvolve = false;
        _interactionIcon.gameObject.SetActive(false);

        Debug.Log($"{_currentEvolution} is evolving into {evolution}!");

        EvolutionLogic newEvolution = Instantiate(evolution, transform);

        _audioManager.PlaySFX(EnvironmentSFXType.Evolution2);

        Destroy(_currentEvolution.gameObject);

        _currentEvolution = newEvolution;
        InitializeEvolution();

        _chimeraBehavior.ChangeState(ChimeraBehaviorState.Idle);

        _habitatManager.UpdateCurrentChimeras();

        LevelCalculation();

        _chimeraToBecome = null;

        _currentEvolution.EvolutionAnimation.SetActive(true);
        _currentEvolution.FullBody.SetActive(false);

        _chimeraBehavior.ChangeState(ChimeraBehaviorState.DoNothing);
        yield return new WaitForSeconds(3.0f);
        _currentEvolution.FullBody.SetActive(true);
        _currentEvolution.EvolutionAnimation.SetActive(false);
        _chimeraBehavior.ChangeState(ChimeraBehaviorState.Idle);
    }

    public void RevealChimera(bool reveal)
    {
        _currentEvolution.gameObject.SetActive(reveal);
        _boxCollider.enabled = reveal;

        if (reveal == true)
        {
            _chimeraBehavior.ChangeState(ChimeraBehaviorState.Patrol);
        }
        else if (reveal == false)
        {
            _chimeraBehavior.ChangeState(ChimeraBehaviorState.Idle);
        }
    }
}