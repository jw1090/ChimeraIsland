using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacilityShopItem : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private FacilityType _facilityType = FacilityType.None;

    [Header("References")]
    [SerializeField] private BuyFacilityButton _buyFacilityButton = null;
    [SerializeField] private Image _facilityStatIcon = null;
    [SerializeField] private TextMeshProUGUI _name = null;

    private ResourceManager _resourceManager = null;
    private Habitat _habitat = null;

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;

        _name.text = LoadName();
        _facilityStatIcon.sprite = _resourceManager.GetStatSprite(_facilityType);
        _buyFacilityButton.Initialize(_habitat, _facilityType);
    }

    private string LoadName()
    {
        switch (_facilityType)
        {
            case FacilityType.Cave:
                return "Strength";
            case FacilityType.RuneStone:
                return "Intelligence";
            case FacilityType.Waterfall:
                return "Agility";
            default:
                Debug.LogWarning($"{_facilityType} is invalid, please change");
                return "Error";
        }
    }
}