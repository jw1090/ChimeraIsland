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
    private void FadeInLoadScreen(SceneType sceneType)
    {
        RecentSceneChange = true;
        StartCoroutine(_uiManager.FadeInLoadingScreen(sceneType));
    }

    public void SetupUIListeners()
    {
        _uiManager = ServiceLocator.Get<UIManager>();

        _uiManager.CreateButtonListener(_uiManager.SettingsUI.MainMenuButton, StartLoadMainMenu);
        _uiManager.CreateButtonListener(_uiManager.SettingsUI.QuitGameButton, QuitGame);

        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.NewGameButton, CheckNewGame);
        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.LoadGameButton, StartLoadGame);
        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.QuitGameButton, QuitGame);
        _uiManager.CreateButtonListener(_uiManager.MainMenuUI.WarningYesButton, StartNewGame);
    }

    public void CheckNewGame()
    {
        if (_uiManager.MainMenuUI.CheckHasSave() == false)
        {
            StartNewGame();
        }
        else
        {
            _uiManager.MainMenuUI.OpenWarningPanel();
        }
    }

    public void StartNewGame()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        _uiManager.MainMenuUI.ResetToStandard();
        _persistentData.NewSaveData();
        FadeInLoadScreen(SceneType.Starting);
    }

    public void StartLoadGame()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        FadeInLoadScreen(SceneType.Habitat);
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

    public void StartLoadMainMenu()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        if (_uiManager.InHabitatState == true)
        {
            _uiManager.HabitatUI.ResetStandardUI();
        }

        _uiManager.MainMenuUI.CheckShowLoadGameButton();

        SaveSessionData(true);

        FadeInLoadScreen(SceneType.MainMenu);
    }

    public void LoadStonePlains()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        FadeInLoadScreen(SceneType.Habitat);
    }

    public void LoadTemple()
    {
        if (RecentSceneChange == true)
        {
            return;
        }

        SaveSessionData(true);
        FadeInLoadScreen(SceneType.Temple);
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