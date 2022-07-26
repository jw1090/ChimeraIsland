using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailsTransferButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Color _idle;
    [SerializeField] private Color _hover;
    private HabitatUI _uiManager = null;
    private ChimeraDetails _chimeraDetails = null;
    private Image _buttonImage = null;

    public void Initialize(ChimeraDetails chimeraDetails)
    {
        _uiManager = ServiceLocator.Get<UIManager>().HabitatUI;

        _buttonImage = GetComponent<Button>().image;
        _buttonImage.color = _idle;

        _chimeraDetails = chimeraDetails;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonImage.color = _hover;
    }   

    public void OnPointerClick(PointerEventData eventData)
    {
        _buttonImage.color = _idle;
        _uiManager.OpenTransferMap(_chimeraDetails.Chimera);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonImage.color = _idle;
    }
}