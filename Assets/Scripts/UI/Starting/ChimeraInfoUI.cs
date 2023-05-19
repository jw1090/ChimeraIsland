using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraInfoUI : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private TextMeshProUGUI _chimeraName = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private List<StatefulObject> _explorationPreference = new List<StatefulObject>();
    [SerializeField] private List<StatefulObject> _staminaPreference = new List<StatefulObject>();
    [SerializeField] private List<StatefulObject> _wisdomPreference = new List<StatefulObject>();
    [SerializeField] private TextMeshProUGUI _chimeraInfo = null;

    [Header("Purchase Section")]
    [SerializeField] private GameObject _purchaseSection = null;
    [SerializeField] private Button _purchaseButton = null;
    [SerializeField] private TextMeshProUGUI _purchaseButtonText = null;
    [SerializeField] private Button _cancelButton = null;
    private int _currentChimeraPrice = 0;

    [Header("Animation Buttons")]
    [SerializeField] private GameObject _animationSection = null;
    [SerializeField] private Button _walkButton = null;
    [SerializeField] private Button _idleButton = null;
    [SerializeField] private Button _successButton = null;
    [SerializeField] private Button _failureButton = null;

    private ResourceManager _resourceManager = null;
    private CurrencyManager _currencyManager = null;
    private UIManager _uiManager = null;
    private AudioManager _audioManager = null;
    private EvolutionLogic _evolution = null;
    private Temple _temple = null;
    private AnimationType _currentAnimationType = AnimationType.None;

    public Button CancelButton { get => _cancelButton; }

    public void SetAudioManager(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    public void SetTemple(Temple temple) { _temple = temple; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
    }

    public void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_walkButton, WalkClick);
        _uiManager.CreateButtonListener(_idleButton, IdleClick);
        _uiManager.CreateButtonListener(_successButton, SuccessClick);
        _uiManager.CreateButtonListener(_failureButton, FailureClick);
        _uiManager.CreateButtonListener(_purchaseButton, BuyChimera);
    }

    public void LoadChimeraData(EvolutionLogic evolutionLogic)
    {
        _evolution = evolutionLogic;
        _chimeraName.text = evolutionLogic.Name;
        _icon.sprite = _resourceManager.GetElementSprite(evolutionLogic.ElementType);
        _chimeraInfo.text = evolutionLogic.BackgroundInfo;

        LoadStatPreferences(evolutionLogic);
    }

    private void LoadStatPreferences(EvolutionLogic evolutionLogic)
    {
        StatPreference(_explorationPreference, evolutionLogic.ExplorationPreference);
        StatPreference(_staminaPreference, evolutionLogic.StaminaPreference);
        StatPreference(_wisdomPreference, evolutionLogic.WisdomPreference);
    }

    private void StatPreference(List<StatefulObject> iconList, StatPreferenceType preference)
    {
        foreach (StatefulObject icon in iconList)
        {
            icon.SetState("Empty", true);
        }

        int amount = 0;

        switch (preference)
        {
            case StatPreferenceType.Dislike:
                amount = 1;
                break;
            case StatPreferenceType.Neutral:
                amount = 2;
                break;
            case StatPreferenceType.Like:
                amount = 3;
                break;
            default:
                break;
        }

        for (int i = 0; i < amount; ++i)
        {
            iconList[i].SetState("Filled", true);
        }
    }

    public void OpenPurchaseSection()
    {
        UpdatePurchaseSection();

        _purchaseSection.SetActive(true);
        _animationSection.SetActive(false);
    }

    public void OpenAnimationSection()
    {
        _purchaseSection.SetActive(false);
        _animationSection.SetActive(true);
    }

    public void NoAdditionalSections()
    {
        _purchaseSection.SetActive(false);
        _animationSection.SetActive(false);
    }

    public void UpdatePurchaseSection()
    {
        _currentChimeraPrice = _temple.TempleBuyChimeras.GetCurrentPrice(_evolution.ChimeraType);
        _purchaseButtonText.text = $"{_currentChimeraPrice}  <sprite name=Fossil>";

        if (_currentChimeraPrice > _currencyManager.Fossils)
        {
            _purchaseButton.interactable = false;
        }
        else
        {
            _purchaseButton.interactable = true;
        }
    }

    public void BuyChimera()
    {
        _temple.TempleBuyChimeras.BuyChimera(_evolution);

        _currencyManager.SpendFossils(_currentChimeraPrice);
        UpdatePurchaseSection();
    }

    public void IdleEnter()
    {
        _currentAnimationType = AnimationType.Idle;
        SetAnimation("Idle");
    }

    private void IdleClick()
    {
        _currentAnimationType = AnimationType.Idle;
        SetAnimation("Idle");

        _audioManager.PlayUISFX(SFXUIType.StandardClick);
        _audioManager.PlayHeldChimeraSFX(_evolution.ChimeraType);
    }

    private void WalkClick()
    {
        _currentAnimationType = AnimationType.Walk;
        SetAnimation("Walk");

        _audioManager.PlayUISFX(SFXUIType.StandardClick);
        _audioManager.PlayHeldChimeraSFX(_evolution.ChimeraType);
    }

    private void SuccessClick()
    {
        _currentAnimationType = AnimationType.Success;
        SetAnimation("Success");

        _audioManager.PlayUISFX(SFXUIType.StandardClick);
        _audioManager.PlayHappyChimeraSFX(_evolution.ChimeraType);
    }

    private void FailureClick()
    {
        _currentAnimationType = AnimationType.Fail;
        SetAnimation("Fail");
        _audioManager.PlaySadChimeraSFX(_evolution.ChimeraType);

        _audioManager.PlayUISFX(SFXUIType.StandardClick);
        _audioManager.PlaySadChimeraSFX(_evolution.ChimeraType);
    }

    private void SetAnimation(string animationName)
    {
        EvaluateInteractable();

        _temple.ChimeraGallery.SetAnimation(animationName);
    }

    private void EvaluateInteractable()
    {
        _idleButton.interactable = true;
        _walkButton.interactable = true;
        _successButton.interactable = true;
        _failureButton.interactable = true;

        switch (_currentAnimationType)
        {
            case AnimationType.Idle:
                _idleButton.interactable = false;
                break;
            case AnimationType.Walk:
                _walkButton.interactable = false;
                break;
            case AnimationType.Success:
                _successButton.interactable = false;
                break;
            case AnimationType.Fail:
                _failureButton.interactable = false;
                break;
            default:
                Debug.LogError($"Animation Type is not valid [{_currentAnimationType}]");
                break;
        }
    }
}