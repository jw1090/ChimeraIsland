using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    private void Awake()
    {
        GameLoader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {

    }

    public void NewGameClicked()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.STARTER_SELECT_SCENE);
    }

    public void LoadGameClicked()
    {
        SceneManager.LoadSceneAsync(GameConsts.LevelToLoadInts.WORLD_MAP_SCENE);
    }
}
