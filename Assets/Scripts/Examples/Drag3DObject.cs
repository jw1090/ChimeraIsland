using UnityEngine;

public class Drag3DObject : MonoBehaviour
{
    [SerializeField] private bool _snapToGround = false;

    // Vector3.down is a GC allocation per call so we are caching it.
    private Vector3 _down = Vector3.down;
    private float _rayLength = 100f;
    private float _heightOffset = 1.0f;
    private float _cameraZDistance = 0.0f;
    private Camera _mainCam = null;
    private Vector3 _newPosition = Vector3.zero;
    private bool _shouldUpdate = false;

    private void OnEnable()
    {
        _mainCam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (_shouldUpdate) 
        {
            transform.position = _newPosition;
            _shouldUpdate = false;
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _cameraZDistance;
        return _mainCam.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseDrag()
    {
        _shouldUpdate = true;
        _cameraZDistance = _mainCam.WorldToScreenPoint(transform.position).z;

        if (!_snapToGround)
        {
            _newPosition = GetMouseAsWorldPoint();
            return;
        }

        SnapToGround(GetMouseAsWorldPoint());
    }

    private void SnapToGround(Vector3 mouseWorldPos)
    {
        Ray objToGround = new Ray(mouseWorldPos, _down);
        Physics.Raycast(objToGround, out RaycastHit hit, _rayLength);
        var collider = hit.collider;
        if (collider != null && collider.CompareTag("Ground"))
        {
            Vector3 groundPos = collider.ClosestPoint(transform.position);
            _newPosition = new Vector3(mouseWorldPos.x, groundPos.y + _heightOffset, mouseWorldPos.z);
        }
    }
}
