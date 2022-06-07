using UnityEngine;

public class FacilityShopItem : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private FacilityType _facilityType = FacilityType.None;
    private Habitat _habitat = null;

    [Header("References")]
    [SerializeField] private BuyFacilityButton _buyFacilityButton = null;

    public void Initialize()
    {
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _buyFacilityButton.Initialize(_habitat, _facilityType);
    }
}