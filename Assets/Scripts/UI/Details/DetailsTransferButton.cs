using UnityEngine;
using UnityEngine.EventSystems;

public class DetailsTransferButton : MonoBehaviour, IPointerClickHandler
{
    private UIManager _uiManager = null;
    private ChimeraDetails _chimeraDetails = null;

    public void Initialize(ChimeraDetails chimeraDetails)
    {
        _uiManager = ServiceLocator.Get<UIManager>();
        _chimeraDetails = chimeraDetails;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _uiManager.OpenTransferMap(_chimeraDetails.Chimera);
    }
}