using TMPro;
using UnityEngine;

public class FacilityShop : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private FacilityType facilityType = FacilityType.None;
    [SerializeField] private Facility facility;

    private void Start()
    {
        facility = GameManager.Instance.GetActiveHabitat().FacilitySearch(facilityType);
    }

    // Handles the facility purchasing. Typically called by the BuyFacility script.
    public void PurchaseFacility()
    {
        GameManager.Instance.GetActiveHabitat().AddFacility(facilityType);
    }

    #region Getters & Setters
    public FacilityType GetFacilityType() { return facilityType; }
    public Facility GetFacility() { return facility; }
    #endregion
}