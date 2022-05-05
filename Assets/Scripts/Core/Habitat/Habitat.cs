using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private HabitatType _habitatType = HabitatType.None;
    [SerializeField] private int _chimeraCapacity = 3;
    [SerializeField] private int _facilityCapacity = 3;
    [SerializeField] private float _unhappyRate = 5;
    [SerializeField] private List<Facility> _facilities = new List<Facility>();

    [Header("Tick Info")]
    [SerializeField] private float _tickTimer = 0.2f;

    [Header("References")]
    [SerializeField] private GameObject _chimeraFolder = null;
    [SerializeField] private GameObject _spawnPoint = null;
    [SerializeField] private PatrolNodes _patrolNodes = null;

    private EssenceManager _essenceManager = null;
    private int _tickTracker = 0;
    private bool _isActive = false;

    public List<Chimera> Chimeras { get; private set; } = new List<Chimera>();
    public int GetCapacity() { return _chimeraCapacity; }

    public HabitatType GetHabitatType() { return _habitatType; }
    public List<Transform> GetPatrolNodes() { return _patrolNodes.GetNodes(); }
    public void SetChimeraCapacity(int cap) { _chimeraCapacity = cap; }
    public Habitat Initialize()
    {
        Debug.Log("<color=Orange> Initializing Habitat ... </color>");
        _isActive = true;

        _essenceManager = ServiceLocator.Get<EssenceManager>();

        if (_patrolNodes == null)
        {
            Debug.LogError(gameObject + "'s PatrolNodes ref is null. Please assign it from the list of Habitat's children in the hierarchy!");
            Debug.Break();
        }
        _patrolNodes.Initialize();

        LoadChimeras();

        //FileManager fileManager = ServiceLocator.Get<FileManager>();
        //if(fileManager != null)
        //{
        //    SpawnChimeras();

        //    fileManager.CurrentHabitat = this;
        //    fileManager.LoadSavedData();
        //}

        UIManager uIManager = ServiceLocator.Get<UIManager>();
        if(uIManager != null)
        {
            uIManager.LoadMarketplace(this);
            uIManager.LoadDetails(this);
        }

        StartCoroutine(TickTimer());

        return this;
    }

    private void LoadChimeras()
    {
        HabitatManager habitatManager = ServiceLocator.Get<HabitatManager>();

        if (habitatManager == null)
        {
            return;
        }

        habitatManager.GetChimerasForHabitat(_habitatType);
    } 
    private void SpawnChimeras()
    {
        foreach(Chimera chimera in _chimeraFolder.GetComponentsInChildren<Chimera>())
        {
            Chimeras.Add(chimera);
            chimera.CreateChimera(this, _essenceManager);
        }
    }

    public void ClearChimeras()
    {
        foreach (Chimera chimera in _chimeraFolder.GetComponentsInChildren<Chimera>())
        {
            Destroy(chimera.gameObject);
        }

        Chimeras.Clear();
    }

    // Coroutine in the start loop. If active, do the following.
    // Go into each Chimera in the Chimera Array and call its ChimeraTap function. Pass the experience rates.
    private IEnumerator TickTimer()
    {
        while (_isActive)
        {
            yield return new WaitForSeconds(_tickTimer);

            ++_tickTracker;

            foreach(Chimera chimera in Chimeras)
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

    // Instantiate a Facility prefab on a map at some set of xyz coordinates.
    // Make sure only one can be active at a time.
    public void AddFacility(Facility facility)
    {
        // Return if no room for another Facility.
        if (ActiveFacilitiesCount() >= _facilityCapacity && facility.CurrentTier == 0)
        {
            Debug.Log("Facility capacity is too small to add another Facility.");
            return;
        }

        if(facility.CurrentTier == 3)
        {
            Debug.Log("Facility is already at max tier.");
            return;
        }

        if (_essenceManager.SpendEssence(facility.GetPrice()) == false)
        {
            Debug.Log
            (
                "Can't afford this facility. It costs " +
                facility.GetPrice() + " Essence and you only have " +
                _essenceManager.CurrentEssence + " Essence."
            );
            return;
        }

        facility.BuyFacility();
    }

    // Called by the BuyChimera Script on a button to check price nad purchase an egg on the active habitat.
    // Adds it to the chimera list of that habitat and instantiates it as well
    public void BuyChimera(Chimera chimeraPrefab)
    {
        if (_chimeraCapacity == Chimeras.Count)
        {
            Debug.Log("You must increase the Chimera capacity to add more chimeras.");
            return;
        }

        int price = chimeraPrefab.GetPrice();

        if (_essenceManager.SpendEssence(price) == false)
        {
            Debug.Log
            (
                "Can't afford this chimera. It costs " +
                price + " Essence and you only have " +
                _essenceManager.CurrentEssence + " Essence."
            );
            return;
        }
        AddChimera(chimeraPrefab);
    }
    public Chimera AddChimera(Chimera chimeraPrefab)
    {
        Chimera newChimera = Instantiate(chimeraPrefab, _spawnPoint.transform.localPosition, Quaternion.identity, _chimeraFolder.transform);

        Chimeras.Add(newChimera);
        newChimera.CreateChimera(this, _essenceManager);

        return newChimera;
    }
    
    public void KillCap()
    {
        for (int i = _chimeraCapacity; i < Chimeras.Count; ++i)
        {
            Destroy(Chimeras[i].gameObject);
            Chimeras.RemoveAt(i);
        }
    }
    public Facility GetFacility(FacilityType facilityType)
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
}
