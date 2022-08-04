using UnityEngine;
using UnityEngine.EventSystems;

public class TabPress : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private bool _startSelected = false;
    private TabGroup _tabGroup = null;
    private AudioManager _audioManager = null;

    public void Initialize(TabGroup tabGroup)
    {
        _tabGroup = tabGroup;
        _audioManager = ServiceLocator.Get<AudioManager>();

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
        _audioManager.PlayElementsSFX(ElementsSFX.StandardClick);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroup.OnTabExit();
    }
}