using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyExpeditionData", menuName = "ScriptableObjects/CurrencyExpedition", order = 1)]
public class CurrencyExpeditionData : ExpeditionBaseData
{
    public float AmountGained = 0.0f;

    public CurrencyExpeditionData DeepCopy()
    {
        CurrencyExpeditionData deepCopy = CreateInstance<CurrencyExpeditionData>();

        deepCopy.Title = Title;
        deepCopy.Type = Type;
        deepCopy.SuggestedLevel = SuggestedLevel;
        deepCopy.EnergyCost = EnergyCost;
        deepCopy.Duration = Duration;
        deepCopy.AmountGained = AmountGained;

        deepCopy.Modifiers = new List<ModifierType>();

        foreach (var modifier in Modifiers)
        {
            deepCopy.Modifiers.Add(modifier);
        }

        return deepCopy;
    }
}