using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private List<Facility> _facilities = new List<Facility>();

    [Header("References")]
    [SerializeField] private GameObject _chimeraFolder = null;
    [SerializeField] private CrystalManager _crystalManager = null;
    [SerializeField] private GameObject _spawnPoint = null;
    [SerializeField] private GameObject _templeSpawnPoint = null;
    [SerializeField] private PatrolNodes _patrolNodes = null;
    [SerializeField] private Environment _environment = null;
    [SerializeField] private TempleStructure _temple = null;
    [SerializeField] private TapVFX _tapVfx = null;

    private UIManager _uiManager = null;
    private ChimeraCreator _chimeraCreator = null;
    private HabitatManager _habitatManager = null;
    private AudioManager _audioManager = null;
    private LightingManager _lightingManager = null;
    private List<Chimera> _activeChimeras = new List<Chimera>();
    private bool _isInitialized = false;
    private int _currentTier = 1;

    public TapVFX TapVFX { get => _tapVfx; }
    public TempleStructure Temple { get => _temple; }
    public CrystalManager CrystalManager { get => _crystalManager; }
    public Transform SpawnPoint { get => _spawnPoint.transform; }
    public Transform TempleSpawnPoint { get => _templeSpawnPoint.transform; }
    public List<Chimera> ActiveChimeras { get => _activeChimeras; }
    public List<Facility> Facilities { get => _facilities; }
    public List<Transform> PatrolNodes { get => _patrolNodes.Nodes; }
    public int CurrentTier { get => _currentTier; }
    public Environment Environment { get => _environment; }

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

    public Chimera GetFirstChimera()
    {
        foreach (Chimera chimera in ActiveChimeras)
        {
            if (chimera.FirstChimera == true)
            {
                return chimera;
            }
        }
        Debug.LogError("Missing first chimera");
        return new Chimera();
    }

    public void SetLightingManager(LightingManager lightingManager) { _lightingManager = lightingManager; }

    public void SetTier(int tier)
    {
        _currentTier = tier;
        LoadHabitatTier();
    }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _crystalManager.SetExpeditionManager(expeditionManager);
    }

    public void ToggleFireflies(bool toggleOn) { _environment.ToggleFireflies(toggleOn); }

    public Habitat Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _chimeraCreator = ServiceLocator.Get<ChimeraCreator>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
        _uiManager = ServiceLocator.Get<UIManager>();

        _audioManager.SetHabitat(this);
        _tapVfx.SetAudioManager(_audioManager);
        _crystalManager.Initialize(this);
        _patrolNodes.Initialize();
        _environment.Initialize();

        SetTier(_habitatManager.HabitatData.CurrentTier);

        foreach (Facility facility in _facilities)
        {
            facility.Initialize(this);
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

            AddChimera(newChimera.transform, true);
        }
    }

    public void CreateFacilitiesFromData(List<FacilityData> facilitiesToBuild, Queue<FacilityType> upgradeQueue)
    {
        foreach (var facilityInfo in facilitiesToBuild)
        {
            Facility facility = GetFacility(facilityInfo.Type);

            int upgrades = 0;
            foreach (var upgrade in upgradeQueue)
            {
                if (facilityInfo.Type == upgrade)
                {
                    upgrades++;
                }
            }

            facility.SetFacilityData(facilityInfo);
            for (int i = 0; i < facilityInfo.CurrentTier - upgrades; ++i)
            {
                facility.BuildFacility();
            }
        }
    }

    public void MoveChimerasToFacility()
    {
        foreach (Facility facility in _facilities)
        {
            if (facility.LoadedFacilityData != null && facility.LoadedFacilityData.StoredChimeraId != 0)
            {
                foreach (Chimera chimera in _activeChimeras)
                {
                    if (chimera.UniqueID == facility.LoadedFacilityData.StoredChimeraId)
                    {
                        facility.PlaceChimeraFromPersistantData(chimera);
                    }
                }
            }
        }
    }

    public Vector3 GetRandomPatrolNode()
    {
        List<Transform> nodes = _patrolNodes.Nodes;
        int random = (int)(Random.value * 100.0f) % nodes.Count;
        return nodes[random].position;
    }

    public void AddChimera(Transform newChimera, bool loadingIn = false)
    {
        if (ActiveChimeras.Count == 0)
        {
            newChimera.position = RandomDistanceFromPoint(_habitatManager.CurrentHabitat.SpawnPoint.position);
        }
        else if (loadingIn == true)
        {
            newChimera.position = RandomDistanceFromPoint(GetRandomPatrolNode());
        }
        else
        {
            newChimera.position = RandomDistanceFromPoint(_habitatManager.CurrentHabitat.TempleSpawnPoint.position);
        }

        newChimera.rotation = Quaternion.identity;
        newChimera.parent = _chimeraFolder.transform;

        Chimera chimeraComp = newChimera.GetComponent<Chimera>();
        _activeChimeras.Add(chimeraComp);

        chimeraComp.Initialize();
    }

    public Vector3 RandomDistanceFromPoint(Vector3 spawnPoint)
    {
        spawnPoint.x = spawnPoint.x + Random.Range(-2.0f, 2.0f);
        spawnPoint.z = spawnPoint.z + Random.Range(-2.0f, 2.0f);

        if (NavMesh.SamplePosition(spawnPoint, out NavMeshHit navMeshHit, 1f, 1))
        {
            spawnPoint = new Vector3(navMeshHit.position.x, navMeshHit.position.y, navMeshHit.position.z);
        }

        return spawnPoint;
    }

    public void BuildFacility(FacilityType facilityType, bool moveCamera = false)
    {
        Facility facility = GetFacility(facilityType);
        if(moveCamera == true)
        {
            StartCoroutine(facility.BuildFacilityWithVFX());
        }
        else
        {
            facility.BuildFacility();
        }
        _habitatManager.AddNewFacility(facility);

        if (facilityType == FacilityType.RuneStone) // Enums don't have spaces
        {
            _uiManager.AlertText.CreateAlert($"You Have Unlocked The Tier {facility.CurrentTier + 1} Rune Stone Facility!");
        }
        else
        {
            _uiManager.AlertText.CreateAlert($"You Have Unlocked The Tier {facility.CurrentTier + 1}  {facilityType} Facility!");
        }
    }

    public void UpgradeHabitatTier()
    {
        if (_currentTier + 1 >= 4)
        {
            return;
        }

        ++_currentTier;

        _uiManager.AlertText.CreateAlert($"You Have Upgraded The Habitat To Tier {_currentTier}!");

        _habitatManager.SetHabitatTier(_currentTier);
        _audioManager.PlayHabitatMusic();
        _audioManager.PlayHabitatAmbient();

        LoadHabitatTier();
        EvaluateFireflies();
    }

    private void LoadHabitatTier()
    {
        switch (_currentTier)
        {
            case 1:
            case 2:
            case 3:
                _environment.SwitchTier(_currentTier);
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
        StartCoroutine(StandardTickTimer());
        StartCoroutine(FacilityTickTimer());
    }

    private IEnumerator StandardTickTimer()
    {
        while (_isInitialized)
        {
            yield return new WaitForSeconds(_habitatManager.TickTimer * 5);

            foreach (Chimera chimera in _activeChimeras)
            {
                chimera.EnergyTick();
            }

            _crystalManager.SpawnTick();
        }
    }

    private IEnumerator FacilityTickTimer()
    {
        while (_isInitialized)
        {
            yield return new WaitForSeconds(_habitatManager.TickTimer);

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