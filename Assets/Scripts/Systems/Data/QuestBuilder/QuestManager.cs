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
        if(IsActiveQuest(questType) == true)
        {
            _activeQuest.Remove(questType);
        }
    }

    public void ActivateQuest(QuestType questType)
    {
        if (IsActiveQuest(questType) == false)
        {
            _activeQuest.Add(questType);
        }
        else
        {
            Debug.LogError("This Quest is Already Active!");
        }
    }

    public void DisplayActiveQuest()
    {

    }

    private bool IsActiveQuest(QuestType questType)
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
