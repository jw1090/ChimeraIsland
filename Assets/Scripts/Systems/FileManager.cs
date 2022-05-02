using UnityEngine;


public class FileManager : MonoBehaviour
{
    private IPersistentData _persistentData = null;

    public FileManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing File Manager ... </color>");

        _persistentData = ServiceLocator.Get<IPersistentData>();

        LoadSavedData();

        return this;
    }
    private void LoadSavedData()
    {
        ServiceLocator.Get<EssenceManager>().LoadEssence();
    }
    public SaveJsonList GetChimeraJsonList()
    {
        SaveJsonList sjl = new SaveJsonList { };
        foreach (Chimera chimera in ServiceLocator.Get<Habitat>().Chimeras)
        {
            ChimeraJson temp = new ChimeraJson
            (
                chimera.GetInstanceID(), chimera.GetChimeraType(),
                chimera.Level, chimera.Endurance, chimera.Intelligence, chimera.Strength, chimera.Happiness,
                ServiceLocator.Get<Habitat>().GetHabitatType()
            );

            sjl.addToChimeraList(temp);
        }
        return sjl;
    }
}