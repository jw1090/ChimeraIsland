using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExpeditionManager : MonoBehaviour
{
    [SerializeField] private List<ExpeditionData> _essenceExpeditions = new List<ExpeditionData>();
    [SerializeField] private List<ExpeditionData> _fossilExpeditions = new List<ExpeditionData>();
    [SerializeField] private List<ExpeditionData> _habitatExpeditions = new List<ExpeditionData>();
    [SerializeField] private int _currentEssenceProgress = 0;
    [SerializeField] private int _currentFossilProgress = 0;
    [SerializeField] private int _currentHabitatProgress = 0;
    private ExpeditionData _selectedExpedition = null;
    private ExpeditionData _essenceExpeditionOption = null;
    private ExpeditionData _fossilExpeditionOption = null;
    private ExpeditionData _habitatExpeditionOption = null;
    private UIManager _uiManager = null;
    private List<Chimera> _chimeras = new List<Chimera>();
    private ExpeditionUI _uiExpedition = null;
    private CurrencyManager _currencyManager = null;
    private HabitatManager _habitatManager = null;
    private AudioManager _audioManager = null;
    private bool _activeInProgressTimer = false;
    private float _difficultyValue = 0;
    private float _chimeraPower = 0;
    private float _explorationModifier = 1.0f;
    private float _staminaModifer = 1.0f;
    private float _wisdomModifier = 1.0f;
    private float _aquaBonus = 0.0f;
    private float _bioBonus = 0.0f;
    private float _firaBonus = 0.0f;
    private float _currentDuration = 0.0f;
    private ExpeditionState _expeditionState = ExpeditionState.None;

    public ExpeditionState State { get => _expeditionState; }
    public List<Chimera> Chimeras { get => _chimeras; }
    public ExpeditionData EssenceExpeditionOption { get => _essenceExpeditionOption; }
    public ExpeditionData FossilExpeditionOption { get => _fossilExpeditionOption; }
    public ExpeditionData HabitatExpeditionOption { get => _habitatExpeditionOption; }
    public ExpeditionData SelectedExpedition { get => _selectedExpedition; }
    public int CurrentEssenceProgress { get => _currentEssenceProgress; }
    public int CurrentFossilProgress { get => _currentFossilProgress; }
    public int CurrentHabitatProgress { get => _currentHabitatProgress; }
    public int UpgradeMissionBounds { get => _habitatExpeditions.Count - 1; }

    public void SetExpeditionState(ExpeditionState expeditionState) { _expeditionState = expeditionState; }

    public void ResetSelectedExpedition()
    {
        _selectedExpedition = null;
        ChimerasOnExpedition(false);
    }

    public void SetSelectedExpedition(ExpeditionType expeditionType)
    {
        switch (expeditionType)
        {
            case ExpeditionType.Essence:
                _selectedExpedition = _essenceExpeditionOption;
                break;
            case ExpeditionType.Fossils:
                _selectedExpedition = _fossilExpeditionOption;
                break;
            case ExpeditionType.HabitatUpgrade:
                _selectedExpedition = _habitatExpeditionOption;
                break;
            default:
                Debug.LogError($"Expedition Type [{expeditionType}] is invalid!");
                break;
        }

        _uiExpedition.LoadExpeditionSetup();
    }

    public ExpeditionManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _uiManager = ServiceLocator.Get<UIManager>();
        _uiExpedition = _uiManager.HabitatUI.ExpeditionPanel;
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _expeditionState = ExpeditionState.Selection;

        HabitatData data = _habitatManager.HabitatDataList[(int)_habitatManager.CurrentHabitat.Type];
        _currentEssenceProgress = data.expeditionEssenceProgress;
        _currentFossilProgress = data.expeditionFossilProgress;
        _currentHabitatProgress = data.expeditionHabitatProgress;

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
        SetupExpeditionOption(_habitatExpeditionOption, ExpeditionType.HabitatUpgrade);
    }

    private void SetupExpeditionOption(ExpeditionData expedition, ExpeditionType expeditionType)
    {
        if (expedition != null) // Already Created
        {
            return;
        }

        ExpeditionData newExpedition = ExpeditionDataByType(expeditionType).DeepCopy(); // Get the correct data reference
        AnalyseRandomModifiers(newExpedition.Modifiers);
        AnalyseRandomUpgrade(newExpedition);

        switch (newExpedition.Type)
        {
            case ExpeditionType.Essence:
                _essenceExpeditionOption = newExpedition;
                break;
            case ExpeditionType.Fossils:
                _fossilExpeditionOption = newExpedition;
                break;
            case ExpeditionType.HabitatUpgrade:
                _habitatExpeditionOption = newExpedition;
                break;
            default:
                Debug.LogError($"Expedition Type [{newExpedition.Type} is invalid, please fix!]");
                break;
        }
    }

    private ExpeditionData ExpeditionDataByType(ExpeditionType expeditionType)
    {
        switch (expeditionType)
        {
            case ExpeditionType.Essence:
                return _essenceExpeditions[_currentEssenceProgress];
            case ExpeditionType.Fossils:
                return _fossilExpeditions[_currentFossilProgress];
            case ExpeditionType.HabitatUpgrade:
                return _habitatExpeditions[_currentHabitatProgress];
            default:
                Debug.LogError($"Expedition Type [{expeditionType}] is invalid!");
                return null;
        }
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

    private void AnalyseRandomUpgrade(ExpeditionData expeditionData)
    {
        if (expeditionData.UpgradeType == HabitatRewardType.Random)
        {
            FacilityType facilityType = _habitatManager.CurrentHabitat.GetRandomAvailableFacilityType();

            switch (facilityType)
            {
                case FacilityType.Cave:
                    expeditionData.UpgradeType = HabitatRewardType.CaveExploring;
                    break;
                case FacilityType.RuneStone:
                    expeditionData.UpgradeType = HabitatRewardType.RuneStone;
                    break;
                case FacilityType.Waterfall:
                    expeditionData.UpgradeType = HabitatRewardType.Waterfall;
                    break;
                default:
                    Debug.Log($"Facility Type [{facilityType}] is invalid, please change!");
                    break;
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
        if (_chimeras.Count == 3)
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            Debug.LogWarning($"Expedition is full!");
            return false;
        }

        _chimeras.Add(chimera);
        EvaluateRosterChange();

        _uiExpedition.SetupUI.ToggleConfirmButton(true);

        return true;
    }

    public bool RemoveChimera(Chimera chimera)
    {
        _chimeras.Remove(chimera);
        EvaluateRosterChange();

        if (_chimeras.Count == 0)
        {
            _uiExpedition.SetupUI.ToggleConfirmButton(false);
        }

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
        float difficultyValue = Mathf.Pow(minimumLevel * 1.2f, 1.5f) * 20.0f;

        _difficultyValue = difficultyValue;

        _uiExpedition.SetupUI.UpdateDifficultyValue(_difficultyValue);
    }

    private void CalculateChimeraPower()
    {
        float power = 0;

        CalculateModifiers();

        foreach (var chimera in _chimeras)
        {
            power += chimera.Stamina * (_staminaModifer + ElementTypeModifier(chimera.ElementalType)) * 6.5f;
            power += chimera.Wisdom * (_wisdomModifier + ElementTypeModifier(chimera.ElementalType)) * 6.5f;
            power += chimera.Exploration * (_explorationModifier + ElementTypeModifier(chimera.ElementalType)) * 6.5f;
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
                    _aquaBonus = 0.2f;
                    break;
                case ModifierType.Bio:
                    _bioBonus = 0.2f;
                    break;
                case ModifierType.Fira:
                    _firaBonus = 0.2f;
                    break;
                case ModifierType.Exploration:
                    _explorationModifier = 1.5f;
                    break;
                case ModifierType.Stamina:
                    _staminaModifer = 1.5f;
                    break;
                case ModifierType.Wisdom:
                    _wisdomModifier = 1.5f;
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
        _staminaModifer = 1.0f;
        _wisdomModifier = 1.0f;
        _explorationModifier = 1.0f;
    }

    public float CalculateSuccessChance()
    {
        float successChance = 0.0f;

        Debug.Log
        (
            $"Chimera Power: {_chimeraPower.ToString("F2")} | Difficulty Value: {_difficultyValue.ToString("F2")}\n" +
            $"Success Chance: {successChance.ToString("F2")}"
        );

        if (Mathf.Approximately(_chimeraPower, _difficultyValue) == true)
        {
            successChance = 100.0f;
        }
        else
        {
            successChance = (_chimeraPower / _difficultyValue) * 100.0f;
        }

        return successChance;
    }

    private void InProgressTimerUpdate()
    {
        _currentDuration -= Time.deltaTime;
        _uiExpedition.InProgressUI.UpdateInProgressTimeRemainingText(_currentDuration);

        if (_currentDuration <= 0)
        {
            _audioManager.PlayUISFX(SFXUIType.Completion);
            _currentDuration = 0;
            _activeInProgressTimer = false;

            _uiExpedition.TimerComplete();
        }
    }

    public bool RandomSuccesRate()
    {

        if (_selectedExpedition.AutoSucceed == true)
        {
            Debug.Log($"Auto suceed activated, you win!");
            return true;
        }

        float successRoll = Random.Range(0.0f, _difficultyValue);

        Debug.Log($"You rolled: {successRoll} | You needed: {_difficultyValue - _chimeraPower}");

        if (successRoll >= _difficultyValue - _chimeraPower)
        {
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
            case ExpeditionType.Essence:
                _currencyManager.IncreaseEssence(_selectedExpedition.AmountGained);
                break;
            case ExpeditionType.Fossils:
                _uiManager.EnableUIByType(UIElementType.MarketplaceButton);
                _uiManager.EnableUIByType(UIElementType.FossilsWallets);
                _currencyManager.IncreaseFossils(_selectedExpedition.AmountGained);

                if (_selectedExpedition.UnlocksNewChimera == true)
                {
                    ChimeraType chimeraType = _uiManager.HabitatUI.Marketplace.ActivateRandomChimera();
                    Debug.Log($"You've unlocked Chimera of type {chimeraType}!");
                }
                break;
            case ExpeditionType.HabitatUpgrade:
                _uiManager.EnableUIByType(UIElementType.EssenceWallets);
                switch (_selectedExpedition.UpgradeType)
                {
                    case HabitatRewardType.Waterfall:
                        _habitatManager.CurrentHabitat.BuildFacility(FacilityType.Waterfall, true);
                        break;
                    case HabitatRewardType.CaveExploring:
                        _habitatManager.CurrentHabitat.BuildFacility(FacilityType.Cave, true);
                        break;
                    case HabitatRewardType.RuneStone:
                        _habitatManager.CurrentHabitat.BuildFacility(FacilityType.RuneStone, true);
                        break;
                    case HabitatRewardType.Habitat:
                        _habitatManager.CurrentHabitat.UpgradeHabitatTier();
                        break;
                    default:
                        Debug.LogError($"Upgrade type is invalid [{_selectedExpedition.UpgradeType}], please change!");
                        break;
                }
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
                if (_currentEssenceProgress < _essenceExpeditions.Count - 1)
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
                if (_currentFossilProgress < _fossilExpeditions.Count - 1)
                {
                    ++_currentFossilProgress;
                }
                else
                {
                    Debug.LogWarning($"Fossil Expedition is at max rank [{_currentFossilProgress}]");
                }
                break;
            case ExpeditionType.HabitatUpgrade:
                _habitatExpeditionOption = null;
                if (_currentHabitatProgress < _habitatExpeditions.Count - 1)
                {
                    ++_currentHabitatProgress;
                }
                else
                {
                    Debug.LogWarning($"Habitat Expedition is at max rank [{_currentHabitatProgress}]");
                }
                break;
            default:
                Debug.LogError($"Expedition Type [{_selectedExpedition.Type}] is invalid.");
                break;
        }
        _habitatManager.SetExpeditionProgress(_currentEssenceProgress, _currentFossilProgress, _currentHabitatProgress);
    }

    public void ChimerasOnExpedition(bool onExpedition)
    {
        foreach (Chimera chimera in _chimeras)
        {
            chimera.RevealChimera(!onExpedition);
            chimera.SetOnExpedition(onExpedition);
        }

        _uiManager.HabitatUI.DetailsPanel.ToggleDetailsButtons(DetailsButtonType.Expedition);

        if (onExpedition == false)
        {
            _chimeras.Clear();
        }
    }
}