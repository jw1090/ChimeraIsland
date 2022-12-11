using TMPro;
using UnityEngine;

public class TooltipSections : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private TextMeshProUGUI _description = null;

    public TextMeshProUGUI Title { get => _title; }
    public TextMeshProUGUI Description { get => _description; }
}