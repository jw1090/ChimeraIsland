using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private List<Facility> _facilities = new List<Facility>();
    [SerializeField] private HabitatType _habitatType = HabitatType.None;

    [Header("References")]
    [SerializeField] private GameObject _chimeraFolder = null;
    [SerializeField] private CrystalManager _crystalManager = null;
    [SerializeField] private GameObject _spawnPoint = null;
    [SerializeField] private PatrolNodes _patrolNodes = null;
    [SerializeField] private Environment _environment = null;
    [SerializeField] private StatefulObject _tiers = null;
    [SerializeField] private StatefulObject _temple = null;

    private ChimeraCreator _chimeraCreator = null;
    private CurrencyManager _currencyManager = null;
    private HabitatManager _habitatManager = null;
    private AudioManager _audioManager = null;
    private LightingManager _lightingManager = null;
    private List<Chimera> _activeChimeras = new List<Chimera>();
    private bool _isInitialized = false;
    private int _currentTier = 1;

    public StatefulObject Temple { get => _temple; }
    public int CurrentTier { get => _currentTier; }
    public CrystalManager CrystalManager { get => _crystalManager; }
    public Transform SpawnPoint { get => _spawnPoint.transform; }
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

    public FacilityType GetRandomAvailableFacilityType()
    {
        List<Facility> facilitiesToBuild = new List<Facility>();

        foreach (Facility facility in _facilities)
        {
            if (facility.IsBuilt == false)
            {
                facilitiesToBuild.Add(facility);
            }
        }

        int rand = Random.Range(0, facilitiesToBuild.Count);

        return facilitiesToBuild[rand].Type;
    }

    public void SetLightingManager(LightingManager lightingManager) { _lightingManager = lightingManager; }

    public void SetTier(int tier)
    {
        _currentTier = tier;
        LoadHabitatTier();
    }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        foreach (Facility facility in _facilities)
        {
            facility.SetExpeditionManager(expeditionManager);
        }

        _crystalManager.SetExpeditionManager(expeditionManager);
    }

    public void ToggleFireflies(bool toggleOn) { _environment.Tiers[_currentTier - 1].ToggleFireflies(toggleOn); }

    public Habitat Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _chimeraCreator = ServiceLocator.Get<ChimeraCreator>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _audioManager.SetHabitat(this);
        _crystalManager.Initialize(this);
        _patrolNodes.Initialize();
        _environment.Initialize();

        SetTier(_habitatManager.HabitatDataList[(int)Type].currentTier);

        foreach (Facility facility in _facilities)
        {
            facility.Initialize();
        }

        ToggleFireflies(false);

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

            facility.SetFacilityData(facilityInfo);
            for (int i = 0; i < facilityInfo.currentTier; ++i)
            {
                facility.BuildFacility();
            }
        }
    }

    public void MoveChimerasToFacility()
    {
        foreach (Facility facility in _facilities)
        {
            if (facility.LoadedFacilityData != null && facility.LoadedFacilityData.storedChimeraId != 0)
            {
                foreach (Chimera chimera in _activeChimeras)
                {
                    if (chimera.UniqueID == facility.LoadedFacilityData.storedChimeraId)
                    {
                        facility.PlaceChimeraFromPersistantData(chimera);
                    }
                }
            }
        }
    }

    public bool BuyChimera(Chimera chimeraPrefab)
    {
        if (_habitatManager.ChimeraCapacity == _activeChimeras.Count)
        {
            Debug.Log("You must increase the Chimera capacity to add more chimeras.");
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);
            return false;
        }

        int price = chimeraPrefab.Price;

        if (_currencyManager.SpendFossils(price) == false)
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);
            Debug.Log
            (
                $"Can't afford this chimera. It costs {price} " +
                $"Fossil and you only have {_currencyManager.Fossils} Fossils."
            );
            return false;
        }

        GameObject newChimera = _chimeraCreator.CreateChimeraByType(chimeraPrefab.ChimeraType);
        AddChimera(newChimera.transform);

        _habitatManager.AddNewChimera(newChimera.GetComponent<Chimera>());

        return true;
    }

    public void AddChimera(Transform newChimera)
    {
        newChimera.position = RandomSpawnPoint();
        newChimera.rotation = Quaternion.identity;
        newChimera.parent = _chimeraFolder.transform;

        Chimera chimeraComp = newChimera.GetComponent<Chimera>();
        _activeChimeras.Add(chimeraComp);

        chimeraComp.Initialize();
    }

    public Vector3 RandomSpawnPoint()
    {
        Vector3 spawnPos = _habitatManager.CurrentHabitat.SpawnPoint.transform.position;
        spawnPos.x = spawnPos.x + Random.Range(-2.0f, 2.0f);
        spawnPos.z = spawnPos.z + Random.Range(-2.0f, 2.0f);

        return spawnPos;
    }

    public bool TransferChimera(Chimera chimeraToTransfer, HabitatType habitatType)
    {
        chimeraToTransfer.SetHabitatType(habitatType);

        if (_habitatManager.AddNewChimera(chimeraToTransfer) == false)
        {
            chimeraToTransfer.SetHabitatType(_habitatType); // Transfer was not successful, reset habitatType.
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

    public void BuildFacility(FacilityType facilityType, bool moveCamera = false)
    {
        Facility facility = GetFacility(facilityType);

        facility.BuildFacility(moveCamera);
        _habitatManager.AddNewFacility(facility);
    }

    public bool BuyFacility(Facility facility)
    {
        if (facility.CurrentTier >= _currentTier)
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);
            Debug.Log($"Cannot increase facility tier until habitat is upgraded. Requires Habitat Tier {_currentTier + 1}.");
            return false;
        }

        if (_currencyManager.SpendEssence(facility.Price) == false)
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);
            Debug.Log
            (
                $"Can't afford this facility." +
                $"It costs {facility.Price} Essence and you" +
                $"only have {_currencyManager.Essence} Essence."
            );
            return false;
        }

        facility.BuildFacility(true);
        _habitatManager.AddNewFacility(facility);

        return true;
    }

    public void UpgradeHabitatTier()
    {
        if (_currentTier + 1 >= 4)
        {
            return;
        }

        ++_currentTier;
        _habitatManager.SetHabitatTier(_currentTier, Type);
        _audioManager.PlayHabitatMusic(_habitatType);
        _audioManager.PlayHabitatAmbient(_habitatType);
        LoadHabitatTier();
        EvaluateFireflies();
    }

    private void LoadHabitatTier()
    {
        switch (_currentTier)
        {
            case 1:
                _tiers.SetState("Tier 1", true);
                break;
            case 2:
                _tiers.SetState("Tier 2", true);
                break;
            case 3:
                _tiers.SetState("Tier 3", true);
                break;
            default:
                Debug.LogWarning($"Habitat tier [{_currentTier}] is invalid. Please fix!");
                --_currentTier;
                break;
        }
    }

    public void ActivateGlow(bool value)
    {
        foreach (Facility facility in _facilities)
        {
            if (facility.IsBuilt == true && facility.IsChimeraStored() == false)
            {
                facility.GlowObject.ActivateGlow(value);
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
                chimera.EnergyTick();
            }

            _crystalManager.SpawnTick();

            foreach (Facility facility in _facilities)
            {
                if (facility.IsBuilt)
                {
                    facility.FacilityTick();
                }
            }
        }
    }

    private void EvaluateFireflies()
    {
        if (_lightingManager.DayType == DayType.DayTime)
        {
            ToggleFireflies(false);
        }
        else
        {
            ToggleFireflies(true);
        }
    }
}