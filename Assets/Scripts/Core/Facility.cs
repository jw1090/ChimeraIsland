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
    [SerializeField] private Chimera storedChimera;
    [SerializeField] private bool isActive = false;

    // - Made by: Joe 2/9/2022
    // - Logic for buying a facility. Enables mesh renderer which is used to visualize the game object.
    public void BuyFacility()
    {
        price = (int)(price * 7.5);
        ++currentTier;

        if (currentTier == 1)
        {
            isActive = true;
            Debug.Log(facilityType + " was purchased!");

            // If it has a child, activate the fancy model, otherwise use the primative mesh.
            if(transform.childCount != 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            ++statModifier;
            Debug.Log(facilityType + " was increased to Tier " + currentTier + "!");
        }

        int newMod = statModifier + 1;

        Debug.Log(facilityType + " now generates " + newMod + " " + statType + " for Chimeras per tick!");
    }

    public void FacilityTick()
    {
        if(storedChimera != null)
        {
            storedChimera.ExperienceTick(statType, statModifier);
            FlatStatBoost();
            HappinessCheck();
        }
    }

    private void FlatStatBoost()
    {
        storedChimera.ExperienceTick(StatType.Endurance, 1);
        storedChimera.ExperienceTick(StatType.Intelligence, 1);
        storedChimera.ExperienceTick(StatType.Strength, 1);
    }

    private void HappinessCheck()
    {
        if(storedChimera.GetStatPreference() == statType)
        {
            storedChimera.IncreaseHappiness(1);
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