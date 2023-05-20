using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestManifest _questManifest = null;
    private Dictionary<QuestType, QuestData> _activeQuests = new Dictionary<QuestType, QuestData>();
    private Dictionary<QuestType, QuestData> _questLibrary = new Dictionary<QuestType, QuestData>();
    private UIManager _uiManager = null;
    private HabitatManager _habitatManager = null;

    private void SaveActiveQuests()
    {
        List<QuestData> questDataList = new List<QuestData>();
        foreach (QuestData questData in _activeQuests.Values)
        {
            questDataList.Add(questData);
        }
        _habitatManager.HabitatData.questDataList = questDataList;
    }

    public void SetHabitatManager(HabitatManager habitatManager)
    {
        _habitatManager = habitatManager;
        if(_habitatManager.HabitatData.questDataList.Count > 0) 
        {
            foreach (QuestData questData in _habitatManager.HabitatData.questDataList)
            {
                _activeQuests.Add(questData.QuestType, questData);
            }
        }
    }

    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public QuestManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        SetupQuestLibrary();

        return this;
    }

    private void SetupQuestLibrary()
    {
        foreach (QuestData questData in _questManifest.QuestData)
        {
            _questLibrary.Add(questData.QuestType, questData);
        }
    }

    public void CompleteQuest(QuestType questType)
    {
        if (IsActiveQuest(questType) == true)
        {
            _activeQuests.Remove(questType);
        }
        SaveActiveQuests();
    }

    public void ActivateQuest(QuestType questType)
    {
        if (IsActiveQuest(questType) == false)
        {
            _activeQuests.Add(questType, _questLibrary[questType]); 
            SaveActiveQuests();
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
        if (_activeQuests.ContainsKey(questType))
        {
            return true;
        }
        return false;
    }

}
