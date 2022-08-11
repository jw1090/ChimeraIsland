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

    private FacilitySFX _facilitySFX = null;
    private HabitatUI _habitatUI = null;
    private AudioManager _audioManager = null;
    private Chimera _storedChimera = null;
    private TutorialManager _tutorialManager = null;
    private UIManager _uiManager = null;
    private UITraining _uiTraining = null;
    private bool _isInitialized = false;
    private int _currentTier = 0;
    private int _trainToLevel = 0;
    private bool _activateTraining = false;

    public TrainingFacilityIcon MyFacilityIcon { get => _trainingIcon; }
    public Transform CameraTransitionNode { get => _cameraTransitionNode; }
    public GlowMarker GlowObject { get => _glowMarker; }
    public StatType StatType { get => _statType; }
    public FacilityType Type { get => _facilityType; }
    public bool ActivateTraining { get => _activateTraining; }
    public bool IsInitialized { get => _isInitialized; }
    public int StatModifier { get => _statModifier; }
    public int CurrentTier { get => _currentTier; }
    public int Price { get => _price; }

    public void SetTrainToLevel(int trainTo) { _trainToLevel = trainTo; }
    public void SetActivateTraining(bool Active) { _activateTraining = Active; }

    public void Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing {this.GetType()} ... </color>");

        _audioManager = ServiceLocator.Get<AudioManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

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
        if (_isInitialized == false)
        {
            return false;
        }

        if (_storedChimera == null)
        {
            return false;
        }

        return true;
    }

    public void BuildFacility()
    {
        _price = (int)(_price * 5.0f);

        string debugString = "";

        if (_currentTier == 0)
        {
            debugString += $"{_facilityType} was purchased";

            _tiers.SetState("Tier 1");

            _glowMarker.ActivateGlowCollider(true);


            _facilitySFX.Initialize(this);
            _facilitySFX.BuildSFX();
            

            _isInitialized = true;
        }
        else
        {
            ++_statModifier;
            debugString += $"{_facilityType} was upgraded to Tier {_currentTier + 1}";
        }

        ++_currentTier;
        _habitatUI.UpdateShopUI();

        Debug.Log($" {debugString} and now generates {_statModifier} {_statType}!");
    }

    // Called to properly link a chimera to a facility and adjust its states properly.
    public bool PlaceChimera(Chimera chimera)
    {
        if (_storedChimera != null) // Something is already in the facility.
        {
            Debug.Log($"Cannot add {chimera}. {_storedChimera} is already in this facility.");
            return false;
        }

        _habitatUI.ResetStandardUI();
        _uiTraining.SetupTrainingUI(chimera, this);
        _habitatUI.OpenTrainingPanel();

        _trainingIcon.gameObject.SetActive(true);
        _trainingIcon.SetIcon(chimera.ChimeraIcon);

        _storedChimera = chimera;
        _storedChimera.SetInFacility(true);

        _glowMarker.ActivateGlowCollider(false);
        RevealChimera(false);

        Debug.Log($"{_storedChimera} added to the facility.");

        return true;
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
            _tutorialManager.ShowTutorialStage(TutorialStageType.Details);
        }

        _activateTraining = false;
        AI.Behavior.ChimeraBehavior chimeraBehavior = _storedChimera.gameObject.GetComponent<AI.Behavior.ChimeraBehavior>();
        chimeraBehavior.ChangeState(chimeraBehavior.States[AI.Behavior.StateEnum.Patrol]);

        _trainingIcon.ResetIcon();
        _trainingIcon.gameObject.SetActive(false);
        _storedChimera.SetInFacility(false);

        _glowMarker.ActivateGlowCollider(true);
        RevealChimera(true);

        _audioManager.PlayUISFX(SFXUIType.RemoveChimera);
        _facilitySFX.StopSFX();

        Debug.Log($"{ _storedChimera} has been removed from the facility.");

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

    private void RevealChimera(bool reveal)
    {
        _storedChimera.CurrentEvolution.gameObject.SetActive(reveal);

        if (reveal == true)
        {
            _storedChimera.Animator.SetBool("Walk", true);
        }
    }
}