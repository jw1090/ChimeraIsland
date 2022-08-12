using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpeditionData", menuName = "ScriptableObjects/Expedition", order = 1)]
public class ExpeditionData : ScriptableObject
{
    public string expeditionName = "";
    public ExpeditionType type = ExpeditionType.None;
    public ModifierAmount modifierAmount = ModifierAmount.None;
    public List<ModifierType> modifiers = new List<ModifierType>();
    public int suggestedLevel = 0;
    public int energyCost = 0;
    public float duration = 0.0f;
}