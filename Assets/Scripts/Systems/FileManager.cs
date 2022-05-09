using UnityEngine;
using System.Collections.Generic;

public class FileManager : MonoBehaviour
{
    private Chimera _chimeraPrefabA = null;
    private Chimera _chimeraPrefabB = null;
    private Chimera _chimeraPrefabC = null;

    public Habitat CurrentHabitat { get; set; } = null;

    public FileManager Initialize()
    {
        Debug.Log($"<color=lime> {this.GetType()} Initialized!</color>");

        _chimeraPrefabA = Resources.Load<Chimera>("Chimera/ChimeraPrefabA");
        _chimeraPrefabB = Resources.Load<Chimera>("Chimera/ChimeraPrefabA");
        _chimeraPrefabC = Resources.Load<Chimera>("Chimera/ChimeraPrefabA");

        return this;
    }

    private Chimera FindPrefab(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
                return _chimeraPrefabA;
            case ChimeraType.A1:
                return _chimeraPrefabA;
            case ChimeraType.A2:
                return _chimeraPrefabA;
            case ChimeraType.A3:
                return _chimeraPrefabA;
            case ChimeraType.B:
                return _chimeraPrefabB;
            case ChimeraType.B1:
                return _chimeraPrefabB;
            case ChimeraType.B2:
                return _chimeraPrefabB;
            case ChimeraType.B3:
                return _chimeraPrefabB;
            case ChimeraType.C:
                return _chimeraPrefabC;
            case ChimeraType.C1:
                return _chimeraPrefabC;
            case ChimeraType.C2:
                return _chimeraPrefabC;
            case ChimeraType.C3:
                return _chimeraPrefabC;
            default:
                Debug.LogWarning($"Unhandled prefab type {type}");
                return null;
        }
    }

    public bool LoadSavedData()
    {
        ServiceLocator.Get<EssenceManager>().LoadEssence();
        List<ChimeraJson> jList = FileHandler.ReadListFromJSON<ChimeraJson>("myChimerasList" + CurrentHabitat.gameObject.name);

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
            Chimera newChimera = CurrentHabitat.AddChimera(FindPrefab(chimeraJson.chimeraType));
            newChimera.SetChimeraType(chimeraJson.chimeraType);
            newChimera.Level = chimeraJson.level;
            newChimera.Endurance = chimeraJson.endurance;
            newChimera.Intelligence = chimeraJson.intelligence;
            newChimera.Strength = chimeraJson.strength;
            newChimera.Happiness = chimeraJson.happiness;
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
                CurrentHabitat.GetHabitatType(), chimera.GetChimeraType(), chimera.GetInstanceID(),
                chimera.Level, chimera.Endurance, chimera.Intelligence, chimera.Strength, chimera.Happiness
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
