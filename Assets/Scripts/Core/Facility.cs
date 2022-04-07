using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private FacilityType facilityType = FacilityType.None;
    [SerializeField] private StatType statType = StatType.None;
    [SerializeField] private int currentTier = 0;
    [SerializeField] private int statModifier = 1;
    [SerializeField] private int price = 100;
    [SerializeField] private Chimera chimera;
    [SerializeField] private bool isActive = false;
    //public MoveState moveState;
    
    // - Made by: Joe 2/9/2022
    // - Logic for buying a facility. Enables mesh renderer which is used to visualize the game object.
    public void BuyFacility()
    {
        price = (int)(price * 7.5);
        ++currentTier;

        if (currentTier == 1)
        {

            //After Facilites Active
            //moveState.AddFacilitiesPos(this.transform);

            isActive = true;
            Debug.Log(facilityType + " was purchased!");
            Debug.Log(facilityType + " will generate an additional " + statModifier + " " + statType + " for Chimeras per tick!");
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            ++statModifier;

            Debug.Log(facilityType + " was increased to Tier " + currentTier + "!");
            Debug.Log(facilityType + " now generates an additional " + statModifier + " " + statType + " for Chimeras per tick!");
        }
    }

    public void FacilityTick()
    {
        if(chimera != null)
        { 
            chimera.ExperienceTick(statType,statModifier);
        }
    }

    #region Getters & Setters
    public FacilityType GetFacilityType() { return facilityType; }
    public StatType GetStatType() { return statType; }
    public int GetStatModifier() { return statModifier; }
    public int GetTier() { return currentTier; }
    public int GetPrice() { return price; }
    public bool IsActive() { return isActive; }
    #endregion
}