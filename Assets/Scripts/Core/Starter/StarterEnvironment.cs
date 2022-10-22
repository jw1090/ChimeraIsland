using System.Collections;
using UnityEngine;

public class StarterEnvironment : MonoBehaviour
{
    [SerializeField] GameObject _originNode = null;
    [SerializeField] GameObject _cameraANode = null;
    [SerializeField] GameObject _cameraBNode = null;
    [SerializeField] GameObject _cameraCNode = null;
    //private Coroutine _transitionCoroutine = null;
    //private float _standardTransitionSpeed = 0.06f;
    private Camera _cameraMain = null;
    private CameraUtil _camera = null;

    public void SetCameraUtil(CameraUtil cameraUtil)
    {
        _camera = cameraUtil;
        _cameraMain = _camera.CameraCO;
    }

    public StarterEnvironment Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");


        return this;
    }
    public void CameraToOrigin()
    {
        _cameraMain.transform.position = _originNode.transform.position;
    }
    public void ChimeraCloseUp(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                _cameraMain.transform.position = Vector3.Lerp(_originNode.transform.position, _cameraANode.transform.position,1f);
                break;
            case ChimeraType.B:
                _cameraMain.transform.position = Vector3.Lerp(_originNode.transform.position, _cameraBNode.transform.position, 1f);
                break;
            case ChimeraType.C:
                _cameraMain.transform.position = Vector3.Lerp(_originNode.transform.position, _cameraCNode.transform.position, 1f);
                break;
            default:
                Debug.LogWarning($"{chimeraType} is not a valid type. Please fix!");
                break;
        }
    }
    //private void CameraShift(Vector3 position)
    //{
    //    if (_transitionCoroutine != null)
    //    {
    //        StopCoroutine(_transitionCoroutine);
    //    }

    //    _transitionCoroutine = StartCoroutine(MoveCamera(position, _standardTransitionSpeed));
    //}

    //private IEnumerator MoveCamera(Vector3 target, float time)
    //{
    //    while (Vector3.Distance(transform.position, target) > 0.5f)
    //    {
    //        yield return new WaitForSeconds(0.01f);

    //        transform.position = Vector3.Lerp(_originNode.transform.position, target, time);
    //    }
    //}
}