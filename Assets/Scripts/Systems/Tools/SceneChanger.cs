using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private HabitatManager _habitatManager = null;
    private PersistentData _persistentData = null;

    public SceneChanger Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        return this;
    }

    public void NewGame()
    {
        _persistentData.NewSaveData();
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STARTER_SELECT_SCENE);
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES_SCENE);
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
        SaveSessionData(true);
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.MAIN_MENU_SCENE);
    }

    public void LoadWorldMap()
    {
        SaveSessionData(false);
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.WORLD_MAP_SCENE);
    }

    public void LoadStonePlains()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STONE_PLANES_SCENE);
    }

    public void LoadTreeOfLife()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.TREE_OF_LIFE_SCENE);
    }

    public void LoadAshlands()
    {
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