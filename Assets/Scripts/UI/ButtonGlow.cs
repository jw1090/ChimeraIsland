using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Color _idle;
    [SerializeField] private Color _hover;
    private Image _buttonImage = null;

    public void Awake()
    {
        _buttonImage = GetComponent<Button>().image;
        _buttonImage.color = _idle;
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