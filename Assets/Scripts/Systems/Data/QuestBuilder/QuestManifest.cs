using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestManifest")]
public class QuestManifest : ScriptableObject
{
    public List<QuestData> QuestData = new List<QuestData>();
}
