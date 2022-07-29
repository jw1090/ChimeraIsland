using UnityEngine;
using UnityEngine.UI;

public class FacilitySign : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    private StatType _statType = StatType.None;
    private ResourceManager _resourceManager = null;

    public void Initialize(FacilityType facilityType)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();

        LoadIcon(facilityType);
    }

    private void LoadIcon(FacilityType facilityType)
    {
        switch (facilityType)
        {
            case FacilityType.Cave:
                _statType = StatType.Strength;
                break;
            case FacilityType.RuneStone:
                _statType = StatType.Intelligence;
                break;
            case FacilityType.Waterfall:
                _statType = StatType.Endurance;
                break;
            default:
                break;
        }

        _icon.sprite = _resourceManager.GetStatSprite(_statType);
    }
}