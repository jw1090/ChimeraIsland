using UnityEngine;
using UnityEngine.UI;

public class TutorialObserver : MonoBehaviour
{
    private UIManager _uiManager = null;
    private TutorialManager _tutorialManager = null;

    public TutorialObserver Initialize(UIManager uiManager)
    {
        _habitatUI = uiManager.HabitatUI;
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        
        return this;
    }

    private void CreateButtonListenerExpeditionRequirements(Button button)
    {
        button.onClick.AddListener(() =>
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.ExpeditionRequirements);
        });
    }

    private void CreateTutorialListener(Button button, TutorialStageType tutorialStageType)
    {
        if (button != null)
        {
            button.onClick.AddListener
            (delegate
            {
                ActivateTutorial(tutorialStageType);
                button.onClick.RemoveListener(() => ActivateTutorial(tutorialStageType));
            });
        }
        else
        {
            Debug.LogError($"{button} is null! Please Fix");
        }
    }

    public void ActivateTutorial(TutorialStageType type)
    {
        _tutorialManager.ShowTutorialStage(type);
    }
}
