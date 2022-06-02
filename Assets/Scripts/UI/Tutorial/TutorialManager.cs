using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private UIManager _uiManager = null;
    private UITutorialOverlay _tutorialOverlay = null;

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

    public TutorialManager Initialize()
    {
        return this;
    }

    public void ShowTutorial()
    {
        _tutorialOverlay = _uiManager.TutorialOverlay;
        _tutorialOverlay.ShowOverlay();
    }
}