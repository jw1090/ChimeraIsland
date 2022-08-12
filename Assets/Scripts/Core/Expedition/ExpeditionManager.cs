using System.Collections.Generic;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    [SerializeField] private List<ExpeditionData> _essenceExpeditions = new List<ExpeditionData>();
    [SerializeField] private List<ExpeditionData> _fossilExpeditions = new List<ExpeditionData>();
    [SerializeField] private List<ExpeditionData> _habitatExpeditions = new List<ExpeditionData>();
    [SerializeField] private int _currentEssenceExpedition = 0;
    [SerializeField] private int _currentFossilExpedition = 0;
    [SerializeField] private int _currentHabitatExpedition = 0;
    private List<Chimera> _chimeras = new List<Chimera>();
    private UIExpedition _uiExpedition = null;
    private CurrencyManager _currencyManager = null;
    private HabitatManager _habitatManager = null;
    private bool _activeInProgressTimer = false;
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
    private ExpeditionType _currentExpeditionType = ExpeditionType.None;

    public ExpeditionState State { get => _expeditionState; }
    public List<Chimera> Chimeras { get => _chimeras; }

    public ExpeditionData GetCurrentExpeditionData()
    {
        switch (_currentExpeditionType)
        {
            case ExpeditionType.Essence:
                return _essenceExpeditions[_currentEssenceExpedition];
            case ExpeditionType.Fossils:
                return _fossilExpeditions[_currentFossilExpedition];
            case ExpeditionType.HabitatUpgrade:
                return _habitatExpeditions[_currentHabitatExpedition];
            default:
                Debug.LogError($"Expedition Type [{_currentExpeditionType}] is invalid.");
                return null;
        }
    }

    public void SetExpeditionState(ExpeditionState expeditionState) { _expeditionState = expeditionState; }

    public ExpeditionManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _uiExpedition = ServiceLocator.Get<UIManager>().HabitatUI.ExpeditionPanel;
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _expeditionState = ExpeditionState.Setup;

        return this;
    }

    public void Update()
    {
        if (State != ExpeditionState.InProgress)
        {
            return;
        }

        if (_activeInProgressTimer == true)
        {
            InProgressTimerUpdate();
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
        _currentDuration = GetCurrentExpeditionData().duration;
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
        _uiExpedition.UpdateIcons(_chimeras);

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
        float minimumLevel = GetCurrentExpeditionData().suggestedLevel;
        float difficultyValue = Mathf.Pow(minimumLevel * 1.3f, 1.5f) * 15.0f;

        _difficultyValue = difficultyValue;

        _uiExpedition.UpdateDifficultValue(_difficultyValue);
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

        _uiExpedition.UpdateChimeraPower(_chimeraPower);
    }

    private void CalculateModifiers()
    {
        ResetMultipliers();

        foreach (ModifierType modifierType in GetCurrentExpeditionData().modifiers)
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
        _uiExpedition.SetInProgressTimeRemainingText(_currentDuration);

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
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SuccessRewards()
    {
        switch (GetCurrentExpeditionData().type)
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
                Debug.LogWarning($"Reward type is not valid [{GetCurrentExpeditionData().type}], please change!");
                break;
        }

        IncreaseCurrentExpedition();
    }

    public void IncreaseCurrentExpedition()
    {
        switch (_currentExpeditionType)
        {
            case ExpeditionType.Essence:
                if (_currentEssenceExpedition < _essenceExpeditions.Count - 1)
                {
                    ++_currentEssenceExpedition;
                }
                break;
            case ExpeditionType.Fossils:
                if (_currentFossilExpedition < _habitatExpeditions.Count - 1)
                {
                    ++_currentFossilExpedition;
                }
                break;
            case ExpeditionType.HabitatUpgrade:
                if (_currentHabitatExpedition < _habitatExpeditions.Count - 1)
                {
                    ++_currentHabitatExpedition;
                }
                break;
            default:
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
    }
}