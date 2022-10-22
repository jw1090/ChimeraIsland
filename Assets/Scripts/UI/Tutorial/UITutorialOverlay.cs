using System;
using UnityEngine;
public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private UITextInfo _textInfo = null;
    [SerializeField] private StatefulObject _darken = null;
    private HabitatUI _habitatUI = null;
    private TutorialStageData _tutorialData = null;
    private HabitatManager _habitatManager = null;
    private TutorialManager _tutorialManager = null;
    private TutorialStageType _tutorialType;
    private int _tutorialStep = -1;

    public void Initialize(HabitatUI habitatUI)
    {
        _darken.SetState("StandardBG");
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        _habitatUI = habitatUI;

        this.gameObject.SetActive(false);
    }

    public void ShowOverlay(TutorialStageData tutorialSteps, TutorialStageType tutorialType)
    {
        switch ((TutorialDarkenType)Enum.Parse(typeof(TutorialDarkenType), tutorialSteps.Darken, true))
        {
            case TutorialDarkenType.Standard:
                _darken.SetState("StandardBG");
                break;
            case TutorialDarkenType.firstExpedition:
                _darken.SetState("FirstExpeditionBG");
                break;
            case TutorialDarkenType.ChimeraList:
                _darken.SetState("ChimeraListBG");
                break;
            case TutorialDarkenType.ReccomendedTraits:
                _darken.SetState("ReccomendedTraitsBG");
                break;
            default:
                Debug.LogError($"TutorialDarkenType: {tutorialSteps.Darken} not available");
                break;
        }
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
            ++_tutorialStep;
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

        Sprite icon = _habitatManager.CurrentHabitat.GetFirstChimera().ChimeraIcon;

        _textInfo.Load(_tutorialData.StepData[_tutorialStep].Description, icon);
    }
}