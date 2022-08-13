using System.Collections.Generic;
using UnityEngine;

public abstract class ExpeditionBaseData : ScriptableObject
{
    public string Title = "";
    public ExpeditionType Type = ExpeditionType.None;
    public List<ModifierType> Modifiers = new List<ModifierType>();
    public int SuggestedLevel = 0;
    public int EnergyCost = 0;
    public float Duration = 0.0f;
}