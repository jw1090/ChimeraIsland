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
        _tutorialData = FileHandler.ReadFromJSON<TutorialData>(GameConsts.JsonSaveKeys.TUTORIAL_DATA);
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

        FileHandler.SaveToJSON(_tutorialData, GameConsts.JsonSaveKeys.TUTORIAL_DATA);

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

        _currentStage = tutorialType;

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        Debug.Log($"Showing Tutorial Stage {(int)_currentStage}: {_currentStage}");
        _habitatUI.StartTutorial(tutorialStage);
    }

    public void TutorialStageCheck()
    {
        if (_tutorialsEnabled == false) { return; }

        _habitatUI.TutorialDisableUI();

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        if (_currentStage == TutorialStageType.Intro && tutorialStage.finished == false)
        {
            ShowTutorialStage(TutorialStageType.Intro);
        }
        else
        {
            Debug.Log($"Last Tutorial was Stage {(int)_currentStage}: {_currentStage}");

            EnableUIByProgress();
        }
    }

    private void EnableUIByProgress()
    {
        switch (_currentStage)
        {
            case TutorialStageType.Intro:
                break;
            case TutorialStageType.MarketplaceButton:
            case TutorialStageType.PurchasingFacilities:
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                break;
            case TutorialStageType.DetailsButton:
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                break;
            case TutorialStageType.ExpeditionsButton:
            case TutorialStageType.NewFacilities:
            case TutorialStageType.Evolution:
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.ExpeditionButton);
                break;
            case TutorialStageType.Fossils:
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.ExpeditionButton);
                // TODO: Fossil Currency UI Element
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.MarketplaceChimeraTab);
                break;
            case TutorialStageType.WorldMap:
            case TutorialStageType.NewHabitats:
            case TutorialStageType.TreeOfLife:
            case TutorialStageType.AshLands:
                _habitatUI.EnableTutorialUIByType(TutorialUIElementType.All);
                break;
            default:
                Debug.LogError($"{_currentStage} is invalid. Please change!");
                break;
        }
    }
}