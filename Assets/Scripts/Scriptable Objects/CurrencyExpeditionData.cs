using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyExpeditionData", menuName = "ScriptableObjects/CurrencyExpedition", order = 1)]
public class CurrencyExpeditionData : ExpeditionBaseData
{
    public float AmountGained = 0.0f;

    public CurrencyExpeditionData(CurrencyExpeditionData other)
    {
       Title = other.Title;
       Type = other.Type;
       SuggestedLevel = other.SuggestedLevel;
       EnergyCost = other.EnergyCost;
       Duration = other.Duration;
       AmountGained = other.AmountGained;

        Modifiers = new List<ModifierType>();

        foreach (var modifier in other.Modifiers)
        {
            Modifiers.Add(modifier);
        }
    }

    public CurrencyExpeditionData DeepCopy()
    {
        CurrencyExpeditionData deepCopy = new CurrencyExpeditionData(this);

        return deepCopy;
    }
}