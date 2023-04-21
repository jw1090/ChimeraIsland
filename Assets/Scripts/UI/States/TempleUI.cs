using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempleUI : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private TextMeshProUGUI _subtitle = null;

    [Header("Main")]
    [SerializeField] private ChimeraInfoUI _chimeraInfo = null;
    [SerializeField] private UIFossilWallet _fossilWallet = null;

    [Header("Buttons")]
    [SerializeField] private StatefulObject _backButtonStates = null;
    [SerializeField] private Button _backToHabitatButton = null;
    [SerializeField] private Button _backButton = null;
    [SerializeField] private Button _goLeftButton = null;
    [SerializeField] private Button _goRightButton = null;
    [SerializeField] private Button _buyUpgradeButton = null;

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

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _inputManager = ServiceLocator.Get<InputManager>();

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
        _uiManager.CreateButtonListener(_goLeftButton, TransitionLeft);
        _uiManager.CreateButtonListener(_goRightButton, TransitionRight);
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
        _title.gameObject.SetActive(true);
        _subtitle.gameObject.SetActive(true);

        _goLeftButton.gameObject.SetActive(true);
        _goRightButton.gameObject.SetActive(true);

        _currentTempleSection = TempleSectionType.Buying;
    }

    private void TransitionLeft()
    {
        switch (_currentTempleSection)
        {
            case TempleSectionType.Buying: // Go To Collections
                _goLeftButton.gameObject.SetActive(false);
                _currentTempleSection = TempleSectionType.Collection;
                _tutorialManager.ShowTutorialStage(TutorialStageType.Collections);
                break;
            case TempleSectionType.Upgrades: // Go To Buying
                _goRightButton.gameObject.SetActive(true);
                _currentTempleSection = TempleSectionType.Buying;
                break;
            default:
                Debug.LogError($"Current temple section [{_currentTempleSection}] is not supported by left transitions.");
                break;
        }
        _cameraUtil.TempleTransition(_currentTempleSection);
        ChangeSectionUIState();
    }

    private void TransitionRight()
    {
        switch (_currentTempleSection)
        {
            case TempleSectionType.Buying: // Go To Upgrades
                _goRightButton.gameObject.SetActive(false);
                _currentTempleSection = TempleSectionType.Upgrades;
                _tutorialManager.ShowTutorialStage(TutorialStageType.Upgrade);
                break;
            case TempleSectionType.Collection: // Go To Buying
                _goLeftButton.gameObject.SetActive(true);
                _currentTempleSection = TempleSectionType.Buying;
                break;
            default:
                Debug.LogError($"Current temple section [{_currentTempleSection}] is not supported by right transitions.");
                break;
        }
        _cameraUtil.TempleTransition(_currentTempleSection);
        ChangeSectionUIState();
    }

    public void EnteringTempleTransition()
    {
        _cameraUtil.TempleTransition(_currentTempleSection);

        ShowDefaultUI();
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
            ShowDefaultUI();
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

    private void ExitChimeraCloseUp()
    {

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

    private void ChangeSectionUIState()
    {
        switch (_currentTempleSection)
        {
            case TempleSectionType.Buying:
                break;
            case TempleSectionType.Chimera:
                break;
            case TempleSectionType.Upgrades:
                break;
            case TempleSectionType.Collection:
                break;
            case TempleSectionType.Gallery:
            default:
                Debug.LogError($"Current temple section [{_currentTempleSection}] is invalid!");
                break;
        }
    }

    public void UpdateFossilWallets()
    {
        _fossilWallet.UpdateWallet();
    }

    public void BuyChimera(EvolutionLogic evolutionLogic)
    {
        int price = _temple.TempleBuyChimeras.GetCurrentPrice(evolutionLogic.ChimeraType);

        if (_currencyManager.SpendFossils(price) == false)
        {
            _uiManager.AlertText.CreateAlert($"Can't Afford The {evolutionLogic.Name} Chimera. It Costs {price} Fossils!");
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        _temple.TempleBuyChimeras.BuyChimera(evolutionLogic);
    }

    public void BuyFacility(UpgradeNode upgradeNode)
    {
        if (_habitatManager.IsFacilityBuilt(upgradeNode) == true)
        {
            _uiManager.AlertText.CreateAlert($"This Facility Tier Has Already Been Built!");
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        if (upgradeNode.Tier == 1)
        {
            _uiManager.AlertText.CreateAlert($"Complete More Expeditions To Unlock New Facility Upgrades!");
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        int price = _temple.TempleUpgrades.GetPrice(upgradeNode.Tier);

        if (_currencyManager.SpendFossils(price) == false)
        {
            _uiManager.AlertText.CreateAlert($"Can't Afford The Facility Upgrade. It Costs {price} Fossils.");
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        _temple.TempleUpgrades.BuyUpgrade(upgradeNode);
    }
}