using System;
using System.Collections.Generic;
using UnityEngine;

public class FacilityShop : MonoBehaviour
{
    private List<FacilityShopItem> _facilityShopItems = new List<FacilityShopItem>();
    private Marketplace _marketplace = null;
    private HabitatManager _habitatManager = null;
    public FacilityShopItem GetShopItem(FacilityType facilityType)
    {
        foreach(FacilityShopItem facilityShopItem in _facilityShopItems)
        {
            if (facilityShopItem.FacilityType == facilityType) return facilityShopItem;
        }
        Debug.LogError($"Facility Shop Item of type {facilityType} does not exist");
        return null;
    }

    public void Initialize(Marketplace marketplace)
    {
        foreach (Transform child in transform)
        {
            FacilityShopItem shopItem = child.GetComponent<FacilityShopItem>();

            _facilityShopItems.Add(shopItem);
            shopItem.Initialize();
        }
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _marketplace = marketplace;
    }

    public void CheckShowIcons()
    {
        //if facility is tier is less than habitat & facility unlocked
        foreach (FacilityType type in Enum.GetValues(typeof(FacilityType)))
        {
            if (type != FacilityType.None)
            {
                Facility facility = _habitatManager.CurrentHabitat.GetFacility(type);
                if (facility.CurrentTier < _habitatManager.CurrentHabitat.CurrentTier && _marketplace.IsFacilityUnlocked(type) == true)
                {
                    GetShopItem(type).gameObject.SetActive(true);
                }
                else
                {
                    GetShopItem(type).gameObject.SetActive(false);
                }
            }
        }
    }

    public void UpdateUI()
    {
        foreach (FacilityShopItem shopItem in _facilityShopItems)
        {
            shopItem.UpdateUI();
        }
    }
}