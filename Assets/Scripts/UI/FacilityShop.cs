using TMPro;
using UnityEngine;

public class FacilityShop : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private FacilityType facilityType = FacilityType.None;
    [SerializeField] private Facility facility;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI tierText;

    private void Start()
    {
        facility = GameManager.Instance.GetActiveHabitat().FacilitySearch(facilityType);
        tierText.text = "T" + (facility.GetTier() + 1).ToString();
    }

    // Handles the facility purchasing. Typically called by the BuyFacility script.
    public void PurchaseFacility()
    {
        GameManager.Instance.GetActiveHabitat().AddFacility(facilityType);
        tierText.text = "T" + (facility.GetTier() + 1).ToString();
    }

    #region Getters & Setters
    public FacilityType GetFacilityType() { return facilityType; }
    public Facility GetFacility() { return facility; }
    #endregion
}