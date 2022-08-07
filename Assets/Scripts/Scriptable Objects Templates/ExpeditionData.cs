using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpeditionData", menuName = "ScriptableObjects/Expedition", order = 1)]
public class ExpeditionData : ScriptableObject
{
    public string expeditionName = "";
    public List<ModifierType> modifiers = new List<ModifierType>();
    public int minimumLevel = 0;
    public ExpeditionRewardType rewardType = ExpeditionRewardType.None;
    public double duration = 0.0;
}