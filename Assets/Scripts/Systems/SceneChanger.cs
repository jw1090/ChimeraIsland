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
        _uiManager.CreateButtonListener(_uiManager.HabitatUI.MainMenuButton, LoadMainMenu);
        _uiManager.CreateButtonListener(_uiManager.HabitatUI.WorldMapButton, LoadWorldMap);
        _uiManager.CreateButtonListener(_uiManager.HabitatUI.QuitGameButton, QuitGame);
        _uiManager.CreateButtonListener(_uiManager.WorldMapUI.StonePlainsButton, LoadStonePlains);
        _uiManager.CreateButtonListener(_uiManager.WorldMapUI.TreeOfLifeButton, LoadTreeOfLife);
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

    public void LoadWorldMap()
    {
        SaveSessionData(false);
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.WORLD_MAP);
    }

    public void LoadStonePlains()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES);
    }

    public void LoadTreeOfLife()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.TREE_OF_LIFE);
    }

    public void LoadAshlands()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.ASHLANDS);
    }

    private void SaveSessionData(bool quitGame)
    {
        if (_habitatManager.CurrentHabitat == null)
        {
            Debug.LogError("Current Habitat is null! Cannot save!");
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