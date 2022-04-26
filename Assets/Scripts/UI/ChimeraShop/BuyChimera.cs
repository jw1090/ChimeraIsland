using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyChimera : MonoBehaviour, IPointerClickHandler
{
    [Header("General Info")]
    [SerializeField] Chimera chimera;

    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = chimera.GetPrice().ToString();
    }

    // Adds a chimera based on the assigned chimera prefab.
    public void OnPointerClick(PointerEventData eventData)
    {
        ServiceLocator.Get<Habitat>().BuyChimera(chimera);
    }
}