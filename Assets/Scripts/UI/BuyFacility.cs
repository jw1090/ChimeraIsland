using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyFacility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] FacilityType facility = FacilityType.None;
    [SerializeField] Habitat habitat;
    [SerializeField] Button button;

    // - Made by: Joe 2/2/2022
    // - Uses an event listener to detect clicks.
    private void OnEnable()
    {
        button.onClick.AddListener(() => OnPointerClick());
    }

    // - Made by: Joe 2/2/2022
    // - Adds a facility based on the assigned facilityType.
    private void OnPointerClick()
    {
        habitat.AddFacility(facility);
        //Debug.Log(facility);    
    }
}