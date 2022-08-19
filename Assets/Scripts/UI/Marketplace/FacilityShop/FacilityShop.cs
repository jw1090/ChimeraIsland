using System;
using System.Collections.Generic;
using UnityEngine;

public class FacilityShop : MonoBehaviour
{
    [SerializeField] private GameObject _itemsFolder = null;
    private StatefulObject _statefulMenu = null;
    private List<FacilityShopItem> _facilityShopItems = new List<FacilityShopItem>();
    private Marketplace _marketplace = null;
    private HabitatManager _habitatManager = null;

    public FacilityShopItem GetShopItem(FacilityType facilityType)
    {
        foreach (FacilityShopItem facilityShopItem in _facilityShopItems)
        {
            if (facilityShopItem.FacilityType == facilityType) return facilityShopItem;
        }
        Debug.LogError($"Facility Shop Item of type {facilityType} does not exist");
        return null;
    }

    public void Initialize(Marketplace marketplace)
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        _statefulMenu = GetComponent<StatefulObject>();
        _marketplace = marketplace;

        _statefulMenu.SetState("Sold Out", true);

        foreach (Transform child in _itemsFolder.transform)
        {
            FacilityShopItem shopItem = child.GetComponent<FacilityShopItem>();

            _facilityShopItems.Add(shopItem);
            shopItem.Initialize();
        }

    }

    public void CheckShowIcons()
    {
        bool soldOut = true;

        foreach (FacilityShopItem shopItem in _facilityShopItems)
        {
            Facility facility = _habitatManager.CurrentHabitat.GetFacility(shopItem.FacilityType);
            if (facility.CurrentTier < _habitatManager.CurrentHabitat.CurrentTier && _marketplace.IsFacilityUnlocked(shopItem.FacilityType) == true)
            {
                shopItem.gameObject.SetActive(true);
                _statefulMenu.SetState("Items");

                soldOut = false;
            }
            else
            {
                shopItem.gameObject.SetActive(false);
            }
        }

        if (soldOut == true)
        {
            _statefulMenu.SetState("Sold Out");
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