using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        if (_uiManager.NewGameButton != null)
        {
            CreateButtonListener(_uiManager.NewGameButton, NewGame);
        }
        if (_uiManager.LoadGameButton != null)
        {
            CreateButtonListener(_uiManager.LoadGameButton, LoadGame);
        }
        if (_uiManager.MainMenuButton != null)
        {
            CreateButtonListener(_uiManager.MainMenuButton, LoadMainMenu);
        }
        if (_uiManager.WorldMapButton != null)
        {
            CreateButtonListener(_uiManager.WorldMapButton, LoadWorldMap);
        }
        if (_uiManager.StonePlainsButton != null)
        {
            CreateButtonListener(_uiManager.StonePlainsButton, LoadStonePlains);
        }
        if (_uiManager.TreeOfLifeButton != null)
        {
            CreateButtonListener(_uiManager.TreeOfLifeButton, LoadTreeOfLife);
        }
        if (_uiManager.AshLandsButton != null)
        {
            CreateButtonListener(_uiManager.AshLandsButton, LoadAshlands);
        }
    }

    private void CreateButtonListener(Button button, Action action)
    {
        if (button != null)
        {
            button.onClick.AddListener
            (delegate
            {
                action();
            });
        }
    }

    public void NewGame()
    {
        _uiManager.DisableAllSceneTypeUI();
        _persistentData.NewSaveData();
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STARTER_SELECT_SCENE);
    }

    public void LoadGame()
    {
        _uiManager.DisableAllSceneTypeUI();
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES_SCENE);
    }

    public void QuitGame()
    {
        _uiManager.DisableAllSceneTypeUI();
        SaveSessionData(true);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    public void LoadMainMenu()
    {
        _uiManager.DisableAllSceneTypeUI();
        SaveSessionData(true);
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.MAIN_MENU_SCENE);
    }

    public void LoadWorldMap()
    {
        _uiManager.DisableAllSceneTypeUI();
        SaveSessionData(false);
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.WORLD_MAP_SCENE);
    }

    public void LoadStonePlains()
    {
        _uiManager.DisableAllSceneTypeUI();
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES_SCENE);
    }

    public void LoadTreeOfLife()
    {
        _uiManager.DisableAllSceneTypeUI();
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.TREE_OF_LIFE_SCENE);
    }

    public void LoadAshlands()
    {
        _uiManager.DisableAllSceneTypeUI();
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.ASHLANDS_SCENE);
    }

    private void SaveSessionData(bool quitGame)
    {
        if (_habitatManager.CurrentHabitat == null)
        {
            Debug.LogError("Current Habitat is null! Cannot save!");
            return;
        }

        _habitatManager.UpdateCurrentHabitatChimeras();

        if(quitGame == true)
        {
            _persistentData.QuitGameSave();
        }
        else
        {
            _persistentData.SaveSessionData();
        }
    }
}