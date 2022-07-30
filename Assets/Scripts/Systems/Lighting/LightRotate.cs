using UnityEngine;

public class LightRotate : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    private Vector3 _rotationVector = Vector3.zero;

    private void Awake()
    {
        _rotationVector = new Vector3(0.0f, _speed, 0.0f);
    }

    private void Update()
    {
        this.transform.Rotate(_rotationVector * Time.deltaTime);
    }
}