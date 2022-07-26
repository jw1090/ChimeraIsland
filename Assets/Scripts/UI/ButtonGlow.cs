using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private Color _idle;
    private Color _hover;
    private Image _buttonImage = null;

    public void Awake()
    {
        _buttonImage = GetComponent<Button>().image;
        _idle = new Color(255, 255, 255, 255);
        _hover = new Color(222, 226, 178, 255);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonImage.color = _hover;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _buttonImage.color = _idle;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonImage.color = _idle;
    }
}