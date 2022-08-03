using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpeditionData", menuName = "ScriptableObjects/Expedition", order = 1)]
public class ExpeditionSO : ScriptableObject
{
    public string expeditionName = "";
    public List<ModifierBadgeType> badges = new List<ModifierBadgeType>();
    public int minimumLevel = 0;
    public ExpeditionRewardType expeditionRewardType = ExpeditionRewardType.None;
    public float duration = 0.0f;
}