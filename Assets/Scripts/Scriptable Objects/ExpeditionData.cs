using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpeditionData", menuName = "ScriptableObjects/Expedition", order = 1)]
public class ExpeditionData : ScriptableObject
{
    public string Title = "";
    public ExpeditionType Type = ExpeditionType.None;
    public HabitatRewardType UpgradeType = HabitatRewardType.None;
    public float AmountGained = 0.0f;
    public int SuggestedLevel = 0;
    public int EnergyCost = 0;
    public float Duration = 0.0f;
    public List<ModifierType> Modifiers = new List<ModifierType>();

    public ExpeditionData DeepCopy()
    {
        ExpeditionData deepCopy = CreateInstance<ExpeditionData>();

        deepCopy.Title = Title;
        deepCopy.Type = Type;
        deepCopy.SuggestedLevel = SuggestedLevel;
        deepCopy.EnergyCost = EnergyCost;
        deepCopy.Duration = Duration;
        deepCopy.AmountGained = AmountGained;
        deepCopy.UpgradeType = UpgradeType;

        deepCopy.Modifiers = new List<ModifierType>();

        foreach (var modifier in Modifiers)
        {
            deepCopy.Modifiers.Add(modifier);
        }

        return deepCopy;
    }
}