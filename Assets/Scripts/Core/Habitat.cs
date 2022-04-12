using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private bool isActive = false;
    [SerializeField] private int chimeraCapacity = 3;
    [SerializeField] private int facilityCapacity = 3;
    [SerializeField] private List<Chimera> chimeras;
    [SerializeField] private List<Facility> facilities;

    [Header("Tick Info")]
    [SerializeField] private float tickTimer = 60.0f;
    private Coroutine tickCoroutine;

    [Header("References")]
    [SerializeField] private GameObject chimeraFolder;
    [SerializeField] private GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
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
        Facility toBuyFacility = GetFacility(facilityType);

        // Return if no room for another Facility.
        if (ActiveFacilitiesCount() >= facilityCapacity && toBuyFacility.GetTier() == 0)
        {
            Debug.Log("Facility capacity is too small to add another Facility.");
            return;
        }

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
    }

    // - Made by: Joe 2/23/2022
    // - Called by the BuyChimera Script on a button to check price nad purchase an egg on the active habitat.
    // - Adds it to the chimera list of that habitat and instantiates it as well
    public void BuyChimera(Chimera chimeraPrefab)
    {
        // Return if no room for another Chimera.
        if (chimeraCapacity == chimeras.Count)
        {
            Debug.Log("You must increase the Chimera capacity to add more chimeras.");
            return;
        }

        int price = chimeraPrefab.GetPrice();

        if (GameManager.Instance.SpendEssence(price) == false)
        {
            Debug.Log
            (
                "Can't afford this chimera. It costs " +
                price + " Essence and you only have " +
                GameManager.Instance.GetEssence() + " Essence."
            );
            return;
        }

        Chimera newChimera = Instantiate(chimeraPrefab, spawnPoint.transform.localPosition, Quaternion.identity, chimeraFolder.transform);

        chimeras.Add(newChimera);
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
}