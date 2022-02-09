using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private bool isActive = false;
    [SerializeField] private int costToActivate = 0;
    [SerializeField] private int habitatTier = 1;
    [SerializeField] private int costToUpgrade = 300;
    [SerializeField] private int chimeraCapacity = 1;
    [SerializeField] private int facilityCapacity = 2;
    [SerializeField] private Chimera[] chimeras;
    [SerializeField] private Facility[] facilities;

    [Header("Stat Rates")]
    [SerializeField] private int baseExperienceRate = 1;
    private int agilityExpRate = 0;
    private int defenseExpRate = 0;
    private int staminaExpRate = 0;
    private int strengthExpRate = 0;
    private int wisdomExpRate = 0;

    [Header("Tick Info")]
    [SerializeField] private float tickTimer = 60.0f;
    private Coroutine tickCoroutine;

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
                chimera.ChimeraTick(agilityExpRate, defenseExpRate, staminaExpRate, strengthExpRate, wisdomExpRate);
                //Debug.Log("Tick");
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

        Facility ToBuyFacility = GetFacility(facilityType);

        // Return if Facility already exists.
        if(ToBuyFacility.IsActive())
        {
            Debug.Log("Facility is already active.");
            return;
        }

        if (GameManager.Instance.SpendEssence(ToBuyFacility.GetPrice()) == false)
        {
            Debug.Log("Can't afford this facility. It costs " + 
                ToBuyFacility.GetPrice() + " Essence" + "and you only have " + 
                GameManager.Instance.GetEssence() + " Essence.");
            return;
        }

        ToBuyFacility.BuyFacility();

        UpdateStatRates();
    }

    // - Made by: Joe 2/2/2022
    // - Helper function to calculate current stat rates. Looks at baseExperienceRate and each facilities stat's that they provide.
    // - Called on start and whenever a facility is added.
    private void UpdateStatRates()
    {
        agilityExpRate = baseExperienceRate;
        defenseExpRate = baseExperienceRate;
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
                case StatType.Agility:
                    agilityExpRate += facility.GetStatModifier();
                    Debug.Log("Now gaining " + agilityExpRate + " agility per tick.");
                    break;
                case StatType.Defense:
                    defenseExpRate += facility.GetStatModifier();
                    Debug.Log("Now gaining " + defenseExpRate + " defense per tick.");
                    break;
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

        habitatTier++;

        Debug.Log("Habit upgraded to Tier: " + habitatTier);

        // Alternates upgrades.
        if (habitatTier % 2 == 0)
        {
            facilityCapacity++;
            Debug.Log("It can now hold " + facilityCapacity + " Facilities.");
        }
        else
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
}