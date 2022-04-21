using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private bool _isActive = false;
    [SerializeField] private int _chimeraCapacity = 3;
    [SerializeField] private int _facilityCapacity = 3;
    [SerializeField] private float _unhappyRate = 10;
    [SerializeField] private List<Chimera> _chimeras;
    [SerializeField] private List<Facility> _facilities;

    [Header("Tick Info")]
    [SerializeField] private float _tickTimer = 0.2f;
    [SerializeField] private int _tickTracker = 0;

    [Header("References")]
    [SerializeField] private GameObject _chimeraFolder;
    [SerializeField] private GameObject _spawnPoint;
     private PatrolNodes _patrolNodes;

    public List<Chimera> GetChimeras() { return _chimeras; }
    public List<Transform> GetPatrolNodes() { return _patrolNodes.GetNodes(); }

    public void Initialize()
    {
        Debug.Log("<color=Orange> Initializing Habitat ... </color>");
        _isActive = true;

        _patrolNodes = GetComponentInChildren<PatrolNodes>();
        _patrolNodes.Initialize();

        StartCoroutine(TickTimer());

        InitializeChimeras();
    }

    private void InitializeChimeras()
    {
        foreach(Chimera chimera in _chimeraFolder.GetComponentsInChildren<Chimera>())
        {
            _chimeras.Add(chimera);
            chimera.Initialize(this);
        }
    }

    // Coroutine in the start loop. If active, do the following.
    // Go into each Chimera in the Chimera Array and call its ChimeraTap function. Pass the experience rates.
    private IEnumerator TickTimer()
    {
        while (_isActive)
        {
            yield return new WaitForSeconds(_tickTimer);

            ++_tickTracker;

            foreach(Chimera chimera in _chimeras)
            {
                if(chimera.isActiveAndEnabled)
                {
                    chimera.EssenceTick();
                    
                    if (_tickTracker % _unhappyRate == 0)
                    {
                        chimera.HappinessTick();
                        _tickTracker = 0;
                    }
                }
            }

            foreach(Facility facility in _facilities)
            {
                if(facility.IsActive())
                {
                    facility.FacilityTick();
                }
            }
        }
    }

    // Buy Facility using GameManager to pay.
    // Instantiate a Facility prefab on a map at some set of xyz coordinates.
    // Make sure only one can be active at a time.
    public void AddFacility(FacilityType facilityType)
    {
        Facility toBuyFacility = GetFacility(facilityType);

        // Return if no room for another Facility.
        if (ActiveFacilitiesCount() >= _facilityCapacity && toBuyFacility.GetTier() == 0)
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

    // Called by the BuyChimera Script on a button to check price nad purchase an egg on the active habitat.
    // Adds it to the chimera list of that habitat and instantiates it as well
    public void BuyChimera(Chimera chimeraPrefab)
    {
        // Return if no room for another Chimera.
        if (_chimeraCapacity == _chimeras.Count)
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

        Chimera newChimera = Instantiate(chimeraPrefab, _spawnPoint.transform.localPosition, Quaternion.identity, _chimeraFolder.transform);

        _chimeras.Add(newChimera);
        newChimera.Initialize(this);
    }

    // Look through array to find Facility.
    private Facility GetFacility(FacilityType facilityType)
    {
        foreach (Facility facility in _facilities)
        {
            if (facility.GetFacilityType() == facilityType)
            {
                return facility;
            }
        }

        return null;
    }

    // Coints how many facilities are active in the Habitat
    private int ActiveFacilitiesCount()
    {
        int facilityCount = 0;

        foreach (Facility facility in _facilities)
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
        foreach(Facility facility in _facilities)
        {
            if(facility.GetFacilityType() == facilityType)
            {
                return facility;
            }
        }

        return null;
    }
}