using UnityEngine;

public class PersistentData : MonoBehaviour, IPersistentData
{
    private bool _isLoaded = false;
    private int _levelToLoad = 2;

    public PersistentData Initialize()
    {
        LoadGameData();
        Debug.Log($"<color=lime> {this.GetType()} Initialized!</color>");
        return this;
    }

    public void LoadGameData()
    {
        Debug.Log("<color=cyan> Loading Game Data ... </color>");
        _levelToLoad = PlayerPrefs.GetInt(GameConsts.GameSaveKeys.LEVEL_TO_LOAD, 2);
        _isLoaded = true;
    }

    public void SaveGameData()
    {
        Debug.Log("<color=cyan> Saving Game Data ... </color>");
        PlayerPrefs.SetInt(GameConsts.GameSaveKeys.LEVEL_TO_LOAD, 2);
        
    }

    public void SaveData(string key, int val){PlayerPrefs.SetInt(key, val);}
    public void SaveData(string key, float val) {PlayerPrefs.SetFloat(key, val);}
    public void SaveData(string key, string val) {PlayerPrefs.SetString(key, val);}

    public int LoadDataInt(string key) { return PlayerPrefs.GetInt(key, 0); }
    public float LoadDataFloat(string key) { return PlayerPrefs.GetFloat(key, 0.0f); }
    public string LoadDataString(string key) { return PlayerPrefs.GetString(key, "default"); }

    public int GetLevelToLoad()
    {
        if (!_isLoaded)
        {
            Debug.LogError("Persistent Data Has Not Finished Loading!");
            return 0;
        }

        return _levelToLoad;
    }
}
