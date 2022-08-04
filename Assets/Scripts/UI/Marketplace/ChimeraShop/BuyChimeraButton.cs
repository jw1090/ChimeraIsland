using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip _purchaseClickSFX = null;
    private Chimera _chimera = null;
    private Habitat _habitat = null;
    private AudioManager _audioManager = null;
    private TextMeshProUGUI _tmpText = null;
    private CameraController _cameraController = null;
    private HabitatUI _habitatUI = null;

    public void Initialize(Chimera chimera, Habitat habitat)
    {
        _audioManager = ServiceLocator.Get<AudioManager>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;
        _cameraController = ServiceLocator.Get<CameraController>();

        _tmpText = GetComponentInChildren<TextMeshProUGUI>();

        _chimera = chimera;
        _habitat = habitat;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _tmpText.text = "1"; // TODO: Replace with chimera price update.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _habitat.BuyChimera(_chimera);
        _audioManager.PlaySFX(_purchaseClickSFX);

        switch (_habitat.Type)
        {
            case HabitatType.StonePlains:
                _cameraController.CallMoveCameraToDesintation(new Vector3(18.0f, 20.0f, 16.6f), 0.25f);
                break;
            case HabitatType.TreeOfLife:
                _cameraController.CallMoveCameraToDesintation(new Vector3(-5.6f, 24.0f, 5.5f), 0.5f);
                break;
            default:
                Debug.Log("Habitat type shouldn't exist.");
                break;
        }

        _habitatUI.CloseMarketplace();
    }
}