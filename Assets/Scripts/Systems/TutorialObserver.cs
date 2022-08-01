using UnityEngine;
using UnityEngine.UI;

public class TutorialObserver : MonoBehaviour
{
    private HabitatUI _habitatUI = null;
    private TutorialManager _tutorialManager = null;

    public bool DetailsTutorial { get; set; } = false;

    public TutorialObserver Initialize(UIManager uiManager)
    {
        _habitatUI = uiManager.HabitatUI;
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        CreateTutorialListener(_habitatUI.ExpeditionButton, TutorialStageType.ExpeditionRequirements);
        CreateTutorialListener(_habitatUI.MarketplaceButton, TutorialStageType.FacilityShop);
        CreateTutorialListener(_habitatUI.WaterfallButton, TutorialStageType.Training);

        return this;
    }

    private void CreateTutorialListener(Button button, TutorialStageType tutorialStageType)
    {
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                _tutorialManager.ShowTutorialStage(tutorialStageType);
            });
        }
    }
}