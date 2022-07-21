using UnityEngine;
using UnityEngine.EventSystems;

public class TabPress : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private bool _startSelected = false;
    private TabGroup _tabGroup = null;

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