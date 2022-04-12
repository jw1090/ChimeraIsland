using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuyEgg : MonoBehaviour, IPointerClickHandler
{
    [Header("General Info")]
    [SerializeField] Chimera chimera;

    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = chimera.GetPrice().ToString();
    }

    // - Made by: Joe 3/02/2022
    // - Adds an egg based on the assigned egg prefab.
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.GetActiveHabitat().BuyChimera(chimera);
    }
}