using UnityEngine;

public class Drag3DObject : MonoBehaviour
{
    private Vector3 offset = Vector3.zero;
    private float zCoord = 0.0f;
    private Camera cam = null;

    private void OnEnable()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        zCoord = cam.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zCoord;
        return cam.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + offset;
    }
}
