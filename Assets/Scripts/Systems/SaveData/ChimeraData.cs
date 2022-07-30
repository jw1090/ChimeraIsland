using System;

[Serializable]
public class ChimeraData
{
    public ChimeraType chimeraType = ChimeraType.None;
    public HabitatType habitatType = HabitatType.None;
    public int level = 0;
    public int agility = 0;
    public int intelligence = 0;
    public int strength = 0;

    public ChimeraData(Chimera chimera)
    {
        habitatType = chimera.HabitatType;
        chimeraType = chimera.ChimeraType;
        level = chimera.Level;
        agility = chimera.Agility;
        intelligence = chimera.Intelligence;
        strength = chimera.Strength;
    }
}