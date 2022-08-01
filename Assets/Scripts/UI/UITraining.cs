using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITraining : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Slider _slider;
    private Facility _myFacility;
    private Chimera _chimera;
    private CurrencyManager _currencyManager;
    int _myCost;
    int _levelTo;
    int attribute;
    public void Initialize(Facility facility)
    {
        _myFacility = facility;
        _title.text = $"{_myFacility.MyStatType} Training";
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
    }

    public void IntializeChimera(Chimera chimera)
    {
        _chimera = chimera;
        attribute = _chimera.GetAttribute(_myFacility.MyStatType);
        _slider.minValue = attribute;
        _slider.maxValue = attribute + 5;
        _levelTo = attribute + 1;
        if(_chimera.Level == 99)
        {
            _amount.text = "Your Chimera has reached the maximum level";
        }
        _slider.value = _levelTo;
        SetCost();
    }

    public void SetCost()
    {
        //every tick the chimera _stat modifier xp and is charged 5 * _statModifier
        int XpNeeded = _chimera.GetXPtoThreshold(_myFacility.MyStatType, _levelTo);
        int ticksTo = (int)(XpNeeded / _myFacility.StatModifier);
        if (XpNeeded % (_myFacility.StatModifier) != 0) ++ticksTo;
        _myCost = ticksTo * 5 * _myFacility.StatModifier;
        _cost.text = $"{_myCost} Essence";
        if (_myCost > _currencyManager.Essence)
        {
            _cost.color = Color.red;
        }
        else
        {
            _cost.color = Color.white;
        }
        _amount.text = $"{attribute} + {_levelTo - attribute}";
    }

    public void OnClickDecrease()
    {
        if(_levelTo > attribute + 1)
        {
            _levelTo--;
            _slider.value = _levelTo;
            SetCost();
        }
    }

    public void OnClickIncrease()
    {
        if (_levelTo < 5 + attribute)
        {
            _levelTo++;
            _slider.value = _levelTo;
            SetCost();
        }
    }

    public void OnClickExit()
    {
        _myFacility.RemoveChimera();
        this.gameObject.SetActive(false);
    }

    public void OnClickConfirm()
    {
        if(_chimera.Level < 99 && _levelTo > 1 && _currencyManager.Essence > _myCost)
        {
            _myFacility.MyFacilityIcon.setSliderAttributes(attribute, _levelTo);
            _myFacility.SetTrainToLevel(_levelTo);
            _myFacility.SetActivateTraining(true);
            this.gameObject.SetActive(false);
        }
    }

}
