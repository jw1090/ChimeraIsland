using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    [SerializeField] private TextAsset _tutorialJsonPath = null;

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
    public void ShowOverlay()
    {
        DialogSteps loadedTutorial = JsonUtility.FromJson<DialogSteps>(_tutorialJsonPath.text);
        Debug.Log($"Descrpition: {loadedTutorial.Steps[1].description}  Icon:{loadedTutorial.Steps[1].icon}");
        var icon = Resources.Load<Sprite>(loadedTutorial.Steps[1].icon);
       _textInfo.Load(loadedTutorial.Steps[1].description, icon);
    }
}
