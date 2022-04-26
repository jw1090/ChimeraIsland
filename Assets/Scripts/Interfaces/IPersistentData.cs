public interface IPersistentData
{
    PersistentData Initialize();
    void SaveGameData();
    void LoadGameData();

    int GetLevelToLoad();

    void SaveData(string key, int value);
    public void SaveData(string key, float val);
    public void SaveData(string key, string val);

    public int LoadDataInt(string key);
    public float LoadDataFloat(string key);
    public string LoadDataString(string key);

}
