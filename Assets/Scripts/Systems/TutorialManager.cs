using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TutorialData _tutorialData = null;
    private HabitatUI _habitatUI = null;
    private TutorialStageType _currentStage = TutorialStageType.Intro;
    private bool _tutorialsEnabled = true;
    private HabitatManager _habitatManager = null;

    public TutorialStageType CurrentStage { get => _currentStage; }
    public bool TutorialsEnabled { get => _tutorialsEnabled; }

    public void SetHabitatUI(HabitatUI habitatUI) { _habitatUI = habitatUI; }

    public TutorialManager Initialize()
    {
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;

        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        LoadTutorialFromJson();

        CurrentStageInitialize();

        _habitatManager = ServiceLocator.Get<HabitatManager>();
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
        Debug.Log($"Loading Tutorial Data From: {GameConsts.JsonSaveKeys.TUTORIAL_DATA}");

        _tutorialData = FileHandler.ReadFromJSON<TutorialData>(GameConsts.JsonSaveKeys.TUTORIAL_DATA);
    }

    private void CurrentStageInitialize()
    {
        if (_tutorialsEnabled == false) { return; }

        if (_tutorialData == null)
        {
            Debug.LogWarning("No Tutorial Data Loaded! Disabling Tutorials!");
            _tutorialsEnabled = false;
            return;
        }

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

        FileHandler.SaveToJSON(_tutorialData, GameConsts.JsonSaveKeys.TUTORIAL_DATA);
    }

    public void ResetTutorialProgress()
    {
        if (_tutorialsEnabled == false) { return; }

        if (_tutorialData != null)
        {
            foreach (var tutorial in _tutorialData.Tutorials)
            {
                tutorial.finished = false;
            }
        }

        _currentStage = 0;

        SaveTutorialProgress();

        Debug.Log($"<color=Red> Tutorial progress reset. </color>");
    }

    public void ShowTutorialStage(TutorialStageType tutorialType)
    {
        if (_tutorialsEnabled == false) { return; }

        if (IsStageComplete(tutorialType)) { return; }

        _currentStage = tutorialType;

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        Debug.Log($"Showing Tutorial Stage {(int)_currentStage}: {_currentStage}");
        _habitatUI.StartTutorial(tutorialStage);
    }

    private bool IsStageComplete(TutorialStageType stage)
    {
        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)stage];
        return tutorialStage.finished;
    }

    public void TutorialStageCheck()
    {
        if (_tutorialsEnabled == false) { return; }

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        switch (_habitatManager.CurrentHabitat.Type)
        {
            case HabitatType.StonePlains:
                if (_currentStage == TutorialStageType.Intro && tutorialStage.finished == false)
                {
                    ShowTutorialStage(TutorialStageType.Intro);
                }
                else
                {
                    Debug.Log($"Last Tutorial was Stage {(int)_currentStage}: {_currentStage}");
                }
                break;
            case HabitatType.TreeOfLife:
                break;
            default:
                Debug.Log($"Habitat type \"{_habitatManager.CurrentHabitat.Type}\" shouldn't exist.");
                break;
        }
    }
}