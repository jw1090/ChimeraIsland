using UnityEngine;
using UnityEngine.UI;

public class TutorialObserver : MonoBehaviour
{
    private UIManager _uiManager = null;
    private HabitatUI _habitatUI = null;
    private TutorialManager _tutorialManager = null;
    private HabitatManager _habitatManager = null;
    private CurrencyManager _currencyManager = null;
    public bool DetailsTutorial { get; set; } = false;

    public TutorialObserver Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _habitatUI = _uiManager.HabitatUI;
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        SetupListeners();

        return this;
    }

    private void SetupListeners()
    {
        CreateTutorialListener(_habitatUI.ExpeditionButton, TutorialStageType.ExpeditionRequirements);
        CreateTutorialListener(_habitatUI.MarketplaceButton, TutorialStageType.FacilityShop);
        _uiManager.CreateButtonListener(_habitatUI.WaterfallButton, BuyFirstFacility);
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

    private void BuyFirstFacility()
    {
        if(_habitatManager.CurrentHabitat.GetFacility(FacilityType.Waterfall).Price <= _currencyManager.Essence)
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.Training);
        }
    }
}