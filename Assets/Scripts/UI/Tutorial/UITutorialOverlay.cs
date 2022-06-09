using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    private ResourceManager _resourceManager = null;
    private int _tutorialStep = 0;
    private int _tutorialId = 0;

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void NextStep()
    {
        _tutorialStep++;
        if(_tutorialStep >= 3)
        {
            _tutorialId++;
        }
        Debug.Log($"Current Tutorial Step: { _tutorialStep}");
        ShowOverlay();
    }

    public void ShowOverlay()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        Tutorial tutorialData = FileHandler.ReadFromJSON<Tutorial>(GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
        if (tutorialData == null)
        {
            Debug.Log($"No tutorial Data found");
            tutorialData = new Tutorial();
        }


        _tutorialStep = _tutorialStep % tutorialData.Tutorials.Length;
        _tutorialId = _tutorialId % tutorialData.Tutorials[_tutorialStep].StepData.Length;

        TutorialStepData loadedStep = tutorialData.Tutorials[_tutorialId].StepData[_tutorialStep];
        Sprite icon = _resourceManager.GetChimeraSprite(loadedStep.type);

        Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");
       _textInfo.Load(tutorialData.Tutorials[_tutorialId].StepData[_tutorialStep].description, icon);
    }
}