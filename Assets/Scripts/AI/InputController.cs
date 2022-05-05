using AI.Behavior;
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
        if (Input.GetMouseButtonDown(0) && !InputController.Instance.IsInput)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _chimeraLayer))
            {
                hit.transform.gameObject.GetComponent<ChimeraBehavior>().ChimeraSelect(true);
                if (CurrObj != hit.transform.gameObject)
                {
                    CurrObj = hit.transform.gameObject;
                }

                IsInput = true;
            }
        }
        if (Input.GetMouseButtonUp(0) && IsInput)
        {
            CurrObj.GetComponent<ChimeraBehavior>().ChimeraSelect(false);
            IsInput = false;
            CurrObj = null;

        }
    }
}