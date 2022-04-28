using UnityEngine;

public class FacilityShop : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private FacilityType facilityType = FacilityType.None;
    [SerializeField] private Facility facility;

    public FacilityType GetFacilityType() { return facilityType; }
    public Facility GetFacility() { return facility; }

    private void Start()
    {
        ServiceLocator.Get<Habitat>().GetFacility(facilityType);
    }

    // Handles the facility purchasing. Typically called by the BuyFacility script.
    public void PurchaseFacility()
    {
        ServiceLocator.Get<Habitat>().AddFacility(facilityType);
    }
}