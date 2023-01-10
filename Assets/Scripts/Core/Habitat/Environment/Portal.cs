using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Material _portalShader = null;
    [ColorUsage(true, true)] [SerializeField] private Color _standardPortal = new Color();
    [ColorUsage(true, true)] [SerializeField] private Color _inProgressPortal = new Color();
    [ColorUsage(true, true)] [SerializeField] private Color _successPortal = new Color();
    [ColorUsage(true, true)] [SerializeField] private Color _failurePortal = new Color();

    public void ChangePortal(ExpeditionState type, bool expeditionSuccess)
    {
        switch (type)
        {
            case ExpeditionState.Selection:
            case ExpeditionState.Setup:
                _portalShader.SetColor("_PortalColor", _standardPortal);
                break;
            case ExpeditionState.InProgress:
                _portalShader.SetColor("_PortalColor", _inProgressPortal);
                break;
            case ExpeditionState.Result:
                if(expeditionSuccess == true)
                {
                    _portalShader.SetColor("_PortalColor", _successPortal);
                }
                else
                {
                    _portalShader.SetColor("_PortalColor", _failurePortal);
                }
                break;
            default:
                Debug.LogError($"Unhandled Expedition state: {type}. Please change!");
                break;
        }
    }
}