using System.Collections.Generic;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    [SerializeField] private List<ExpeditionData> _habitatExpeditions = new List<ExpeditionData>();
    [SerializeField] private int _currentExpedition = 0;
    private List<Chimera> _chimeras = new List<Chimera>();
    private UIExpedition _uiExpedition = null;
    private CurrencyManager _currencyManager = null;
    private HabitatManager _habitatManager = null;
    private float _difficultyValue = 0;
    private float _chimeraPower = 0;
    private float _agilityModifer = 1.0f;
    private float _intelligenceModifier = 1.0f;
    private float _strengthModifier = 1.0f;
    private float _currentDuration = 0.0f;
    public bool _activeInProgressTimer = false;
    private ExpeditionState _expeditionState = ExpeditionState.None;

    public ExpeditionState State { get => _expeditionState; }
    public ExpeditionData CurrentExpeditionData { get => _habitatExpeditions[_currentExpedition]; }

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
        ResetMultipliers();
        CalculateCurrentDifficultyValue();
        CalculateChimeraPower();
    }

    public void ClearChimeras()
    {
        _chimeras.Clear();
    }

    public void EnterInProgressState()
    {
        _expeditionState = ExpeditionState.InProgress;
        _currentDuration = CurrentExpeditionData.duration;
        _activeInProgressTimer = true;
    }

    public bool AddChimera(Chimera chimera)
    {
        if (chimera.Level >= CurrentExpeditionData.minimumLevel == false)
        {
            Debug.Log($"<color=Red>{chimera.name} is too low of a level. You must be at least level {CurrentExpeditionData.minimumLevel}.</color>");

            return false;
        }
        _chimeras.Add(chimera);
        _uiExpedition.UpdateIcons(_chimeras);

        CalculateChimeraPower();

        return true;
    }

    public bool RemoveChimera(Chimera chimera)
    {
        _chimeras.Remove(chimera);
        _uiExpedition.UpdateIcons(_chimeras);

        CalculateChimeraPower();

        return true;
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
        float minimumLevel = CurrentExpeditionData.minimumLevel;
        float difficultyValue = Mathf.Pow(minimumLevel * 1.3f, 1.5f) * 15.0f;

        _difficultyValue = difficultyValue;

        _uiExpedition.UpdateDifficultValue(_difficultyValue);
    }

    private void CalculateChimeraPower()
    {
        float power = 0;

        foreach (var chimera in _chimeras)
        {
            power += chimera.Agility * _agilityModifer * 7.5f;
            power += chimera.Intelligence * _intelligenceModifier * 7.5f;
            power += chimera.Strength * _strengthModifier * 7.5f;
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
            $"Chimera Power: {_chimeraPower.ToString("F2")} | Difficulty Value: { _difficultyValue.ToString("F2")}\n" +
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

        Debug.Log($"You rolled {successRoll} out of {_difficultyValue}");

        if(successRoll >= _chimeraPower)
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
        switch (CurrentExpeditionData.rewardType)
        {
            case ExpeditionRewardType.HabitatUpgrade:
                _habitatManager.CurrentHabitat.UpgradeHabitatTier();
                Debug.Log("Success! Your habitat upgraded!");
                break;
            case ExpeditionRewardType.Fossils:
                _currencyManager.IncreaseFossils(1);
                Debug.Log("Success! You recieved 1 Fossil!");
                break;
            default:
                Debug.LogWarning($"Reward type is not valid [{CurrentExpeditionData.rewardType}], please change!");
                break;
        }

        ++_currentExpedition;
    }
}