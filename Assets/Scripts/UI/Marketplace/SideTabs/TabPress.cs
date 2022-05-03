using UnityEngine;
using UnityEngine.EventSystems;

public class TabPress : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup _tabGroup = null;
    [SerializeField] private bool _startSelected = false;

    public void Initialize(TabGroup tabGroup)
    {
        _tabGroup = tabGroup;

        if (_startSelected)
        {
            _tabGroup.OnTabSelected(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tabGroup.OnTabEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroup.OnTabSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroup.OnTabExit();
    }
}