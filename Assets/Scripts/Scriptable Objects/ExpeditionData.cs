using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpeditionData", menuName = "ScriptableObjects/Expedition", order = 1)]
public class ExpeditionData : ScriptableObject
{
    public string Title = "";
    public bool AutoSucceed = false;
    public ExpeditionType Type = ExpeditionType.None;
    public HabitatRewardType UpgradeType = HabitatRewardType.None;
    public bool UnlocksNewChimera = false;
    public int AmountGained = 0;
    public int SuggestedLevel = 0;
    public int EnergyCost = 0;
    public float Duration = 0.0f;
    public List<ModifierType> Modifiers = new List<ModifierType>();

    public ExpeditionData DeepCopy()
    {
        ExpeditionData deepCopy = CreateInstance<ExpeditionData>();

        deepCopy.Title = Title;
        deepCopy.AutoSucceed = AutoSucceed; 
        deepCopy.Type = Type;
        deepCopy.UpgradeType = UpgradeType;
        deepCopy.UnlocksNewChimera = UnlocksNewChimera;
        deepCopy.AmountGained = AmountGained;
        deepCopy.SuggestedLevel = SuggestedLevel;
        deepCopy.EnergyCost = EnergyCost;
        deepCopy.Duration = Duration;

        deepCopy.Modifiers = new List<ModifierType>();

        foreach (var modifier in Modifiers)
        {
            deepCopy.Modifiers.Add(modifier);
        }

        return deepCopy;
    }
}