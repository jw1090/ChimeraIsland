using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TutorialData _tutorialData = null;
    private HabitatUI _habitatUI = null;
    private TutorialStageType _currentStage = TutorialStageType.None;

    public bool TutorialsEnabled { get => _tutorialsEnabled; }
    private bool _tutorialsEnabled = true;

    public void SetHabitatUI(HabitatUI habiatUI) { _habitatUI = habiatUI; }

    public TutorialManager Initialize()
    {
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;

        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        LoadTutorialFromJson();

        CurrentStageInitialize();

        return this;
    }

    private void OnDestroy()
    {
        DebugConfig.DebugConfigLoaded -= OnDebugConfigLoaded;
    }

    private void OnDebugConfigLoaded()
    {
        _tutorialsEnabled = ServiceLocator.Get<DebugConfig>().TutorialsEnabled;
        if (_tutorialsEnabled == false)
        {
            Debug.LogWarning("Tutorials are DISABLED");
        }
    }

    private void LoadTutorialFromJson()
    {
        _tutorialData = FileHandler.ReadFromJSON<TutorialData>(GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
    }

    private void CurrentStageInitialize()
    {
        if (_tutorialsEnabled == false) { return; }

        foreach (TutorialStageData tutorialStage in _tutorialData.Tutorials)
        {
            if (tutorialStage.finished)
            {
                ++_currentStage;
            }
            else
            {
                return;
            }
        }
    }

    public void SaveTutorialProgress()
    {
        if (_tutorialsEnabled == false) { return; }

        FileHandler.SaveToJSON(_tutorialData, GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);

        Debug.Log("Tutorial progress saved.");
    }

    public void ResetTutorialProgress()
    {
        if (_tutorialsEnabled == false) { return; }

        foreach (var tutorial in _tutorialData.Tutorials)
        {
            tutorial.finished = false;
        }

        _currentStage = 0;

        SaveTutorialProgress();

        Debug.Log($"<color=Red> Tutorial progress reset. </color>");
    }

    public void ShowTutorialStage(TutorialStageType tutorialType)
    {
        if (_tutorialsEnabled == false) { return; }

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        Debug.Log($"Showing tutorial Stage {(int)_currentStage}: {_currentStage}");
        _habitatUI.StartTutorial(tutorialStage);
    }

    public void TutorialStageCheck()
    {
        if (_tutorialsEnabled == false) { return; }

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        if (_currentStage == TutorialStageType.Intro && tutorialStage.finished == false)
        {
            _habitatUI.DisableUI();

            ShowTutorialStage(TutorialStageType.Intro);
        }
        else
        {
            Debug.Log($"Last tutorial was Stage {(int)_currentStage}: {_currentStage}");

            TutorialStageUILoad();
        }
    }

    private void TutorialStageUILoad()
    {

    }
}