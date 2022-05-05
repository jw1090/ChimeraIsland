using AI.Behavior;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    [SerializeField] private LayerMask _chimeraLayer;

    private void Awake()
    {
        Instance = this;
    }
    public bool IsInput = false;
    public GameObject CurrObj;

    private void Update()
    {
        SelectHeldState();
    }

    public void SelectHeldState()
    {
        if ((Input.GetMouseButtonDown(0) && !InputController.Instance.IsInput) /*|| (Input.GetMouseButton(0) && InputControl.Instance.CurrObj == this.gameObject)*/)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _chimeraLayer))
            {
                hit.transform.gameObject.GetComponent<ChimeraBehavior>().Select(true);
                if (CurrObj != hit.transform.gameObject)
                {
                    CurrObj = hit.transform.gameObject;
                }

                IsInput = true;
            }
        }
        if (Input.GetMouseButtonUp(0) && IsInput)
        {
            CurrObj.GetComponent<ChimeraBehavior>().Select(false);
            IsInput = false;
            CurrObj = null;

        }
    }
}