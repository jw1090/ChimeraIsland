using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private Tutorial _tutorialData = null;
    private HabitatUI _habitatUI = null;
    private UIManager _uiManager = null;

    public bool TutorialsEnabled { get => _tutorialsEnabled; }
    private bool _tutorialsEnabled = true;

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }
    public void SetHabitatUI(HabitatUI habiatUI) { _habitatUI = habiatUI; }

    public TutorialManager Initialize()
    {
#if CHIMERA_DEBUG
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;
#endif
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        LoadTutorialFromJson();

        return this;
    }

    private void OnDestroy()
    {
#if CHIMERA_DEBUG
        DebugConfig.DebugConfigLoaded -= OnDebugConfigLoaded;
#endif        
    }

    private void LoadTutorialFromJson()
    {
        _tutorialData = FileHandler.ReadFromJSON<Tutorial>(GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
    }

    private void OnDebugConfigLoaded()
    {
        _tutorialsEnabled = ServiceLocator.Get<DebugConfig>().TutorialsEnabled;
        if (!_tutorialsEnabled)
        {
            Debug.LogWarning("Tutorials are DISABLED");
        }
    }

    public void SetupTutorial()
    {
        int tutorialId = 0;
        ShowTutorial(tutorialId);
        if (!_tutorialsEnabled) { return; }

        ShowTutorial(tutorialId);
        ShowTutorial((int)TutorialIds.StarterTutorial);
    }

    public void SaveTutorialProgress()
    {
        if (!_tutorialsEnabled) { return; }

        Debug.Log("Tutorial progress saved.");
        FileHandler.SaveToJSON(_tutorialData, GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
    }

    public void ResetTutorialProgress()
    {
        if (!_tutorialsEnabled) { return; }

        foreach (var tutorial in _tutorialData.Tutorials)
        {
            tutorial.finished = false;
        }
        SaveTutorialProgress();
    }

    private void ShowTutorial(int tutorialId)
    {
        if (!_tutorialsEnabled) { return; }

        TutorialSteps tutorialStep = _tutorialData.Tutorials[tutorialId];
        if (tutorialStep.finished == true)
        {
            Debug.Log("Finished Tutorial");
            return;
        }
        if (tutorialStep == null)
        {
            Debug.LogError($"Tutorial result is null!");
        }

        _habitatUI.StartTutorial(tutorialStep);
    }

    public bool FirstStepCheck()
    {
        if(_tutorialData.Tutorials[(int)TutorialIds.StarterTutorial].finished == false)
        {
            return true;
        }

        return false;
    }
}