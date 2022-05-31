using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private List<Facility> _facilities = new List<Facility>();
    [SerializeField] private HabitatType _habitatType = HabitatType.None;
    [SerializeField] private float _unhappyRate = 5;
    [SerializeField] private int _facilityCapacity = 3;

    [Header("Tick Info")]
    [SerializeField] private float _tickTimer = 0.2f;

    [Header("References")]
    [SerializeField] private GameObject _chimeraFolder = null;
    [SerializeField] private GameObject _spawnPoint = null;
    [SerializeField] private PatrolNodes _patrolNodes = null;

    private ChimeraCreator _chimeraCreator = null;
    private EssenceManager _essenceManager = null;
    private HabitatManager _habitatManager = null;
    private List<Chimera> _activeChimeras = new List<Chimera>();
    private bool _isInitialized = false;
    private int _tickTracker = 0;

    public List<Chimera> ActiveChimeras { get => _activeChimeras; }
    public List<Transform> PatrolNodes { get => _patrolNodes.Nodes; }
    public HabitatType Type { get => _habitatType; }

    public Habitat Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _chimeraCreator = ServiceLocator.Get<ToolsManager>().ChimeraCreator;
        _essenceManager = ServiceLocator.Get<EssenceManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        if (_patrolNodes == null)
        {
            Debug.LogError($"{this.GetType()}'s PatrolNodes ref is null. Please assign it from the list of Habitat's children in the hierarchy!");
            Debug.Break();
        }
        _patrolNodes.Initialize();

        _isInitialized = true;

        return this;
    }

    public void SpawnChimeras(List<ChimeraData> chimerasToSpawn)
    {
        foreach (var chimeraInfo in chimerasToSpawn)
        {
            var newChimera = _chimeraCreator.CreateChimera(chimeraInfo);

            AddChimera(newChimera.transform);
        }
    }
    public void SpawnFacilities(FacilityData[] facilities)
    {
        foreach (Facility facility in _facilities)
        {
            for (int i = 0; i < facilities[(int)facility.GetFacilityType()].currentTier; i++)
            {
                facility.BuildFacility();
            }
        }
    }

    public void StartTickTimer()
    {
        StartCoroutine(TickTimer());
    }

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

        facility.BuildFacility();
        _habitatManager.UpdateCurrentHabitatFacilities(facility);
    }

    // Called by the BuyChimera Script on a button to check price and purchase an egg on the active habitat.
    // Adds it to the chimera list of that habitat and instantiates it as well
    public void BuyChimera(Chimera chimeraPrefab)
    {
        if (_habitatManager.ChimeraCapacity == _activeChimeras.Count)
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
        AddChimera(newChimera.transform);

        _habitatManager.AddNewChimera(newChimera.GetComponent<Chimera>());
    }

    public void AddChimera(Transform newChimera)
    {
        newChimera.position = _spawnPoint.transform.localPosition;
        newChimera.rotation = Quaternion.identity;
        newChimera.parent = _chimeraFolder.transform;

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