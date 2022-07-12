using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    private HabitatUI _habitatUI = null;
    private ResourceManager _resourceManager = null;
    private TutorialSteps _tutorialData = null;
    private int _tutorialStep = -1;

    public void Initialize(HabitatUI habitatUI)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _habitatUI = habitatUI;
    }

    public void ShowOverlay(TutorialSteps tutorialSteps)
    {
        _tutorialStep = -1;
        _tutorialData = tutorialSteps;
        _textInfo.gameObject.SetActive(true);
        NextStep();
    }

    public void NextStep()
    {
        _tutorialStep++;
        ShowStep();

        // Debug.Log($"Current Tutorial Step: { _tutorialStep}");
    }

    public void ShowStep()
    {
        if(_tutorialStep >= _tutorialData.StepData.Length)
        {
            _tutorialData.finished = true;
            _habitatUI.EndTutorial();
            return;
        }
        TutorialStepData loadedStep = _tutorialData.StepData[_tutorialStep];

        if(loadedStep.activateElement != UIElementType.None)
        {
            _habitatUI.EnableHabitatUIByType(loadedStep.activateElement);
        }

        Sprite icon = _resourceManager.GetChimeraSprite(loadedStep.type);

       _textInfo.Load(_tutorialData.StepData[_tutorialStep].description, icon);

        // Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");
    }
}