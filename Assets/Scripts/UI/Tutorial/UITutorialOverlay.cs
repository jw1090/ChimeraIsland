using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
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
        DialogSteps tutorialData = FileHandler.ReadFromJSON<DialogSteps>(GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
        if (tutorialData == null)
        {
            Debug.Log($"No tutorial Data found");
            tutorialData = new DialogSteps();
        }

        DialogInfo loadedStep = tutorialData.Steps[stepNumber];
        Sprite icon = _resourceManager.GetChimeraSprite(loadedStep.type);

        Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");

       _textInfo.Load(tutorialData.Steps[stepNumber].description, icon);
    }
}