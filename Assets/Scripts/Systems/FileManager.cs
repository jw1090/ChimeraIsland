using UnityEngine;
using System.Collections.Generic;

public class FileManager : MonoBehaviour
{
    [SerializeField] Chimera ChimeraPrefabA;
    [SerializeField] Chimera ChimeraPrefabB;
    [SerializeField] Chimera ChimeraPrefabC;
    private IPersistentData _persistentData = null;

    public FileManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing File Manager ... </color>");

        _persistentData = ServiceLocator.Get<IPersistentData>();

        LoadSavedData();

        return this;
    }

    private Chimera getPrefab(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
                return ChimeraPrefabA;
            case ChimeraType.A1:
                return ChimeraPrefabA;
            case ChimeraType.A2:
                return ChimeraPrefabA;
            case ChimeraType.A3:
                return ChimeraPrefabA;
            case ChimeraType.B:
                return ChimeraPrefabB;
            case ChimeraType.B1:
                return ChimeraPrefabB;
            case ChimeraType.B2:
                return ChimeraPrefabB;
            case ChimeraType.B3:
                return ChimeraPrefabB;
            case ChimeraType.C:
                return ChimeraPrefabC;
            case ChimeraType.C1:
                return ChimeraPrefabC;
            case ChimeraType.C2:
                return ChimeraPrefabC;
            case ChimeraType.C3:
                return ChimeraPrefabC;
        }
        return ChimeraPrefabC;
    }

    private void LoadSavedData()
    {
        ServiceLocator.Get<EssenceManager>().LoadEssence();
        List<ChimeraJson> list = FileHandler.ReadListFromJSON<ChimeraJson>("myChimerasList" + ServiceLocator.Get<Habitat>().gameObject.name);
        int cap = FileHandler.ReadFromJSON<int>("myChimerasList" + ServiceLocator.Get<Habitat>().gameObject.name);
        if (list == null)
        {
            Debug.Log("Chimera Save not found");
            return;
        }
        if(ServiceLocator.Get<Habitat>().Chimeras.Count < cap) ServiceLocator.Get<Habitat>().SetChimeraCapacity(cap);
        ServiceLocator.Get<Habitat>().Massacre();
        foreach (ChimeraJson chimeraJson in list)
        {
            Chimera prefab = getPrefab(chimeraJson.cType);
            prefab.SetEndurance(chimeraJson.endurance);
            prefab.SetHappiness(chimeraJson.happiness);
            prefab.SetIntelligence(chimeraJson.intelligence);
            prefab.SetLevel(chimeraJson.level);
            prefab.SetChimeraType(chimeraJson.cType);
            prefab.SetStrength(chimeraJson.strength); 
            ServiceLocator.Get<Habitat>().AddChimera(prefab);
        }
        ServiceLocator.Get<Habitat>().KillCap();
    }
    public SaveJsonList GetChimeraJsonList()
    {
        SaveJsonList sjl = new SaveJsonList { };
        sjl.setChimeraCapacity(ServiceLocator.Get<Habitat>().Chimeras.Count);
        foreach (Chimera chimera in ServiceLocator.Get<Habitat>().Chimeras)
        {
            ChimeraJson temp = new ChimeraJson(chimera.GetInstanceID(), chimera.GetChimeraType(), chimera.GetLevel(), chimera.GetIntelligence(), chimera.GetStrength(), chimera.GetEndurance(), chimera.GetHappiness(), ServiceLocator.Get<Habitat>().GetHabitatType());
            sjl.addToChimeraList(temp);
        }
        return sjl;
    }
    public void SaveChimeras()
    {
        SaveJsonList myData = GetChimeraJsonList();
        FileHandler.SaveToJSON(myData.getChimeraList(), "myChimerasList" + ServiceLocator.Get<Habitat>().gameObject.name);
        FileHandler.SaveToJSON(myData.getChimeraCapacity(), "myChimerasCapacity" + ServiceLocator.Get<Habitat>().gameObject.name);
    }
    public void OnApplicationQuit()
    {
        SaveChimeras();
    }
}