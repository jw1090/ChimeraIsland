using UnityEngine;
using UnityEngine.AI;

public class Facility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private FacilityType facilityType = FacilityType.None;
    [SerializeField] private StatType statType = StatType.None;
    [SerializeField] private int currentTier = 0;
    [SerializeField] private int statModifier = 1;
    [SerializeField] private int price = 100;
    [SerializeField] private bool isActive = false;

    [Header("References")]
    [SerializeField] private Chimera storedChimera = null;

    // Logic for buying a facility. Enables mesh renderer which is used to visualize the game object.
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

    // Called to properly link a chimera to a facility and adjust its states properly.
    public bool PlaceChimera(Chimera chimera)
    {
        if(storedChimera != null) // Something is already in the facility.
        {
            Debug.Log("Cannot add " + chimera + ". " + storedChimera + " is already in this facility.");
            return false;
        }

        storedChimera = chimera;
        storedChimera.SetInFacility(true);
        chimera.gameObject.transform.localPosition = this.gameObject.transform.localPosition;

        Debug.Log(storedChimera + " added to the facility.");
        return true;
    }

    // Removes Chimera from facility and cleans up chimera and facility logic.
    public bool RemoveChimera()
    {
        if(storedChimera == null) // Facility is empty.
        {
            Debug.Log("Cannot remove Chimera, facility is empty.");
            return false;
        }

        Debug.Log(storedChimera + " has been removed from the facility.");
        storedChimera.SetInFacility(false);
        NavMeshHit myNavHit;

        // Find nearby walkable position.
        if (NavMesh.SamplePosition(transform.position, out myNavHit, 100, -1))
        {
            storedChimera.transform.position = myNavHit.position;
        }
        storedChimera = null;

        return true;
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
            int happinessAmount = 1;
            if(storedChimera.GetPassive() == Passives.WorkMotivated)
            {
                happinessAmount = 2;
                storedChimera.ChangeHappiness(happinessAmount);
            }
            storedChimera.ChangeHappiness(happinessAmount);
        }
    }

    #region Getters & Setters
    public FacilityType GetFacilityType() { return facilityType; }
    public StatType GetStatType() { return statType; }
    public int GetStatModifier() { return statModifier; }
    public int GetTier() { return currentTier; }
    public int GetPrice() { return price; }
    public bool IsActive() { return isActive; }
    public bool IsChimeraStored()
    {
        if (isActive == false)
        {
            Debug.Log("This Facility is not active!");
            return isActive;
        }
        return storedChimera != null;
    }
    #endregion
}