using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
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

    public void LoadGameClicked()
    {
        int sceneIndex = _persistentData.GetLevelToLoad();
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
