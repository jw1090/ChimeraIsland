using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private HabitatManager _habitatManager = null;
    private PersistentData _persistentData = null;
    private UIManager _uiManager = null;

    public SceneChanger Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        return this;
    }

    public void SetupUIListeners()
    {
        _uiManager = ServiceLocator.Get<UIManager>();

        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.NewGameButton, NewGame);
        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.LoadGameButton, LoadGame);
        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.QuitGameButton, QuitGame);
        _uiManager.CreateButtonListener(_uiManager.HabitatUI.Settings.MainMenuButton, LoadMainMenu);
        _uiManager.CreateButtonListener(_uiManager.HabitatUI.Settings.QuitGameButton, QuitGame);
        _uiManager.CreateButtonListener(_uiManager.WorldMapUI.StonePlainsButton, LoadStonePlains);
    }

    public void NewGame()
    {
        _persistentData.NewSaveData();
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STARTER_SELECT);
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES);
    }

    public void QuitGame()
    {
        SaveSessionData(true);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    public void LoadMainMenu()
    {
        _uiManager.HabitatUI.ResetStandardUI();
        SaveSessionData(true);
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.MAIN_MENU);
    }

    public void LoadStonePlains()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES);
    }

    private void SaveSessionData(bool quitGame)
    {
        if (_habitatManager.CurrentHabitat == null)
        {
            Debug.Log("Current Habitat is null!");
            return;
        }

        if (quitGame == true)
        {
            _persistentData.QuitGameSave();
        }
        else
        {
            _persistentData.SaveSessionData();
        }
    }
}