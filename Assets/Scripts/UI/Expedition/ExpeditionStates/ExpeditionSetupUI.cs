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
    [SerializeField] private TextMeshProUGUI _energyDrain = null;
    [SerializeField] private List<IconUI> _modifiers = new List<IconUI>();
    [SerializeField] private Slider _successSlider = null;
    [SerializeField] private TextMeshProUGUI _successText = null;
    [SerializeField] private Button _confirmButton = null;
    [SerializeField] private Button _backButton = null;
    private TutorialManager _tutoiralManager = null;
    private UIManager _uiManager = null;
    private HabitatUI _habitatUI = null;
    private ExpeditionUI _expeditionUI = null;
    private ResourceManager _resourceManager = null;
    private ExpeditionManager _expeditionManager = null;
    private AudioManager _audioManager = null;

    public void ToggleConfirmButton(bool toggle) { _confirmButton.gameObject.SetActive(toggle); }
    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
    }

    public void Initialize(ExpeditionUI expeditionUI, UIManager uiManager)
    {
        _uiManager = uiManager;
        _expeditionUI = expeditionUI;

        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _tutoiralManager = ServiceLocator.Get<TutorialManager>();

        _habitatUI = _uiManager.HabitatUI;
    }

    public void SetupListeners()
    {
        _uiManager.CreateButtonListener(_confirmButton, ConfirmClick);
        _uiManager.CreateButtonListener(_backButton, BackClick);
    }

    private void ConfirmClick()
    {
        if (_expeditionManager.Chimeras.Count < 1)
        {
            Debug.Log($"<color=Red>Please add a Chimera to send it on an expedition.</color>");
        }

        _audioManager.PlayUISFX(SFXUIType.ConfirmClick);
        _expeditionManager.ChimerasOnExpedition(true);

        _expeditionUI.ForegroundUIStates.SetState("In Progress Panel");
        _habitatUI.UpdateHabitatUI();
        _expeditionManager.EnterInProgressState();

        _backButton.gameObject.SetActive(false);
    }

    private void BackClick()
    {
        _expeditionManager.RemoveAllChimeras();
        _expeditionUI.BackgroundStates.SetState("Selection Panel");
        _habitatUI.UpdateHabitatUI();
        _habitatUI.DetailsManager.CloseDetails();

        _audioManager.PlayUISFX(SFXUIType.StandardClick);
    }

    public void LoadExpeditionData()
    {
        ToggleConfirmButton(false);

        foreach (var icon in _chimeraIcons)
        {
            icon.Icon.sprite = null;
        }

        ExpeditionData data = _expeditionManager.SelectedExpedition;

        _expeditionTitle.text = data.Title;
        _energyDrain.text = $"Energy Drain: {data.EnergyDrain}";
        UpdateRewards(data);
        UpdateDuration(data);
        LoadModifiers(data.Modifiers);

        _backButton.gameObject.SetActive(true);

        if (_expeditionManager.CurrentHabitatProgress == 0)
        {
            _tutoiralManager.ShowTutorialStage(TutorialStageType.ExpeditionSetup);
        }
        else if (_expeditionManager.CurrentFossilProgress == 0)
        {
            _tutoiralManager.ShowTutorialStage(TutorialStageType.ReccomendedTraits);
        }
    }

    public void UpdateRewards(ExpeditionData data)
    {
        string reward = "";

        switch (data.Type)
        {
            case ExpeditionType.Essence:
                reward = $"{data.ActualAmountGained} (+{(int)(data.BaseAmountGained * data.RewardModifier)}) Essence";
                break;
            case ExpeditionType.Fossils:
                reward = $"{data.ActualAmountGained} (+{(int)(data.BaseAmountGained * data.RewardModifier)}) Fossils";
                break;
            case ExpeditionType.HabitatUpgrade:
                switch (data.UpgradeType)
                {
                    case HabitatRewardType.Waterfall:
                        reward = $"Waterfall";
                        break;
                    case HabitatRewardType.CaveExploring:
                        reward = $"Explorable Cave";
                        break;
                    case HabitatRewardType.RuneStone:
                        reward = $"Rune Stones";
                        break;
                    case HabitatRewardType.Habitat:
                        reward = $"Habitat Upgrade";
                        break;
                    default:
                        Debug.LogError($"Upgrade Type [{data.UpgradeType}] was invalid, please change!");
                        break;
                }
                break;
            default:
                Debug.LogError($"Reward Type [{data.Type}] was invalid, please change!");
                break;
        }

        _rewardType.text = $"Rewards: {reward}";
    }

    public void UpdateDuration(ExpeditionData data)
    {
        _duration.text = $"Duration: {data.ActualDuration.ToString("F1")} (-{(data.BaseDuration * data.DurationModifier).ToString("F1")}) Seconds";
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