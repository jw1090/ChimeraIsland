using UnityEngine;
using System.Collections.Generic;

public class PersistentData : MonoBehaviour
{
    public Habitat CurrentHabitat { get; set; } = null;

    public List<ChimeraSaveData> GetChimeraList()
    {
        return FileHandler.ReadListFromJSON<ChimeraSaveData>(GameConsts.JsonSaveKeys.CHIMERA_SAVE_DATA_FILE);
    }

    public PersistentData Initialize()
    {
        Debug.Log($"<color=lime> {this.GetType()} Initialized!</color>");

        return this;
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

        return true;
    }

    private SaveJsonList GetChimeraJsonList()
    {
        if (CurrentHabitat == null)
        {
            Debug.Log("No Current Habitat Stored.");

            return null;
        }

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
