using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionInProgressUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _inProgressSuccessChance = null;
    [SerializeField] private Slider _durationSlider = null;
    [SerializeField] private TextMeshProUGUI _timeRemainingText = null;

    public void SetSuccesText(string successChance) { _inProgressSuccessChance.text = $"Success Chance: {successChance}%"; }

    public void UpdateInProgressTimeRemainingText(float timeRemaining)
    {
        _timeRemainingText.text = $"Duration: {timeRemaining.ToString("F1")} Seconds";
        _durationSlider.value = timeRemaining;
    }

    public void UpdateSuccessText(float duration)
    {
        _timeRemainingText.text = $"Duration: {duration.ToString("F1")} Seconds";
        _durationSlider.maxValue = duration;
        _durationSlider.value = _durationSlider.maxValue;
    }
}