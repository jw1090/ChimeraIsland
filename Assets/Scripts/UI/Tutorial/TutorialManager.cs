using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{
    private Tutorial _tutorialData = null;
    private UIManager _uiManager = null;

    public event Action OnTutorialComplete;

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

    public TutorialManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        LoadTutorialFromJson();

        return this;
    }

    private void LoadTutorialFromJson()
    { 
        _tutorialData = FileHandler.ReadFromJSON<Tutorial>(GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
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