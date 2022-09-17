using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private UITextInfo _textInfo = null;
    private HabitatUI _habitatUI = null;
    private TutorialStageData _tutorialData = null;
    private HabitatManager _habitatManager = null;
    private int _tutorialStep = -1;

    public void Initialize(HabitatUI habitatUI)
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _habitatUI = habitatUI;

        this.gameObject.SetActive(false);
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
        if (_tutorialStep >= _tutorialData.StepData.Length)
        {
            _tutorialData.finished = true;
            _habitatUI.EndTutorial();
            return;
        }

        TutorialStepData loadedStep = _tutorialData.StepData[_tutorialStep];

        Sprite icon = _habitatManager.CurrentHabitat.GetFirstChimera().ChimeraIcon;

        _textInfo.Load(_tutorialData.StepData[_tutorialStep].description, icon);

        // Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");
    }
}