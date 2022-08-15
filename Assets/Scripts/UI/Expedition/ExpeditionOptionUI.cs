using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExpeditionOptionUI : MonoBehaviour
{
    [SerializeField] private ExpeditionType _expeditionType = ExpeditionType.None;
    [SerializeField] private Image _icon = null;
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private TextMeshProUGUI _suggestLevel = null;
    [SerializeField] private TextMeshProUGUI _energyCost = null;
    [SerializeField] private TextMeshProUGUI _reward = null;
    [SerializeField] private List<IconUI> _modifiers = new List<IconUI>();
    private ResourceManager _resourceManager = null;
    private ExpeditionManager _expeditionManager = null;

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
    }

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
                break;
        }
    }

    private void LoadRewardInfo(CurrencyExpeditionData currencyExpeditionData)
    {
        _title.text = $"{currencyExpeditionData.Title}";
        _suggestLevel.text = $"{currencyExpeditionData.Title}";
        _energyCost.text = $"{currencyExpeditionData.Title}";

        switch (currencyExpeditionData.Type)
        {
            case ExpeditionType.Essence:
                _reward.text = $"Reward: {currencyExpeditionData.amountGained} Essence";
                break;
            case ExpeditionType.Fossils:
                _reward.text = $"Reward: {currencyExpeditionData.amountGained} Fossils";
                break;
            default:
                Debug.LogWarning($"Expedition Type is invalid [{currencyExpeditionData.Type}], please change!");
                break;
        }

        EvaluateModifiers(currencyExpeditionData.Modifiers);
    }

    private void LoadRewardInfo(HabitatExpeditionData habitatExpeditionData)
    {
        _title.text = $"{habitatExpeditionData.Title}";
        _suggestLevel.text = $"{habitatExpeditionData.Title}";
        _energyCost.text = $"{habitatExpeditionData.Title}";

        switch (habitatExpeditionData.RewardType)
        {
            case HabitatRewardType.Waterfall:
                _reward.text = $"Reward: Build {habitatExpeditionData.RewardType}";
                break;
            case HabitatRewardType.CaveExploring:
                _reward.text = $"Reward: Build {habitatExpeditionData.RewardType}";
                break;
            case HabitatRewardType.RuneStones:
                _reward.text = $"Reward: Build {habitatExpeditionData.RewardType}";
                break;
            case HabitatRewardType.Upgrade:
                _reward.text = $"Reward: Build {habitatExpeditionData.RewardType}";
                break;
            default:
                Debug.LogWarning($"Expedition Type is invalid [{habitatExpeditionData.Type}], please change!");
                break;
        }

        EvaluateModifiers(habitatExpeditionData.Modifiers);
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
                case ModifierType.Random:
                    // TODO: Random Logic
                    break;
                case ModifierType.Aqua:
                case ModifierType.Bio:
                case ModifierType.Fira:
                case ModifierType.Agility:
                case ModifierType.Intelligence:
                case ModifierType.Strength:
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
}