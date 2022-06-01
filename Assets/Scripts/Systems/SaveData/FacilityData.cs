using System;

[Serializable]
public class FacilityData
{
    public FacilityType facilityType = FacilityType.None;
    public HabitatType habitatType = HabitatType.None;
    public int currentTier = 0;

    public FacilityData(Facility facility, HabitatType facilityHabitatType)
    {
        facilityType = facility.Type;
        habitatType = facilityHabitatType;
        currentTier = facility.CurrentTier;
    }
}