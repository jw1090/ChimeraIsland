using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    [SerializeField] private TextAsset _tutorialJsonPath = null;
    [SerializeField] private int stepNumber = 0;

    [System.Serializable]
    public class DialogInfo
    {
        public string description;
        public string info;
        public string icon;
    }

    [System.Serializable]
    public class DialogSteps
    {
        public DialogInfo[] Steps;
    }
    public void NextStep()
    {
        stepNumber++;
        Debug.Log($"Current Tutorial Step: { stepNumber}");
        ShowOverlay();
    }
    public void ShowOverlay()
    {
        DialogSteps loadedTutorial = JsonUtility.FromJson<DialogSteps>(_tutorialJsonPath.text);
        Debug.Log($"Descrpition: {loadedTutorial.Steps[stepNumber].description}  Icon:{loadedTutorial.Steps[stepNumber].icon}");
        var icon = Resources.Load<Sprite>(loadedTutorial.Steps[stepNumber].icon);
       _textInfo.Load(loadedTutorial.Steps[stepNumber].description, icon);
    }
}