using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITraining : MonoBehaviour
{
    [SerializeField] private Color _validColor = Color.white;
    [SerializeField] private Color _badColor = Color.red;
    [SerializeField] private Button _screenWideOffButton = null;
    [SerializeField] private Button _increaseButton = null;
    [SerializeField] private Button _decreaseButton = null;
    [SerializeField] private Button _confirmButton = null;
    [SerializeField] private Button _declineButton = null;
    [SerializeField] private TextMeshProUGUI _statInfoText = null;
    [SerializeField] private TextMeshProUGUI _costText = null;
    [SerializeField] private Slider _slider = null;
    [SerializeField] private Image _sliderImage = null;

    private Facility _facility = null;
    private Chimera _chimera = null;
    private CurrencyManager _currencyManager = null;
    private HabitatUI _habitatUI = null;
    private UIManager _uiManager = null;
    private int _cost = 0;
    private int _levelGoal = 0;
    private int _attribute = 0;
    public Button IncreaseButton { get => _increaseButton; }
    public Button DecreaseButton { get => _decreaseButton; }
    public Button ConfirmButton { get => _confirmButton; }
    public Button DeclineButton { get => _declineButton; }

    public void Initialize(UIManager uiManager)
    {
        _currencyManager = ServiceLocator.Get<CurrencyManager>();

        _uiManager = uiManager;
        _habitatUI = _uiManager.HabitatUI;

        SetupUIListeners();
    }

    public void SetupUIListeners()
    {
        _uiManager.CreateButtonListener(_screenWideOffButton, ResetTrainingUI);
        _uiManager.CreateButtonListener(_increaseButton, IncreaseStatGoal);
        _uiManager.CreateButtonListener(_decreaseButton, DecreaseStatGoal);
        _uiManager.CreateButtonListener(_confirmButton, Confirm);
        _uiManager.CreateButtonListener(_declineButton, ResetTrainingUI);
    }

    public void SetupTrainingUI(Chimera chimera, Facility facility)
    {
        _chimera = chimera;
        _facility = facility;

        chimera.GetStatByType(_facility.StatType, out _attribute);

        _slider.minValue = _attribute;
        _slider.maxValue = 5 + _attribute;
        _levelGoal = _attribute + 1;

        if(_levelGoal > 5 + _attribute)
        {
            _statInfoText.text = "Your Chimera has reached the maximum level";
        }

        _slider.value = _levelGoal;

        DetermineCost();
    }

    public void DetermineCost()
    {
        int expNeeded = _chimera.GetEXPThresholdDifference(_facility.StatType, _levelGoal);
        int ticksRequired = (int)(expNeeded / _facility.StatModifier);

        if (expNeeded % (_facility.StatModifier) != 0)
        {
            ++ticksRequired;
        }

         _cost = ticksRequired * 5 * _facility.StatModifier;
        _costText.text = $"Cost: {_cost} Essence";

        if (_cost > _currencyManager.Essence)
        {
            _costText.color = _badColor;
            _sliderImage.color = _badColor;
        }
        else
        {
            _costText.color = Color.white;
            _sliderImage.color = _validColor;
        }

        _statInfoText.text = $" {_facility.StatType}: {_attribute} (+{_levelGoal - _attribute})";
    }

    public void DecreaseStatGoal()
    {
        if(_levelGoal <= _attribute + 1)
        {
            return;
        }

        --_levelGoal;
        _slider.value = _levelGoal;
        DetermineCost();
    }

    public void IncreaseStatGoal()
    {
        if (_levelGoal >= 5 + _attribute)
        {
            return;
        }

        ++_levelGoal;
        _slider.value = _levelGoal;
        DetermineCost();
    }

    public void ResetTrainingUI()
    {
        if(_facility != null)
        {
            if (_facility.IsChimeraStored() == true)
            {
                _facility.RemoveChimera();
                _habitatUI.RevealElementsHiddenByTraining();
            }
        }

        this.gameObject.SetActive(false);
    }

    public void Confirm()
    {
        if(_levelGoal > 5 + _attribute)
        {
            Debug.Log($"Level goal [{_levelGoal}] is too high.");
            return;
        }

        if(_levelGoal <= 1)
        {
            Debug.Log($"Level goal [{_levelGoal}] is too low.");
            return;
        }

        if (EssenceCost() == false)
        {
            return;
        }

        _facility.MyFacilityIcon.SetSliderAttributes(_attribute, _levelGoal);
        _facility.SetTrainToLevel(_levelGoal);
        _facility.SetActivateTraining(true);

        _facility.PlayTrainingSFX();

        _habitatUI.RevealElementsHiddenByTraining();
        this.gameObject.SetActive(false);
    }

    private bool EssenceCost()
    {
        if (_currencyManager.SpendEssence(_cost) == false)
        {
            Debug.Log($"Essence [{_currencyManager.Essence}] too low to afford.");
            return false;
        }

        return true;
    }
}