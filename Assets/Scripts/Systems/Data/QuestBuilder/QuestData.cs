using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/Quest", order = 2)]
public class QuestData : ScriptableObject
{
    public QuestType QuestType = QuestType.None;
    public string Title = "";
    [TextArea(3, 10)] public string Description = "";
    public List<QuestType> QuestUnlocks = new List<QuestType>();
}
