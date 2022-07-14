using UnityEngine;

public class Marketplace : MonoBehaviour
{
    [SerializeField] private TabGroup _tabGroup = null;
    [SerializeField] private ChimeraShop _chimeraShop = null;
    [SerializeField] private FacilityShop _facilityShop = null;
    public void Initialize()
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        _tabGroup.Initialize();
        _chimeraShop.Initialize();
        _facilityShop.Initialize();
    }

    public void ChimeraTabSetActive(bool value)
    {
        _tabGroup.ChimeraTab.gameObject.SetActive(value);
    }
}