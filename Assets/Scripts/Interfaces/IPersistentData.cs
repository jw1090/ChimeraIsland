public interface IPersistentData
{
    PersistentData Initialize();
    void SaveGameData();
    void LoadGameData();

    int GetLevelToLoad();
}
