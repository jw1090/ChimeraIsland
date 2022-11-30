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
    private CameraUtil _cameraUtil = null;
    private UIManager _uiManager = null;
    private HabitatUI _habitatUI = null;
    private List<Chimera> _chimeras = new List<Chimera>();
    private ExpeditionUI _uiExpedition = null;
    private CurrencyManager _currencyManager = null;
    private HabitatManager _habitatManager = null;
    private AudioManager _audioManager = null;
    private TutorialManager _tutorialManager = null;
    private ExpeditionState _expeditionState = ExpeditionState.None;
    private const float _difficultyFlatModifier = 10.0f;
    private const float _difficultyScalar = 14.5f;
    private const float _difficultyExponent = 1.5f;
    private const float _powerScalar = 6.5f;
    private const float _rewardDenominator = 65.0f;
    private const float _rewardExponent = 0.9f;
    private const float _duartionDenominator = 300.0f;
    private const float _durationExponent = 0.45f;

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

    public bool HasChimeraBeenAdded(Chimera chimeraToFind) { return _chimeras.Contains(chimeraToFind); }

    public void SetExpeditionState(ExpeditionState expeditionState) 
    { 
        _expeditionState = expeditionState; 
        SetPortalColor();
    }

    public void SetPortalColor()
    {
        _habitatManager.CurrentHabitat.Environment.Portal.ChangePortal(_expeditionState, _uiExpedition.ExpeditionResult.ExpeditionSuccess);
    }

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
        _habitatUI = _uiManager.HabitatUI;
        _uiExpedition = _uiManager.HabitatUI.ExpeditionPanel;
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _cameraUtil = ServiceLocator.Get<CameraUtil>();

        SetExpeditionState(ExpeditionState.Selection);

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

        if (_selectedExpedition == null)
        {
            return;
        }

        if (_selectedExpedition.ActiveInProgressTimer == true)
        {
            InProgressTimerUpdate();
        }
    }

    public void LoadExpeditionOptions()
    {
        SetupExpeditionOption(ref _essenceExpeditionOption, ExpeditionType.Essence);
        SetupExpeditionOption(ref _fossilExpeditionOption, ExpeditionType.Fossils);
        SetupExpeditionOption(ref _habitatExpeditionOption, ExpeditionType.HabitatUpgrade);
    }

    private void SetupExpeditionOption(ref ExpeditionData expedition, ExpeditionType expeditionType)
    {
        if (expedition != null) // Already Created
        {
            return;
        }

        if (ExpeditionDataByType(expeditionType, out ExpeditionData newExpedition) == true)
        {
            AssignExpeditionUpgradeType(newExpedition.Modifiers);
            AnalyseRandomUpgrade(newExpedition);
            expedition = newExpedition;
        }
    }

    private bool ExpeditionDataByType(ExpeditionType expeditionType, out ExpeditionData newExpedition)
    {
        newExpedition = null;

        switch (expeditionType)
        {
            case ExpeditionType.Essence:
                newExpedition = _essenceExpeditions[_currentEssenceProgress].DeepCopy();
                return true;
            case ExpeditionType.Fossils:
                newExpedition = _fossilExpeditions[_currentFossilProgress].DeepCopy();
                return true;
            case ExpeditionType.HabitatUpgrade:
                if (_currentHabitatProgress >= _habitatExpeditions.Count) // Finished
                {
                    return false;
                }
                else
                {
                    newExpedition = _habitatExpeditions[_currentHabitatProgress].DeepCopy();
                    return true;
                }
            default:
                Debug.LogError($"Expedition Type [{expeditionType}] is invalid!");
                return false;
        }
    }

    private void AssignExpeditionUpgradeType(List<ModifierType> modifiers) // If a modifier is random, it will randomize it.
    {
        int modifierMax = (int)ModifierType.Max;

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

        SetExpeditionState(ExpeditionState.Setup);
        CalculateCurrentDifficultyValue();
        CalculateChimeraPower();
    }

    public void EnterInProgressState()
    {
        SetExpeditionState(ExpeditionState.InProgress);

        _uiExpedition.InProgressUI.SetupSliderInfo(_selectedExpedition.ActualDuration);
        _selectedExpedition.CurrentDuration = _selectedExpedition.ActualDuration;

        _selectedExpedition.ActiveInProgressTimer = true;
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
        chimera.DrainEnergy(_selectedExpedition.EnergyDrain);
        EvaluateRosterChange();

        _uiExpedition.SetupUI.ToggleConfirmButton(true);

        return true;
    }

    public bool RemoveChimera(Chimera chimera)
    {
        _chimeras.Remove(chimera);
        chimera.AddEnergy(_selectedExpedition.EnergyDrain);
        EvaluateRosterChange();

        if (_chimeras.Count == 0)
        {
            _uiExpedition.SetupUI.ToggleConfirmButton(false);
        }

        return true;
    }

    public void RemoveAllChimeras()
    {
        List<Chimera> tempChimeraList = new List<Chimera>();

        foreach (var chimera in _chimeras)
        {
            tempChimeraList.Add(chimera);
        }

        foreach (var chimera in tempChimeraList)
        {
            RemoveChimera(chimera);
        }

        _selectedExpedition = null;
        SetExpeditionState(ExpeditionState.Selection);
        _habitatUI.UpdateHabitatUI();
    }

    private void EvaluateRosterChange()
    {
        _uiExpedition.SetupUI.UpdateIcons();

        CalculateChimeraPower();
    }

    private void CalculateCurrentDifficultyValue()
    {
        float suggestedLevel = _selectedExpedition.Difficulty;
        float difficultyValue = Mathf.Pow(suggestedLevel, _difficultyExponent) * _difficultyScalar + _difficultyFlatModifier;

        _selectedExpedition.DifficultyValue = difficultyValue;
        _uiExpedition.SetupUI.UpdateDifficultyValue(difficultyValue);
    }

    private void CalculateChimeraPower()
    {
        float power = 0;

        CalculateModifiers();

        foreach (var chimera in _chimeras)
        {
            float elementalTypeModifier = ElementTypeModifier(chimera.ElementalType);

            power += chimera.Stamina * (_selectedExpedition.StaminaModifer + elementalTypeModifier) * _powerScalar;
            power += chimera.Wisdom * (_selectedExpedition.WisdomModifier + elementalTypeModifier) * _powerScalar;
            power += chimera.Exploration * (_selectedExpedition.ExplorationModifier + elementalTypeModifier) * _powerScalar;
        }

        CalculateRewardModifier();
        CalculateDurationModifier();

        _selectedExpedition.ChimeraPower = power >= _selectedExpedition.DifficultyValue ? _selectedExpedition.DifficultyValue : power;
        _uiExpedition.SetupUI.UpdateRewards(_selectedExpedition);
        _uiExpedition.SetupUI.UpdateDuration(_selectedExpedition);
        _uiExpedition.SetupUI.UpdateChimeraPower(_selectedExpedition.ChimeraPower);
    }

    private void CalculateModifiers()
    {
        _selectedExpedition.ResetMultipliersAndModifiers();

        foreach (ModifierType modifierType in _selectedExpedition.Modifiers)
        {
            switch (modifierType)
            {
                case ModifierType.None:
                    break;
                case ModifierType.Water:
                    _selectedExpedition.AquaBonus = 0.25f;
                    break;
                case ModifierType.Grass:
                    _selectedExpedition.BioBonus = 0.25f;
                    break;
                case ModifierType.Fire:
                    _selectedExpedition.FiraBonus = 0.25f;
                    break;
                case ModifierType.Exploration:
                    _selectedExpedition.ExplorationModifier = 1.5f;
                    break;
                case ModifierType.Stamina:
                    _selectedExpedition.StaminaModifer = 1.5f;
                    break;
                case ModifierType.Wisdom:
                    _selectedExpedition.WisdomModifier = 1.5f;
                    break;
                default:
                    Debug.LogWarning($"Modifier type is not valid [{modifierType}]. Please fix.");
                    break;
            }
        }
    }

    private void CalculateRewardModifier()
    {
        _selectedExpedition.RewardModifier = 0.0f;

        int totalPartyWisdom = 0;

        foreach (var chimera in _chimeras)
        {
            totalPartyWisdom += chimera.Wisdom;
        }

        if (totalPartyWisdom == 1) // 1 wisdom is the base so it should not give any benefit.
        {
            totalPartyWisdom = 0;
        }

        _selectedExpedition.RewardModifier = Mathf.Pow(totalPartyWisdom / _rewardDenominator, _rewardExponent);
    }

    private void CalculateDurationModifier()
    {
        _selectedExpedition.DurationModifier = 1.0f;

        int totalPartyExploration = 0;

        foreach (var chimera in _chimeras)
        {
            totalPartyExploration += chimera.Exploration;
        }

        if (totalPartyExploration == 1) // 1 wisdom is the base so it should not give any benefit.
        {
            totalPartyExploration = 0;
        }

        _selectedExpedition.DurationModifier = Mathf.Pow(totalPartyExploration / _duartionDenominator, _durationExponent);
    }

    private float ElementTypeModifier(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Water:
                return _selectedExpedition.AquaBonus;
            case ElementType.Grass:
                return _selectedExpedition.BioBonus;
            case ElementType.Fire:
                return _selectedExpedition.FiraBonus;
            default:
                Debug.LogWarning($"Modifier type is not valid [{elementType}]. Please fix.");
                return 0.0f;
        }
    }

    public float CalculateSuccessChance()
    {
        float successChance = 0.0f;

        Debug.Log
        (
            $"Chimera Power: {_selectedExpedition.ChimeraPower.ToString("F2")} | Difficulty Value: {_selectedExpedition.DifficultyValue.ToString("F2")}\n" +
            $"Success Chance: {successChance.ToString("F2")}"
        );

        if (Mathf.Approximately(_selectedExpedition.ChimeraPower, _selectedExpedition.DifficultyValue) == true)
        {
            successChance = 100.0f;
        }
        else
        {
            successChance = (_selectedExpedition.ChimeraPower / _selectedExpedition.DifficultyValue) * 100.0f;
        }

        return successChance;
    }

    private void InProgressTimerUpdate()
    {
        _selectedExpedition.CurrentDuration -= Time.deltaTime;
        _uiExpedition.InProgressUI.UpdateSliderInfo(_selectedExpedition.CurrentDuration);

        if (_selectedExpedition.CurrentDuration <= 0)
        {
            _selectedExpedition.CurrentDuration = 0;
            _selectedExpedition.ActiveInProgressTimer = false;

            _uiExpedition.TimerComplete();
        }
    }

    public bool RandomSuccesRate()
    {
        if (_selectedExpedition.AutoSucceed == true)
        {
            _audioManager.PlayUISFX(SFXUIType.Completion);
            Debug.Log($"Auto suceed activated, you win!");
            return true;
        }

        float successRoll = Random.Range(0.0f, _selectedExpedition.DifficultyValue);
        Debug.Log($"You rolled: {successRoll} | You needed: {_selectedExpedition.DifficultyValue - _selectedExpedition.ChimeraPower}");

        if (successRoll >= _selectedExpedition.DifficultyValue - _selectedExpedition.ChimeraPower)
        {
            _audioManager.PlayUISFX(SFXUIType.Completion);
            return true;
        }
        else
        {
            _audioManager.PlayUISFX(SFXUIType.Failure);
            return false;
        }
    }

    public void SuccessRewards()
    {
        switch (_selectedExpedition.Type)
        {
            case ExpeditionType.Essence:
                _currencyManager.IncreaseEssence(_selectedExpedition.ActualAmountGained);
                break;
            case ExpeditionType.Fossils:
                if (_habitatManager.CurrentHabitat.Temple.IsCompleted == false)
                {
                    _habitatManager.CurrentHabitat.Temple.Build();
                    _cameraUtil.TempleCameraShift();
                    _uiManager.HabitatUI.EnableUIElementByType(UIElementType.FossilsWallets);
                }

                _currencyManager.IncreaseFossils(_selectedExpedition.ActualAmountGained);
                _tutorialManager.ShowTutorialStage(TutorialStageType.FossilShop);
                break;
            case ExpeditionType.HabitatUpgrade:
                if (_currentHabitatProgress == 0)
                {
                    _uiManager.HabitatUI.EnableUIElementByType(UIElementType.EssenceWallets);
                    _habitatManager.CurrentHabitat.CrystalManager.TripleSpawn();
                }

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
                        _tutorialManager.ShowTutorialStage(TutorialStageType.FacilityUpgrades);
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
                if (_currentHabitatProgress < _habitatExpeditions.Count)
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

            if (onExpedition == true)
            {
                Vector3 positionAttempt = _habitatManager.CurrentHabitat.RandomDistanceFromPoint(_habitatManager.CurrentHabitat.SpawnPoint.position);

                chimera.gameObject.transform.position = positionAttempt;
            }
        }

        _habitatUI.UpdateHabitatUI();

        if (onExpedition == false)
        {
            _chimeras.Clear();
        }
    }

    public void CompleteCurrentUpgradeExpedition()
    {
        SetupExpeditionOption(ref _habitatExpeditionOption, ExpeditionType.HabitatUpgrade);

        if (_habitatExpeditionOption == null)
        {
            Debug.Log("You've finished all habitat expeditions");
            return;
        }

        _selectedExpedition = _habitatExpeditionOption;
        _uiExpedition.CompleteCurrentHabitatExpedition();
    }
}