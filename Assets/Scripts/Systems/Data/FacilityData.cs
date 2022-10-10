using System;

[Serializable]
public class FacilityData
{
    public FacilityType facilityType = FacilityType.None;
    public HabitatType habitatType = HabitatType.None;
    public int currentTier = 0;
    public int storedChimeraId = 0;
    public int trainToLevel = 0;
    public int chimeraStatXp = 0;
    public float sliderValue = 0.0f;
    public float sliderMax = 0.0f;

    public FacilityData(Facility facility, HabitatType facilityHabitatType)
    {
        facilityType = facility.Type;
        habitatType = facilityHabitatType;
        currentTier = facility.CurrentTier;

        if (facility.ActivateTraining == true)
        {
            storedChimeraId = facility.StoredChimera.UniqueID;
            trainToLevel = facility.TrainToLevel;
            chimeraStatXp = facility.StoredChimera.GetXP(facility.StatType);
            sliderValue = facility.TrainingIcon.currentValue;
            sliderMax = facility.TrainingIcon.max;
        }
    }
}