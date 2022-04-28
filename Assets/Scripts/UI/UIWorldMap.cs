using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWorldMap : MonoBehaviour
{
    private IPersistentData _persistentData = null;

    private void Awake()
    {
        GameLoader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        _persistentData = ServiceLocator.Get<IPersistentData>();
    }
    public void LoadWorldMap()
    {
        _persistentData.GetLevelToLoad();
        SceneManager.LoadSceneAsync(2);
    }
    public void LoadStonePlains()
    {
        _persistentData.GetLevelToLoad();
        SceneManager.LoadSceneAsync(3);
    }
    public void LoadTreeOfLife()
    {
        _persistentData.GetLevelToLoad();
        SceneManager.LoadSceneAsync(4);
    }
}
