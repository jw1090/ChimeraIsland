using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestManifest _questManifest = null;
    private List<QuestType> _activeQuest = new List<QuestType>();
    private UIManager _uiManager = null;

    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public QuestManager Initialize()
    {
        return this;
    }

    public void CompleteQuest(QuestType questType)
    {
        if(IsQuestActive(questType) == true) 
        { 
            _activeQuest.Remove(questType);
        }
    }

    public void ActivateQuest(QuestType questType)
    {
        if(IsQuestActive(questType) == true)
        {
            Debug.LogError("The quest is already active!");
        }
        else
        {
            _activeQuest.Add(questType);
        }
    }

    public void DisplayActiveQuest()
    {

    }

    private bool IsQuestActive(QuestType questType)
    {
        foreach (QuestType quests in _activeQuest)
        {
            if (quests == questType)
            {
                return true;
            }
        }
        return false;
    }

}
