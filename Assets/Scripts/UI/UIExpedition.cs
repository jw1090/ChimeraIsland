using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExpedition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _expeditionName = null;
    [SerializeField] private GameObject _badgeFolder = null;
    [SerializeField] private List<GameObject> _badgeContainers = new List<GameObject>();
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
        int activeBadgeCount = 1;
        int i = 0;
        foreach(var badgeType in badgeData)
        {
            if(badgeType == ModifierBadgeType.None)
            {
                _badgeContainers[i].gameObject.SetActive(false);
            }
            else
            {
                _badges[i].sprite = _resourceManager.GetBadgeSprite(badgeType);
                _badgeContainers[i].gameObject.SetActive(true);
                ++activeBadgeCount;
            }

            ++i;
        }

        if(activeBadgeCount > 1)
        {
            _badgeFolder.SetActive(true);

            RectTransform rectTransform = _badgeFolder.GetComponent<RectTransform>();

            switch (activeBadgeCount)
            {
                case 1:
                    rectTransform.sizeDelta = new Vector2(150.0f, 80.0f);
                    break;
                case 2:
                    rectTransform.sizeDelta = new Vector2(250.0f, 80.0f);
                    break;
                case 3:
                    rectTransform.sizeDelta = new Vector2(350.0f, 80.0f);
                    break;
                default:
                    Debug.LogWarning($"Active Badge Count is out of range. Badge Count [{activeBadgeCount}]");
                    break;
            }
        }
        else
        {
            _badgeFolder.SetActive(false);
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