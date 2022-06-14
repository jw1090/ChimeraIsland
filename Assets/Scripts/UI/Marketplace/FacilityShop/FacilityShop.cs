using System.Collections.Generic;
using UnityEngine;

public class FacilityShop : MonoBehaviour
{
    private List<FacilityShopItem> _facilityShopItems = new List<FacilityShopItem>();

    public void Initialize()
    {
        foreach (Transform child in transform)
        {
            FacilityShopItem shopItem = child.GetComponent<FacilityShopItem>();

            _facilityShopItems.Add(shopItem);
            shopItem.Initialize();
        }
    }
}