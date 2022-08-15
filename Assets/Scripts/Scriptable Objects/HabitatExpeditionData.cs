using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HabitatExpeditionData", menuName = "ScriptableObjects/HabitatExpedition", order = 2)]
public class HabitatExpeditionData : ExpeditionBaseData
{
    public HabitatRewardType RewardType = HabitatRewardType.None;

    public HabitatExpeditionData DeepCopy()
    {
        HabitatExpeditionData deepCopy = CreateInstance<HabitatExpeditionData>();

        deepCopy.Title = Title;
        deepCopy.Type = Type;
        deepCopy.SuggestedLevel = SuggestedLevel;
        deepCopy.EnergyCost = EnergyCost;
        deepCopy.Duration = Duration;
        deepCopy.RewardType = RewardType;

        deepCopy.Modifiers = new List<ModifierType>();

        foreach (var modifier in Modifiers)
        {
            deepCopy.Modifiers.Add(modifier);
        }

        return deepCopy;
    }
}