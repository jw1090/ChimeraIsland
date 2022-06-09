using UnityEngine;

public class UISceneChanger : MonoBehaviour
{
    public void NewGameClicked()
    {
        ServiceLocator.Get<SceneChanger>().NewGame();
    }

    public void LoadGameClicked()
    {
        ServiceLocator.Get<SceneChanger>().LoadGame();
    }

    public void QuitGameClicked()
    {
        ServiceLocator.Get<SceneChanger>().QuitGame();
    }

    public void MainMenuClicked()
    {
        ServiceLocator.Get<SceneChanger>().LoadMainMenu();
    }

    public void LoadWorldMap()
    {
        ServiceLocator.Get<SceneChanger>().LoadWorldMap();
    }

    public void LoadStonePlains()
    {
        ServiceLocator.Get<SceneChanger>().LoadStonePlains();
    }

    public void LoadTreeOfLife()
    {
        ServiceLocator.Get<SceneChanger>().LoadTreeOfLife();
    }

    public void LoadAshlands()
    {
        ServiceLocator.Get<SceneChanger>().LoadAshlands();
    }
}