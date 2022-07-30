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

    public void Initialize(Chimera chimera, Habitat habitat)
    {
        _tmpText = GetComponentInChildren<TextMeshProUGUI>();

        _chimera = chimera;
        _habitat = habitat;
        _audioManager = ServiceLocator.Get<AudioManager>();
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
    }
}