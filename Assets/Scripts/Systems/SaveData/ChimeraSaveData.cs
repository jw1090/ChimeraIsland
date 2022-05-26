using System;

[Serializable]
public class ChimeraSaveData
{
    public HabitatType habitatType;
    public ChimeraType chimeraType;
    public int id = 0;
    public int level = 0;
    public int intelligence = 0;
    public int strength = 0;
    public int endurance = 0;
    public int happiness = 0;

    public ChimeraSaveData(
        int newId,
        ChimeraType newChimeraType,
        int newLevel,
        int newEndurance,
        int newIntelligence,
        int newStrength,
        int newHappiness,
        HabitatType newHabitatType)
    {
        id = newId;
        chimeraType = newChimeraType;
        level = newLevel;
        endurance = newEndurance;
        intelligence = newIntelligence;
        strength = newStrength;
        happiness = newHappiness;
        habitatType = newHabitatType;
    }
}
