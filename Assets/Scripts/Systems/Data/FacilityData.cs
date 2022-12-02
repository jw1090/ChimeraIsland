using System;

[Serializable]
public class FacilityData
{
    public FacilityType Type = FacilityType.None;
    public int CurrentTier = 0;
    public int StoredChimeraId = 0;
    public int TrainToLevel = 0;
    public int ChimeraStatEXP = 0;
    public float SliderValue = 0.0f;
    public float SliderMax = 0.0f;

    public FacilityData(Facility facility)
    {
        Type = facility.Type;
        CurrentTier = facility.CurrentTier;

        if (facility.ActivateTraining == true)
        {
            StoredChimeraId = facility.StoredChimera.UniqueID;
            TrainToLevel = facility.TrainToLevel;
            ChimeraStatEXP = facility.StoredChimera.GetEXP(facility.StatType);
            SliderValue = facility.TrainingIcon.currentValue;
            SliderMax = facility.TrainingIcon.max;
        }
    }
}