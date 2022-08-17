using UnityEngine;

public class Marketplace : MonoBehaviour
{
    [SerializeField] private TabGroup _tabGroup = null;
    [SerializeField] private ChimeraShop _chimeraShop = null;
    [SerializeField] private FacilityShop _facilityShop = null;
    [SerializeField] private bool _waterfallUnlocked = false;
    [SerializeField] private bool _runeUnlocked = false;
    [SerializeField] private bool _caveUnlocked = false;
    private HabitatManager _habitatManager = null;

    public bool GetFacilityUnlocked(FacilityType facilityType)
    {
        switch (facilityType)
        {
            case FacilityType.Cave:
                return _caveUnlocked;
            case FacilityType.RuneStone:
                return _runeUnlocked;
            case FacilityType.Waterfall:
                return _waterfallUnlocked;
            default:
                Debug.LogError($"Facility type {facilityType} does not exist");
                return false;
        }
    }

    public void SetFacilityUnlocked(FacilityType type)
    {
        switch (type)
        {
            case FacilityType.Cave:
                _caveUnlocked = true;
                break;
            case FacilityType.RuneStone:
                _runeUnlocked = true;
                break;
            case FacilityType.Waterfall:
                _waterfallUnlocked = true;
                break;
            default:
                Debug.LogError($"Facility type {type} does not exist");
                break;
        }
    }

    public void Initialize()
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        _tabGroup.Initialize();
        _chimeraShop.Initialize();
        _facilityShop.Initialize(this);
        _habitatManager = ServiceLocator.Get<HabitatManager>();
    }

    public bool ChimeraTabIsActive()
    {
        return _tabGroup.ChimeraTab.gameObject.activeSelf;
    }

    public void ChimeraTabSetActive(bool value)
    {
        _tabGroup.ChimeraTab.gameObject.SetActive(value);
    }

    public void FacilityTabCheckActive()
    {
        if(_habitatManager.CurrentHabitat.CurrentTier >= 2)
        {
            _tabGroup.FacilitiesTab.gameObject.SetActive(true);
            _facilityShop.CheckShowIcons();
        }
        else
        {
            _tabGroup.FacilitiesTab.gameObject.SetActive(false);
        }
    }

    public void UpdateShopUI()
    {
        _facilityShop.UpdateUI();
        _chimeraShop.UpdateUI();
    }

    
}