using UnityEngine;
using System.Collections.Generic;

public class FileManager : MonoBehaviour
{
    [SerializeField] Chimera ChimeraPrefabA;
    [SerializeField] Chimera ChimeraPrefabB;
    [SerializeField] Chimera ChimeraPrefabC;

    public Habitat CurrentHabitat { get; set; } = null;

    public FileManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing File Manager ... </color>");

        return this;
    }

    private Chimera GetPrefab(ChimeraType type)
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

    public bool LoadSavedData()
    {
        ServiceLocator.Get<EssenceManager>().LoadEssence();
        List<ChimeraJson> jList = FileHandler.ReadListFromJSON<ChimeraJson>("myChimerasList" + CurrentHabitat.gameObject.name);
        //List<ChimeraJson> list = jList.getChimeraList();

        if (jList == null || jList.Count == 0)
        {
            Debug.Log("Chimera Save not found");
            return false;
        }

        int cap = 3;

        if (cap > CurrentHabitat.GetCapacity())
        {
            CurrentHabitat.SetChimeraCapacity(cap);
        }

        CurrentHabitat.ClearChimeras();

        foreach (ChimeraJson chimeraJson in jList)
        {
            Chimera newChimera = CurrentHabitat.AddChimera(GetPrefab(chimeraJson.cType));
            newChimera.Endurance = chimeraJson.endurance;
            newChimera.Happiness = chimeraJson.happiness;
            newChimera.Intelligence = chimeraJson.intelligence;
            newChimera.Level = chimeraJson.level;
            newChimera.SetChimeraType(chimeraJson.cType);
            newChimera.Strength = chimeraJson.strength;
        }

        return true;
    }

    public SaveJsonList GetChimeraJsonList()
    {
        SaveJsonList sjl = new SaveJsonList { };
        sjl.CurrentChimeraCapacity = CurrentHabitat.GetCapacity();
        foreach (Chimera chimera in CurrentHabitat.Chimeras)
        {
            ChimeraJson temp = new ChimeraJson
            (
                chimera.GetInstanceID(), chimera.GetChimeraType(),
                chimera.Level, chimera.Endurance, chimera.Intelligence, chimera.Strength, chimera.Happiness,
                CurrentHabitat.GetHabitatType()
            );

            sjl.AddToChimeraList(temp);
        }
        return sjl;
    }

    public void SaveChimeras()
    {
        SaveJsonList myData = GetChimeraJsonList();
        FileHandler.SaveToJSON(myData.GetChimeraList(), "myChimerasList" + CurrentHabitat.gameObject.name);
    }

    public void OnApplicationQuit()
    {
        SaveChimeras();
    }
}
