using System;
using System.Collections.Generic;

[Serializable]
public class TutorialStageData
{
    public TutorialStepData[] StepData;
    public List<string> QuestType = new List<string>();
    public string Darken = TutorialDarkenType.Standard.ToString();
}