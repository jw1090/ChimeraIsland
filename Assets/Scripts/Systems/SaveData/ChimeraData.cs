using System;

[Serializable]
public class ChimeraData
{
    public ChimeraType chimeraType = ChimeraType.None;
    public HabitatType habitatType = HabitatType.None;
    public int level = 0;
    public int exploration = 0;
    public int stamina = 0;
    public int wisdom = 0;
    public int currentEnergy = 0;

    public ChimeraData(Chimera chimera)
    {
        habitatType = chimera.HabitatType;
        chimeraType = chimera.ChimeraType;
        level = chimera.Level;
        stamina = chimera.Stamina;
        wisdom = chimera.Wisdom;
        exploration = chimera.Exploration;
        currentEnergy = chimera.CurrentEnergy;
    }
}