using UnityEngine;
using System;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private UITextInfo _textInfo = null;
    private UIManager _uiManager = null;
    private ResourceManager _resourceManager = null;
    private TutorialStageData _tutorialData = null;
    private int _tutorialStep = -1;

    public void Initialize(UIManager uiManager)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _uiManager = uiManager;
    }

    public void ShowOverlay(TutorialStageData tutorialSteps)
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
            _uiManager.EndTutorial();
            return;
        }

        TutorialStepData loadedStep = _tutorialData.StepData[_tutorialStep];
        if(loadedStep.activateElement != UIElementType.None.ToString())
        {
            _uiManager.EnableUIByType((UIElementType) Enum.Parse(typeof(UIElementType),loadedStep.activateElement));
        }

        Sprite icon = _resourceManager.GetTutorialSprite((TutorialIconType)Enum.Parse(typeof(TutorialIconType), loadedStep.type));

       _textInfo.Load(_tutorialData.StepData[_tutorialStep].description, icon);

        // Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");
    }
}