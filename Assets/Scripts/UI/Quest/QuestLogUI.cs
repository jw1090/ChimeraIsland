using System.Collections.Generic;
using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private List<QuestUI> _questUIList = new List<QuestUI>();

    public void DisplayActiveQuests(List<QuestData> quests)
    {
        foreach (QuestUI questUI in _questUIList)
        {
            questUI.gameObject.SetActive(false);
        }

        for (int i = 0; i < quests.Count; i++)
        {
            QuestData data = quests[i];
            QuestUI questUI = _questUIList[i];

            questUI.LoadText(data);
            questUI.gameObject.SetActive(true);
        }
    }
}
