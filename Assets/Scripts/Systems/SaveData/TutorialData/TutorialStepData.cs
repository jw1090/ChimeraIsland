using System;

[Serializable]
public class TutorialStepData
{
    public string description;
    public string type = TutorialIconType.None.ToString();
    public string activateElement = TutorialUIElementType.None.ToString();
}