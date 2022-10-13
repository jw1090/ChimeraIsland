using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TutorialData _tutorialData = null;
    private HabitatUI _habitatUI = null;
    private TutorialStageType _currentStage = TutorialStageType.Intro;
    private bool _tutorialsEnabled = true;
    private HabitatManager _habitatManager = null;
    private PersistentData _persistentData = null;
    private TutorialCompletionData _tutorialCompletion = null;
    public TutorialStageType CurrentStage { get => _currentStage; }
    public bool TutorialsEnabled { get => _tutorialsEnabled; }

    public void SetHabitatUI(HabitatUI habitatUI) { _habitatUI = habitatUI; }

    public TutorialManager Initialize()
    {
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;

        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();

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
        _tutorialData = FileHandler.ReadFromJSON<TutorialData>(GameConsts.JsonSaveKeys.TUTORIAL_DATA, false);
        _tutorialCompletion = _persistentData.MyTutorialCompletion;
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

        for(int i = 0; i < _tutorialData.Tutorials.Length; i++)
        {
            if (_tutorialCompletion.IsCompleted((TutorialStageType) i))
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
        _persistentData.SetTutorialCompletion(_tutorialCompletion);
    }

    public void ResetTutorialProgress()
    {
        if (_tutorialsEnabled == false) { return; }

        if (_tutorialData != null)
        {
            _tutorialCompletion.Reset();
        }

        _currentStage = 0;

        SaveTutorialProgress();

        Debug.Log($"<color=Red> Tutorial progress reset. </color>");
    }

    public void ShowTutorialStage(TutorialStageType tutorialType)
    {
        if (_tutorialsEnabled == false) { return; }

        if (_tutorialCompletion.IsCompleted(tutorialType)) { return; }

        _currentStage = tutorialType;

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        Debug.Log($"Showing Tutorial Stage {(int)_currentStage}: {_currentStage}");
        _habitatUI.StartTutorial(tutorialStage, tutorialType);
    }

    public void TutorialStageCheck()
    {
        if (_tutorialsEnabled == false) { return; }

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        switch (_habitatManager.CurrentHabitat.Type)
        {
            case HabitatType.StonePlains:
                if (_currentStage == TutorialStageType.Intro && _tutorialCompletion.IsCompleted(TutorialStageType.Intro) == false)
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

    public void TutorialComplete(TutorialStageType type)
    {
        _tutorialCompletion.Complete(type);
    }
}