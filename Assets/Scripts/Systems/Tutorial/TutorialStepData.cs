using System;

[Serializable]
public class TutorialStepData
{
    public string description;
    public ChimeraType type = ChimeraType.None;
    public string activateElement = TutorialUIElementType.None.ToString();
}