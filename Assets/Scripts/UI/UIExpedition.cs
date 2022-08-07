using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExpedition : MonoBehaviour
{
    [Header("Expedition Setup")]
    [SerializeField] private GameObject _setupPanel = null;
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

    [Header("Results")]
    [SerializeField] private GameObject _rewardPanel = null;
    [SerializeField] private TextMeshProUGUI _successResults = null;
    [SerializeField] private TextMeshProUGUI _resultsDescription = null;
    [SerializeField] private Button _closeButton = null;

    private ExpeditionManager _expeditionManager = null;
    private ResourceManager _resourceManager = null;
    private List<Chimera> _currentChimeras = new List<Chimera>();

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void SceneCleanup()
    {
        foreach(var icon in _chimeraIcons)
        {
            icon.sprite = null;
        }
    }

    public void SetupExpeditionUI()
    {
        if(_expeditionManager == null)
        {
            Debug.LogError($"Please set the Expedition Manager, it is currently null");
        }

        _currentChimeras = null;

        _expeditionManager.ExpeditionSetup();

        LoadData();
    }

    public void LoadData()
    {
        ExpeditionData data = _expeditionManager.CurrentExpeditionData;

        _expeditionName.text = data.expeditionName;
        LoadModifiers(data.modifiers);
        _minimumLevel.text = $"Minimum Level: {data.minimumLevel}";
        _rewardType.text = $"Rewards: {RewardTypeToString(data.rewardType)}";

        var ts = TimeSpan.FromSeconds(data.duration);
        _duration.text = $"Duration: {string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds)}";
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

    public void UpdateIcons(List<Chimera> chimeras) // Standard
    {
        _currentChimeras = chimeras;

        UpdateIcons();
    }

    public void UpdateIcons() // Used to update UI during evolutions
    {
        if(_currentChimeras == null)
        {
            return;
        }

        foreach (var icon in _chimeraIcons)
        {
            icon.sprite = null;
        }

        for (int i = 0; i < _currentChimeras.Count; ++i)
        {
            _chimeraIcons[i].sprite = _currentChimeras[i].ChimeraIcon;
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
        _successText.text = $"{_expeditionManager.CalculateSuccessChance().ToString("F2")}%";
    }

    public void OpenExpeditionUI()
    {
        _inProgressPanel.gameObject.SetActive(false);
        _rewardPanel.gameObject.SetActive(false);

        switch (_expeditionManager.State)
        {
            case ExpeditionState.Setup: // On by default
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

    private void ConfirmClick()
    {

    }
}