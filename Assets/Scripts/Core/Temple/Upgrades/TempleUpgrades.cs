using System.Collections.Generic;
using UnityEngine;

public class TempleUpgrades : MonoBehaviour
{
    [SerializeField] private int _tier2UpgradeCost = 5;
    [SerializeField] private int _tier3UpgradeCost = 10;
    [SerializeField] private List<UpgradeSections> _upgradeSections = new List<UpgradeSections>();

    private PersistentData _persistentData = null;
    private CurrencyManager _currencyManager = null;
    private HabitatManager _habitatManager = null;
    private AudioManager _audioManager = null;
    private UpgradeNode _lastSelectedUpgradeNode = null;

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

    public void ResetUpgradeNode() { _lastSelectedUpgradeNode = null; }
    public void SelectUpgradeNode(UpgradeNode upgradeNode)
    {
        _lastSelectedUpgradeNode = upgradeNode;
    }

    public void Initalize()
    {
        _persistentData = ServiceLocator.Get<PersistentData>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        foreach (UpgradeSections upgradeSection in _upgradeSections)
        {
            upgradeSection.Initialize();
        }
    }

    public void BuyUpgrade()
    {
        int price = GetPrice(_lastSelectedUpgradeNode.Tier);
        _currencyManager.SpendFossils(price);

        FacilityData facilityData = new FacilityData(_lastSelectedUpgradeNode);
        _habitatManager.AddNewFacility(facilityData);
        _habitatManager.AddToUpgradeQueue(facilityData.Type);

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