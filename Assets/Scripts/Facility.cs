using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private FacilityType facilityType = FacilityType.None;
    [SerializeField] private StatType statType = StatType.None;
    [SerializeField] private int currentTier = 1;
    [SerializeField] private int statModifier = 1;
    [SerializeField] private int price = 100;

    public void UpgradeFacility()
    {

    }

    #region Getters & Setters
    public StatType GetStatType() { return statType; }
    public int GetStatModifier() { return statModifier; }
    public int GetTier() { return currentTier; }
    public int GetPrice() { return price; }
    #endregion
}