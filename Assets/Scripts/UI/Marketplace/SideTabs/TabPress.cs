using UnityEngine;
using UnityEngine.EventSystems;

public class TabPress : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private bool _startSelected = false;
    [SerializeField] private AudioClip _clickSFX = null;
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
        _audioManager.PlaySFX(_clickSFX);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroup.OnTabExit();
    }
}