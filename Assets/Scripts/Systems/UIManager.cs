using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scene Types")]
    [SerializeField] private StatefulObject _uiStatefulObject = null;
    [SerializeField] private HabitatUI _habitatUI = null;
    [SerializeField] private MainMenuUI _mainMenuUI = null;
    [SerializeField] private StartingUI _startingUI = null;
    [SerializeField] private WorldMapUI _worldMapUI = null;
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;
    private TutorialObserver _tutorialObserver = null;
    private TutorialManager _tutorialManager = null;

    public HabitatUI HabitatUI { get => _habitatUI; }
    public MainMenuUI MainMenuUI { get => _mainMenuUI; }
    public StartingUI StartingUI { get => _startingUI; }
    public WorldMapUI WorldMapUI { get => _worldMapUI; }
    public TutorialObserver TutorialObserver { get => _tutorialObserver; }

    public void SetAudioManager(AudioManager audioManager) { _startingUI.SetAudioManager(audioManager); }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _tutorialObserver = GetComponent<TutorialObserver>();

        _startingUI.Initialize();
        _habitatUI.Initialize(this);

        _tutorialOverlay.Initialize(this);
        _tutorialObserver.Initialize(this);

        return this;
    }

    public void ShowUIByScene(SceneType uiSceneType)
    {
        Debug.Log($"<color=Cyan> Show {uiSceneType} UI.</color>");

        switch (uiSceneType)
        {
            case SceneType.Habitat:
                _uiStatefulObject.SetState("Habitat UI");
                _habitatUI.ResetStandardUI();
                _habitatUI.LoadCurrentUIProgress();
                break;
            case SceneType.MainMenu:
                _uiStatefulObject.SetState("Main Menu UI");
                break;
            case SceneType.Starting:
                _uiStatefulObject.SetState("Starting UI");
                break;
            case SceneType.WorldMap:
                _uiStatefulObject.SetState("World MapUI");
                break;
            default:
                Debug.LogError($"{uiSceneType} is invalid. Please change!");
                break;
        }
    }

    public void EnableTutorialUIByType(TutorialUIElementType uiElementType)
    {
        switch (uiElementType)
        {
            case TutorialUIElementType.All:
            case TutorialUIElementType.MarketplaceButton:
            case TutorialUIElementType.OpenDetailsButton:
            case TutorialUIElementType.ExpeditionButton:
            case TutorialUIElementType.MarketplaceChimeraTab:
            case TutorialUIElementType.WorldMapButton:
            case TutorialUIElementType.OtherFacilityButtons:
            case TutorialUIElementType.FossilButtons:
                _habitatUI.EnableTutorialUIByType(uiElementType); // Habitat UI
                break;
            default:
                Debug.LogError($"{uiElementType} is invalid. Please change!");
                break;
        }
    }

    public void TutorialDisableUI()
    {
        _habitatUI.TutorialDisableUI();
    }

    public void StartTutorial(TutorialStageData tutorialSteps)
    {
        _tutorialOverlay.gameObject.SetActive(true);
        _tutorialOverlay.ShowOverlay(tutorialSteps);
    }

    public void EndTutorial()
    {
        _tutorialOverlay.gameObject.SetActive(false);
        _tutorialManager.SaveTutorialProgress();
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
}