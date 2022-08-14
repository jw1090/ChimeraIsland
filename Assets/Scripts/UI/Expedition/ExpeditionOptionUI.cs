using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExpeditionOptionUI : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private TextMeshProUGUI _suggestLevel = null;
    [SerializeField] private TextMeshProUGUI _energyCost = null;
    [SerializeField] private TextMeshProUGUI _reward = null;
    [SerializeField] private Button _button = null;
    [SerializeField] private List<IconUI> _modifiers = new List<IconUI>();
    private ResourceManager _resourceManager = null;

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void LoadOptionUI(CurrencyExpeditionData currencyExpeditionData, List<ModifierType> modifiers)
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

        EvaluateModifiers(modifiers);
    }

    public void LoadOptionUI(HabitatExpeditionData habitatExpeditionData, List<ModifierType> modifiers)
    {
        _title.text = $"{habitatExpeditionData.Title}";
        _suggestLevel.text = $"{habitatExpeditionData.Title}";
        _energyCost.text = $"{habitatExpeditionData.Title}";

        switch (habitatExpeditionData.RewardType)
        {
            case HabitatRewardType.Waterfall:
                _reward.text = $"Reward: {habitatExpeditionData.RewardType} Unlocked";
                break;
            case HabitatRewardType.CaveExploring:
                _reward.text = $"Reward: {habitatExpeditionData.RewardType} Unlocked";
                break;
            case HabitatRewardType.RuneStones:
                _reward.text = $"Reward: {habitatExpeditionData.RewardType} Unlocked";
                break;
            case HabitatRewardType.Upgrade:
                _reward.text = $"Reward: {habitatExpeditionData.RewardType} Unlocked";
                break;
            default:
                Debug.LogWarning($"Expedition Type is invalid [{habitatExpeditionData.Type}], please change!");
                break;
        }

        EvaluateModifiers(modifiers);
    }

    private void EvaluateModifiers(List<ModifierType> modifiers)
    {
        int activeBadgeCount = 1;
        int i = 0;
        foreach (var modifierType in modifiers)
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
}