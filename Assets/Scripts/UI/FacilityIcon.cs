using UnityEngine;
using UnityEngine.UI;

public class FacilityIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _icon = null;
    [SerializeField] private Slider _slider = null;
    private Facility _facility = null;
    public void Initialize(Facility facility)
    {
        _facility = facility;
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void setSliderAttributes(int starting, int ending)
    {
        _slider.minValue = starting;
        _slider.value = starting;
        _slider.maxValue = ending;
    }

    public void updateSlider(int currentXP)
    {
        _slider.value = currentXP;
    }

    public void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }

    public void RemoveIcon()
    {
        _icon.sprite = null;
    }
}