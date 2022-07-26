using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyChimeraButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip _purchaseClickSFX = null;
    private Chimera _chimera = null;
    private Habitat _habitat = null;
    private AudioManager _audioManager = null;

    public void Initialize(Chimera chimera, Habitat habitat)
    {
        _chimera = chimera;
        _habitat = habitat;
        _audioManager = ServiceLocator.Get<AudioManager>();
        GetComponentInChildren<TextMeshProUGUI>().text = "1";
        //GetComponentInChildren<TextMeshProUGUI>().text = _chimera.Price.ToString(); TODO: Replace with chimera price update.
    }

    // Adds a chimera based on the assigned chimera prefab.
    public void OnPointerClick(PointerEventData eventData)
    {
        _habitat.BuyChimera(_chimera);
        _audioManager.PlaySFX(_purchaseClickSFX);
    }
}