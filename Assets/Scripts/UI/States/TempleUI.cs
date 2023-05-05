using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempleUI : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private GameObject _title = null;
    [SerializeField] private TextMeshProUGUI _titleText = null;
    [SerializeField] private GameObject _subtitle = null;
    [SerializeField] private TextMeshProUGUI _subtitleText = null;

    [Header("Main")]
    [SerializeField] private ChimeraInfoUI _buyChimeraInfo = null;
    [SerializeField] private ChimeraInfoUI _galleryChimeraInfo = null;
    [SerializeField] private UIFossilWallet _fossilWallet = null;

    [Header("Top Left Buttons")]
    [SerializeField] private GameObject _topLeftSection = null;
    [SerializeField] private StatefulObject _backButtonStates = null;
    [SerializeField] private Button _backToHabitatButton = null;
    [SerializeField] private Button _backButton = null;
    [SerializeField] private Button _settingsButton = null;

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
    private TempleUpgrades _templeUpgrades = null;
    private InputManager _inputManager = null;
    private TutorialManager _tutorialManager = null;
    private TempleSectionType _currentTempleSection = TempleSectionType.None;

    public Button BackToHabitatButton { get => _backToHabitatButton; }
    public ChimeraInfoUI GalleryChimeraInfo { get => _galleryChimeraInfo; }
    public Button GoLeftButton { get => _goLeftButton; }
    public Button GoRightButton { get => _goRightButton; }
    public TempleSectionType CurrentTempleSection { get => _currentTempleSection; }

    public void SetCameraUtil(CameraUtil cameraUtil) { _cameraUtil = cameraUtil; }
    public void SetAudioManager(AudioManager audioManager)
    {
        _audioManager = audioManager;
        _buyChimeraInfo.SetAudioManager(audioManager);
        _galleryChimeraInfo.SetAudioManager(audioManager);
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

        _buyChimeraInfo.Initialize(_uiManager);
        _galleryChimeraInfo.Initialize(_uiManager);

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
        _uiManager.CreateButtonListener(_settingsButton, OpenSettingsUI);

        _uiManager.CreateButtonListener(_buyChimeraInfo.CancelButton, ExitChimeraCloseUp);

        _uiManager.CreateButtonListener(_goLeftButton, TransitionLeft);
        _uiManager.CreateButtonListener(_goRightButton, TransitionRight);

        _uiManager.CreateButtonListener(_upgradeButton, BuyUpgrade);

        _buyChimeraInfo.SetupButtonListeners();
        _galleryChimeraInfo.SetupButtonListeners();
    }

    public void SceneSetup()
    {
        _temple = ServiceLocator.Get<Temple>();
        _templeUpgrades = _temple.TempleUpgrades;

        _buyChimeraInfo.SetTemple(_temple);
        _galleryChimeraInfo.SetTemple(_temple);

        _currentTempleSection = TempleSectionType.Buying;
    }

    private void ShowBuyUI()
    {
        _titleText.text = "Chimera Shop";
        _subtitleText.text = "Select a Statue to Buy Chimera";

        _goLeftButton.gameObject.SetActive(true);
        _goRightButton.gameObject.SetActive(true);
        _upgradeButton.gameObject.SetActive(false);

        _galleryChimeraInfo.gameObject.SetActive(false);
        _buyChimeraInfo.gameObject.SetActive(false);

        _currentTempleSection = TempleSectionType.Buying;
    }

    private void ShowUpgradesUI()
    {
        _titleText.text = "Upgrades Shop";
        ClearUpgrades();

        _goLeftButton.gameObject.SetActive(true);
        _goRightButton.gameObject.SetActive(false);

        _currentTempleSection = TempleSectionType.Upgrades;
    }

    private void ShowCollectionsUI()
    {
        _title.SetActive(true);
        _titleText.text = "Collections";

        _subtitle.SetActive(true);
        _subtitleText.text = "Select a Chimera Figurine";

        SetBackButtonState(UITempleBackType.BackToHabitat);

        _goLeftButton.gameObject.SetActive(false);
        _goRightButton.gameObject.SetActive(true);

        _currentTempleSection = TempleSectionType.Collection;
    }

    public void ShowGalleryUI()
    {
        _title.SetActive(false);
        _subtitle.SetActive(false);
        _fossilWallet.gameObject.SetActive(false);

        SetBackButtonState(UITempleBackType.BackToTemple);

        _galleryChimeraInfo.OpenAnimationSection();
        _galleryChimeraInfo.gameObject.SetActive(true);
        _galleryChimeraInfo.IdleClick();

        _goRightButton.gameObject.SetActive(false);

        _currentTempleSection = TempleSectionType.Gallery;
    }

    public void ExitGallery()
    {
        _fossilWallet.gameObject.SetActive(true);
        _galleryChimeraInfo.gameObject.SetActive(false);

        ShowCollectionsUI();
        _audioManager.PlayUISFX(SFXUIType.StandardClick);

        _temple.ChimeraGallery.ExitGallery();
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

    public void EnterGallery(ChimeraType chimeraType)
    {
        _temple.ChimeraGallery.EnterGallery(chimeraType);
        _audioManager.PlayUISFX(SFXUIType.Whoosh);
        ShowGalleryUI();
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
        _buyChimeraInfo.LoadChimeraData(evolutionLogic);
        _buyChimeraInfo.OpenPurchaseSection();
        _buyChimeraInfo.gameObject.SetActive(true);

        _title.gameObject.SetActive(false);
        _subtitle.gameObject.SetActive(false);
        _goLeftButton.gameObject.SetActive(false);
        _goRightButton.gameObject.SetActive(false);

        _currentTempleSection = TempleSectionType.Chimera;

        _audioManager.PlayUISFX(SFXUIType.Whoosh);

        SetBackButtonState(UITempleBackType.BackToTemple);
    }

    private void ExitChimeraCloseUp()
    {
        _title.gameObject.SetActive(true);
        _subtitle.gameObject.SetActive(true);
        _buyChimeraInfo.gameObject.SetActive(false);

        SetBackButtonState(UITempleBackType.BackToHabitat);
        _audioManager.PlayUISFX(SFXUIType.StandardClick);
        _audioManager.PlayUISFX(SFXUIType.Whoosh);

        _currentTempleSection = TempleSectionType.Buying;
        _cameraUtil.TempleTransition(_currentTempleSection);
        _inputManager.DisableOutline(false);

        ShowBuyUI();
    }

    public void SelectFacilityUpgrade(UpgradeNode upgradeNode)
    {
        _subtitleText.text = BuildUpgradeString(upgradeNode.FacilityType, upgradeNode.Tier);
        UpdateUpgradeButton(upgradeNode);

        _audioManager.PlaySFX(EnvironmentSFXType.StoneClick);
        _templeUpgrades.SelectUpgradeNode(upgradeNode);
    }

    private string BuildUpgradeString(FacilityType facilityType, int tier)
    {
        string facilityString = "";

        switch (facilityType)
        {
            case FacilityType.Cave:
                facilityString += "Cave Facility";
                break;
            case FacilityType.RuneStone:
                facilityString += "Rune Stone Facility";
                break;
            case FacilityType.Waterfall:
                facilityString += "Waterfall Facility";
                break;
            default:
                Debug.LogError($"Invalid Facility Type [{facilityType}]");
                break;
        }

        facilityString += $" Tier {tier}";

        return facilityString;
    }

    private void UpdateUpgradeButton(UpgradeNode upgradeNode)
    {
        int price = _templeUpgrades.GetPrice(upgradeNode.Tier);

        if (_habitatManager.IsFacilityBuilt(upgradeNode))
        {
            _upgradeText.text = "Owned";
            _upgradeButton.interactable = false;
        }
        else if (upgradeNode.Tier == 1)
        {
            _upgradeText.text = "Unavailable";
            _upgradeButton.interactable = false;
        }
        else if (_habitatManager.IsPreviousFacilityBuilt(upgradeNode) == false)
        {
            _upgradeText.text = "Unavailable";
            _upgradeButton.interactable = false;
        }
        else
        {
            _upgradeText.text = $"{price}  <sprite name=Fossil>";

            if (price > _currencyManager.Fossils)
            {
                _upgradeButton.interactable = false;
            }
            else
            {
                _upgradeButton.interactable = true;
            }
        }

        _upgradeButton.gameObject.SetActive(true);
    }

    private void BuyUpgrade()
    {
        _templeUpgrades.BuyUpgrade();
        _upgradeButton.gameObject.SetActive(false);
    }

    private void ClearUpgrades()
    {
        _subtitleText.text = "Select an Upgrade Node";
        _upgradeButton.gameObject.SetActive(false);
        _templeUpgrades.ResetUpgradeNode();
    }

    private void OpenSettingsUI()
    {
        _title.gameObject.SetActive(false);
        _subtitle.gameObject.SetActive(false);
        _fossilWallet.gameObject.SetActive(false);

        _buyChimeraInfo.gameObject.SetActive(false);
        _galleryChimeraInfo.gameObject.SetActive(false);

        _goLeftButton.gameObject.SetActive(false);
        _goRightButton.gameObject.SetActive(false);
        _topLeftSection.gameObject.SetActive(false);

        _upgradeButton.gameObject.SetActive(false);

        _uiManager.SettingsUI.OpenSettingsUI();
    }

    public void CloseSettingsUI()
    {
        _topLeftSection.gameObject.SetActive(true);

        switch (_currentTempleSection)
        {
            case TempleSectionType.Habitat:
                break;
            case TempleSectionType.Buying:
                _title.gameObject.SetActive(true);
                _subtitle.gameObject.SetActive(true);
                _goLeftButton.gameObject.SetActive(true);
                _goRightButton.gameObject.SetActive(true);
                _fossilWallet.gameObject.SetActive(true);
                break;
            case TempleSectionType.Chimera:
                _buyChimeraInfo.gameObject.SetActive(true);
                _fossilWallet.gameObject.SetActive(true);
                break;
            case TempleSectionType.Upgrades:
                _title.gameObject.SetActive(true);
                _subtitle.gameObject.SetActive(true);
                _goLeftButton.gameObject.SetActive(true);
                _fossilWallet.gameObject.SetActive(true);
                ClearUpgrades();
                break;
            case TempleSectionType.Collection:
                _title.gameObject.SetActive(true);
                _subtitle.gameObject.SetActive(true);
                _goRightButton.gameObject.SetActive(true);
                _fossilWallet.gameObject.SetActive(true);
                break;
            case TempleSectionType.Gallery:
                _galleryChimeraInfo.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError($"Current Temple Section is invalid [{_currentTempleSection}]");
                break;
        }
    }
}