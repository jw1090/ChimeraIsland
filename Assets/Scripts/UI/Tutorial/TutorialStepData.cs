using System;

[Serializable]
public class TutorialSteps
{
    public TutorialStepData[] StepData;
}

[Serializable]
public class TutorialStepData
{
    public string description;
    public ChimeraType type;
}