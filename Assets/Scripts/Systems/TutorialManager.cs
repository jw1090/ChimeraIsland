using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TutorialData _tutorialData = null;
    private UIManager _uiManager = null;
    private TutorialStageType _currentStage = TutorialStageType.Intro;
    private bool _tutorialsEnabled = true;
    private HabitatManager _habitatManager = null;

    public TutorialStageType CurrentStage { get => _currentStage; }
    public bool TutorialsEnabled { get => _tutorialsEnabled; }

    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

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
        _tutorialData = FileHandler.ReadFromJSON<TutorialData>(GameConsts.JsonSaveKeys.TUTORIAL_DATA);
    }

    private void CurrentStageInitialize()
    {
        if (_tutorialsEnabled == false) { return; }

        if(_tutorialData == null)
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

        switch (tutorialType)
        {
            case TutorialStageType.TierTwoStonePlains:
                if (IsStageComplete(TutorialStageType.TierThreeStonePlains))
                {
                    tutorialType = TutorialStageType.WorldMapButton;
                }
                else if (IsStageComplete(TutorialStageType.Fossils))
                {
                    tutorialType = TutorialStageType.TierThreeStonePlains;
                }
                else if (IsStageComplete(TutorialStageType.TierTwoStonePlains))
                {
                    tutorialType = TutorialStageType.Fossils;
                }
                break;
            case TutorialStageType.ExpeditionRequirements:
                if (IsStageComplete(TutorialStageType.TierTwoStonePlains))
                {
                    tutorialType = TutorialStageType.UnlockExpeditionModifiers;
                }
                else if (IsStageComplete(TutorialStageType.Details))
                {
                    tutorialType = TutorialStageType.ExpeditionsInfo;
                }
                break;
            case TutorialStageType.Transfers:
                if(IsStageComplete(TutorialStageType.TreeOfLife) == false)
                {
                    return;
                }
                break;
        }

        if (IsStageComplete(tutorialType)) { return; }

        _currentStage = tutorialType;

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)_currentStage];

        Debug.Log($"Showing Tutorial Stage {(int)_currentStage}: {_currentStage}");
        _uiManager.StartTutorial(tutorialStage);
    }

    private bool IsStageComplete(TutorialStageType stage)
    {
        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)stage];
        return tutorialStage.finished;
    }

    public void TutorialStageCheck()
    {
        if (_tutorialsEnabled == false) { return; }

        _uiManager.TutorialDisableUI();

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

                    EnableUIByProgress();
                }
                break;
            case HabitatType.TreeOfLife:
                EnableUIByProgress();
                ShowTutorialStage(TutorialStageType.TreeOfLife);
                break;
            default:
                Debug.Log($"Habitat type \"{_habitatManager.CurrentHabitat.Type}\" shouldn't exist.");
                break;
        }
    }

    private void EnableUIByProgress()
    {
        switch (_currentStage)
        {
            case TutorialStageType.Intro:
                break;
            case TutorialStageType.ExpeditionRequirements:
            case TutorialStageType.FacilityShop:
                _uiManager.EnableUIByType(UIElementType.MarketplaceButton);
                break;
            case TutorialStageType.Training:
                _uiManager.EnableUIByType(UIElementType.MarketplaceButton);
                _uiManager.EnableUIByType(UIElementType.OtherFacilityButtons);
                break;
            case TutorialStageType.Details:
            case TutorialStageType.ExpeditionsInfo:
            case TutorialStageType.TierTwoStonePlains:
            case TutorialStageType.UnlockExpeditionModifiers:
            case TutorialStageType.Fossils:
                _uiManager.EnableUIByType(UIElementType.MarketplaceButton);
                _uiManager.EnableUIByType(UIElementType.OtherFacilityButtons);
                _uiManager.EnableUIByType(UIElementType.OpenDetailsButton);
                break;
            case TutorialStageType.ChimeraShop:
            case TutorialStageType.TierThreeStonePlains:
                _uiManager.EnableUIByType(UIElementType.MarketplaceButton);
                _uiManager.EnableUIByType(UIElementType.OtherFacilityButtons);
                _uiManager.EnableUIByType(UIElementType.OpenDetailsButton);
                _uiManager.EnableUIByType(UIElementType.MarketplaceChimeraTab);
                break;
            case TutorialStageType.WorldMapButton:
            case TutorialStageType.WorldMapAndTheTreeOfLife:
            case TutorialStageType.TreeOfLife:
            case TutorialStageType.Transfers:
                _uiManager.EnableUIByType(UIElementType.MarketplaceButton);
                _uiManager.EnableUIByType(UIElementType.OtherFacilityButtons);
                _uiManager.EnableUIByType(UIElementType.OpenDetailsButton);
                _uiManager.EnableUIByType(UIElementType.MarketplaceChimeraTab);
                _uiManager.EnableUIByType(UIElementType.WorldMapButton);
                break;
            default:
                Debug.LogError($"{_currentStage} is invalid. Please change!");
                break;
        }
    }
}