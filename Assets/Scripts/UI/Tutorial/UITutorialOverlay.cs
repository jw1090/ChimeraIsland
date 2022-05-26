using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    [SerializeField] private TextAsset _tutorialJsonPath = null;
    
    [System.Serializable]
    public class TutorialJson
    {
        public string description;
        public string info;
        public string icon;
    }

    public void ShowOverlay()
    {
        var loadedTutorial = JsonUtility.FromJson<TutorialJson>(_tutorialJsonPath.text);
        Debug.Log($"Descrpition: {loadedTutorial.description}  Icon:{loadedTutorial.icon}");
        var icon = Resources.Load<Sprite>(loadedTutorial.icon);
        _textInfo.Load(loadedTutorial.description, icon);
    }
}
