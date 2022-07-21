using System;

[Serializable]
public class TutorialStepData
{
    public string description;
    public TutorialIconType type = TutorialIconType.None;
    public string activateElement = TutorialUIElementType.None.ToString();
}