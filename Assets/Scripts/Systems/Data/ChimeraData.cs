using System;

[Serializable]
public class ChimeraData
{
    public ChimeraType chimeraType = ChimeraType.None;
    public HabitatType habitatType = HabitatType.None;
    public int exploration = 0;
    public int stamina = 0;
    public int wisdom = 0;
    public int currentEnergy = 0;
    public int uniqueId = 0;
    public bool first = false;
    public string customName = null;

    public ChimeraData(Chimera chimera)
    {
        first = chimera.FirstChimera;
        habitatType = chimera.HabitatType;
        chimeraType = chimera.ChimeraType;
        stamina = chimera.Stamina;
        wisdom = chimera.Wisdom;
        exploration = chimera.Exploration;
        currentEnergy = chimera.CurrentEnergy;
        customName = chimera.CustomName;
        uniqueId = chimera.UniqueID;
    }
}