using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyFacilityButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip _purchaseClickSFX = null;
    private Facility _facility = null;
    private Habitat _habitat = null;
    private AudioManager _audioManager = null;
    private TextMeshProUGUI _tmpText = null;

    public void Initialize(Habitat habitat, FacilityType facilityType)
    {
        _habitat = habitat;
        _facility = _habitat.GetFacility(facilityType);
        _audioManager = ServiceLocator.Get<AudioManager>();
        _tmpText = GetComponentInChildren<TextMeshProUGUI>();
        _tmpText.text = _facility.Price.ToString();
    }

    public void OnEnable()
    {
        _tmpText.text = _facility?.Price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _habitat.AddFacility(_facility);
        _tmpText.text = _facility.Price.ToString();
        _audioManager.PlaySFX(_purchaseClickSFX);
    }
}