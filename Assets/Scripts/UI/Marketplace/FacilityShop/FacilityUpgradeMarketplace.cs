using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityUpgradeMarketplace : MonoBehaviour
{
    [SerializeField] FacilityShopItem _facilityShopItemWaterfall = null;
    [SerializeField] FacilityShopItem _facilityShopItemRune = null;
    [SerializeField] FacilityShopItem _facilityShopItemCave = null;
    [SerializeField] private bool _waterfallUnlocked = false;
    [SerializeField] private bool _runeUnlocked = false;
    [SerializeField] private bool _caveUnlocked = false;
    private HabitatManager _habitatManager = null;
    private UIManager _uiManager = null;

    public FacilityShopItem GetShopItem(FacilityType facilityType)
    {
        switch (facilityType)
        {
            case FacilityType.Cave:
                return _facilityShopItemCave;
            case FacilityType.RuneStone:
                return _facilityShopItemRune;
            case FacilityType.Waterfall:
                return _facilityShopItemWaterfall;
            default:
                Debug.LogError($"GetShopItem tried to grab {facilityType} instead of available FacilityType");
                return null;
        }
    }

    public bool IsFacilityUnlocked(FacilityType facilityType)
    {
        switch (facilityType)
        {
            case FacilityType.Cave:
                return _caveUnlocked;
            case FacilityType.RuneStone:
                return _runeUnlocked;
            case FacilityType.Waterfall:
                return _waterfallUnlocked;
            default:
                Debug.LogError($"Facility type {facilityType} does not exist");
                return false;
        }
    }

    public bool CheckActive()
    {
        return _habitatManager.CurrentHabitat.CurrentTier >= 2;
    }

    public void SetFacilityUnlocked(FacilityType type)
    {
        switch (type)
        {
            case FacilityType.Cave:
                _caveUnlocked = true;
                break;
            case FacilityType.RuneStone:
                _runeUnlocked = true;
                break;
            case FacilityType.Waterfall:
                _waterfallUnlocked = true;
                break;
            default:
                Debug.LogError($"Facility type {type} does not exist");
                break;
        }
        _habitatManager.SetHabitatUIProgressFacility(_caveUnlocked, _runeUnlocked, _waterfallUnlocked);
    }

    public void Initialize(UIManager uiManager)
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        _habitatManager = ServiceLocator.Get<HabitatManager>();

        _uiManager = uiManager;

        HabitatData data = _habitatManager.HabitatDataList[(int)_habitatManager.CurrentHabitat.Type];
        _waterfallUnlocked = data.waterfallUnlocked;
        _runeUnlocked = data.runeUnlocked;
        _caveUnlocked = data.caveUnlocked;

        _facilityShopItemWaterfall.Initialize(_uiManager.HabitatUI);
        _facilityShopItemCave.Initialize(_uiManager.HabitatUI);
        _facilityShopItemRune.Initialize(_uiManager.HabitatUI);
    }

    public void CloseUI()
    {
        _facilityShopItemCave.gameObject.SetActive(false);
        _facilityShopItemRune.gameObject.SetActive(false);
        _facilityShopItemWaterfall.gameObject.SetActive(false);
    }

    public void ShowShop(FacilityType facilityType)
    {
        FacilityShopItem shopItem = GetShopItem(facilityType);
        Facility facility = _habitatManager.CurrentHabitat.GetFacility(shopItem.FacilityType);
        if (IsFacilityUnlocked(shopItem.FacilityType) == true)
        {
            shopItem.gameObject.SetActive(true);
            shopItem.Display(facility.CurrentTier >= _habitatManager.CurrentHabitat.CurrentTier);
        }
        else
        {
            shopItem.gameObject.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        _facilityShopItemRune.UpdateUI();
        _facilityShopItemWaterfall.UpdateUI();
        _facilityShopItemCave.UpdateUI();
    }
}
