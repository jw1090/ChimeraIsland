using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExpeditionManager : MonoBehaviour
{
    [SerializeField] private List<CurrencyExpeditionData> _essenceExpeditions = new List<CurrencyExpeditionData>();
    [SerializeField] private List<CurrencyExpeditionData> _fossilExpeditions = new List<CurrencyExpeditionData>();
    [SerializeField] private List<HabitatExpeditionData> _habitatExpeditions = new List<HabitatExpeditionData>();
    [SerializeField] private int _currentEssenceProgress = 0;
    [SerializeField] private int _currentFossilProgress = 0;
    [SerializeField] private int _currentHabitatProgress = 0;
    private ExpeditionBaseData _selectedExpedition = null;
    private CurrencyExpeditionData _essenceExpeditionOption = null;
    private CurrencyExpeditionData _fossilExpeditionOption = null;
    private HabitatExpeditionData _habitatExpeditionOption = null;
    private List<Chimera> _chimeras = new List<Chimera>();
    private ExpeditionUI _uiExpedition = null;
    private CurrencyManager _currencyManager = null;
    private HabitatManager _habitatManager = null;
    private bool _activeInProgressTimer = false;
    private bool _expeditionSuccess = false;
    private float _difficultyValue = 0;
    private float _chimeraPower = 0;
    private float _agilityModifer = 1.0f;
    private float _intelligenceModifier = 1.0f;
    private float _strengthModifier = 1.0f;
    private float _aquaBonus = 0.0f;
    private float _bioBonus = 0.0f;
    private float _firaBonus = 0.0f;
    private float _currentDuration = 0.0f;
    private ExpeditionState _expeditionState = ExpeditionState.None;

    public ExpeditionState State { get => _expeditionState; }
    public List<Chimera> Chimeras { get => _chimeras; }
    public CurrencyExpeditionData EssenceExpeditionOption { get => _essenceExpeditionOption; }
    public CurrencyExpeditionData FossilExpeditionOption { get => _fossilExpeditionOption; }
    public HabitatExpeditionData HabitatExpeditionOption { get => _habitatExpeditionOption; }
    public ExpeditionBaseData SelectedExpedition { get => _selectedExpedition; }

    public bool ExpeditionSuccess { get => _expeditionSuccess; }

    public void SetExpeditionState(ExpeditionState expeditionState) { _expeditionState = expeditionState; }

    public ExpeditionManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _uiExpedition = ServiceLocator.Get<UIManager>().HabitatUI.ExpeditionPanel;
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();

        _expeditionState = ExpeditionState.Selection;
        _expeditionSuccess = false;

        return this;
    }

    public void Update()
    {
        if (_expeditionState != ExpeditionState.InProgress)
        {
            return;
        }

        if (_activeInProgressTimer == true)
        {
            InProgressTimerUpdate();
        }
    }

    public void LoadExpeditionOptions()
    {
        SetupExpeditionOption(_essenceExpeditionOption, ExpeditionType.Essence);
        SetupExpeditionOption(_fossilExpeditionOption, ExpeditionType.Fossils);
        SetupExpeditionOption(_habitatExpeditionOption);
    }

    private void SetupExpeditionOption(CurrencyExpeditionData expedition, ExpeditionType expeditionType)
    {
        if (expedition != null)
        {
            Debug.Log($"{expeditionType} Expedition has already been created.");
            return;
        }

        CurrencyExpeditionData newExpedition = ExpeditionDataByType(expeditionType).DeepCopy(); // Get the correct data reference
        AnalyseRandomModifiers(newExpedition.Modifiers);

        switch (newExpedition.Type)
        {
            case ExpeditionType.Essence:
                _essenceExpeditionOption = newExpedition;
                break;
            case ExpeditionType.Fossils:
                _fossilExpeditionOption = newExpedition;
                break;
            default:
                break;
        }
    }

    private CurrencyExpeditionData ExpeditionDataByType(ExpeditionType expeditionType)
    {
        switch (expeditionType)
        {
            case ExpeditionType.Essence:
                return _essenceExpeditions[_currentEssenceProgress];
            case ExpeditionType.Fossils:
                return _fossilExpeditions[_currentFossilProgress];
            default:
                Debug.LogError($"Expedition Type [{expeditionType}] is invalid!");
                return null;
        }
    }

    private void SetupExpeditionOption(HabitatExpeditionData expedition)
    {
        if (expedition != null)
        {
            Debug.Log($"{_habitatExpeditionOption.Type} Expedition has already been created.");
            return;
        }

        HabitatExpeditionData newExpedition = _habitatExpeditions[_currentHabitatProgress].DeepCopy(); // Get the correct data reference
        AnalyseRandomModifiers(newExpedition.Modifiers);

        if (newExpedition.RewardType == HabitatRewardType.Random)
        {
            FacilityType facilityType = _habitatManager.CurrentHabitat.GetRandomAvailableFacilityType();

            switch (facilityType)
            {
                case FacilityType.Cave:
                    newExpedition.RewardType = HabitatRewardType.CaveExploring;
                    break;
                case FacilityType.RuneStone:
                    newExpedition.RewardType = HabitatRewardType.RuneStone;
                    break;
                case FacilityType.Waterfall:
                    newExpedition.RewardType = HabitatRewardType.Waterfall;
                    break;
                default:
                    Debug.Log($"Facility Type [{facilityType}] is invalid, please change!");
                    break;
            }
        }

        _habitatExpeditionOption = newExpedition;
    }

    private void AnalyseRandomModifiers(List<ModifierType> modifiers) // If a modifier is random, it will randomize it.
    {
        int modifierMax = Enum.GetValues(typeof(ModifierType)).Length - 1;

        for (int i = 0; i < modifiers.Count; ++i)
        {
            if (modifiers[i] != ModifierType.Random)
            {
                continue;
            }

            bool repeated = true;

            while (repeated == true)
            {
                repeated = false;
                modifiers[i] = (ModifierType)Random.Range(1, modifierMax); // 1 is Aqua

                for (int j = 0; j < modifiers.Count; ++j)
                {
                    if (i == j) // Skip Self
                    {
                        continue;
                    }

                    if (modifiers[i] == modifiers[j]) // If true, it was repeated
                    {
                        repeated = true;
                    }
                }
            }
        }
    }

    public void ExpeditionSetup()
    {
        _chimeras.Clear();

        CalculateCurrentDifficultyValue();
        CalculateChimeraPower();
    }

    public void EnterInProgressState()
    {
        _expeditionState = ExpeditionState.InProgress;
        _currentDuration = _selectedExpedition.Duration;
        _activeInProgressTimer = true;
    }

    public bool AddChimera(Chimera chimera)
    {
        _chimeras.Add(chimera);
        EvaluateRosterChange();

        return true;
    }

    public bool RemoveChimera(Chimera chimera)
    {
        _chimeras.Remove(chimera);

        EvaluateRosterChange();

        return true;
    }

    private void EvaluateRosterChange()
    {
        _uiExpedition.SetupUI.UpdateIcons();

        CalculateChimeraPower();
    }

    public bool HasChimeraBeenAdded(Chimera chimeraToFind)
    {
        foreach (var chimera in _chimeras)
        {
            if (chimeraToFind == chimera)
            {
                return true;
            }
        }

        return false;
    }

    private void CalculateCurrentDifficultyValue()
    {
        float minimumLevel = _selectedExpedition.SuggestedLevel;
        float difficultyValue = Mathf.Pow(minimumLevel * 1.3f, 1.5f) * 15.0f;

        _difficultyValue = difficultyValue;

        _uiExpedition.SetupUI.UpdateDifficultyValue(_difficultyValue);
    }

    private void CalculateChimeraPower()
    {
        float power = 0;

        CalculateModifiers();

        foreach (var chimera in _chimeras)
        {
            power += chimera.Agility * (_agilityModifer + ElementTypeModifier(chimera.ElementalType)) * 6.5f;
            power += chimera.Intelligence * (_intelligenceModifier + ElementTypeModifier(chimera.ElementalType)) * 6.5f;
            power += chimera.Strength * (_strengthModifier + ElementTypeModifier(chimera.ElementalType)) * 6.5f;
        }

        if (power >= _difficultyValue)
        {
            _chimeraPower = _difficultyValue;
        }
        else
        {
            _chimeraPower = power;
        }

        _uiExpedition.SetupUI.UpdateChimeraPower(_chimeraPower);
    }

    private void CalculateModifiers()
    {
        ResetMultipliers();

        foreach (ModifierType modifierType in _selectedExpedition.Modifiers)
        {
            switch (modifierType)
            {
                case ModifierType.None:
                    break;
                case ModifierType.Aqua:
                    _aquaBonus = 0.1f;
                    break;
                case ModifierType.Bio:
                    _bioBonus = 0.1f;
                    break;
                case ModifierType.Fira:
                    _firaBonus = 0.1f;
                    break;
                case ModifierType.Agility:
                    _agilityModifer = 1.2f;
                    break;
                case ModifierType.Intelligence:
                    _intelligenceModifier = 1.2f;
                    break;
                case ModifierType.Strength:
                    _strengthModifier = 1.2f;
                    break;
                default:
                    Debug.LogWarning($"Modifier type is not valid [{modifierType}]. Please fix.");
                    break;
            }
        }
    }

    private float ElementTypeModifier(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Aqua:
                return _aquaBonus;
            case ElementType.Bio:
                return _bioBonus;
            case ElementType.Fira:
                return _firaBonus;
            default:
                Debug.LogWarning($"Modifier type is not valid [{elementType}]. Please fix.");
                return 0.0f;
        }
    }

    private void ResetMultipliers()
    {
        _agilityModifer = 1.0f;
        _intelligenceModifier = 1.0f;
        _strengthModifier = 1.0f;
    }

    public float CalculateSuccessChance()
    {
        float successChance = 0.0f;

        if (Mathf.Approximately(_chimeraPower, _difficultyValue) == true)
        {
            successChance = 100.0f;
        }
        else
        {
            successChance = (_chimeraPower / _difficultyValue) * 100.0f;
        }

        Debug.Log
        (
            $"Chimera Power: {_chimeraPower.ToString("F2")} | Difficulty Value: {_difficultyValue.ToString("F2")}\n" +
            $"Success Chance: {successChance.ToString("F2")}"
        );

        return successChance;
    }

    private void InProgressTimerUpdate()
    {
        _currentDuration -= Time.deltaTime;
        _uiExpedition.InProgressUI.UpdateInProgressTimeRemainingText(_currentDuration);

        if (_currentDuration <= 0)
        {
            _currentDuration = 0;
            _activeInProgressTimer = false;

            _uiExpedition.TimerComplete();
        }
    }

    public bool RandomSuccesRate()
    {
        float successRoll = Random.Range(0.0f, _difficultyValue);

        Debug.Log($"You rolled {successRoll} out of {_difficultyValue - _chimeraPower}");

        if (successRoll >= _difficultyValue - _chimeraPower)
        {
            _expeditionSuccess = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SuccessRewards()
    {
        switch (_selectedExpedition.Type)
        {
            case ExpeditionType.HabitatUpgrade:
                _habitatManager.CurrentHabitat.UpgradeHabitatTier();
                Debug.Log("Success! Your habitat upgraded!");
                break;
            case ExpeditionType.Fossils:
                _currencyManager.IncreaseFossils(1);
                Debug.Log("Success! You recieved 1 Fossil!");
                break;
            default:
                Debug.LogWarning($"Reward type is not valid [{_selectedExpedition.Type}], please change!");
                break;
        }

        ProgressToNextExpedition();
    }

    private void ProgressToNextExpedition()
    {
        if (_selectedExpedition == null)
        {
            Debug.LogError($"Selected Expedition is null. Please change!");
        }

        switch (_selectedExpedition.Type)
        {
            case ExpeditionType.Essence:
                _essenceExpeditionOption = null;
                if (_currentEssenceProgress + 1 < _essenceExpeditions.Count)
                {
                    ++_currentEssenceProgress;
                }
                else
                {
                    Debug.LogWarning($"Essence Expedition is at max rank [{_currentEssenceProgress}]");
                }
                break;
            case ExpeditionType.Fossils:
                _fossilExpeditionOption = null;
                if (_currentFossilProgress + 1 < _fossilExpeditions.Count)
                {
                    ++_currentFossilProgress;
                }
                else
                {
                    Debug.LogWarning($"Essence Expedition is at max rank [{_currentFossilProgress}]");
                }
                break;
            case ExpeditionType.HabitatUpgrade:
                _habitatExpeditionOption = null;
                if (_currentHabitatProgress + 1 < _habitatExpeditions.Count)
                {
                    ++_currentHabitatProgress;
                }
                else
                {
                    Debug.LogWarning($"Essence Expedition is at max rank [{_currentHabitatProgress}]");
                }
                break;
            default:
                Debug.LogError($"Expedition Type [{_selectedExpedition.Type}] is invalid.");
                break;
        }
    }

    public void PostExpeditionCleanup(bool onExpedition)
    {
        foreach (Chimera chimera in _chimeras)
        {
            chimera.SetOnExpedition(onExpedition);
        }

        if (onExpedition == false)
        {
            _chimeras.Clear();
        }

        _expeditionSuccess = false;
        _expeditionState = ExpeditionState.Selection;
        _selectedExpedition = null;
    }
}