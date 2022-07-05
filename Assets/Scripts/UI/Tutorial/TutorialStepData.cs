using System;

[Serializable]
public class TutorialSteps
{
    public TutorialStepData[] StepData;
    public bool finished = false;
}

[Serializable]
public class TutorialStepData
{
    public string description;
    public ChimeraType type;
}