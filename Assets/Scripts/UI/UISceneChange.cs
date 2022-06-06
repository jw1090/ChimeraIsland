using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneChange : MonoBehaviour
{
    HabitatManager _habitatManager = null;
    PersistentData _persistentData = null;

    public void Initialize()
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();
    }

    public void LoadWorldMap()
    {
        UpdateHabitatInfo();
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

    private void UpdateHabitatInfo()
    {
        _habitatManager.UpdateCurrentHabitatChimeras();
        _persistentData.SaveSessionData();
    }
}