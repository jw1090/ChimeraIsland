using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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

    // - Made by: Joe 3/02/2022
    // - Adds a facility based on the assigned facilityType.
    public void OnPointerClick(PointerEventData eventData)
    {
        facilityShop.PurchaseFacility();
        GetComponentInChildren<TextMeshProUGUI>().text = facility.GetPrice().ToString();
    }
}