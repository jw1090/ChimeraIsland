using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExpedition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _expeditionName = null;
    [SerializeField] private List<Image> _badges = new List<Image>();
    [SerializeField] private TextMeshProUGUI _minimumLevel = null;
    [SerializeField] private TextMeshProUGUI _rewardType = null;
    [SerializeField] private TextMeshProUGUI _duration = null;
    private ExpeditionManager _expeditionManager = null;
    private ResourceManager _resourceManager = null;

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
        LoadBadges(data.badges);
        _minimumLevel.text = $"Minimum Level: {data.minimumLevel}";
        _rewardType.text = $"Rewards: {RewardTypeToString(data.rewardType)}";
        //_duration.text = TimeSpan.FromSeconds(data.duration).ToString("mm:ss");;
    }

    private void LoadBadges(List<ModifierBadgeType> badgeData)
    {
        int i = 0;
        foreach(var badgeType in badgeData)
        {
            if(badgeType == ModifierBadgeType.None)
            {
                _badges[i].gameObject.SetActive(false);
            }
            else
            {
                _badges[i].sprite = _resourceManager.GetBadgeSprite(badgeType);
                _badges[i].gameObject.SetActive(true);
            }

            ++i;
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