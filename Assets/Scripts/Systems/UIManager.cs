using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scene Types")]
    [SerializeField] private StatefulObject _uiStatefulObject = null;
    [SerializeField] private HabitatUI _habitatUI = null;
    [SerializeField] private MainMenuUI _mainMenuUI = null;
    [SerializeField] private StartingUI _startingUI = null;
    [SerializeField] private TempleUI _templeUI = null;
    [SerializeField] private AlertText _alertText = null;

    [Header("Settings")]
    [SerializeField] private SettingsUI _settingsUI = null;

    [Header("Tooltip")]
    [SerializeField] private Tooltip _tooltip = null;

    [Header("Loading")]
    [SerializeField] private float _fadeInDuration = 1.0f;
    [SerializeField] private float _fadeOutDuration = 0.5f;
    [SerializeField] private float _waitDuration = 1.0f;
    [SerializeField] private Image _loadingImage = null;
    [SerializeField] private Image _loadingGif = null;
    [SerializeField] private Animator _loadingGifAnimator = null;
    [SerializeField] private TextMeshProUGUI _loadingText = null;
    private Color _loadingTextColor = Color.white;

    [Header("Tutorial")]
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;

    private TutorialManager _tutorialManager = null;
    private LevelLoader _levelLoader = null;
    private bool _tutorialOpen = false;
    private bool _uiVisible = true;

    public UITutorialOverlay TutorialOverlay { get => _tutorialOverlay; }
    public bool TutorialOpen { get => _tutorialOpen; }
    public MainMenuUI MainMenuUI { get => _mainMenuUI; }
    public StartingUI StartingUI { get => _startingUI; }
    public HabitatUI HabitatUI { get => _habitatUI; }
    public TempleUI TempleUI { get => _templeUI; }
    public SettingsUI SettingsUI { get => _settingsUI; }
    public Tooltip Tooltip { get => _tooltip; }
    public AlertText AlertText { get => _alertText; }
    public bool InHabitatState { get => _levelLoader.CurrentSceneType == SceneType.Habitat; }
    public bool InTempleState { get => _levelLoader.CurrentSceneType == SceneType.Temple; }
    public bool InMainMenuState { get => _levelLoader.CurrentSceneType == SceneType.MainMenu; }
    public bool IsSettingsOpen { get => _settingsUI.gameObject.activeInHierarchy; }
    public bool UIActive { get => _uiVisible; }

    private HabitatManager _habitatManager = null;

    public void SetAudioManager(AudioManager audioManager)
    {
        _settingsUI.SetAudioManager(audioManager);
        _habitatUI.SetAudioManager(audioManager);
        _templeUI.SetAudioManager(audioManager);

        _settingsUI.InitializeVolumeSettings();
    }

    public void SetLevelLoader(LevelLoader levelLoader) { _levelLoader = levelLoader;  }

    public IEnumerator FadeInLoadingScreen(SceneType sceneType)
    {
        _loadingImage.gameObject.SetActive(true);
        _loadingGifAnimator.SetTrigger("Play");
        Color startColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Color endColorWhite = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color endColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        float timer = 0.0f;
        while (timer < _fadeInDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _fadeInDuration;
            _loadingImage.color = Color.Lerp(startColor, endColor, progress);
            _loadingGif.color = Color.Lerp(startColor, endColorWhite, progress);
            _loadingText.color = Color.Lerp(startColor, _loadingTextColor, progress);

            yield return null;
        }

        yield return new WaitForSeconds(_waitDuration);

        switch (sceneType)
        {
            case SceneType.MainMenu:
                SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.MAIN_MENU);
                break;
            case SceneType.Starting:
                SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STARTER_SELECT);
                break;
            case SceneType.Habitat:
                SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES);
                break;
            case SceneType.Temple:
                SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.TEMPLE);
                break;
            default:
                Debug.LogError($"{sceneType} is invalid. Please change!");
                break;
        }
    }

    public IEnumerator FadeOutLoadingScreen()
    {
        Color startColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Color startColorWhite = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color endColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        float timer = 0.0f;
        while (timer < _fadeOutDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _fadeOutDuration;
            _loadingImage.color = Color.Lerp(startColor, endColor, progress);
            _loadingGif.color = Color.Lerp(startColorWhite, endColor, progress);
            _loadingText.color = Color.Lerp(_loadingTextColor, endColor, progress);

            yield return null;
        }
        _loadingGifAnimator.SetTrigger("Pause");
        _loadingImage.gameObject.SetActive(false);

    }

    public IEnumerator FadeInAndOutLoadingScreen()
    {
        _loadingImage.gameObject.SetActive(true);
        _loadingGifAnimator.SetTrigger("Play");
        Color startColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Color endColorWhite = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color endColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        float timer = 0.0f;
        while (timer < _fadeInDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _fadeInDuration;
            _loadingImage.color = Color.Lerp(startColor, endColor, progress);
            _loadingGif.color = Color.Lerp(startColor, endColorWhite, progress);
            _loadingText.color = Color.Lerp(startColor, _loadingTextColor, progress);

            yield return null;
        }

        yield return new WaitForSeconds(_waitDuration * 3.0f);
        _habitatManager.CurrentHabitat.UpgradeHabitatTier();

        startColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Color startColorWhite = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        endColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        timer = 0.0f;
        while (timer < _fadeOutDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _fadeOutDuration;
            _loadingImage.color = Color.Lerp(startColor, endColor, progress);
            _loadingGif.color = Color.Lerp(startColorWhite, endColor, progress);
            _loadingText.color = Color.Lerp(_loadingTextColor, endColor, progress);

            yield return null;
        }
        _loadingGifAnimator.SetTrigger("Pause");
        _loadingImage.gameObject.SetActive(false);
    }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _settingsUI.Initialize(this);
        _alertText.Initialize();
        _tooltip.Initialize();

        _mainMenuUI.Initialize(this);
        _startingUI.Initialize(this);
        _habitatUI.Initialize(this);
        _templeUI.Initialize(this);

        InitializeWallets();

        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _uiStatefulObject.SetState("Transparent", true);
        _tutorialOverlay.Initialize(this);

        _loadingTextColor = _loadingText.color;

        return this;
    }

    private void InitializeWallets()
    {
        _habitatUI.InitializeWallets();
        _templeUI.InitializeWallets();
    }

    public void CreateButtonListener(Button button, Action action)
    {
        if (button != null)
        {
            button.onClick.AddListener
            (delegate
            {
                action?.Invoke();
            });
        }
        else
        {
            Debug.LogError($"{button} is null! Please Fix");
        }
    }

    public void CreateDropdownListener(TMP_Dropdown dropdown, Action action)
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener
            (delegate
            {
                action?.Invoke();
            });
        }
        else
        {
            Debug.LogError($"{dropdown} is null! Please Fix");
        }
    }

    public void ShowUIByScene(SceneType uiSceneType)
    {
        Debug.Log($"<color=Cyan> Show {uiSceneType} UI.</color>");

        _settingsUI.CloseSettingsUI();

        switch (uiSceneType)
        {
            case SceneType.MainMenu:
                _uiStatefulObject.SetState("Main Menu UI", true);
                break;
            case SceneType.Starting:
                _uiStatefulObject.SetState("Starting UI", true);
                _startingUI.SceneSetup();
                break;
            case SceneType.Habitat:
                _uiStatefulObject.SetState("Habitat UI", true);
                _habitatUI.ResetStandardUI();
                _habitatUI.LoadCurrentUIProgress();
                break;
            case SceneType.Temple:
                _uiStatefulObject.SetState("Temple UI", true);
                _templeUI.SceneSetup();
                break;
            default:
                Debug.LogError($"{uiSceneType} is invalid. Please change!");
                break;
        }
    }

    public void UpdateEssenceWallets()
    {
        _habitatUI.UpdateEssenceWallets();
    }

    public void UpdateFossilWallets()
    {
        _habitatUI.UpdateFossilWallets();
        _templeUI.UpdateFossilWallets();
    }

    public void ToggleUI()
    {
        _uiVisible = !_uiVisible;
        gameObject.SetActive(_uiVisible);
    }


    public void StartTutorial(TutorialStageData tutorialSteps, TutorialStageType tutorialType)
    {
        _tutorialOverlay.gameObject.SetActive(true);
        _tutorialOverlay.ShowOverlay(tutorialSteps, tutorialType);

        if (InHabitatState)
        {
            _habitatUI.ActivateStandardUI(false);
        }

        _tutorialOpen = true;
    }

    public void EndTutorial()
    {
        _tutorialOverlay.gameObject.SetActive(false);
        _tutorialManager.SaveTutorialProgress();

        if (InHabitatState)
        {
            _habitatUI.ActivateStandardUI(true);
        }

        _tutorialOpen = false;
    }
}