using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyFacilityButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _priceText = null;
    private Facility _facility = null;
    private Habitat _habitat = null;

    public void Initialize(Habitat habitat, FacilityType facilityType)
    {
        _habitat = habitat;
        _facility = _habitat.GetFacility(facilityType);

        _priceText.text = _facility.Price.ToString();
    }

    public void OnEnable()
    {
        _priceText.text = _facility.Price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _habitat.AddFacility(_facility);
        _priceText.text = _facility.Price.ToString();
    }
}