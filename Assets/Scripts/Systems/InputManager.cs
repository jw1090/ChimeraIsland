using UnityEngine;
using UnityEngine.UI;
using AI.Behavior;

public class InputManager : MonoBehaviour
{
    [Header("Interact Stats")]
    [SerializeField] private float _clickHeldSeconds = 2.0f;
    [SerializeField] private float _clickHeldCounter = 0.0f;

    [Header("References")]
    [SerializeField] private Slider _releaseSlider = null;
    [SerializeField] private LayerMask _chimeraLayer;

    public GameObject CurrObj { get; private set; } = null;
    public bool IsInput { get; private set; } = false;

    public InputManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing Input Manager ... </color>");

        return this;
    }

    private void Update()
    {
        CheckRemove();
        SelectHeldState();
    }

    private void CheckRemove()
    {
        // Check remove held down.
        if (Input.GetMouseButton(0))
        {
            Camera cameraMain = ServiceLocator.Get<CameraController>().CameraCO;
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 200.0f);

            if (hit.collider == null)
            {
                return;
            }

            if (hit.collider.CompareTag("Facility") && hit.collider.GetComponent<Facility>().IsChimeraStored())
            {
                _clickHeldCounter += Time.deltaTime;
                if (_clickHeldCounter >= _clickHeldSeconds)
                {
                    hit.collider.GetComponent<Facility>().RemoveChimera();
                    _clickHeldCounter = 0.0f;
                }
            }
            else _clickHeldCounter = 0.0f;
        }
        else
        {
            _clickHeldCounter = 0.0f;
        }

        // Control remove slider
        if (_clickHeldCounter > 0.0f)
        {
            _releaseSlider.gameObject.SetActive(true);
            _releaseSlider.transform.position = Input.mousePosition + new Vector3(75.0f, 0.0f, 0.0f);
            _releaseSlider.value = _clickHeldCounter / 2.0f;
        }
        else
        {
            _releaseSlider.gameObject.SetActive(false);
        }
    }

    public void SelectHeldState()
    {
        if (Input.GetMouseButtonDown(0) && IsInput == false)
        {
            Camera cameraMain = ServiceLocator.Get<CameraController>().CameraCO;
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
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