using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyChimeraButton : MonoBehaviour, IPointerClickHandler
{
    private Chimera _chimera = null;
    private Habitat _habitat = null;
    private AudioManager _audioManager = null;
    private TextMeshProUGUI _tmpText = null;
    private CameraUtil _cameraUtil = null;
    private HabitatUI _habitatUI = null;

    public void Initialize(Chimera chimera, Habitat habitat)
    {
        _audioManager = ServiceLocator.Get<AudioManager>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;
        _cameraUtil = ServiceLocator.Get<CameraUtil>();

        _tmpText = GetComponentInChildren<TextMeshProUGUI>();

        _chimera = chimera;
        _habitat = habitat;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _tmpText.text = "5"; // TODO: Replace with chimera price update.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_habitat.BuyChimera(_chimera) == true)
        {
            _audioManager.PlayUISFX(SFXUIType.PurchaseClick);
            _habitatUI.ResetStandardUI();
            _cameraUtil.ChimeraCameraShift();
        }
    }
}