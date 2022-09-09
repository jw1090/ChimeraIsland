using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacilityShopItem : MonoBehaviour
{
    [SerializeField] private FacilityType _facilityType = FacilityType.None;
    [SerializeField] private BuyFacilityButton _buyFacilityButton = null;
    [SerializeField] private Image _facilityStatIcon = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TextMeshProUGUI _tier = null;
    [SerializeField] private GameObject _defaultPanel = null;
    [SerializeField] private GameObject _soldOutPanel = null;
    private ResourceManager _resourceManager = null;
    private Habitat _habitat = null;
    private Facility _facility = null;
    private HabitatUI _habitatUI = null;

    public FacilityType FacilityType { get => _facilityType; }

    public void Initialize(HabitatUI habitatUI)
    {
        _habitatUI = habitatUI;
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;

        _facility = _habitat.GetFacility(_facilityType);

        _name.text = LoadName();
        _tier.text = $"T{_facility.CurrentTier + 1}";
        _facilityStatIcon.sprite = _resourceManager.GetStatSprite(_facilityType);

        _buyFacilityButton.Initialize(_habitat, _facilityType);
    }

    public void Display(bool soldOut)
    {
        _defaultPanel.SetActive(!soldOut);
        _buyFacilityButton.gameObject.SetActive(!soldOut);
        _soldOutPanel.SetActive(soldOut);
    }

    private string LoadName()
    {
        switch (_facilityType)
        {
            case FacilityType.Cave:
                return "Cave";
            case FacilityType.RuneStone:
                return "Rune Stones";
            case FacilityType.Waterfall:
                return "Waterfall";
            default:
                Debug.LogWarning($"{_facilityType} is invalid, please change");
                return "Error";
        }
    }

    public void UpdateUI()
    {
        _tier.text = $"T{_facility.CurrentTier + 1}";

        _buyFacilityButton.UpdateUI();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        _habitatUI.ResetStandardUI();
    }
}