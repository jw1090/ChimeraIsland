using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWorldMap : MonoBehaviour
{
    private void Awake()
    {
        GameLoader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {

    }

    public void LoadWorldMap()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void LoadStonePlains()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void LoadTreeOfLife()
    {
        SceneManager.LoadSceneAsync(5);
    }
}
