using UnityEngine;

public class LightRotate : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;

    private void Update()
    {
        transform.Rotate(Vector3.up, _speed * Time.deltaTime);
    }
}