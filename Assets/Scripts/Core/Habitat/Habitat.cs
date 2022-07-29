using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private List<Facility> _facilities = new List<Facility>();
    [SerializeField] private HabitatType _habitatType = HabitatType.None;
    [SerializeField] private int _facilityCapacity = 3;

    [Header("References")]
    [SerializeField] private GameObject _chimeraFolder = null;
    [SerializeField] private GameObject _spawnPoint = null;
    [SerializeField] private PatrolNodes _patrolNodes = null;

    private ChimeraCreator _chimeraCreator = null;
    private CurrencyManager _currencyManager = null;
    private HabitatManager _habitatManager = null;
    private List<Chimera> _activeChimeras = new List<Chimera>();
    private bool _isInitialized = false;

    public List<Chimera> ActiveChimeras { get => _activeChimeras; }
    public List<Facility> Facilities { get => _facilities; }
    public List<Transform> PatrolNodes { get => _patrolNodes.Nodes; }
    public HabitatType Type { get => _habitatType; }

    public Facility GetFacility(FacilityType facilityType)
    {
        foreach (Facility facility in _facilities)
        {
            if (facility.Type == facilityType)
            {
                return facility;
            }
        }

        return null;
    }

    public Habitat Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _chimeraCreator = ServiceLocator.Get<ChimeraCreator>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        if (_patrolNodes == null)
        {
            Debug.LogError($"{this.GetType()}'s PatrolNodes ref is null. Please assign it from the list of Habitat's children in the hierarchy!");
            Debug.Break();
        }
        _patrolNodes.Initialize();

        foreach  (Facility facility in _facilities)
        {
            facility.Initialize();
        }

        _isInitialized = true;

        return this;
    }

    public void CreateChimerasFromData(List<ChimeraData> chimerasToSpawn)
    {
        foreach (var chimeraInfo in chimerasToSpawn)
        {
            var newChimera = _chimeraCreator.CreateChimera(chimeraInfo);

            AddChimera(newChimera.transform);
        }
    }

    public void CreateFacilitiesFromData(List<FacilityData> facilitiesToBuild)
    {
        foreach (var facilityInfo in facilitiesToBuild)
        {
            Facility facility = GetFacility(facilityInfo.facilityType);

            for (int i = 0; i < facilityInfo.currentTier; ++i)
            {
                facility.BuildFacility();
            }
        }
    }


    public void BuyChimera(Chimera chimeraPrefab)
    {
        if (_habitatManager.ChimeraCapacity == _activeChimeras.Count)
        {
            Debug.Log("You must increase the Chimera capacity to add more chimeras.");
            return;
        }

        //int price = chimeraPrefab.Price;  TODO Update price logic

        int price = 1;

        if (_currencyManager.SpendFossils(price) == false)
        {
            Debug.Log
            (
                $"Can't afford this chimera. It costs {price} " +
                $"Fossil and you only have {_currencyManager.Fossils} Fossils."
            );
            return;
        }

        GameObject newChimera = _chimeraCreator.CreateChimeraByType(chimeraPrefab.ChimeraType);
        AddChimera(newChimera.transform);

        _habitatManager.AddNewChimera(newChimera.GetComponent<Chimera>());
    }

    public void AddChimera(Transform newChimera)
    {
        Vector3 spawnPos = _spawnPoint.transform.localPosition;
        float spawnPointX = spawnPos.x + Random.Range(-5, 5);
        float spawnPointZ = spawnPos.z + Random.Range(-5, 5);

        newChimera.position = new Vector3(spawnPointX, spawnPos.y, spawnPointZ);
        newChimera.rotation = Quaternion.identity;
        newChimera.parent = _chimeraFolder.transform;

        Chimera chimeraComp = newChimera.GetComponent<Chimera>();
        _activeChimeras.Add(chimeraComp);
        chimeraComp.Initialize();
    }

    public bool TransferChimera(Chimera chimeraToTransfer, HabitatType habitatType)
    {
        chimeraToTransfer.SetHabitatType(habitatType);

        if(_habitatManager.AddNewChimera(chimeraToTransfer) == false)
        {
            chimeraToTransfer.SetHabitatType(_habitatType); // Transfer was not succeful, reset habitatType.
            return false;
        }

        RemoveChimera(chimeraToTransfer);
        return true;
    }

    private void RemoveChimera(Chimera chimeraToRemove)
    {
        _activeChimeras.Remove(chimeraToRemove);
        Destroy(chimeraToRemove.gameObject);
        _habitatManager.UpdateCurrentHabitatChimeras();
    }

    public void AddFacility(Facility facility)
    {
        // Return if no room for another Facility.
        if (FacilitiesCount() >= _facilityCapacity && facility.CurrentTier == 0)
        {
            Debug.Log("Facility capacity is too small to add another Facility.");
            return;
        }

        if (facility.CurrentTier == 3)
        {
            Debug.Log("Facility is already at max tier.");
            return;
        }

        if (_currencyManager.SpendEssence(facility.Price) == false)
        {
            Debug.Log
            (
                $"Can't afford this facility." +
                $"It costs {facility.Price} Essence and you" +
                $"only have {_currencyManager.Essence} Essence."
            );
            return;
        }

        facility.BuildFacility();
        _habitatManager.AddNewFacility(facility);
    }

    private int FacilitiesCount()
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

    public void ActivateGlow(bool value)
    {
        foreach (Facility facility in _facilities)
        {
            if (facility.CurrentTier > 0 && !facility.IsChimeraStored())
            {
                facility.GlowObject.enabled = value;
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
            yield return new WaitForSeconds(_habitatManager.TickTimer);

            foreach (Chimera chimera in _activeChimeras)
            {
                if (chimera.isActiveAndEnabled)
                {
                    chimera.EssenceTick();
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
}