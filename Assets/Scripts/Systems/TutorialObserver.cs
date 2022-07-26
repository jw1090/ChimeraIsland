using UnityEngine;
using UnityEngine.UI;

public class TutorialObserver : MonoBehaviour
{
    private UIManager _uiManager = null;
    private TutorialManager _tutorialManager = null;

    public TutorialObserver Initialize(UIManager ui)
    {
        _uiManager = ui;
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        CreateButtonListenerExpeditionRequirements(_uiManager.HabitatUI.ExpeditionButton);
        CreateButtonFacilityShop(_uiManager.HabitatUI.MarketplaceButton);
        CreateButtonTraining(_uiManager.HabitatUI.WaterfallButton);
        return this;
    }

    private void CreateButtonListenerExpeditionRequirements(Button button)
    {
        button.onClick.AddListener(() =>
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.ExpeditionRequirements);
        });
    }

    private void CreateButtonFacilityShop(Button button)
    {
        if (button != null)
        {
            button.onClick.AddListener
            (delegate
            {
                ActivateTutorial(TutorialStageType.FacilityShop);
                button.onClick.RemoveListener(() => ActivateTutorial(TutorialStageType.FacilityShop));
            });
        }
        else
        {
            Debug.LogError($"{button} is null! Please Fix");
        }
    }

    private void CreateButtonTraining(Button button)
    {
        if (button != null)
        {
            button.onClick.AddListener
            (delegate
            {
                ActivateTutorial(TutorialStageType.Training);
                button.onClick.RemoveListener(() => ActivateTutorial(TutorialStageType.Training));
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
