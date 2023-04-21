using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempleUI : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private TextMeshProUGUI _subtitle = null;

    [Header("Top Left Buttons")]
    [SerializeField] private StatefulObject _backStates = null;
    [SerializeField] private Button _backToHabitatButton = null;
    [SerializeField] private Button _backButton = null;

    [Header("Buttons")]
    [SerializeField] private UIFossilWallet _fossilWallet = null;
    [SerializeField] private Button _goLeftButton = null;
    [SerializeField] private Button _goRightButton = null;

    [Header("Info Panel")]
    [SerializeField] private ChimeraInfoUI _chimeraInfo = null;

    private HabitatManager _habitatManager = null;
    private CurrencyManager _currencyManager = null;
    private UIManager _uiManager = null;
    private CameraUtil _cameraUtil = null;
    private AudioManager _audioManager = null;
    private SceneChanger _sceneChanger = null;
    private Temple _templeEnvironment = null;
    private InputManager _inputManager = null;
    private TempleSectionType _currentTempleSection = TempleSectionType.None;

    public Button BackToHabitatButton { get => _backToHabitatButton; }
    public ChimeraInfoUI ChimeraInfo { get => _chimeraInfo; }
    public Button GoLeftButton { get => _goLeftButton; }
    public Button GoRightButton { get => _goRightButton; }

    public void SetCameraUtil(CameraUtil cameraUtil) { _cameraUtil = cameraUtil; }
    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _inputManager = ServiceLocator.Get<InputManager>();

        _chimeraInfo.Initialize();

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
        _uiManager.CreateButtonListener(_backButton, ExitGallery);
        _uiManager.CreateButtonListener(_goLeftButton, TransitionLeft);
        _uiManager.CreateButtonListener(_goRightButton, TransitionRight);

        _chimeraInfo.SetupButtonListeners();
    }

    public void SceneSetup()
    {
        _templeEnvironment = ServiceLocator.Get<Temple>();

        _templeEnvironment.ChimeraGallery.SetTempleUI(this);
        _currentTempleSection = TempleSectionType.Buying;
    }

    private void ExitBuyChimera()
    {

    }

    private void ExitGallery()
    {
        _templeEnvironment.ChimeraGallery.ExitGallery();
        ShowSharedUIState();
    }

    private void TransitionLeft()
    {
        switch (_currentTempleSection)
        {
            case TempleSectionType.Buying: // Go To Collections
                _goLeftButton.gameObject.SetActive(false);
                _currentTempleSection = TempleSectionType.Collection;
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

    public void EnteringTempleTransition()
    {
        _cameraUtil.TempleTransition(_currentTempleSection);

        ShowTempleUI(false);
        StartCoroutine(HabitatTransitionCoroutine(false));
    }

    public void ShowGalleryUI()
    {

    }

    private void ChangeSectionUIState()
    {
        switch (_currentTempleSection)
        {
            case TempleSectionType.Buying:

                break;
            case TempleSectionType.Upgrades:

                break;
            case TempleSectionType.Collection:

                break;
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
        int price = _templeEnvironment.TempleBuyChimeras.GetCurrentPrice(evolutionLogic.ChimeraType);

        if (_currencyManager.SpendFossils(price) == false)
        {
            _uiManager.AlertText.CreateAlert($"Can't Afford The {evolutionLogic.Name} Chimera. It Costs {price} Fossils!");
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        _templeEnvironment.TempleBuyChimeras.BuyChimera(evolutionLogic);
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

        int price = _templeEnvironment.TempleUpgrades.GetPrice(upgradeNode.Tier);

        if (_currencyManager.SpendFossils(price) == false)
        {
            _uiManager.AlertText.CreateAlert($"Can't Afford The Facility Upgrade. It Costs {price} Fossils.");
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);

            return;
        }

        _templeEnvironment.TempleUpgrades.BuyUpgrade(upgradeNode);
    }
}