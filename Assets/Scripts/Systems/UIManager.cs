using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scene Types")]
    [SerializeField] private HabitatUI _habitatUI = null;
    [SerializeField] private MainMenuUI _mainMenuUI = null;
    [SerializeField] private StartingUI _startingUI = null;
    [SerializeField] private WorldMapUI _worldMapUI = null;
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;
    private TutorialManager _tutorialManager = null;

    public HabitatUI HabitatUI { get => _habitatUI; }
    public MainMenuUI MainMenuUI { get => _mainMenuUI; }
    public StartingUI StartingUI { get => _startingUI; }
    public WorldMapUI WorldMapUI { get => _worldMapUI; }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        _startingUI.Initialize(this);
        _habitatUI.Initialize();

        _tutorialOverlay.Initialize(this);

        DisableAllSceneTypeUI();

        return this;
    }

    public void ShowUIByScene(SceneType uiSceneType)
    {
        Debug.Log($"<color=Cyan> Show {uiSceneType} UI.</color>");

        switch (uiSceneType)
        {
            case SceneType.None:
                DisableAllSceneTypeUI();
                break;
            case SceneType.Habitat:
                _habitatUI.gameObject.SetActive(true);
                break;
            case SceneType.MainMenu:
                _mainMenuUI.gameObject.SetActive(true);
                break;
            case SceneType.Starting:
                _startingUI.gameObject.SetActive(true);
                break;
            case SceneType.WorldMap:
                _worldMapUI.gameObject.SetActive(true);
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
                _habitatUI.EnableTutorialUIByType(uiElementType); // Habitat UI
                break;
            default:
                Debug.LogError($"{uiElementType} is invalid. Please change!");
                break;
        }
    }

    public void DisableAllSceneTypeUI()
    {
        _habitatUI.gameObject.SetActive(false);
        _mainMenuUI.gameObject.SetActive(false);
        _startingUI.gameObject.SetActive(false);
        _worldMapUI.gameObject.SetActive(false);
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
}