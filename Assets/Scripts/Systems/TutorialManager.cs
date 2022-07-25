using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TutorialData _tutorialData = null;
    private UIManager _uiManager = null;
    private TutorialStageType _currentStage = TutorialStageType.None;
    private bool _tutorialsEnabled = true;

    public TutorialStageType CurrentStage { get => _currentStage; }
    public bool TutorialsEnabled { get => _tutorialsEnabled; }

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

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
        if(tutorialStage.finished == true) { return; }
        Debug.Log($"Showing Tutorial Stage {(int)_currentStage}: {_currentStage}");
        _uiManager.StartTutorial(tutorialStage);
    }

    public void TutorialStageCheck()
    {
        if (_tutorialsEnabled == false) { return; }

        _uiManager.TutorialDisableUI();

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
            case TutorialStageType.ExpeditionRequirements:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                break;
            case TutorialStageType.FacilityShop:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                break;
            case TutorialStageType.Training:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                break;
            case TutorialStageType.Details:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                break;
            case TutorialStageType.ExpeditionsInfo:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                break;
            case TutorialStageType.TierTwoStonePlains:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                break;
            case TutorialStageType.UnlockExpeditionModifiers:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                break;
            case TutorialStageType.Fossils:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                break;
            case TutorialStageType.ChimeraShop:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceChimeraTab);
                break;
            case TutorialStageType.TierThreeStonePlains:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceChimeraTab);
                break;
            case TutorialStageType.WorldMapButton:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceChimeraTab);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.WorldMapButton);
                break;
            case TutorialStageType.WorldMapAndTheTreeOfLife:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceChimeraTab);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.WorldMapButton);
                break;
            case TutorialStageType.TreeOfLife:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceChimeraTab);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.WorldMapButton);
                break;
            case TutorialStageType.Transfers:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceChimeraTab);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.WorldMapButton);
                break;
            case TutorialStageType.Ashlands:
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OtherFacilityButtons);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.OpenDetailsButton);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.MarketplaceChimeraTab);
                _uiManager.EnableTutorialUIByType(TutorialUIElementType.WorldMapButton);
                break;
            default:
                Debug.LogError($"{_currentStage} is invalid. Please change!");
                break;
        }
    }
}