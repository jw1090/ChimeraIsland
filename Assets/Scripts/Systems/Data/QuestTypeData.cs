using System;

[Serializable]
public class QuestTypeData
{
    public QuestType questType = QuestType.None;

    public void SetVolume(QuestType _questType)
    {
        questType = _questType;
    }

    public QuestTypeData() { }
}