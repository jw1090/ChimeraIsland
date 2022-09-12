using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ExpeditionOptionUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ExpeditionType _expeditionType = ExpeditionType.None;
    [SerializeField] private Image _icon = null;
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private TextMeshProUGUI _difficulty = null;
    [SerializeField] private TextMeshProUGUI _energyCost = null;
    [SerializeField] private TextMeshProUGUI _reward = null;
    [SerializeField] private List<IconUI> _modifiers = new List<IconUI>();
    private ResourceManager _resourceManager = null;
    private ExpeditionManager _expeditionManager = null;
    private AudioManager _audioManager = null;

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
    }
    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }

    public ExpeditionType ExpeditionType { get => _expeditionType; }

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void LoadExpeditionData()
    {
        switch (_expeditionType)
        {
            case ExpeditionType.Essence:
                LoadRewardInfo(_expeditionManager.EssenceExpeditionOption);
                break;
            case ExpeditionType.Fossils:
                LoadRewardInfo(_expeditionManager.FossilExpeditionOption);
                break;
            case ExpeditionType.HabitatUpgrade:
                LoadRewardInfo(_expeditionManager.HabitatExpeditionOption);
                break;
            default:
                Debug.LogError($"Expedition Type is invalid [{_expeditionType}], please change!");
                break;
        }
    }

    private void LoadRewardInfo(ExpeditionData expeditionData)
    {
        _icon.sprite = _resourceManager.GetExpeditionTypeSprite(expeditionData.Type);
        _title.text = $"{expeditionData.Title}";
        _difficulty.text = $"Difficulty: {expeditionData.Difficulty}";
        _energyCost.text = $"Energy Drain: {expeditionData.EnergyDrain}";

        switch (expeditionData.Type)
        {
            case ExpeditionType.Essence:
                _reward.text = $"Reward: {expeditionData.BaseAmountGained} Essence";
                break;
            case ExpeditionType.Fossils:
                _reward.text = $"Reward: {expeditionData.BaseAmountGained} Fossils";
                break;
            case ExpeditionType.HabitatUpgrade:
                switch (expeditionData.UpgradeType)
                {
                    case HabitatRewardType.Waterfall:
                        _reward.text = $"Reward: Waterfall";
                        break;
                    case HabitatRewardType.CaveExploring:
                        _reward.text = $"Reward: Explorable Cave";
                        break;
                    case HabitatRewardType.RuneStone:
                        _reward.text = $"Reward: Rune Stones";
                        break;
                    case HabitatRewardType.Habitat:
                        _reward.text = $"Reward: Upgrade Habitat";
                        break;
                    default:
                        Debug.LogError($"Expedition Type is invalid [{expeditionData.UpgradeType}], please change!");
                        break;
                }
                break;
            default:
                Debug.LogError($"Expedition Type is invalid [{expeditionData.Type}], please change!");
                break;
        }

        EvaluateModifiers(expeditionData.Modifiers);
    }

    private void EvaluateModifiers(List<ModifierType> modifiers)
    {
        int activeBadgeCount = 1;
        int i = 0;

        foreach (var modifierType in modifiers)
        {
            switch (modifierType)
            {
                case ModifierType.None:
                    _modifiers[i].gameObject.SetActive(false);
                    break;
                case ModifierType.Aqua:
                case ModifierType.Bio:
                case ModifierType.Fira:
                case ModifierType.Stamina:
                case ModifierType.Wisdom:
                case ModifierType.Exploration:
                    _modifiers[i].Icon.sprite = _resourceManager.GetModifierSprite(modifierType);
                    _modifiers[i].gameObject.SetActive(true);
                    ++activeBadgeCount;
                    break;
                default:
                    break;
            }

            ++i;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _expeditionManager.SetSelectedExpedition(_expeditionType);
        _audioManager.PlayUISFX(SFXUIType.StandardClick);
    }
}