using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{
    private UIManager _uiManager = null;
    private UITutorialOverlay _tutorialOverlay = null; // TODO: Kill me
    private Tutorial _tutorialData;

    public event Action OnTutorialComplete;

    public TutorialManager Initialize()
    {
        LoadTutorialFromJson();
        return this;
    }

    public void SetUIManager(UIManager uiManager) 
    {
        _uiManager = uiManager;
    }

    private void LoadTutorialFromJson()
    { 
        _tutorialData = FileHandler.ReadFromJSON<Tutorial>(GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
        Debug.Log("<color=yelow> Tutorial Data Loaded</color>");
    }

    public void ShowTutorial(int tutorialId)
    {
        TutorialSteps tutorialSteps = GetTutorialData(tutorialId);
        _uiManager.StartTutorial(tutorialSteps);

        // UI elements should be managed by the UIManager - this logic can be done by the UIManager.
        //_tutorialOverlay = _uiManager.TutorialOverlay;
        //_tutorialOverlay.ShowOverlay();
    }

    private TutorialSteps GetTutorialData(int id)
    {
        var result = _tutorialData.Tutorials[id];
        // error check that result is not null maybe
        return result;
    }
}