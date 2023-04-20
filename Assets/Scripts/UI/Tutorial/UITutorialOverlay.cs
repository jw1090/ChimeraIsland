using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private UITextInfo _textInfo = null;
    [SerializeField] private StatefulObject _darken = null;
    private UIManager _UIManager = null;
    private TutorialStageData _tutorialData = null;
    private HabitatManager _habitatManager = null;
    private TutorialManager _tutorialManager = null;
    private TutorialStageType _tutorialType;
    private int _tutorialStep = -1;
    private Sprite _icon = null;

    public void SetFirstChimeraSprite(Sprite icon) { _icon = icon; }

    public void Initialize(UIManager UIManager)
    {
        _darken.SetState("StandardBG");
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        _UIManager = UIManager;

        this.gameObject.SetActive(false);
    }

    public void ShowOverlay(TutorialStageData tutorialSteps, TutorialStageType tutorialType)
    {
        //switch ((TutorialDarkenType)Enum.Parse(typeof(TutorialDarkenType), tutorialSteps.Darken, true))
        //{
        //    case TutorialDarkenType.Standard:
        //        _darken.SetState("StandardBG");
        //        break;
        //    case TutorialDarkenType.FirstExpedition:
        //        _darken.SetState("FirstExpeditionBG");
        //        break;
        //    case TutorialDarkenType.ChimeraList:
        //        _darken.SetState("ChimeraListBG");
        //        break;
        //    case TutorialDarkenType.ReccomendedTraits:
        //        _darken.SetState("ReccomendedTraitsBG");
        //        break;
        //    default:
        //        Debug.LogError($"TutorialDarkenType: {tutorialSteps.Darken} not available");
        //        break;
        //}

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
            _UIManager.EndTutorial();
            _textInfo.Done();
            return;
        }

        _textInfo.Load(_tutorialData.StepData[_tutorialStep].Description, _icon);
    }
}