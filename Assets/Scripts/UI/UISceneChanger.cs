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

    public void NewGameClicked()
    {
        _sceneChanger.NewGame();
    }

    public void LoadGameClicked()
    {
        _sceneChanger.LoadGame();
    }

    public void QuitGameClicked()
    {
        _sceneChanger.QuitGame();
    }

    public void MainMenuClicked()
    {
        _sceneChanger.LoadMainMenu();
    }

    public void LoadWorldMap()
    {
        _sceneChanger.LoadWorldMap();
    }

    public void LoadStonePlains()
    {
        _sceneChanger.LoadStonePlains();
    }

    public void LoadTreeOfLife()
    {
        _sceneChanger.LoadTreeOfLife();
    }

    public void LoadAshlands()
    {
        _sceneChanger.LoadAshlands();
    }
}