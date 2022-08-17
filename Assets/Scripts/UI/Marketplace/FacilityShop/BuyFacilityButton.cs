using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BuyFacilityButton : MonoBehaviour, IPointerClickHandler
{
    private Facility _facility = null;
    private Habitat _habitat = null;
    private AudioManager _audioManager = null;
    private TextMeshProUGUI _tmpText = null;
    private CameraUtil _cameraUtil = null;
    private HabitatUI _habitatUI = null;

    public void Initialize(Habitat habitat, FacilityType facilityType)
    {
        _audioManager = ServiceLocator.Get<AudioManager>();

        _tmpText = GetComponentInChildren<TextMeshProUGUI>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;
        _cameraUtil = ServiceLocator.Get<CameraUtil>();

        _habitat = habitat;
        _facility = _habitat.GetFacility(facilityType);

        UpdateUI();
    }

    public void UpdateUI()
    {
        _tmpText.text = _facility.Price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_habitat.BuyFacility(_facility) == true) // Purchase Success
        {
            _audioManager.PlayUISFX(SFXUIType.PurchaseClick);
            _habitatUI.ResetStandardUI();
        }
    }
}