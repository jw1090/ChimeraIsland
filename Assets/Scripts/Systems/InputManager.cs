using AI.Behavior;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private LayerMask _chimeraLayer = new LayerMask();
    private LayerMask _facilityLayer = new LayerMask();
    private bool _isInitialized = false;
    private bool _sliderUpdated = false;
    private bool _isHolding = false;
    private ChimeraBehavior _heldChimera = null;
    private ReleaseSlider _releaseSlider = null;
    private Camera _cameraMain = null;

    public void SetReleaseSlider(ReleaseSlider releaseSlider) { _releaseSlider = releaseSlider; }
    public void SetCameraMain(Camera cameraMain) { _cameraMain = cameraMain; }

    public InputManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _chimeraLayer = LayerMask.GetMask("Chimera");
        _facilityLayer = LayerMask.GetMask("Facility");

        _isInitialized = true;

        return this;
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            RemoveFromFacility();
        }

        if ((Input.GetMouseButtonDown(0)))
        {
            EnterHeldState();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ResetSliderInfo();
            ExitHeldState();
        }
    }

    private void RemoveFromFacility()
    {
        if(_cameraMain == null)
        {
            return;
        }

        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 200.0f, _facilityLayer);

        if (hit.collider == null)
        {
            Debug.Log("Hit nothing");
            return;
        }

        if (hit.collider.CompareTag("Facility") == false)
        {
            ResetSliderInfo();
            return;
        }

        _releaseSlider.Hold(hit);
        _releaseSlider.UpdateSliderUI();
        _sliderUpdated = true;
    }

    private void ResetSliderInfo()
    {
        if (_sliderUpdated == false)
        {
            return;
        }

        _releaseSlider.ResetSlider();
        _releaseSlider.UpdateSliderUI();
        _sliderUpdated = false;
    }

    private void EnterHeldState()
    {
        if (_cameraMain == null)
        {
            return;
        }

        if (_isHolding == true)
        {
            return;
        }

        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _chimeraLayer))
        {
            _heldChimera = hit.transform.gameObject.GetComponent<ChimeraBehavior>();
            _heldChimera.ChimeraSelect(true);
            _isHolding = true;
        }
    }

    private void ExitHeldState()
    {
        if (_isHolding == false)
        {
            return;
        }

        _heldChimera.GetComponent<ChimeraBehavior>().ChimeraSelect(false);
        _isHolding = false;
        _heldChimera = null;
    }
}