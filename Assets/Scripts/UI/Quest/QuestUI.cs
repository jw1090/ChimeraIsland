using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private TextMeshProUGUI _description = null;

    public void LoadText(QuestData questData)
    {
        _title.text = questData.Title;
        _description.text = questData.Description;
    }
}
