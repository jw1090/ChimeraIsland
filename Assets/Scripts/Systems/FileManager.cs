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
        foreach (Chimera chimera in ServiceLocator.Get<Habitat>().GetChimeras())
        {
            ChimeraJson temp = new ChimeraJson(chimera.GetInstanceID(), chimera.GetChimeraType(), chimera.GetLevel(), chimera.GetIntelligence(), chimera.GetStrength(), chimera.GetEndurance(), chimera.GetHappiness(), ServiceLocator.Get<Habitat>().GetHabitatType());
            sjl.addToChimeraList(temp);
        }
        return sjl;
    }
}