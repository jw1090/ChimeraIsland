using System.Collections.Generic;
using UnityEngine;

public class Marketplace : MonoBehaviour
{
    [SerializeField] private TabGroup _tabGroup = null;
    [SerializeField] private ChimeraShop _chimeraShop = null;
    [SerializeField] private FacilityShop _facilityShop = null;
    [SerializeField] private bool _waterfallUnlocked = false;
    [SerializeField] private bool _runeUnlocked = false;
    [SerializeField] private bool _caveUnlocked = false;
    [SerializeField] private bool _aUnlocked = false;
    [SerializeField] private bool _bUnlocked = false;
    [SerializeField] private bool _cUnlocked = false;
    private HabitatManager _habitatManager = null;
    private ExpeditionManager _expeditionManager = null;

    public bool IsFacilityUnlocked(FacilityType facilityType)
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

    public bool IsChimeraUnlocked(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                return _aUnlocked;
            case ChimeraType.B:
                return _bUnlocked;
            case ChimeraType.C:
                return _cUnlocked;
            default:
                Debug.LogError($"Facility type {chimeraType} does not exist");
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
        _chimeraShop.Initialize(this);
        _facilityShop.Initialize(this);
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _expeditionManager = ServiceLocator.Get<ExpeditionManager>();
    }

    public bool ChimeraTabIsActive()
    {
        return _tabGroup.ChimeraTab.gameObject.activeSelf;
    }

    public void ChimeraTabSetActive(bool value)
    {
        if (value == true && _expeditionManager.CurrentFossilProgress >= 1)
        {
            _tabGroup.ChimeraTab.gameObject.SetActive(true);
            _chimeraShop.CheckIcons();
        }
        else
        {
            _tabGroup.ChimeraTab.gameObject.SetActive(false);
        }
    }

    public void FacilityTabCheckActive()
    {
        if (_habitatManager.CurrentHabitat.CurrentTier >= 2)
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


    public void ActivateFacility(FacilityType type)
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

    public void ActivateChimera(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                _aUnlocked = true;
                break;
            case ChimeraType.B:
                _bUnlocked = true;
                break;
            case ChimeraType.C:
                _cUnlocked = true;
                break;
            default:
                Debug.LogError($"Facility type {chimeraType} does not exist");
                break;
        }
    }

    public ChimeraType ActivateRandomChimera()
    {
        List<ChimeraType> deactivatedChimeras = new List<ChimeraType>();

        if (_aUnlocked == false)
        {
            deactivatedChimeras.Add(ChimeraType.A);
        }
        if (_bUnlocked == false)
        {
            deactivatedChimeras.Add(ChimeraType.B);
        }
        if (_cUnlocked == false)
        {
            deactivatedChimeras.Add(ChimeraType.C);
        }

        if (deactivatedChimeras.Count == 0)
        {
            return ChimeraType.None;
        }

        int random = Random.Range(0, deactivatedChimeras.Count - 1);

        ActivateChimera(deactivatedChimeras[random]);

        return deactivatedChimeras[random];
    }

}