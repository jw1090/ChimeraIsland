using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempleUI : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private GameObject _title = null;
    [SerializeField] private TextMeshProUGUI _titleText = null;
    [SerializeField] private GameObject _subtitle = null;
    [SerializeField] private TextMeshProUGUI _subtitleText = null;

    [Header("Main")]
    [SerializeField] private ChimeraInfoUI _chimeraInfo = null;
    [SerializeField] private UIFossilWallet _fossilWallet = null;

    [Header("Top Left Buttons")]
    [SerializeField] private StatefulObject _backButtonStates = null;
    [SerializeField] private Button _backToHabitatButton = null;
    [SerializeField] private Button _backButton = null;

    [Header("Side Buttons")]
    [SerializeField] private Button _goLeftButton = null;
    [SerializeField] private Button _goRightButton = null;

    [Header("Upgrades")]
    [SerializeField] private Button _upgradeButton = null;
    [SerializeField] private TextMeshProUGUI _upgradeText = null;

    private HabitatManager _habitatManager = null;
    private CurrencyManager _currencyManager = null;
    private UIManager _uiManager = null;
    private CameraUtil _cameraUtil = null;
    private AudioManager _audioManager = null;
    private SceneChanger _sceneChanger = null;
    private Temple _temple = null;
    private InputManager _inputManager = null;
    private TutorialManager _tutorialManager = null;
    private TempleSectionType _currentTempleSection = TempleSectionType.None;

    public Button BackToHabitatButton { get => _backToHabitatButton; }
    public ChimeraInfoUI ChimeraInfo { get => _chimeraInfo; }
    public Button GoLeftButton { get => _goLeftButton; }
    public Button GoRightButton { get => _goRightButton; }
    public TempleSectionType CurrentTempleSection { get => _currentTempleSection; }

    public void SetCameraUtil(CameraUtil cameraUtil) { _cameraUtil = cameraUtil; }
    public void SetAudioManager(AudioManager audioManager)
    {
        _audioManager = audioManager;
        _chimeraInfo.SetAudioManager(audioManager);
    }

    public void SetBackButtonState(UITempleBackType backType)
    {
        switch (backType)
        {
            case UITempleBackType.BackToHabitat:
                _backButtonStates.SetState("Habitat Button");
                break;
            case UITempleBackType.BackToTemple:
                _backButtonStates.SetState("Back Button");
                break;
            default:
                Debug.LogError($"UITemple Back Type is invalid [{backType}]");
                break;
        }
    }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _inputManager = ServiceLocator.Get<InputManager>();

        _chimeraInfo.Initialize(_uiManager);

        SetupButtonListeners();

        _currentTempleSection = TempleSectionType.Buying;
    }

    public void InitializeWallets()
    {
        _fossilWallet.Initialize();
    }

    private void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_backToHabitatButton, LeavingTempleTransition);
        _uiManager.CreateButtonListener(_backButton, BackButtonPress);

        _uiManager.CreateButtonListener(_chimeraInfo.CancelButton, ExitChimeraCloseUp);

        _uiManager.CreateButtonListener(_goLeftButton, TransitionLeft);
        _uiManager.CreateButtonListener(_goRightButton, TransitionRight);

        _chimeraInfo.SetupButtonListeners();
    }

    public void SceneSetup()
    {
        _temple = ServiceLocator.Get<Temple>();
        _chimeraInfo.Temple(_temple);

        _temple.ChimeraGallery.SetTempleUI(this);

        _currentTempleSection = TempleSectionType.Buying;
    }

    public void ShowDefaultUI()
    {
        
    }

    private void ShowBuyUI()
    {
        _titleText.text = "Chimera Shop";
        _subtitleText.text = "Select a Statue to Buy Chimera";

        _goLeftButton.gameObject.SetActive(true);
        _goRightButton.gameObject.SetActive(true);

        _currentTempleSection = TempleSectionType.Buying;
    }

    private void ShowUpgradesUI()
    {
        _titleText.text = "Upgrades Shop";
        _subtitleText.text = "Select an Upgrade Node";

        _goLeftButton.gameObject.SetActive(true);
        _goRightButton.gameObject.SetActive(false);

        _currentTempleSection = TempleSectionType.Upgrades;
    }

    private void ShowCollectionsUI()
    {
        _titleText.text = "Collections";
        _subtitleText.text = "Select a Chimera Figurine";

        _goLeftButton.gameObject.SetActive(false);
        _goRightButton.gameObject.SetActive(true);

        _currentTempleSection = TempleSectionType.Collection;
    }

    private void TransitionLeft()
    {
        switch (_currentTempleSection)
        {
            case TempleSectionType.Buying: // Go To Collections
                ShowCollectionsUI();
                _tutorialManager.ShowTutorialStage(TutorialStageType.Collections);
                break;
            case TempleSectionType.Upgrades: // Go To Buying
                ShowBuyUI();
                break;
            default:
                Debug.LogError($"Current temple section [{_currentTempleSection}] is not supported by left transitions.");
                break;
        }
        _cameraUtil.TempleTransition(_currentTempleSection);
    }

    private void TransitionRight()
    {
        switch (_currentTempleSection)
        {
            case TempleSectionType.Buying:
                ShowUpgradesUI();
                _tutorialManager.ShowTutorialStage(TutorialStageType.Upgrade);
                break;
            case TempleSectionType.Collection:
                ShowBuyUI();
                break;
            default:
                Debug.LogError($"Current temple section [{_currentTempleSection}] is not supported by right transitions.");
                break;
        }
        _cameraUtil.TempleTransition(_currentTempleSection);
    }

    public void EnteringTempleTransition()
    {
        _cameraUtil.TempleTransition(_currentTempleSection);

        _title.gameObject.SetActive(true);
        _subtitle.gameObject.SetActive(true);

        ShowBuyUI();

        StartCoroutine(HabitatTransitionCoroutine(false));
    }

    private void LeavingTempleTransition()
    {
        _currentTempleSection = TempleSectionType.Habitat;
        _cameraUtil.TempleTransition(_currentTempleSection);

        StartCoroutine(HabitatTransitionCoroutine(true));
    }

    private IEnumerator HabitatTransitionCoroutine(bool leavingTemple)
    {
        yield return new WaitForSeconds(0.3f);

        if (leavingTemple == true)
        {
            _inputManager.SetInTransition(false);
            _sceneChanger.LoadStonePlains();
        }
        else
        {
            _goLeftButton.gameObject.SetActive(true);
            _goRightButton.gameObject.SetActive(true);
        }
    }

    private void BackButtonPress()
    {
        switch (_currentTempleSection)
        {
            case TempleSectionType.Chimera:
                ExitChimeraCloseUp();
                break;
            case TempleSectionType.Gallery:
                ExitGallery();
                break;
            default:
                break;
        }
    }

    public void UpdateFossilWallets()
    {
        _fossilWallet.UpdateWallet();
    }

    public void ChimeraCloseUp(EvolutionLogic evolutionLogic)
    {
        _chimeraInfo.LoadChimeraData(evolutionLogic);
        _chimeraInfo.OpenPurchaseSection();
        _chimeraInfo.gameObject.SetActive(true);

        _title.gameObject.SetActive(false);
        _subtitle.gameObject.SetActive(false);
        _goLeftButton.gameObject.SetActive(false);
        _goRightButton.gameObject.SetActive(false);

        _currentTempleSection = TempleSectionType.Chimera;

        SetBackButtonState(UITempleBackType.BackToTemple);
    }

    private void ExitChimeraCloseUp()
    {
        _title.gameObject.SetActive(true);
        _subtitle.gameObject.SetActive(true);
        _chimeraInfo.gameObject.SetActive(false);

        SetBackButtonState(UITempleBackType.BackToHabitat);

        ShowBuyUI();
    }

    public void BuyFacility(UpgradeNode upgradeNode)
    {
        if (_habitatManager.IsFacilityBuilt(upgradeNode) == true)
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        if (upgradeNode.Tier == 1)
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        int price = _temple.TempleUpgrades.GetPrice(upgradeNode.Tier);

        if (_currencyManager.SpendFossils(price) == false)
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        _temple.TempleUpgrades.BuyUpgrade(upgradeNode);
    }

    public void ShowGalleryUI()
    {
        _currentTempleSection = TempleSectionType.Gallery;
    }

    public void ExitGallery()
    {
        _temple.ChimeraGallery.ExitGallery();

        _currentTempleSection = TempleSectionType.Buying;
    }
}