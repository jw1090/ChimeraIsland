using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyFacilityButton : MonoBehaviour, IPointerClickHandler
{
    private Facility _facility = null;
    private Habitat _habitat = null;

    public void Initialize(Habitat habitat, FacilityType facilityType)
    {
        _habitat = habitat;
        _facility = _habitat.GetFacility(facilityType);

        GetComponentInChildren<TextMeshProUGUI>().text = _facility.GetPrice().ToString();
    }

    // Adds a facility based on the assigned facilityType.
    public void OnPointerClick(PointerEventData eventData)
    {
        ServiceLocator.Get<Habitat>().AddFacility(_facility);
        GetComponentInChildren<TextMeshProUGUI>().text = _facility.GetPrice().ToString();
    }
}
