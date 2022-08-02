using UnityEngine;

public class Facility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private FacilityType _facilityType = FacilityType.None;
    [SerializeField] private StatType _statType = StatType.None;
    [SerializeField] private int _statModifier = 1;
    [SerializeField] private int _price = 50;

    [Header("Audio")]
    [SerializeField] private AudioClip _placeSFX = null;
    [SerializeField] private AudioClip _removeSFX = null;

    [Header("Reference")]
    [SerializeField] private GameObject _rubbleObject = null;
    [SerializeField] private GameObject _tier1Object = null;
    [SerializeField] private MeshRenderer _glowObject = null;
    [SerializeField] private FacilityIcon _trainingIcon = null;
    [SerializeField] private BoxCollider _placeCollider = null;
    [SerializeField] private BoxCollider _releaseCollider = null;
    [SerializeField] private FacilitySign _facilitySign = null;

    private FacilitySFX _facilitySFX = null;
    private HabitatUI _habitatUI = null;
    private AudioManager _audioManager = null;
    private CurrencyManager _currencyManager = null;
    private Chimera _storedChimera = null;
    private TutorialManager _tutorialManager = null;
    private UIManager _uiManager = null;
    private UITraining _uiTraining = null;
    private bool _isInitialized = false;
    private int _currentTier = 0;
    private int _trainToLevel = 0;
    private bool _activateTraining = false;

    public FacilityIcon MyFacilityIcon { get => _trainingIcon; }
    public StatType StatType{ get => _statType; }
    public FacilityType Type { get => _facilityType; }
    public bool ActivateTraining { get => _activateTraining; }
    public bool IsInitialized { get => _isInitialized; }
    public int StatModifier { get => _statModifier; }
    public int TrainToLevel { get => _trainToLevel; }
    public int CurrentTier { get => _currentTier; }
    public int Price { get => _price; }
    public MeshRenderer GlowObject { get => _glowObject; }

    public void SetTrainToLevel(int trainTo) { _trainToLevel = trainTo; }
    public void SetActivateTraining(bool Active) { _activateTraining = Active; }

    public void Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing {this.GetType()} ... </color>");

        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        _habitatUI = _uiManager.HabitatUI;
        _uiTraining = _habitatUI.TrainingPanel;

        _facilitySFX = GetComponent<FacilitySFX>();

        _glowObject.enabled = false;
        _trainingIcon.Initialize();
        _trainingIcon.gameObject.SetActive(false);

        FacilityColliderToggle(FacilityColliderType.None);

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
        _price = (int)(_price * 7.5);
        ++_currentTier;

        string debugString = "";

        if (_currentTier == 1)
        {
            debugString += $"{_facilityType} was purchased";

            _rubbleObject.SetActive(false);
            _tier1Object.SetActive(true);

            FacilityColliderToggle(FacilityColliderType.Place);

            _facilitySFX.Initialize();
            _facilitySFX.BuildSFX();

            _isInitialized = true;
        }
        else
        {
            ++_statModifier;
            debugString += $"{_facilityType} was increased to Tier {CurrentTier}";
        }

        _habitatUI.UpdateShopUI();

        int newMod = _statModifier + 1;

        Debug.Log($" {debugString} and now generates {newMod} {_statType}!");
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

        FacilityColliderToggle(FacilityColliderType.release);
        RevealChimera(false);

        _audioManager.PlaySFX(_placeSFX);
        _facilitySFX.PlaySFX();

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

        if (_storedChimera.Level >= 1 && _uiManager.TutorialObserver.DetailsTutorial == false)
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

        FacilityColliderToggle(FacilityColliderType.Place);
        RevealChimera(true);

        _audioManager.PlaySFX(_removeSFX);
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

        if (_activateTraining == false) //haven't finished training ui setup
        {
            return;
        }

        if (EssenceCost() == false) // Was kicked out.
        {
            return;
        }

        if(_storedChimera.GetAttribute(_statType) >= _trainToLevel)
        {
            RemoveChimera();
            return;
        }

        _trainingIcon.SetIcon(_storedChimera.ChimeraIcon);
        _trainingIcon.UpdateSlider(_storedChimera.GetAttribute(_statType));

        _storedChimera.ExperienceTick(_statType, _statModifier);
    }

    private bool EssenceCost()
    {
        int price = _statModifier * 5;

        if (_currencyManager.SpendEssence(price) == false) // Can't afford training.
        {
            RemoveChimera();

            return false;
        }

        return true;
    }

    private void RevealChimera(bool reveal)
    {
        _storedChimera.CurrentEvolution.gameObject.SetActive(reveal);

        if(reveal == true)
        {
            _storedChimera.Animator.SetBool("Walk", true);
        }
    }

    private void FacilityColliderToggle(FacilityColliderType facilityColliderType)
    {
        _placeCollider.enabled = false;
        _releaseCollider.enabled = false;

        switch (facilityColliderType)
        {
            case FacilityColliderType.None:
                break;
            case FacilityColliderType.Place:
                _placeCollider.enabled = true;
                break;
            case FacilityColliderType.release:
                _releaseCollider.enabled = true;
                break;
            default:
                Debug.LogWarning($"{facilityColliderType} is not valid, please change!");
                break;
        }
    }
}