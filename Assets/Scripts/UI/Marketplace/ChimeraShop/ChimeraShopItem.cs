using UnityEngine;

public class ChimeraShopItem : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private Chimera _chimera = null;
    private Habitat _habitat = null;

    [Header("References")]
    [SerializeField] private BuyChimeraButton _buyChimeraButton = null;

    public void Initialize()
    {
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _buyChimeraButton.Initialize(_chimera, _habitat);
    }
}