using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    private ResourceManager _resourceManager = null;
    private UIManager _uiManager = null;
    private TutorialSteps _tutorialData = null;
    private int _tutorialStep = -1;

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _uiManager = ServiceLocator.Get<UIManager>();
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
            _tutorialData.finished = true;
            _uiManager.EndTutorial();
            return;
        }
        TutorialStepData loadedStep = _tutorialData.StepData[_tutorialStep];
        if(loadedStep.activateElement != UIElementType.None)
        {
            _uiManager.EnableUIByType(loadedStep.activateElement);
        }
        Sprite icon = _resourceManager.GetChimeraSprite(loadedStep.type);

        Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");
       _textInfo.Load(_tutorialData.StepData[_tutorialStep].description, icon);
    }
}