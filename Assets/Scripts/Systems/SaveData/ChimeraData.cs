using System;

[Serializable]
public class ChimeraData
{
    public HabitatType habitatType;
    public ChimeraType chimeraType;
    public int level = 0;
    public int intelligence = 0;
    public int strength = 0;
    public int endurance = 0;
    public int happiness = 0;

    public ChimeraData(Chimera chimera)
    {
        habitatType = chimera.HabitatType;
        chimeraType = chimera.ChimeraType;
        level = chimera.Level;
        endurance = chimera.Endurance;
        intelligence = chimera.Intelligence;
        strength = chimera.Strength;
        happiness = chimera.Happiness;
    }
}