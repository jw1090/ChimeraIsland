using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private Vector3 _pos = Vector3.zero;

    [Header("Zoom")]
    [SerializeField] private float _zoom = 80.0f;
    [SerializeField] private float _zoomAmount = 20.0f;
    [SerializeField] private float _minZoom = 40.0f;
    [SerializeField] private float _maxZoom = 90.0f;

    [Header("References")]
    [SerializeField] private Camera cameraGO = null;

    public void Initialize()
    {
        Debug.Log("<color=Orange> Initializing CameraLogic ... </color>");
        cameraGO = GetComponent<Camera>();
        _pos = transform.position;
    }

    private void Update()
    {
        CameraMovement();
        CameraZoom();
    }

    private void CameraMovement()
    {
        float panSpeed = (Input.GetKey(KeyCode.LeftShift)) ? 2 * _speed : _speed;

        if (Input.GetKey(KeyCode.W))
        {
            _pos.z -= panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _pos.x += panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _pos.x -= panSpeed * Time.deltaTime;
        }

        transform.position = _pos;
    }

    private void CameraZoom()
    {
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            _zoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomAmount;
            _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
            cameraGO.fieldOfView = _zoom;
        }
    }
}