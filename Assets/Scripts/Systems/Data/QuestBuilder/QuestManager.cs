using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestManifest _questManifest = null;
    [SerializeField] private List<QuestType> _activeQuest = new List<QuestType>();
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

    }

    public void ActivateQuest()
    {

    }

    public void DisplayActiveQuest()
    {

    }

    private bool ActiveQuestSearch(QuestType questType)
    {
        foreach(QuestType quests in _activeQuest)
        {
            if(quests == questType)
            {
                return true;
            }
        }
        return false;
    }

}
