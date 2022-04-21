using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyChimera : MonoBehaviour, IPointerClickHandler
{
    [Header("General Info")]
    [SerializeField] Chimera _chimera;

    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = _chimera.GetPrice().ToString();
    }

    // Adds a chimera based on the assigned chimera prefab.
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.GetActiveHabitat().BuyChimera(_chimera);
    }
}