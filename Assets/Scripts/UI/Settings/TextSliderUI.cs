using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSliderUI : MonoBehaviour
{
    [SerializeField] Slider _slider = null;
    [SerializeField] TextMeshProUGUI _text = null;

    public Slider Slider { get => _slider; }
    public TextMeshProUGUI Text { get => _text; }
}