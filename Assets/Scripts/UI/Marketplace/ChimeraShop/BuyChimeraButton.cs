using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyChimeraButton : MonoBehaviour, IPointerClickHandler
{
    private Chimera _chimera = null;
    private Habitat _habitat = null;

    public void Initialize(Chimera chimera, Habitat habitat)
    {
        _chimera = chimera;
        _habitat = habitat;

        GetComponentInChildren<TextMeshProUGUI>().text = "1";
        //GetComponentInChildren<TextMeshProUGUI>().text = _chimera.Price.ToString(); TODO: Replace with chimera price update.
    }

    // Adds a chimera based on the assigned chimera prefab.
    public void OnPointerClick(PointerEventData eventData)
    {
        _habitat.BuyChimera(_chimera);
    }
}