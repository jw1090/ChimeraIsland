using UnityEngine;

public class FileManager : MonoBehaviour
{
    private IPersistentData _persistentData = null;

    public FileManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing File Manager ... </color>");

        _persistentData = ServiceLocator.Get<IPersistentData>();
        Debug.Log($"{(_persistentData == null ? "NULL" : "OK")}");

        LoadSavedData();

        return this;
    }

    private void LoadSavedData()
    {
        ServiceLocator.Get<EssenceManager>().LoadEssence();
    }
}