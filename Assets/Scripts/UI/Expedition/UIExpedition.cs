using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExpedition : MonoBehaviour
{
    [Header("Expedition Setup")]
    [SerializeField] private TextMeshProUGUI _expeditionName = null;
    [SerializeField] private TextMeshProUGUI _minimumLevel = null;
    [SerializeField] private TextMeshProUGUI _rewardType = null;
    [SerializeField] private GameObject _modifierFolder = null;
    [SerializeField] private List<Modifier> _modifiers = new List<Modifier>();
    [SerializeField] private List<Image> _chimeraIcons = new List<Image>();
    [SerializeField] private Slider _successSlider = null;
    [SerializeField] private TextMeshProUGUI _successText = null;
    [SerializeField] private TextMeshProUGUI _duration = null;
    [SerializeField] private Button _confirmButton = null;

    [Header("In Progress")]
    [SerializeField] private GameObject _inProgressPanel = null;
    [SerializeField] private TextMeshProUGUI _inProgressSuccessChance = null;
    [SerializeField] private Slider _durationSlider = null;
    [SerializeField] private TextMeshProUGUI _timeRemainingText = null;
    [SerializeField] private Button _resultsButton = null;
    [SerializeField] private StatefulObject _inProgressStatefulObject = null;

    [Header("Results")]
    [SerializeField] private GameObject _rewardPanel = null;
    [SerializeField] private TextMeshProUGUI _successResults = null;
    [SerializeField] private TextMeshProUGUI _resultsDescription = null;
    [SerializeField] private Button _rewardsCloseButton = null;

    private TutorialManager _tutorialManager = null;
    private ExpeditionManager _expeditionManager = null;
    private ResourceManager _resourceManager = null;
    private UIManager _uiManager = null;
    private ChimeraDetailsFolder _detailsFolder = null;
    private bool _expeditionSuccess = false;

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }
    public void SetInProgressTimeRemainingText(float timeRemaining)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        string newDurationText = $"Duration: {string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds)}";

        _timeRemainingText.text = newDurationText;
        _durationSlider.value = timeRemaining;
    }

    public void Initialize(UIManager uiManager)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _tutorialManager = ServiceLocator.Get<TutorialManager>();

        _uiManager = uiManager;
        _detailsFolder = _uiManager.HabitatUI.DetailsPanel;

        SetupListeners();
    }

    public void LoadExpeditionUI()
    {
        if(_expeditionManager == null)
        {
            Debug.LogError($"Please set the Expedition Manager, it is currently null");
        }

        foreach (var icon in _chimeraIcons)
        {
            icon.sprite = null;
        }

        if (_expeditionManager.State == ExpeditionState.Setup)
        {
            _expeditionManager.ExpeditionSetup();
            _expeditionSuccess = false;
            LoadData();
        }
    }

    public void SetupListeners()
    {
        _uiManager.CreateButtonListener(_confirmButton, ConfirmClick);
        _uiManager.CreateButtonListener(_resultsButton, InProgressClick);
        _uiManager.CreateButtonListener(_rewardsCloseButton, ResultsCloseClick);
    }

    public void LoadData()
    {
        ExpeditionData data = _expeditionManager.CurrentExpeditionData;

        _expeditionName.text = data.expeditionName;
        LoadModifiers(data.modifiers);
        _minimumLevel.text = $"Minimum Level: {data.minimumLevel}";
        _rewardType.text = $"Rewards: {RewardTypeToString(data.rewardType)}";

        LoadDuration(data.duration);
    }

    private void LoadModifiers(List<ModifierType> modifierData)
    {
        int activeBadgeCount = 1;
        int i = 0;
        foreach(var modifierType in modifierData)
        {
            if(modifierType == ModifierType.None)
            {
                _modifiers[i].gameObject.SetActive(false);
            }
            else
            {
                _modifiers[i].icon.sprite = _resourceManager.GetModifierSprite(modifierType);
                _modifiers[i].gameObject.SetActive(true);
                ++activeBadgeCount;
            }

            ++i;
        }

        if(activeBadgeCount > 1)
        {
            _modifierFolder.SetActive(true);
        }
        else
        {
            _modifierFolder.SetActive(false);
        }
    }

    private void LoadDuration(float duration)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
        string durationString = $"Duration: {string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds)}";

        _duration.text = durationString;
        _timeRemainingText.text = durationString;

        _durationSlider.maxValue = duration;
        _durationSlider.value = _durationSlider.maxValue;
    }

    private string RewardTypeToString(ExpeditionRewardType rewardType)
    {
        switch (rewardType)
        {
            case ExpeditionRewardType.HabitatUpgrade:
                return "Habitat Upgrade";
            case ExpeditionRewardType.Fossils:
                return "Fossils";
            default:
                Debug.LogWarning($"Reward Type [{rewardType}] was invalid, please change!");
                return "";
        }
    }

    public void UpdateIcons(List<Chimera> chimeras)
    {
        UpdateIcons();
    }

    public void UpdateIcons() // Used to update UI during evolutions
    {
        if(_expeditionManager == null)
        {
            return;
        }

        foreach (var icon in _chimeraIcons)
        {
            icon.sprite = null;
        }

        for (int i = 0; i < _expeditionManager.Chimeras.Count; ++i)
        {
            _chimeraIcons[i].sprite = _expeditionManager.Chimeras[i].ChimeraIcon;
        }
    }

    public void UpdateDifficultValue(float difficultyValue)
    {
        _successSlider.maxValue = difficultyValue;
    }

    public void UpdateChimeraPower(float power)
    {
        _successSlider.value = power;

        UpdateSuccessText();
    }

    private void UpdateSuccessText()
    {
        string successChance = _expeditionManager.CalculateSuccessChance().ToString("F2");

        _successText.text = $"{successChance}%";
        _inProgressSuccessChance.text = $"Success Chance: {successChance}%";
    }

    public void OpenExpeditionUI()
    {
        _inProgressPanel.gameObject.SetActive(false);
        _rewardPanel.gameObject.SetActive(false);

        switch (_expeditionManager.State)
        {
            case ExpeditionState.Setup:
                break;
            case ExpeditionState.InProgress:
                _inProgressPanel.gameObject.SetActive(true);
                break;
            case ExpeditionState.Result:
                _rewardPanel.gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning($"Expedition state is not valid [{_expeditionManager.State}]. Please change!");
                break;
        }

        this.gameObject.SetActive(true);
    }

    public void CloseExpeditionUI()
    {
        _inProgressPanel.gameObject.SetActive(false);
        _rewardPanel.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    public void TimerComplete()
    {
        _uiManager.HabitatUI.ExpeditionButton.ActivateNotification(true);

        _inProgressStatefulObject.SetState("Results Button");
        _timeRemainingText.text = "Complete!";
        _durationSlider.value = _durationSlider.maxValue;
    }

    private void ConfirmClick()
    {
        if(_expeditionManager == null)
        {
            return;
        }

        if (_expeditionManager.Chimeras.Count < 1)
        {
            Debug.Log($"<color=Red>Please add a Chimera to send it on an expedition.</color>");
        }

        PostExpeditionCleanup(true);

        _inProgressPanel.gameObject.SetActive(true);
        _expeditionManager.EnterInProgressState();

        _inProgressStatefulObject.SetState("In Progress");
    }

    private void InProgressClick()
    {
        _inProgressPanel.gameObject.SetActive(false);
        _rewardPanel.gameObject.SetActive(true);

        _expeditionManager.SetExpeditionState(ExpeditionState.Result);

        if(_expeditionManager.RandomSuccesRate())
        {
            _successResults.text = $"Success";

            if(_expeditionManager.CurrentExpeditionData.rewardType == ExpeditionRewardType.HabitatUpgrade)
            {
                _resultsDescription.text = $"Your Habitat has been upgraded!";
            }
            else
            {
                _resultsDescription.text = $"You've gained 1 Fossil!";
            }

            
            _expeditionSuccess = true;
        }
        else
        {
            _successResults.text = $"Failure";
            _resultsDescription.text = $"Train your Chimera and try again!";
            _expeditionSuccess = false;
        }
    }

    private void ResultsCloseClick()
    {
        _rewardPanel.gameObject.SetActive(false);

        PostExpeditionCleanup(false);

        _expeditionManager.SetExpeditionState(ExpeditionState.Setup);

        if(_expeditionSuccess == true) // Success
        {
            _expeditionManager.SuccessRewards();
            _expeditionSuccess = false;
            _tutorialManager.ShowTutorialStage(TutorialStageType.TierTwoStonePlains);
        }

        _uiManager.HabitatUI.ResetStandardUI();
    }

    private void PostExpeditionCleanup(bool onExpedition)
    {
        _expeditionManager.PostExpeditionCleanup(onExpedition);

        _detailsFolder.ToggleDetailsButtons(DetailsButtonType.Expedition);
    }
}