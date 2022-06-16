using UnityEngine;

public class UISceneChanger : MonoBehaviour
{
    private SceneChanger _sceneChanger = null;

    private void Awake()
    {
        GameLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _sceneChanger = ServiceLocator.Get<SceneChanger>();
    }

    private void SceneChangerSetup()
    {
        if(_sceneChanger == null)
        {
            _sceneChanger = ServiceLocator.Get<SceneChanger>();
        }
    }

    public void NewGameClicked()
    {
        SceneChangerSetup();
        _sceneChanger.NewGame();
    }

    public void LoadGameClicked()
    {
        SceneChangerSetup();
        _sceneChanger.LoadGame();
    }

    public void QuitGameClicked()
    {
        SceneChangerSetup();
        _sceneChanger.QuitGame();
    }

    public void MainMenuClicked()
    {
        SceneChangerSetup();
        _sceneChanger.LoadMainMenu();
    }

    public void LoadWorldMap()
    {
        SceneChangerSetup();
        _sceneChanger.LoadWorldMap();
    }

    public void LoadStonePlains()
    {
        SceneChangerSetup();
        _sceneChanger.LoadStonePlains();
    }

    public void LoadTreeOfLife()
    {
        SceneChangerSetup();
        _sceneChanger.LoadTreeOfLife();
    }

    public void LoadAshlands()
    {
        SceneChangerSetup();
        _sceneChanger.LoadAshlands();
    }
}