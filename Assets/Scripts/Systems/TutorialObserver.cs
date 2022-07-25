using UnityEngine;
using UnityEngine.Events;
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
        if (button != null)
        {
            button.onClick.AddListener
            (delegate
            {
                ActivateTutorial(TutorialStageType.ExpeditionRequirements);
                button.onClick.RemoveListener(() => ActivateTutorial(TutorialStageType.ExpeditionRequirements));
            });
        }
        else
        {
            Debug.LogError($"{button} is null! Please Fix");
        }
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

