using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyFacility : MonoBehaviour, IPointerClickHandler
{
    [Header("Shop Reference")]
    [SerializeField] private FacilityShopElement _facilityShop;
    [SerializeField] private Facility _facility;

    public void Initialize(FacilityShopElement facilityShop, Facility facility)
    {
        _facilityShop = facilityShop;
        _facility = facility;
        GetComponentInChildren<TextMeshProUGUI>().text = _facility.GetPrice().ToString();
    }

    // Adds a facility based on the assigned facilityType.
    public void OnPointerClick(PointerEventData eventData)
    {
        _facilityShop.PurchaseFacility();
        GetComponentInChildren<TextMeshProUGUI>().text = _facility.GetPrice().ToString();
    }
}