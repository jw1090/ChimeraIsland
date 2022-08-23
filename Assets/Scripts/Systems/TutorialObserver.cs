using UnityEngine;
using UnityEngine.UI;

public class TutorialObserver : MonoBehaviour
{
    private UIManager _uiManager = null;
    private HabitatUI _habitatUI = null;
    private TutorialManager _tutorialManager = null;
    public bool DetailsTutorial { get; set; } = false;

    public TutorialObserver Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _habitatUI = _uiManager.HabitatUI;
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        SetupListeners();

        return this;
    }

    private void SetupListeners()
    {
        CreateTutorialListener(_habitatUI.ExpeditionButton.Button, TutorialStageType.ExpeditionSelection);
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