using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scene Types")]
    [SerializeField] private HabitatUI _habitatUI = null;
    [SerializeField] private MainMenuUI _mainMenuUI = null;
    [SerializeField] private StartingUI _startingUI = null;
    [SerializeField] private WorldMapUI _worldMapUI = null;

    public HabitatUI HabitatUI { get => _habitatUI; }
    public MainMenuUI MainMenuUI { get => _mainMenuUI; }
    public StartingUI StartingUI { get => _startingUI; }
    public WorldMapUI WorldMapUI { get => _worldMapUI; }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _startingUI.Initialize(this);
        _habitatUI.Initialize();

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

    public void DisableAllSceneTypeUI()
    {
        _habitatUI.gameObject.SetActive(false);
        _mainMenuUI.gameObject.SetActive(false);
        _startingUI.gameObject.SetActive(false);
        _worldMapUI.gameObject.SetActive(false);
    }
}