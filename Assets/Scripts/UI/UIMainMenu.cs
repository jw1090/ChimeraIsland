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
        SceneManager.LoadSceneAsync(2);
    }

    public void LoadGameClicked()
    {
        SceneManager.LoadSceneAsync(3);
    }
}
