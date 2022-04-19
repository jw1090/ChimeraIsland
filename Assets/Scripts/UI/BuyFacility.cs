using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyFacility : MonoBehaviour, IPointerClickHandler
{
    [Header("Shop Reference")]
    [SerializeField] private FacilityShop facilityShop;
    private Facility facility;

    private void Start()
    {
        facility = facilityShop.GetFacility();
        GetComponentInChildren<TextMeshProUGUI>().text = facility.GetPrice().ToString();
    }

    // Adds a facility based on the assigned facilityType.
    public void OnPointerClick(PointerEventData eventData)
    {
        facilityShop.PurchaseFacility();
        GetComponentInChildren<TextMeshProUGUI>().text = facility.GetPrice().ToString();
    }
}