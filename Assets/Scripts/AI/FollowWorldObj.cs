using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorldObj : MonoBehaviour
{
    public Transform target;   //3D obj
    public RectTransform image;    //follow UI
    public Canvas canvas;   //UI canvas
    public Vector2 offset;

    private Vector2 screenPos;
    private Vector3 mousePos;

    void Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        image.position = screenPos + offset;
    }
}


