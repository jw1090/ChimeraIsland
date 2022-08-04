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
    private CameraController _cameraController = null;
    private HabitatUI _habitatUI = null;

    public void Initialize(Habitat habitat, FacilityType facilityType)
    {
        _audioManager = ServiceLocator.Get<AudioManager>();

        _tmpText = GetComponentInChildren<TextMeshProUGUI>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;
        _cameraController = ServiceLocator.Get<CameraController>();

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
        _habitat.AddFacility(_facility);
        _audioManager.PlayElementsSFX(ElementsSFX.PurchaseClick);
        switch (_habitat.Type)
        {
            case HabitatType.StonePlains:
                switch (_facility.Type)
                {
                    case FacilityType.Cave:
                        _cameraController.MoveCameraCoroutine(new Vector3(-18.0f, 20.0f, -7.0f), 0.25f);
                        break;
                    case FacilityType.Waterfall:
                        _cameraController.MoveCameraCoroutine(new Vector3(41.0f, 20.0f, 51.0f), 0.25f);
                        break;
                    case FacilityType.RuneStone:
                        _cameraController.MoveCameraCoroutine(new Vector3(16.0f, 20.0f, 39.0f), 0.25f);
                        break;
                    default:
                        Debug.Log("Facility type is null.");
                        break;
                }
                break;
            case HabitatType.TreeOfLife:
                switch (_facility.Type)
                {
                    case FacilityType.Cave:
                        _cameraController.MoveCameraCoroutine(new Vector3(16.0f, 24.0f, 44.0f), 0.5f);
                        break;
                    case FacilityType.Waterfall:
                        _cameraController.MoveCameraCoroutine(new Vector3(60.0f, 24.0f, 20.0f), 0.5f);
                        break;
                    case FacilityType.RuneStone:
                        _cameraController.MoveCameraCoroutine(new Vector3(-66.0f, 24.0f, 12.0f), 0.5f);
                        break;
                    default:
                        Debug.Log("Facility type is null.");
                        break;
                }
                break;
            default:
                Debug.Log("Habitat type shouldn't exist.");
                break;
        }

        _habitatUI.CloseMarketplace();
    }
}