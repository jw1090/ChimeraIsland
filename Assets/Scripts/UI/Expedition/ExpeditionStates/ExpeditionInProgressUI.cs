using System;
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
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        string newDurationText = $"Duration: {string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds)}";

        _timeRemainingText.text = newDurationText;
        _durationSlider.value = timeRemaining;
    }

    public void UpdateSuccessText(float duration)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
        string durationString = $"Duration: {string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds)}";

        _timeRemainingText.text = durationString;

        _durationSlider.maxValue = duration;
        _durationSlider.value = _durationSlider.maxValue;
    }
}