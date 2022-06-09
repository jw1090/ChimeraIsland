using UnityEngine;
using UnityEngine.EventSystems;

public class TransferButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private HabitatType _habitatType = HabitatType.None;
    private TransferMap _transferMap = null;
    private Habitat _habitat = null;
    private UIManager _uiManager = null;

    public void Initialize(TransferMap transferMap)
    {
        _transferMap = transferMap;
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _uiManager = ServiceLocator.Get<UIManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (_habitatType)
        {
            case HabitatType.StonePlains:
            case HabitatType.TreeOfLife:
            case HabitatType.Ashlands:
                TransferChimera();
                break;
            default:
                Debug.LogError($"Unhandled habitat type: {_habitatType}. Please change!");
                return;
        }
    }

    private void TransferChimera()
    {
        if(_habitatType == _habitat.Type)
        {
            Debug.Log("Transfer Failed - Cannot transfer to this current habitat!");
            return;
        }

        if(_habitat.TransferChimera(_transferMap.ChimeraToTransfer, _habitatType) == false)
        {
            Debug.Log("Transfer Failed - Transfer to another habitat or close map!");
            return;
        }

        _transferMap.ResetChimera();
        _uiManager.ResetUI();

        Debug.Log($"<color=Cyan> Transfer Success! {_transferMap.ChimeraToTransfer} is now in {_habitatType}!</color>");
    }
}