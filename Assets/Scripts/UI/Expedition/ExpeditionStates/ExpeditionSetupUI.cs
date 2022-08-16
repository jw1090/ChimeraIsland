using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionSetupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _expeditionTitle = null;
    [SerializeField] private TextMeshProUGUI _duration = null;
    [SerializeField] private TextMeshProUGUI _rewardType = null;
    [SerializeField] private List<IconUI> _chimeraIcons = new List<IconUI>();
    [SerializeField] private TextMeshProUGUI _suggestedLevel = null;
    [SerializeField] private List<IconUI> _modifiers = new List<IconUI>();
    [SerializeField] private Slider _successSlider = null;
    [SerializeField] private TextMeshProUGUI _successText = null;
    [SerializeField] private Button _confirmButton = null;
    private UIManager _uiManager = null;
    private ExpeditionUI _expeditionUI = null;
    private ResourceManager _resourceManager = null;
    private ExpeditionManager _expeditionManager = null;

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
    }

    public void Initialize(UIManager uiManager, ExpeditionUI expeditionUI)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();

        _uiManager = uiManager;
        _expeditionUI = expeditionUI;
    }

    public void SetupListeners()
    {
        _uiManager.CreateButtonListener(_confirmButton, ConfirmClick);
    }

    private void ConfirmClick()
    {
        if (_expeditionManager.Chimeras.Count < 1)
        {
            Debug.Log($"<color=Red>Please add a Chimera to send it on an expedition.</color>");
        }

        _expeditionManager.ChimerasOnExpedition(true);

        _expeditionUI.InProgressUI.gameObject.SetActive(true);
        _expeditionManager.EnterInProgressState();
    }

    public void LoadExpeditionData()
    {
        foreach (var icon in _chimeraIcons)
        {
            icon.Icon.sprite = null;
        }

        

        ExpeditionData data = _expeditionManager.SelectedExpedition;

        _expeditionTitle.text = data.Title;
        _suggestedLevel.text = $"Suggested Level: {data.SuggestedLevel}";
        _rewardType.text = $"Rewards: {RewardTypeToString(data)}";

        LoadDuration(data.Duration);
        _expeditionUI.InProgressUI.UpdateSuccessText(data.Duration);
        LoadModifiers(data.Modifiers);
    }

    private void LoadDuration(float duration)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
        string durationString = $"Duration: {string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds)}";

        _duration.text = durationString;
    }

    private string RewardTypeToString(ExpeditionData data)
    {
        switch (data.Type)
        {
            case ExpeditionType.Essence:
                return $"{data.AmountGained} Essence";
            case ExpeditionType.Fossils:
                return $"{data.AmountGained} Fossils";
            case ExpeditionType.HabitatUpgrade:
                switch (data.UpgradeType)
                {
                    case HabitatRewardType.Waterfall:
                        return $"Waterfall";
                    case HabitatRewardType.CaveExploring:
                        return $"Explorable Cave";
                    case HabitatRewardType.RuneStone:
                        return $"Rune Stones";
                    case HabitatRewardType.Upgrade:
                        return $"Habiat Upgrade";
                    default:
                        Debug.LogError($"Upgrade Type [{data.UpgradeType}] was invalid, please change!");
                        return "";
                }
            default:
                Debug.LogError($"Reward Type [{data.Type}] was invalid, please change!");
                return "";
        }
    }

    private void LoadModifiers(List<ModifierType> modifierData)
    {
        int activeBadgeCount = 1;
        int i = 0;
        foreach (var modifierType in modifierData)
        {
            if (modifierType == ModifierType.None)
            {
                _modifiers[i].gameObject.SetActive(false);
            }
            else
            {
                _modifiers[i].Icon.sprite = _resourceManager.GetModifierSprite(modifierType);
                _modifiers[i].gameObject.SetActive(true);
                ++activeBadgeCount;
            }

            ++i;
        }
    }

    public void UpdateIcons() // Used to update UI during evolutions
    {
        foreach (var icon in _chimeraIcons)
        {
            icon.Icon.sprite = null;
        }

        for (int i = 0; i < _expeditionManager.Chimeras.Count; ++i)
        {
            _chimeraIcons[i].Icon.sprite = _expeditionManager.Chimeras[i].ChimeraIcon;
        }
    }

    public void UpdateDifficultyValue(float difficultyValue)
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
        _expeditionUI.InProgressUI.SetSuccesText(successChance);
    }
}