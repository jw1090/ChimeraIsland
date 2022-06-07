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

        GetComponentInChildren<TextMeshProUGUI>().text = _facility.Price.ToString();
    }

    public void OnEnable()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = _facility.Price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _habitat.AddFacility(_facility);
        GetComponentInChildren<TextMeshProUGUI>().text = _facility.Price.ToString();
    }
}