using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{
    private UIManager _uiManager = null;
    private Tutorial _tutorialData = null;

    public event Action OnTutorialComplete;

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

    public TutorialManager Initialize()
    {
        LoadTutorialFromJson();
        return this;
    }

    private void LoadTutorialFromJson()
    { 
        _tutorialData = FileHandler.ReadFromJSON<Tutorial>(GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
        Debug.Log($"<color=yelow> Tutorial Data Loaded</color>");
    }

    public void ShowTutorial(int tutorialId)
    {
        TutorialSteps tutorialStep = _tutorialData.Tutorials[tutorialId];

        if (tutorialStep == null)
        {
            Debug.LogError($"Tutorial result is null!");
        }

        _uiManager.StartTutorial(tutorialStep);
    }
}