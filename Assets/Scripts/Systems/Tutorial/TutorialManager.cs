using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{
    private Tutorial _tutorialData = null;
    private UIManager _uiManager = null;

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

    public void SetupTutorial()
    {
        int tutorialId = 0;
        ShowTutorial(tutorialId);
    }

    public void SaveTutorialProgress()
    {
        Debug.Log("Tutorial progress saved.");
        FileHandler.SaveToJSON(_tutorialData, GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
    }

    public void ResetTutorialProgress()
    {
        foreach(var tutorial in _tutorialData.Tutorials)
        {
            tutorial.finished = false;
        }
        SaveTutorialProgress();
    }

    private void ShowTutorial(int tutorialId)
    {
        TutorialSteps tutorialStep = _tutorialData.Tutorials[tutorialId];
        if(tutorialStep.finished == true)
        {
            Debug.Log("Finished Tutorial");
            return;
        }
        if (tutorialStep == null)
        {
            Debug.LogError($"Tutorial result is null!");
        }

        _uiManager.StartTutorial(tutorialStep);
    }
}