using UnityEngine;

public class Facility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private FacilityType _facilityType = FacilityType.None;
    [SerializeField] private StatType _statType = StatType.None;
    [SerializeField] private int _statModifier = 1;
    [SerializeField] private int _price = 30;

    [Header("Reference")]
    [SerializeField] private StatefulObject _tiers = null;
    [SerializeField] private GlowMarker _glowMarker = null;
    [SerializeField] private TrainingFacilityIcon _trainingIcon = null;
    [SerializeField] private FacilitySign _facilitySign = null;
    [SerializeField] private Transform _cameraTransitionNode = null;

    private CameraUtil _cameraUtil;
    private FacilitySFX _facilitySFX = null;
    private HabitatUI _habitatUI = null;
    private AudioManager _audioManager = null;
    private Chimera _storedChimera = null;
    private TutorialManager _tutorialManager = null;
    private UIManager _uiManager = null;
    private TrainingUI _uiTraining = null;
    private bool _isBuilt = false;
    private int _currentTier = 0;
    private int _trainToLevel = 0;
    private bool _activateTraining = false;
    private FacilityData _loadedFacilityData = null;

    public Chimera StoredChimera { get => _storedChimera; }
    public FacilityData LoadedFacilityData { get => _loadedFacilityData; }
    public GlowMarker GlowObject { get => _glowMarker; }
    public TrainingFacilityIcon TrainingIcon { get => _trainingIcon; }
    public Transform CameraTransitionNode { get => _cameraTransitionNode; }
    public StatType StatType { get => _statType; }
    public FacilityType Type { get => _facilityType; }
    public bool ActivateTraining { get => _activateTraining; }
    public bool IsBuilt { get => _isBuilt; }
    public int StatModifier { get => _statModifier; }
    public int CurrentTier { get => _currentTier; }
    public int Price { get => _price; }
    public int TrainToLevel { get => _trainToLevel; }

    public void SetFacilityData(FacilityData data) { _loadedFacilityData = data; }
    public void SetTrainToLevel(int trainTo) { _trainToLevel = trainTo; }
    public void SetActivateTraining(bool Active) { _activateTraining = Active; }

    public void Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing {this.GetType()} ... </color>");

        _audioManager = ServiceLocator.Get<AudioManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _cameraUtil = ServiceLocator.Get<CameraUtil>();

        _habitatUI = _uiManager.HabitatUI;
        _uiTraining = _habitatUI.TrainingPanel;

        _glowMarker.Initialize(this);
        _trainingIcon.Initialize();

        _facilitySFX = GetComponent<FacilitySFX>();
        _trainingIcon.gameObject.SetActive(false);
        _tiers.SetState("Tier 0");

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

    public void BuildFacility(bool moveCamera = false)
    {
        string debugString = "";

        if (_currentTier == 0)
        {
            _habitatUI.Marketplace.SetFacilityUnlocked(_facilityType);

            debugString += $"{_facilityType} was purchased";

            _tiers.SetState("Tier 1");

            _glowMarker.ActivateGlowCollider(true);

            _facilitySFX.Initialize(this);
            _facilitySFX.BuildSFX();

            _isBuilt = true;
        }
        else
        {
            _price = (int)(_price * 4.0f);
            ++_statModifier;
            debugString += $"{_facilityType} was upgraded to Tier {_currentTier + 1}";
        }

        ++_currentTier;
        _habitatUI.UpdateShopUI();

        if (moveCamera == true)
        {
            _cameraUtil.FacilityCameraShift(Type);
            _tutorialManager.ShowTutorialStage(TutorialStageType.Facilities);
        }

        Debug.Log($" {debugString} and now generates {_statModifier} {_statType}!");
    }

    public bool PlaceChimeraFromUI(Chimera chimera)
    {
        if (_storedChimera != null) // Something is already in the facility.
        {
            Debug.Log($"Cannot add {chimera}. {_storedChimera} is already in this facility.");
            return false;
        }

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
            Debug.Log($"Cannot add {chimera}. {_storedChimera} is already in this facility.");
            return false;
        }

        _trainingIcon.SetSliderAttributes(0, _loadedFacilityData.sliderMax);
        _trainingIcon.SetSliderValue(_loadedFacilityData.sliderValue);

        SetTrainToLevel(_loadedFacilityData.trainToLevel);
        SetActivateTraining(true);
        chimera.SetXPByType(_statType, _loadedFacilityData.chimeraStatXp);

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

        _glowMarker.ActivateGlowCollider(false);
        _storedChimera.RevealChimera(false);
        _storedChimera.Behavior.enabled = false;
        _storedChimera.Behavior.Agent.enabled = false;

        Debug.Log($"{_storedChimera} added to the facility.");
    }

    // Removes Chimera from facility and cleans up chimera and facility logic.
    public void RemoveChimera()
    {
        if (_storedChimera == null) // Facility is empty.
        {
            Debug.LogWarning("Cannot remove Chimera, facility is empty.");
            return;
        }

        if (_storedChimera.Level > 1 && _uiManager.TutorialObserver.DetailsTutorial == false)
        {
            _uiManager.TutorialObserver.DetailsTutorial = true;
        }

        _activateTraining = false;

        _trainingIcon.ResetIcon();
        _trainingIcon.gameObject.SetActive(false);

        _glowMarker.ActivateGlowCollider(true);

        _storedChimera.SetInFacility(false);
        _storedChimera.RevealChimera(true);
        _storedChimera.Behavior.ChangeState(_storedChimera.Behavior.States[AI.Behavior.StateEnum.Patrol]);
        _storedChimera.Behavior.enabled = true;
        _storedChimera.Behavior.Agent.enabled = true;

        _audioManager.PlayUISFX(SFXUIType.RemoveChimera);
        _facilitySFX.StopSFX();

        Debug.Log($"{_storedChimera} has been removed from the facility.");

        _storedChimera = null;
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

        _storedChimera.GetStatByType(_statType, out int currentStatAmount);

        if (currentStatAmount >= _trainToLevel)
        {
            RemoveChimera();
            return;
        }

        _trainingIcon.UpdateSlider(_statModifier);

        _storedChimera.ExperienceTick(_statType, _statModifier);
        _trainingIcon.SetIcon(_storedChimera.ChimeraIcon);
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