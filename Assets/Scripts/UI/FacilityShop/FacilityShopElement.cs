using UnityEngine;

public class FacilityShopElement : MonoBehaviour
{
    [Header("Shop Info")]
    [SerializeField] private FacilityType _facilityType = FacilityType.None;
    [SerializeField] private Facility _facility;
    private BuyFacility _facilityButton;

    private void Start()
    {
        _facility = GameManager.Instance.GetActiveHabitat().FacilitySearch(_facilityType);
        _facilityButton = GetComponentInChildren<BuyFacility>();
        _facilityButton.Initialize(this, _facility);
    }

    // Handles the facility purchasing. Called by the BuyFacility script.
    public void PurchaseFacility()
    {
        GameManager.Instance.GetActiveHabitat().AddFacility(_facilityType);
    }

    #region Getters & Setters
    public FacilityType GetFacilityType() { return _facilityType; }
    public Facility GetFacility() { return _facility; }
    #endregion
}