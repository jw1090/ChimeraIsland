using UnityEngine;

[CreateAssetMenu(fileName = "HabitatExpeditionData", menuName = "ScriptableObjects/HabitatExpedition", order = 2)]
public class HabitatExpeditionData : ExpeditionBaseData
{
    public HabitatRewardType RewardType = HabitatRewardType.None;
}