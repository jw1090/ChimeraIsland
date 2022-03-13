using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private float panSpeed = 20f;
    Vector3 pos;

    private void Update()
    {
        pos = transform.position;

        KeyCameraMovement();

        transform.position = pos;
    }

    // - Made by: Joe 2/9/2022
    // - Basic movement using WASD
    private void KeyCameraMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
    }
}