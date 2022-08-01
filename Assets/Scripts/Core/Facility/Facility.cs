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
    [SerializeField] private FacilityIcon _icon = null;
    [SerializeField] private BoxCollider _placeCollider = null;
    [SerializeField] private BoxCollider _releaseCollider = null;
    [SerializeField] private FacilitySign _facilitySign = null;
    [SerializeField] private UITraining _uITraining = null;
    [SerializeField] private UIManager _uIManager;
    private FacilitySFX _facilitySFX = null;
    private HabitatUI _habitatUI = null;
    private AudioManager _audioManager = null;
    private CurrencyManager _currencyManager = null;
    private Chimera _storedChimera = null;

    private bool _isInitialized = false;
    private int _currentTier = 0;
    private int _trainToLevel = 0;
    private bool _activateTraining = false;

    public FacilityIcon MyFacilityIcon { get => _icon; }
    public StatType MyStatType{ get => _statType; }
    public void SetActivateTraining(bool Active) { _activateTraining = Active; }
    public bool ActivateTraining { get => _activateTraining; }
    public void SetTrainToLevel(int trainTo) { _trainToLevel = trainTo; }
    public int TrainToLevel { get => _trainToLevel; }
    public int StatModifier { get => _statModifier; }
    public FacilityType Type { get => _facilityType; }
    public bool IsInitialized { get => _isInitialized; }
    public int CurrentTier { get => _currentTier; }
    public int Price { get => _price; }
    public MeshRenderer GlowObject { get => _glowObject; }

    public void Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing {this.GetType()} ... </color>");

        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
        _uIManager = ServiceLocator.Get<UIManager>();
        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;

        _facilitySFX = GetComponent<FacilitySFX>();

        _glowObject.enabled = false;
        _icon.Initialize(this);
        _icon.gameObject.SetActive(false);

        FacilityColliderToggle(FacilityColliderType.None);

        _facilitySign.Initialize(_facilityType);

        _uITraining.Initialize(this);
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
        _uITraining.gameObject.SetActive(true);
        _uITraining.IntializeChimera(chimera);
        _icon.gameObject.SetActive(true);
        _icon.GetComponent<FacilityIcon>().SetIcon(chimera.ChimeraIcon);
        _storedChimera = chimera;
        _storedChimera.SetInFacility(true);

        _storedChimera.gameObject.transform.localPosition = gameObject.transform.localPosition;

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
            Debug.Log("Cannot remove Chimera, facility is empty.");
        }
        if (_storedChimera.Level >= 1 && _uIManager.TutorialObserver.tutorialFiveTriggered == false)
        {
            _uIManager.TutorialObserver.tutorialFiveTriggered = true;
            ServiceLocator.Get<TutorialManager>().ShowTutorialStage(TutorialStageType.Details);
        }
        _activateTraining = false;
        AI.Behavior.ChimeraBehavior chimeraBehavior = _storedChimera.gameObject.GetComponent<AI.Behavior.ChimeraBehavior>();
        chimeraBehavior.ChangeState(chimeraBehavior.States[AI.Behavior.StateEnum.Patrol]);

        _icon.RemoveIcon();
        _icon.gameObject.SetActive(false);
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

        if(_storedChimera.GetAttribute(MyStatType) >= _trainToLevel)
        {
            RemoveChimera();
            return;
        }

        _icon.SetIcon(_storedChimera.ChimeraIcon);
        _icon.updateSlider(_storedChimera.GetAttribute(MyStatType));
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