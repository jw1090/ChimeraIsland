using UnityEngine;
using System.Collections.Generic;

public class PersistentData : MonoBehaviour
{
    private Chimera _chimeraPrefabA = null;
    private Chimera _chimeraPrefabB = null;
    private Chimera _chimeraPrefabC = null;

    public Habitat CurrentHabitat { get; set; } = null;

    public List<ChimeraSaveData> GetChimeraList()
    {
        return FileHandler.ReadListFromJSON<ChimeraSaveData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
    }

    public PersistentData Initialize()
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
        List<ChimeraSaveData> jList = FileHandler.ReadListFromJSON<ChimeraSaveData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);

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

        /* TODO load chimera data into HabitatManager dictionary
        CurrentHabitat.ClearChimeras();

        foreach (ChimeraSaveData chimeraJson in jList)
        {
            Chimera newChimera = CurrentHabitat.AddChimera(FindPrefab(chimeraJson.chimeraType));
            newChimera.SetChimeraType(chimeraJson.chimeraType);
            newChimera.Level = chimeraJson.level;
            newChimera.Endurance = chimeraJson.endurance;
            newChimera.Intelligence = chimeraJson.intelligence;
            newChimera.Strength = chimeraJson.strength;
            newChimera.Happiness = chimeraJson.happiness;
        }*/

        return true;
    }

    public SaveJsonList GetChimeraJsonList()
    {
        SaveJsonList sjl = new SaveJsonList { };
        sjl.CurrentChimeraCapacity = CurrentHabitat.GetCapacity();
        foreach (Chimera chimera in CurrentHabitat.Chimeras)
        {
            ChimeraSaveData temp = new ChimeraSaveData
            (
                chimera.GetInstanceID(),
                chimera.GetChimeraType(),
                chimera.Level,
                chimera.Endurance,
                chimera.Intelligence,
                chimera.Strength,
                chimera.Happiness,
                CurrentHabitat.GetHabitatType()
            );

            sjl.AddToChimeraList(temp);
        }
        return sjl;
    }

    public void SaveChimeras()
    {
        SaveJsonList myData = GetChimeraJsonList();
        FileHandler.SaveToJSON(myData.GetChimeraList(), GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
    }

    public void OnApplicationQuit()
    {
        SaveChimeras();
    }
}
