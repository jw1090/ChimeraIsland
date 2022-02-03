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
    [SerializeField] private Chimera[] chimera;
    [SerializeField] private Facility[] facilities;

    [Header("Stat Rates")]
    [SerializeField] private int baseExperienceRate = 1;
    [SerializeField] private int agilityExperienceRate = 0;
    [SerializeField] private int defenseExperienceRate = 0;
    [SerializeField] private int strengthExperienceRate = 0;
    [SerializeField] private int staminaExperienceRate = 0;
    [SerializeField] private int wisdomExperienceRate = 0;

    [Header("Tick Info")]
    [SerializeField] private float tickTimer = 60.0f;

    [Header("Facility Prefabs")]
    [SerializeField] private Facility bungeeCenote;
    [SerializeField] private Facility hikingTrail;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStatRates();
    }

    // TODO: Complete TickTimer
    // - Coroutine in the start loop.if active ( ) { do couroutine }
    // - Go into each chimera in the Chimera Array and grab the public get essence function and add it to currentEssence.
    // - Go into each chimera in the Chimera Array and call the TickExperience function to add into that chimera's personal stored stat experience .
    private IEnumerator TickTimer()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(tickTimer);
        }
    }

    // TODO: Complete AddFacility
    // - Instantiate a Facility prefab on a map at some set of xyz coordinates.
    // - Make sure only one can be active at a time.
    // -
    public void AddFacility(FacilityType facilityType)
    {

    }

    // - Made by: Joe 2/2/2022
    // - Helper function to calculate current stat rates. Looks at baseExperienceRate and each facilities stat's that they provide.
    // - Called on start and whenever a facility is added or upgraded.
    public void UpdateStatRates()
    {
        agilityExperienceRate = baseExperienceRate;
        strengthExperienceRate = baseExperienceRate;
        defenseExperienceRate = baseExperienceRate;
        staminaExperienceRate = baseExperienceRate;
        wisdomExperienceRate = baseExperienceRate;

        foreach (Facility facility in facilities)
        {
            if(facility != null)
            {
                switch (facility.GetStatType())
                {
                    case StatType.Agility:
                        agilityExperienceRate += facility.GetStatModifier();
                        break;
                    case StatType.Defense:
                        defenseExperienceRate += facility.GetStatModifier();
                        break;
                    case StatType.Strength:
                        strengthExperienceRate += facility.GetStatModifier();
                        break;
                    case StatType.Stamina:
                        staminaExperienceRate += facility.GetStatModifier();
                        break;
                    case StatType.Wisdom:
                        wisdomExperienceRate += facility.GetStatModifier();
                        break;
                }
            }
        }
    }

    // TODO: Complete UpgradeHabitatTier
    // - Used to upgrade tiers of the habitat 1 -> 2 -> 3 by pressing on the button.
    // - Spends essence stored in the Gamemanager.Instance.SpendEssence(amount).
    // - SpendEssence automatically check if you can afford and returns false if the purchase is not possible.
    public void UpgradeHabitatTier()
    {

    }
}