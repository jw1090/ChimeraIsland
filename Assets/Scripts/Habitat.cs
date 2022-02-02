using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private bool isActive = false;
    [SerializeField] private int habitatTier = 1;
    [SerializeField] private int chimeraCapacity = 1;
    [SerializeField] private int facilityCapacity = 2;
    [SerializeField] private int costToActivate = 0;
    [SerializeField] private int costToBuy = 0;
    [SerializeField] private Chimera[] chimera;
    [SerializeField] private Facility[] facilities;

    [Header("Stat Bonus")]
    [SerializeField] private int baseExperience = 1;
    [SerializeField] private int agilityExperienceRate = 1;
    [SerializeField] private int strengthExperienceRate = 1;
    [SerializeField] private int defenseExperienceRate = 1;
    [SerializeField] private int staminaExperienceRate = 1;
    [SerializeField] private int wisdomExperienceRate = 1;

    [Header("Tick Info")]
    [SerializeField] private float tickTimer = 60f; 

    [Header("Resources")]
    [SerializeField] private float currentEssence;
    [SerializeField] private float experienceCap;
    [SerializeField] private float essenceCap; 
    [SerializeField] private float essenceRatio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //  - Update is called once per frame
    void Update()
    {
        
    }

    // TODO: Complete this function
    // - add current essence to GameManager.Instance().AddToWallet(amount)
    //  - Made by: Santiago 2/2/2022
    //  - Function to taping on the tick -> Harvest Essence
    private void HarvestEssence()
    {

    }

    // TODO: Complete this function
    //  - Instantiate a cube prefab on a map at some set of coordinates.
    //  - Make sure only one can be active at a time
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
    private void UpgradeTier()
    {

    }

    // TODO: Complete this function
    //  - Made by: Santiago 2/2/2022
    //  - Coroutine in the start loop.if active ( ) { do couroutine }
    //  - Go into each chimera in the Chimera Array and grab the public get essence function and add it to currentEssence.
    //  - Go into each chimera in the Chimera Array and call the TickExperience function to add into that chimera's personal stored stat experience .
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

    public float GetExperienceCap() { return experienceCap; }
}