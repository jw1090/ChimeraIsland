using System;

[Serializable]
public class TutorialStageData
{
    public TutorialStepData[] StepData;
    public string Darken = TutorialDarkenType.Standard.ToString();
}