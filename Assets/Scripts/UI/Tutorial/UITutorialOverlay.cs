using UnityEngine;
public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private UITextInfo _textInfo = null;
    private HabitatUI _habitatUI = null;
    private TutorialStageData _tutorialData = null;
    private HabitatManager _habitatManager = null;
    private int _tutorialStep = -1;
    private TutorialStageType _tutorialType;
    private TutorialManager _tutorialManager = null;
    public void Initialize(HabitatUI habitatUI)
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _habitatUI = habitatUI;

        this.gameObject.SetActive(false);
    }

    public void ShowOverlay(TutorialStageData tutorialSteps, TutorialStageType tutorialType)
    {
            _tutorialType = tutorialType;
            _tutorialStep = -1;
            _tutorialData = tutorialSteps;
            _textInfo.gameObject.SetActive(true);
            NextStep();
    }

    public void NextStep()
    {
        if (_textInfo.Finished == true)
        {
            _tutorialStep++;
            ShowStep();
        }
        else
        {
            _textInfo.FinishNow();
        }

        // Debug.Log($"Current Tutorial Step: { _tutorialStep}");
    }

    public void ShowStep()
    {
        if (_tutorialStep >= _tutorialData.StepData.Length)
        {
            _tutorialManager.TutorialComplete(_tutorialType);
            _habitatUI.EndTutorial();
            _textInfo.Done();
            return;
        }

        TutorialStepData loadedStep = _tutorialData.StepData[_tutorialStep];

        Sprite icon = _habitatManager.CurrentHabitat.GetFirstChimera().ChimeraIcon;

        _textInfo.Load(_tutorialData.StepData[_tutorialStep].description, icon);

        // Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");
    }
}