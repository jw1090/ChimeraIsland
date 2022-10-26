using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraShopItem : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private Chimera _chimera = null;

    [Header("References")]
    [SerializeField] private BuyChimeraButton _buyChimeraButton = null;
    [SerializeField] private Image _chimeraIcon = null;
    [SerializeField] private TextMeshProUGUI _name = null;

    private ResourceManager _resourceManager = null;
    private Habitat _habitat = null;

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;

        _name.text = _chimera.Name;
        _chimeraIcon.sprite = _resourceManager.GetChimeraSprite(_chimera.ChimeraType);
        _buyChimeraButton.Initialize(_chimera, _habitat);
    }
}