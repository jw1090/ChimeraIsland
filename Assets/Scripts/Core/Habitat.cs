using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private bool isActive = false;
    //[SerializeField] private int costToActivate = 0;
    [SerializeField] private int habitatTier = 1;
    [SerializeField] private int costToUpgrade = 300;
    [SerializeField] private int chimeraCapacity = 1;
    [SerializeField] private int facilityCapacity = 2;
    [SerializeField] private List<Chimera> chimeras;
    [SerializeField] private List<Facility> facilities;
    [SerializeField] private PatrolNodeManager patrolNodeManager;

    [Header("Stat Rates")]
    [SerializeField] private int baseExperienceRate = 1;
    private int staminaExpRate = 0;
    private int strengthExpRate = 0;
    private int wisdomExpRate = 0;

    [Header("Tick Info")]
    [SerializeField] private float tickTimer = 60.0f;
    private Coroutine tickCoroutine;

    [Header("References")]
    [SerializeField] private GameObject chimeraFolder;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStatRates();
        tickCoroutine = StartCoroutine(TickTimer());
    }

    // - Made by: Joe 2/9/2022
    // - Coroutine in the start loop. If active, do the following.
    // - Go into each Chimera in the Chimera Array and call its ChimeraTap function. Pass the experience rates.
    private IEnumerator TickTimer()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(tickTimer);
            foreach(Chimera chimera in chimeras)
            {
                //chimera.ChimeraTick(staminaExpRate, strengthExpRate, wisdomExpRate);
                chimera.EssenceTick();
                //Debug.Log("Tick");
                chimera.ChimeraTap();
            }
            foreach(Facility facility in facilities)
            {
                if(facility.IsActive())
                {
                    facility.FacilityTick();
                }
            }
        }
    }

    // - Made by: Santiago and Joe 2/9/2022
    // - Buy Facility using GameManager to pay.
    // - Instantiate a Facility prefab on a map at some set of xyz coordinates.
    // - Make sure only one can be active at a time.
    public void AddFacility(FacilityType facilityType)
    {
        // Return if no room for another Facility.
        if (ActiveFacilitiesCount() >= facilityCapacity)
        {
            Debug.Log("Facility capacity is too small to add another Facility.");
            return;
        }

        Facility toBuyFacility = GetFacility(facilityType);

        if(toBuyFacility.GetTier() == 3)
        {
            Debug.Log("Facility is already at max tier.");
            return;
        }

        if (GameManager.Instance.SpendEssence(toBuyFacility.GetPrice()) == false)
        {
            Debug.Log
            (
                "Can't afford this facility. It costs " + 
                toBuyFacility.GetPrice() + " Essence and you only have " + 
                GameManager.Instance.GetEssence() + " Essence."
            );
            return;
        }

        toBuyFacility.BuyFacility();

        UpdateStatRates();
    }

    // - Made by: Joe 2/23/2022
    // - Called by the BuyEgg Script on a button to check price nad purchase an egg on the active habitat.
    // - Adds it to the chimera list of that habitat and instantiates it as well
    public void BuyEgg(Chimera egg)
    {
        // Return if no room for another Chimera.
        if (chimeraCapacity == chimeras.Count)
        {
            Debug.Log("You must increase the Chimera capacity to add more chimeras.");
            return;
        }

        int price = egg.GetPrice();

        if (GameManager.Instance.SpendEssence(price) == false)
        {
            Debug.Log("Can't afford this chimera. It costs " +
                price + " Essence and you only have " +
                GameManager.Instance.GetEssence() + " Essence.");
            return;
        }

        float boundsX = Random.Range(-8.0f, 7.0f);
        float boundsZ = Random.Range(-1.0f, 13.0f);

        Vector3 testLocation = new Vector3(boundsX, 0.0f, boundsZ); // TODO: Use camera and spawn there

        Chimera newEgg = Instantiate(egg, testLocation, Quaternion.identity, chimeraFolder.transform);

        chimeras.Add(newEgg);
    }

    // - Made by: Joe 2/2/2022
    // - Helper function to calculate current stat rates. Looks at baseExperienceRate and each facilities stat's that they provide.
    // - Called on start and whenever a facility is added.
    private void UpdateStatRates()
    {
        staminaExpRate = baseExperienceRate;
        strengthExpRate = baseExperienceRate;
        wisdomExpRate = baseExperienceRate;

        foreach (Facility facility in facilities)
        {
            if (facility.IsActive() == false)
            {
                continue;
            }

            switch (facility.GetStatType())
            {
                case StatType.Stamina:
                    staminaExpRate += facility.GetStatModifier();
                    Debug.Log("Now gaining " + staminaExpRate + " stamina per tick.");
                    break;
                case StatType.Strength:
                    strengthExpRate += facility.GetStatModifier();
                    Debug.Log("Now gaining " + strengthExpRate + " strength per tick.");
                    break;
                case StatType.Wisdom:
                    wisdomExpRate += facility.GetStatModifier();
                    Debug.Log("Now gaining " + wisdomExpRate + " wisdom per tick.");
                    break;
            }
        }

        if(tickCoroutine == null)
        {
            return;
        }

        StopCoroutine(tickCoroutine);
        tickCoroutine = StartCoroutine(TickTimer());
    }

    // - Made by: Santiago & Joe 2/9/2022
    // - Used to upgrade tiers of the habitat 1 -> 2 -> 3 -> 4 -> 5 by pressing on the button.
    // - Spends essence stored in the Gamemanager.Instance.SpendEssence(amount).
    // - SpendEssence automatically check if you can afford and returns false if the purchase is not possible.
    // - If % 2 increase facilityCap, else increase chimeraCap.
    public void UpgradeHabitatTier()
    {
        if(habitatTier >= 5)
        {
            Debug.Log("Already maxed at Tier " + habitatTier + ".");

            return;
        }

        if (GameManager.Instance.SpendEssence(costToUpgrade) == false)
        {
            Debug.Log("Can't afford an upgrade. " + costToUpgrade + " is too expensive.");
            return;
        }

        ++habitatTier;

        Debug.Log("Habit upgraded to Tier: " + habitatTier);

        // Alternates upgrades.
        if (habitatTier % 2 == 0) // Even levels increase Facility capacity.
        {
            facilityCapacity++;
            Debug.Log("It can now hold " + facilityCapacity + " Facilities.");
        }
        else //Even levels increase Chimera capacity.
        {
            chimeraCapacity++;
            Debug.Log("It can now hold " + chimeraCapacity + " Chimeras.");
        }
    }

    // - Look through array to find Facility.
    private Facility GetFacility(FacilityType facilityType)
    {
        foreach (Facility facility in facilities)
        {
            if (facility.GetFacilityType() == facilityType)
            {
                return facility;
            }
        }

        return null;
    }

    // - Made by: Joe 2/9/2022
    // - Coints how many facilities are active in the Habitat
    private int ActiveFacilitiesCount()
    {
        int facilityCount = 0;

        foreach (Facility facility in facilities)
        {
            if (facility.IsActive())
            {
                ++facilityCount;
            }
        }

        return facilityCount;
    }

    // - Made by: Joe 2/16/2022
    // - Used to evolve and link chimera to habitat
    public void EvolveSwap(ref Chimera child, ref Chimera adult)
    {
        for (int i = 0; i < chimeras.Count; ++i)
        {
            if(chimeras[i] == child)
            {
                chimeras[i] = adult;
                return;
            }
        }
    }

    public Facility FacilitySearch(FacilityType facilityType)
    {
        foreach(Facility facility in facilities)
        {
            if(facility.GetFacilityType() == facilityType)
            {
                return facility;
            }
        }

        return null;
    }

    public List<Chimera> GetChimeras() { return chimeras; }
    public List<Facility> GetFacilities() { return facilities; }
    public List<Transform> GetPatrolNodes() { return patrolNodeManager.GetPatrolNodes(); }
}