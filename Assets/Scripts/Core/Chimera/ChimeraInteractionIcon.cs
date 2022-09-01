using UnityEngine;

public class ChimeraInteractionIcon : MonoBehaviour
{
    private Camera _camera = null;

    public void Initialize()
    {
        _camera = ServiceLocator.Get<CameraUtil>().CameraCO;
    }

    private void Update()
    {
        transform.LookAt(_camera.transform);
    }
}