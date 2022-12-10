using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    private HabitatManager _habitatManager = null;
    private PersistentData _persistentData = null;
    private UIManager _uiManager = null;

    public bool RecentSceneChange { get; set; } = false;

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

        Button mainMenuButton = _uiManager.SettingsUI.MainMenuButton;

        _uiManager.CreateButtonListener(_uiManager.SettingsUI.MainMenuButton, LoadMainMenu);
        _uiManager.CreateButtonListener(_uiManager.SettingsUI.QuitGameButton, QuitGame);

        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.NewGameButton, CheckNewGame);
        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.LoadGameButton, LoadGame);
        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.QuitGameButton, QuitGame);
        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.WarningYesButton, NewGame);
    }

    public void CheckNewGame()
    {
        if (_uiManager.MainMenuUI.CheckHasSave() == false)
        {
            NewGame();
        }
        else
        {
            _uiManager.MainMenuUI.OpenWarningPanel();
        }
    }

    public void NewGame()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        _uiManager.MainMenuUI.ResetToStandard();

        _persistentData.NewSaveData();

        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STARTER_SELECT);
    }

    public void LoadGame()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES);
    }

    public void QuitGame()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        SaveSessionData(true);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    public void LoadMainMenu()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        _uiManager.HabitatUI.ResetStandardUI();
        _uiManager.MainMenuUI.CheckShowLoadGameButton();

        SaveSessionData(true);

        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.MAIN_MENU);
    }

    public void LoadStonePlains()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES);
    }

    public void LoadTemple()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        SaveSessionData(true);

        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.TEMPLE);
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