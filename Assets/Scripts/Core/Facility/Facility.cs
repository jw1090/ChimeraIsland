using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Facility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private FacilityType _facilityType = FacilityType.None;
    [SerializeField] private StatType _statType = StatType.None;

    [Header("Reference")]
    [SerializeField] private StatefulObject _tiers = null;
    [SerializeField] private GlowMarker _glowMarker = null;
    [SerializeField] private TrainingFacilityIcon _trainingIcon = null;
    [SerializeField] private FacilitySign _facilitySign = null;
    [SerializeField] private Transform _cameraTransitionNode = null;
    [SerializeField] private GameObject _tier1VFX = null;
    [SerializeField] private GameObject _tier2VFX = null;
    [SerializeField] private GameObject _tier3VFX = null;

    private CameraUtil _cameraUtil;
    private FacilitySFX _facilitySFX = null;
    private HabitatUI _habitatUI = null;
    private AudioManager _audioManager = null;
    private Chimera _storedChimera = null;
    private TutorialManager _tutorialManager = null;
    private UIManager _uiManager = null;
    private TrainingUI _uiTraining = null;
    private FacilityData _loadedFacilityData = null;
    private InputManager _inputManager = null;
    private Habitat _habitat = null;
    private bool _isBuilt = false;
    private bool _activateTraining = false;
    private int _currentTier = 0;
    private int _trainToLevel = 0;
    private int _experienceRate = 0;

    public Chimera StoredChimera { get => _storedChimera; }
    public FacilityData LoadedFacilityData { get => _loadedFacilityData; }
    public GlowMarker GlowObject { get => _glowMarker; }
    public TrainingFacilityIcon TrainingIcon { get => _trainingIcon; }
    public Transform CameraTransitionNode { get => _cameraTransitionNode; }
    public StatType StatType { get => _statType; }
    public FacilityType Type { get => _facilityType; }
    public bool ActivateTraining { get => _activateTraining; }
    public bool IsBuilt { get => _isBuilt; }
    public int CurrentTier { get => _currentTier; }
    public int TrainToLevel { get => _trainToLevel; }

    public void SetFacilityData(FacilityData data) { _loadedFacilityData = data; }
    public void SetActivateTraining(bool Active) { _activateTraining = Active; }
    public void SetTrainToLevel(int trainTo) { _trainToLevel = trainTo; }

    public void Initialize(Habitat habitat)
    {
        UnityEngine.Debug.Log($"<color=Cyan> Initializing {this.GetType()} ... </color>");

        _habitat = habitat;

        _audioManager = ServiceLocator.Get<AudioManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _cameraUtil = ServiceLocator.Get<CameraUtil>();
        _inputManager = ServiceLocator.Get<InputManager>();

        _habitatUI = _uiManager.HabitatUI;
        _uiTraining = _habitatUI.TrainingPanel;

        _glowMarker.Initialize(this);
        _trainingIcon.Initialize();

        _facilitySFX = GetComponent<FacilitySFX>();
        _trainingIcon.gameObject.SetActive(false);
        _tiers.SetState("Tier 0", true);

        _facilitySign.Initialize(_facilityType);
    }

    public bool IsChimeraStored()
    {
        if (_isBuilt == false)
        {
            return false;
        }

        if (_storedChimera == null)
        {
            return false;
        }

        return true;
    }

    public IEnumerator BuildFacilityWithVFX()
    {
        GameObject vfx;
        bool facilityBuilt = false;

        _cameraUtil.FacilityCameraShift(Type);

        yield return new WaitUntil(() => _cameraUtil.InTransition == false);
        _inputManager.SetInTransition(true);

        switch (_currentTier)
        {
            case 0:
                vfx = _tier1VFX;
                _isBuilt = true;
                break;
            case 1:
                vfx = _tier2VFX;
                break;
            case 2:
                vfx = _tier3VFX;
                break;
            default:
                UnityEngine.Debug.LogError($"facilityTier is not valid [{_currentTier+1}]!");
                yield break;
        }

        float stopwatch = 0.0f;
        vfx.SetActive(true);
        while (stopwatch < 5.0f)
        {
            if (facilityBuilt == false && stopwatch >= 3.0f)
            {
                facilityBuilt = true;
                BuildFacility();
                _tutorialManager.ShowTutorialStage(TutorialStageType.Facilities);
            }
            stopwatch += Time.deltaTime;
            yield return null;
        }
        vfx.SetActive(false);
        _inputManager.SetInTransition(false);
    }

    public void BuildFacility()
    {
        string debugString = "";

        if (_currentTier == 0)
        {
            debugString += $"{_facilityType} was purchased";

            _tiers.SetState("Tier 1", true);

            _facilitySFX.Initialize(this);
            _facilitySFX.BuildSFX();

            _isBuilt = true;
        }
        else
        {
            debugString += $"{_facilityType} was upgraded to Tier {_currentTier + 1}";

            if (_currentTier == 1)
            {
                _tiers.SetState("Tier 2", true);
            }
            else if (_currentTier == 2)
            {
                _tiers.SetState("Tier 3", true);
            }
        }

        ++_currentTier;

        if (Type == FacilityType.Waterfall)
        {
            _habitat.Environment.SwitchWaterfallTier(_currentTier);
        }

        UnityEngine.Debug.Log($" {debugString} and now generates {_currentTier} {_statType}!");
    }

    public bool PlaceChimeraFromUI(Chimera chimera)
    {
        if (_storedChimera != null) // Something is already in the facility.
        {
            UnityEngine.Debug.Log($"Cannot add {chimera}. {_storedChimera} is already in this facility.");
            return false;
        }

        _audioManager.PlayUISFX(SFXUIType.PlaceChimera);
        _habitatUI.ResetStandardUI();
        _habitatUI.OpenTrainingPanel();
        _uiTraining.SetupTrainingUI(chimera, this);

        _trainingIcon.gameObject.SetActive(true);
        _trainingIcon.SetIcon(chimera.ChimeraIcon);

        StoreChimera(chimera);

        return true;
    }

    public bool PlaceChimeraFromPersistantData(Chimera chimera)
    {
        if (_storedChimera != null) // Something is already in the facility.
        {
            UnityEngine.Debug.Log($"Cannot add {chimera}. {_storedChimera} is already in this facility.");
            return false;
        }

        _trainingIcon.SetSliderAttributes(0, _loadedFacilityData.SliderMax);
        _trainingIcon.SetSliderValue(_loadedFacilityData.SliderValue);

        SetTrainToLevel(_loadedFacilityData.TrainToLevel);
        SetActivateTraining(true);
        chimera.SetEXPByType(_statType, _loadedFacilityData.ChimeraStatEXP);

        StoreChimera(chimera);

        return true;
    }

    private void StoreChimera(Chimera chimera)
    {
        _trainingIcon.gameObject.SetActive(true);
        _trainingIcon.SetIcon(chimera.ChimeraIcon);

        _storedChimera = chimera;
        _storedChimera.SetInFacility(true);
        _storedChimera.gameObject.transform.position = _glowMarker.transform.position;

        _storedChimera.RevealChimera(false);

        CalculateExperienceRate();

        UnityEngine.Debug.Log($"{_storedChimera} added to the facility.");
    }

    // Removes Chimera from facility and cleans up chimera and facility logic.
    public void RemoveChimera()
    {
        if (_storedChimera == null) // Facility is empty.
        {
            UnityEngine.Debug.LogWarning("Cannot remove Chimera, facility is empty.");
            return;
        }

        _activateTraining = false;
        if (_inputManager.IsHolding == true)
        {
            _glowMarker.ActivateGlow(true);
        }

        _trainingIcon.ResetIcon();
        _trainingIcon.gameObject.SetActive(false);

        _storedChimera.SetInFacility(false);

        _storedChimera.RevealChimera(true);

        if (_storedChimera.ReadyToEvolve == true)
        {
            _storedChimera.SetEvolutionIconActive();
        }

        _audioManager.PlayUISFX(SFXUIType.RemoveChimera);
        _facilitySFX.StopSFX();

        UnityEngine.Debug.Log($"{_storedChimera} has been removed from the facility.");

        _storedChimera = null;

        _habitatUI.UpdateHabitatUI();
    }

    public void FacilityTick()
    {
        if (_storedChimera == null)
        {
            return;
        }

        if (_activateTraining == false) // Training has not been confirmed yet.
        {
            return;
        }

        int currentStatAmount = _storedChimera.GetCurrentStatAmount(_statType);
        if (currentStatAmount >= _trainToLevel)
        {
            RemoveChimera();
            return;
        }

        _trainingIcon.UpdateSlider(_experienceRate);
        _storedChimera.ExperienceTick(_statType, _experienceRate);
    }

    private void CalculateExperienceRate()
    {
        int staminaBonus = Mathf.CeilToInt(_storedChimera.Stamina / 5.0f);

        int evolutionBonusSpeed = 0;
        if (_storedChimera.EvolutionBonusStat == _statType)
        {
            ++_experienceRate;
        }

        _experienceRate = _currentTier + _habitat.CurrentTier + staminaBonus + evolutionBonusSpeed;
    }

    public void PlayTrainingSFX()
    {
        if (ActivateTraining == true)
        {
            _facilitySFX.PlaySFX();
            _audioManager.PlayUISFX(SFXUIType.PlaceChimera);
        }
    }
}