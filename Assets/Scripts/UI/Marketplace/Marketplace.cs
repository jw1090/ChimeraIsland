using UnityEngine;

public class Marketplace : MonoBehaviour
{
    [SerializeField] TabGroup _tabGroup = null;
    [SerializeField] ChimeraShop _chimeraShop = null;
    [SerializeField] FacilityShop _facilityShop = null;

    public void Initialize()
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        _tabGroup.Initialize();
        _chimeraShop.Initialize();
        _facilityShop.Initialize();
    }
}