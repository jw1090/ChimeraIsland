using System.Collections.Generic;
using UnityEngine;

public class TempleUpgrades : MonoBehaviour
{
    [SerializeField] private int _tier2UpgradeCost = 5;
    [SerializeField] private int _tier3UpgradeCost = 10;
    [SerializeField] private List<UpgradeSections> _upgradeSections = new List<UpgradeSections>();

    private HabitatManager _habitatManager = null;
    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;
    private PersistentData _persistentData = null;

    public int GetPrice(int tier)
    {
        if (tier == 2)
        {
            return _tier2UpgradeCost;
        }
        else
        {
            return _tier3UpgradeCost;
        }
    }

    public void Initalize()
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();

        foreach (UpgradeSections upgradeSection in _upgradeSections)
        {
            upgradeSection.Initialize();
        }
    }

    public void BuyUpgrade(UpgradeNode upgradeNode)
    {
        FacilityData facilityData = new FacilityData(upgradeNode);
        _habitatManager.AddNewFacility(facilityData);
        _habitatManager.AddToUpgradeQueue(facilityData.Type);

        switch (facilityData.Type)
        {
            case FacilityType.RuneStone:
                _uiManager.AlertText.CreateAlert($"You Have Built The Tier {facilityData.CurrentTier} Rune Stones!");
                break;
            case FacilityType.Cave:
            case FacilityType.Waterfall:
                _uiManager.AlertText.CreateAlert($"You Have Built The Tier {facilityData.CurrentTier} {facilityData.Type}!");
                break;
            default:
                Debug.LogError($"Invalid Facility Type {facilityData.Type}");
                break;
        }

        UpdateUpgradeNodes();

        _persistentData.SaveSessionData();

        _audioManager.PlayUISFX(SFXUIType.StoneDrag);
    }

    private void UpdateUpgradeNodes()
    {
        foreach (UpgradeSections upgradeSection in _upgradeSections)
        {
            upgradeSection.EvaluateUpgradeStates();
        }
    }
}