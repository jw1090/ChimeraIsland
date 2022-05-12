using UnityEngine;
using UnityEngine.EventSystems;

public class TransferButton : MonoBehaviour, IPointerClickHandler
{
    private HabitatManager _habitatManager = null;
    private HabitatType _currentHabitat = HabitatType.None;
    private HabitatType _habitatToTransfer = HabitatType.None;
    private Chimera _chimera = null;

    public void Initialize(Habitat habitat, HabitatType habitatType)
    {
        _currentHabitat = habitat.GetHabitatType();
        _habitatToTransfer = habitatType;
    }

    // Adds a facility based on the assigned facilityType.
    public void OnPointerClick(PointerEventData eventData)
    {
        //_habitatManager.TransferChimera(_currentHabitat, _habitatToTransfer, _chimera);
        Debug.Log("This Chimera: " + _chimera + " Has been Transfered from " + _currentHabitat + " To " + _habitatToTransfer);
    }
}
