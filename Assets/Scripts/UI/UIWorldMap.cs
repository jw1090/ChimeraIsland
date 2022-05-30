using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWorldMap : MonoBehaviour
{
    public void LoadWorldMap()
    {
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
}