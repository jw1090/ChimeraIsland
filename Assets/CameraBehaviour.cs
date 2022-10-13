using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    private LayerMask _chimeraLayer = new LayerMask();
    private StartingChimeraButton _startingChimera;

    private void Start()
    {
        _chimeraLayer = LayerMask.GetMask("Chimera");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit chimeraHit, 300.0f, _chimeraLayer))
            {

            
            }
        }
    }
}

