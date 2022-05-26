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

    private List<Chimera> _activeChimeras = new List<Chimera>();
    private EssenceManager _essenceManager = null;
    private ChimeraCreator _chimeraCreator = null;
    private HabitatManager _habitatManager = null;
    private int _tickTracker = 0;
    private bool _isInitialized = false;

    public List<Chimera> ActiveChimeras { get => _activeChimeras; }
    public List<Transform> PatrolNodes { get => _patrolNodes.GetNodes(); }
    public HabitatType Type { get => _habitatType; }

    public Habitat Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _essenceManager = ServiceLocator.Get<EssenceManager>();
        _chimeraCreator = ServiceLocator.Get<ToolsManager>().ChimeraCreator;
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        if (_patrolNodes == null)
        {
            Debug.LogError(gameObject + "'s PatrolNodes ref is null. Please assign it from the list of Habitat's children in the hierarchy!");
            Debug.Break();
        }
        _patrolNodes.Initialize();

        _isInitialized = true;

        return this;
    }

    public void SpawnChimeras(List<ChimeraSaveData> chimerasToSpawn)
    {
        foreach (var chimeraInfo in chimerasToSpawn)
        {
            var newChimera = _chimeraCreator.CreateChimera(chimeraInfo);

            AddChimera(newChimera);
        }
    }

    public void ClearChimeras()
    {
        foreach (Chimera chimera in _chimeraFolder.GetComponentsInChildren<Chimera>())
        {
            Destroy(chimera.gameObject);
        }

        _activeChimeras.Clear();
    }

    public void StartTickTimer()
    {
        StartCoroutine(TickTimer());
    }

    // Coroutine in the start loop. If active, do the following.
    // Go into each Chimera in the Chimera Array and call its ChimeraTap function. Pass the experience rates.
    private IEnumerator TickTimer()
    {
        while (_isInitialized)
        {
            yield return new WaitForSeconds(_tickTimer);

            ++_tickTracker;

            foreach (Chimera chimera in _activeChimeras)
            {
                if (chimera.isActiveAndEnabled)
                {
                    chimera.EssenceTick();

                    if (_tickTracker % _unhappyRate == 0)
                    {
                        chimera.HappinessTick();
                        _tickTracker = 0;
                    }
                }
            }

            foreach (Facility facility in _facilities)
            {
                if (facility.IsInitialized)
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

        if (facility.CurrentTier == 3)
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

    // Called by the BuyChimera Script on a button to check price and purchase an egg on the active habitat.
    // Adds it to the chimera list of that habitat and instantiates it as well
    public void BuyChimera(Chimera chimeraPrefab)
    {
        if (_chimeraCapacity == _activeChimeras.Count)
        {
            Debug.Log("You must increase the Chimera capacity to add more chimeras.");
            return;
        }

        int price = chimeraPrefab.Price;

        if (_essenceManager.SpendEssence(price) == false)
        {
            Debug.Log
            (
                $"Can't afford this chimera. It costs {price} " +
                $"Essence and you only have {_essenceManager.CurrentEssence} Essence."
            );
            return;
        }

        GameObject newChimera = _chimeraCreator.CreateChimeraByType(chimeraPrefab.ChimeraType);
        AddChimera(newChimera);
        _habitatManager.AddChimeraToHabitat(newChimera.GetComponent<Chimera>(), _habitatType);
    }

    public void AddChimera(GameObject newChimera)
    {
        newChimera.transform.position = _spawnPoint.transform.localPosition;
        newChimera.transform.rotation = Quaternion.identity;
        newChimera.transform.parent = _chimeraFolder.transform;

        Chimera chimeraComp = newChimera.GetComponent<Chimera>();
        _activeChimeras.Add(chimeraComp);
        chimeraComp.Initialize();
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
            if (facility.IsInitialized)
            {
                ++facilityCount;
            }
        }

        return facilityCount;
    }
}