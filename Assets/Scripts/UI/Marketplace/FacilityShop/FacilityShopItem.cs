using UnityEngine;

public class FacilityShopItem : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private FacilityType _facilityType = FacilityType.None;

    [Header("References")]
    [SerializeField] private BuyFacilityButton _buyFacilityButton = null;

    public void Initialize(Habitat habitat)
    {
        _buyFacilityButton.Initialize(habitat, _facilityType);
    }
}