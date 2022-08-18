using System.Collections.Generic;
using UnityEngine;

public class ChimeraShop : MonoBehaviour
{
    private List<ChimeraShopItem> _chimeraShopItems = new List<ChimeraShopItem>();
    private Marketplace _marketplace = null;

    public ChimeraShopItem GetShopItem(ChimeraType chimeraType)
    {
        foreach (ChimeraShopItem chimeraShopItem in _chimeraShopItems)
        {
            if (chimeraShopItem.ChimeraType == chimeraType) return chimeraShopItem;
        }
        Debug.LogError($"Facility Shop Item of type {chimeraType} does not exist");
        return null;
    }
    public void Initialize(Marketplace marketplace)
    {
        foreach (Transform child in transform)
        {
            ChimeraShopItem shopItem = child.GetComponent<ChimeraShopItem>();

            _chimeraShopItems.Add(shopItem);
            shopItem.Initialize();
        }

        _marketplace = marketplace;
    }

    public void CheckIcons()
    {
        CheckShowIcon(ChimeraType.A);
        CheckShowIcon(ChimeraType.B);
        CheckShowIcon(ChimeraType.C);
    }

    private void CheckShowIcon(ChimeraType type)
    {
        if (_marketplace.IsChimeraUnlocked(type) == true)
        {
            GetShopItem(type).gameObject.SetActive(true);
        }
        else
        {
            GetShopItem(type).gameObject.SetActive(false);
        }
    }
    public void UpdateUI()
    {
        foreach (ChimeraShopItem shopItem in _chimeraShopItems)
        {
            shopItem.UpdateUI();
        }
    }
}