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
    public int BaseAmountGained = 0;
    public int SuggestedTotalPower = 0;
    public int EnergyCost = 0;
    public float Duration = 0.0f;
    public List<ModifierType> Modifiers = new List<ModifierType>();

    public int ActualAmountGained { get => BaseAmountGained + (int)(BaseAmountGained * RewardModifier); }
    public bool ActiveInProgressTimer { get; set; }  = false;
    public float ExplorationModifier { get; set; } = 1.0f;
    public float StaminaModifer { get; set; } = 1.0f;
    public float WisdomModifier { get; set; } = 1.0f;
    public float AquaBonus { get; set; } = 0.0f;
    public float BioBonus { get; set; } = 0.0f;
    public float FiraBonus { get; set; } = 0.0f;
    public float DifficultyValue { get; set; } = 1.0f;
    public float ChimeraPower { get; set; } = 0.0f;
    public float CurrentDuration { get; set; } = 0.0f;
    public float RewardModifier { get; set; } = 0.0f;

    public ExpeditionData DeepCopy()
    {
        ExpeditionData deepCopy = CreateInstance<ExpeditionData>();

        deepCopy.Title = Title;
        deepCopy.AutoSucceed = AutoSucceed; 
        deepCopy.Type = Type;
        deepCopy.UpgradeType = UpgradeType;
        deepCopy.UnlocksNewChimera = UnlocksNewChimera;
        deepCopy.BaseAmountGained = BaseAmountGained;
        deepCopy.SuggestedTotalPower = SuggestedTotalPower;
        deepCopy.EnergyCost = EnergyCost;
        deepCopy.Duration = Duration;

        deepCopy.Modifiers = new List<ModifierType>();

        foreach (var modifier in Modifiers)
        {
            deepCopy.Modifiers.Add(modifier);
        }

        return deepCopy;
    }

    public void ResetMultipliersAndModifiers()
    {
        AquaBonus = 0.0f;
        BioBonus = 0.0f;
        FiraBonus = 0.0f;
        StaminaModifer = 1.0f;
        WisdomModifier = 1.0f;
        ExplorationModifier = 1.0f;
    }
}