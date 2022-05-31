using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    [SerializeField] private TextAsset _tutorialJsonPath = null;
    private ResourceManager _resourceManager = null;
    private int stepNumber = 0;

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
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
        DialogInfo loadedStep = loadedTutorial.Steps[stepNumber];
        Sprite icon = _resourceManager.GetChimeraSprite(loadedStep.type);

        Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");

       _textInfo.Load(loadedTutorial.Steps[stepNumber].description, icon);
    }
}