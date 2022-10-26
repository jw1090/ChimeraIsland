using System.Collections;
using UnityEngine;

public class StarterEnvironment : MonoBehaviour
{
    [SerializeField] GameObject _originNode = null;
    [SerializeField] GameObject _cameraANode = null;
    [SerializeField] GameObject _cameraBNode = null;
    [SerializeField] GameObject _cameraCNode = null;

    public GameObject OriginNode { get => _originNode; }
    public GameObject ANode { get => _cameraANode; }
    public GameObject BNode { get => _cameraBNode; }
    public GameObject CNode { get => _cameraCNode; }

    private Camera _cameraMain = null;
    private CameraUtil _camera = null;

    public void SetCameraUtil(CameraUtil cameraUtil)
    {
        _camera = cameraUtil;
        _cameraMain = _camera.CameraCO;
    }

    public StarterEnvironment Initialize(CameraUtil cameraUtil)
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");
        _camera = cameraUtil;
        return this;
    }
}