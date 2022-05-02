using System.Collections.Generic;
using UnityEngine;

public class ChimeraShop : MonoBehaviour
{
    private List<ChimeraShopItem> _chimeraShopItems = new List<ChimeraShopItem>();

    public void Initialize(Habitat habitat)
    {
        foreach(Transform child in transform)
        {
            ChimeraShopItem shopItem = child.GetComponent<ChimeraShopItem>();

            _chimeraShopItems.Add(shopItem);
            shopItem.Initialize(habitat);
        }
    }
}
