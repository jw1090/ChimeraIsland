using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIExpedition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _expeditionName = null;
    [SerializeField] private GameObject _modifierFolder = null;
    [SerializeField] private List<Modifier> _modifiers = new List<Modifier>();
    [SerializeField] private TextMeshProUGUI _minimumLevel = null;
    [SerializeField] private TextMeshProUGUI _rewardType = null;
    [SerializeField] private TextMeshProUGUI _duration = null;
    private ExpeditionManager _expeditionManager = null;
    private ResourceManager _resourceManager = null;
    private List<Chimera> _chimeras = new List<Chimera>();

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void SetupExpeditionUI()
    {
        if(_expeditionManager == null)
        {
            Debug.LogError($"Please set the Expedition Manager, it is currently null");
        }

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
}