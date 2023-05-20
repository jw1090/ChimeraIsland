using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TutorialData _tutorialData = null;
    private UIManager _UiManager = null;
    private QuestManager _QuestManager = null;
    private bool _tutorialsEnabled = false;
    private PersistentData _persistentData = null;
    private TutorialCompletionData _tutorialCompletion = null;

    public void SetHabitatUI(UIManager UIManager) { _UiManager = UIManager; }

    public TutorialManager Initialize()
    {
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;

        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();
        _QuestManager = ServiceLocator.Get<QuestManager>();
        LoadTutorialFromJson();

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

    public void SaveTutorialProgress()
    {
        _persistentData.SetTutorialCompletion(_tutorialCompletion);
    }

    public void ResetTutorialProgress()
    {
        if (_tutorialData != null)
        {
            _tutorialCompletion.Reset();
        }

        SaveTutorialProgress();

        Debug.Log($"<color=Red> Tutorial progress reset. </color>");
    }

    public void ShowTutorialStageDelay(TutorialStageType tutorialType, float delaySeconds)
    {
        StartCoroutine(DelayTutorial(tutorialType, delaySeconds));
    }

    private IEnumerator DelayTutorial(TutorialStageType tutorialType, float delaySeconds)
    {

        yield return new WaitForSeconds(delaySeconds);
        ShowTutorialStage(tutorialType);
    }

    public void ShowTutorialStage(TutorialStageType tutorialType)
    {
        if (_tutorialsEnabled == false)
        {
            return;
        }

        if (_tutorialCompletion.IsCompleted(tutorialType))
        {
            return;
        }

        TutorialStageData tutorialStage = _tutorialData.Tutorials[(int)tutorialType];
        _UiManager.StartTutorial(tutorialStage, tutorialType);
    }

    public void TutorialStageCheck()
    {
        if (_tutorialsEnabled == false)
        {
            return;
        }

        if (_tutorialCompletion.IsCompleted(TutorialStageType.Intro) == false)
        {
            ShowTutorialStage(TutorialStageType.Intro);
        }
    }

    public void TutorialComplete(TutorialStageType tutorialType, TutorialStageData tutorialData)
    {
        foreach(var quest in tutorialData.QuestList)
        {
            if (quest != null)
            {
                _QuestManager.ActivateQuest(quest.questType);
            }
        }
        _tutorialCompletion.Complete(tutorialType);
    }
}