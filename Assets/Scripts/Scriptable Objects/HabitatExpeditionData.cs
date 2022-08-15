using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HabitatExpeditionData", menuName = "ScriptableObjects/HabitatExpedition", order = 2)]
public class HabitatExpeditionData : ExpeditionBaseData
{
    public HabitatRewardType RewardType = HabitatRewardType.None;

    public HabitatExpeditionData(HabitatExpeditionData other)
    {
        Title = other.Title;
        Type = other.Type;
        SuggestedLevel = other.SuggestedLevel;
        EnergyCost = other.EnergyCost;
        Duration = other.Duration;
        RewardType = other.RewardType;

        Modifiers = new List<ModifierType>();

        foreach (var modifier in other.Modifiers)
        {
            Modifiers.Add(modifier);
        }
    }

    public HabitatExpeditionData DeepCopy()
    {
        HabitatExpeditionData deepCopy = new HabitatExpeditionData(this);

        return deepCopy;
    }
}