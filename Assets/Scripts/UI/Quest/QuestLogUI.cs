using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI = null;
    [SerializeField] private List<QuestUI> _questUIList = new List<QuestUI>();

    public void Initialize()
    {
        HideQuestUI();
    }

    private void HideQuestUI()
    {
        foreach (QuestUI questUI in _questUIList)
        {
            questUI.gameObject.SetActive(false);
        }

        _textMeshProUGUI.gameObject.SetActive(false);
    }

    public void DisplayActiveQuests(List<QuestData> quests)
    {
        HideQuestUI();

        for (int i = 0; i < quests.Count; i++)
        {
            QuestData data = quests[i];
            QuestUI questUI = _questUIList[i];

            questUI.LoadText(data);
            questUI.gameObject.SetActive(true);
        }

        if (quests.Count > 0)
        {
            _textMeshProUGUI.gameObject.SetActive(true);
        }
    }
}
