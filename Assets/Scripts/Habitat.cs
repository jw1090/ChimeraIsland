using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]

    //  - Made by: Santiago 2/2/2022
    //  - General Stuff for our habitat.

    [SerializeField] private bool isActive = false;
    [SerializeField] private int habitatTier;
    [SerializeField] private int chimeraCapacity;
    [SerializeField] private int facilityCapacity;
    [SerializeField] private Chimera[] chimera; 
    [SerializeField] private Facility[] facilities;
    [SerializeField] private int costToActivate;
    [SerializeField] private int costToBuy;
    

    [Header("Stat Bonus")]

    //  - Made by: Santiago 2/2/2022
    //  - Stats that affect the Chimera, and also keeps account for the current level, etc.

    [SerializeField] private int baseExperience = 1;
    [SerializeField] private int agilityExperienceRate = 1;
    [SerializeField] private int strengthExperienceRate = 1;
    [SerializeField] private int defenseExperienceRate = 1;
    [SerializeField] private int staminaExperienceRate = 1;
    [SerializeField] private int wisdomExperienceRate = 1;
    [SerializeField] private int storedAgilityExperience = 0;
    [SerializeField] private int storedStrenghtExperience = 0;
    [SerializeField] private int storedDefenseExperience = 0;
    [SerializeField] private int storedStaminaExperience = 0;
    [SerializeField] private int storedWisdomExperience = 1;


    [Header("Tick Info")]
    [SerializeField] private float tickTimer; //Actual Timer = 60s
    

    [Header("Resources")]

    //  - Made by: Santiago 2/2/2022
    //  - Prety straigth forward. esscenceRatio is used in the formula for the Essence.

    [SerializeField] private float currentEssence;
    [SerializeField] private float experienceCap;
    [SerializeField] private float essenceCap; 
    [SerializeField] private float essenceRatio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //  - Made by: Santiago 2/2/2022
    //  - Update is called once per frame
    void Update()
    {
        
    }

    //  - Made by: Santiago 2/2/2022
    //  - Function to taping on the tick -> Gets all the xp, essence,etc.
    private void Tap()
    {

    }

    //  - Made by: Santiago 2/2/2022
    //  - Adds Facilities to the Habitat. 
    private void AddFacility(Facility facility)
    {

    }

    //  - Made by: Santiago 2/2/2022
    //  - Adds Chimera to the habitat 
    private void AddChimera(Chimera chimera)
    {

    }

    //  - Made by: Santiago 2/2/2022
    //  - Accumulates essence for the habitat.
    private void EssenceAccumulator()
    {

    }
    //  - Made by: Santiago 2/2/2022
    //  - Same but with xp.
    private void ExperienceAccumulator()
    {

    }

    //  - Made by: Santiago 2/2/2022
    //  - Used to upgrade tiers of the habitat 1..,2..,3.
    private void UpgradeHabitatTier()
    {

    }

    //  - Made by: Santiago 2/2/2022
    //  - Times the tick.
    private IEnumerator TickTimer()
    {
        while(isActive)
        {
            yield return new WaitForSeconds(tickTimer);
        }
    }

    //  - Made by: Santiago 2/2/2022
    //  - Allow us to transfer chimeras through habitats 
    public void TransferChimera(Chimera chimera, Habitat habitat)
    {

    }






}