using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyEgg : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] Chimera egg;
    [SerializeField] Button button;

    // - Made by: Joe 2/23/2022
    // - Uses an event listener to detect clicks.
    private void OnEnable()
    {
        button.onClick.AddListener(() => OnPointerClick());
    }

    // - Made by: Joe 2/23/2022
    // - Adds a facility based on the assigned facilityType.
    private void OnPointerClick()
    {
        GameManager.Instance.GetActiveHabitat().BuyEgg(egg);
        //Debug.Log(facility);    
    }
}