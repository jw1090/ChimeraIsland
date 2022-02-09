using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Basic_PlayerMovement : MonoBehaviour
{
    Camera cam;

    public LayerMask groundLayer;

    public NavMeshAgent playerAgent;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAgent.SetDestination(GetPointUnderCursor());
        }
    }

    private Vector3 GetPointUnderCursor()
    {
        Vector2 mouse_pos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mouse_pos);

        RaycastHit raycast_hit;
        Physics.Raycast(ray, out raycast_hit);


        return raycast_hit.point;
    }

}
