using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    private ResourceManager _resourceManager = null;

    private TutorialSteps _tutorialData;
    private int _tutorialStep = -1;

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void ShowOverlay(TutorialSteps tutorialSteps)
    {
        _tutorialData = tutorialSteps; 
        _textInfo.gameObject.SetActive(true);
        NextStep();
    }

    public void NextStep()
    {
        _tutorialStep++;
        Debug.Log($"Current Tutorial Step: { _tutorialStep}");
        ShowStep();
    }

    public void ShowStep()
    {
        if(_tutorialStep >= _tutorialData.StepData.Length)
        {
            _tutorialStep = 0;
            _textInfo.gameObject.SetActive(false);
            return;
        }

        TutorialStepData loadedStep = _tutorialData.StepData[_tutorialStep];
        Sprite icon = _resourceManager.GetChimeraSprite(loadedStep.type);

        Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");
       _textInfo.Load(_tutorialData.StepData[_tutorialStep].description, icon);
    }
}