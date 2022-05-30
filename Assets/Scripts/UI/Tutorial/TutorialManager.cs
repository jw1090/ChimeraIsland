using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private UIManager _uiManager = null;
    private UITutorialOverlay _tutorialOverlay = null;

    public TutorialManager Initialize()
    {
        _uiManager = ServiceLocator.Get<UIManager>();
        return this;
    }

    public void ShowTutorial()
    {
        _tutorialOverlay = _uiManager.GetTutorialOveraly();
        _tutorialOverlay.ShowOverlay();
    }
}