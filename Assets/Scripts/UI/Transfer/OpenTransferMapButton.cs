using UnityEngine;
using UnityEngine.EventSystems;

public class OpenTransferMapButton : MonoBehaviour, IPointerClickHandler
{
    private ChimeraDetails _chimeraDetails = null;
    private UIManager _uIManager = null;
    public void Initalize(ChimeraDetails chimeraDetails)
    {
        _chimeraDetails = chimeraDetails;
        _uIManager = ServiceLocator.Get<UIManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _uIManager.OpenTransferMap(_chimeraDetails.ChimeraInfo);
    }
}
