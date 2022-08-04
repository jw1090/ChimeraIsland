using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyChimeraButton : MonoBehaviour, IPointerClickHandler
{
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
        if(_habitat.BuyChimera(_chimera) == true)
        {
            _audioManager.PlayElementsSFX(ElementsSFX.PurchaseClick);
            _habitatUI.ResetStandardUI();
            ChimeraCameraShift();
        }
    }

    private void ChimeraCameraShift()
    {
        switch (_habitat.Type)
        {
            case HabitatType.StonePlains:
                _cameraController.MoveCameraCoroutine(new Vector3(18.0f, 20.0f, 16.6f), 0.25f);
                break;
            case HabitatType.TreeOfLife:
                _cameraController.MoveCameraCoroutine(new Vector3(-5.6f, 24.0f, 5.5f), 0.5f);
                break;
            default:
                Debug.Log("Habitat type shouldn't exist.");
                break;
        }
    }
}