using UnityEngine;
using UnityEngine.AI;

public class HeldState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;
    private GameObject _sphereMarker = null;
    private InputManager _inputManager = null;
    private AudioManager _audioManager = null;
    private Vector3 _lastValidPos = Vector3.zero;
    private string _heldAnim = "Held";

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _inputManager = ServiceLocator.Get<InputManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _chimeraBehavior = chimeraBehavior;
        _chimeraBehavior.BoxCollider.enabled = false;
        _chimeraBehavior.CameraUtil.IsHolding = true;

        _audioManager.PlayHeldChimeraSFX(_chimeraBehavior.GetChimeraType());

        _sphereMarker = _inputManager.SphereMarker;
        _sphereMarker.gameObject.SetActive(true);

        _lastValidPos = _chimeraBehavior.transform.position;

        _chimeraBehavior.StopParticles();

        _chimeraBehavior.EnterAnim(_heldAnim);
    }

    public override void Update()
    {
        ObjFollowMouse();

        _sphereMarker.transform.position = _lastValidPos;
    }

    public override void Exit()
    {
        _sphereMarker.gameObject.SetActive(false);

        _chimeraBehavior.gameObject.transform.localPosition = _lastValidPos;

        _chimeraBehavior.Agent.enabled = true;
        _chimeraBehavior.BoxCollider.enabled = true;
        _chimeraBehavior.CameraUtil.IsHolding = false;
        _chimeraBehavior.Dropped = true;
        _chimeraBehavior.ExitAnim(_heldAnim);
    }

    private void ObjFollowMouse()
    {
        Ray ray = _chimeraBehavior.MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 300.0f, 1 << LayerMask.NameToLayer("Ground")))
        {
            // Check if the desired world position is on the NavMesh
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 1f, 1))
            {
                _lastValidPos = new Vector3(navMeshHit.position.x, navMeshHit.position.y, navMeshHit.position.z);
            }

            Vector3 desiredWorldPos = hit.point + Vector3.up;
            _chimeraBehavior.transform.position = desiredWorldPos;

            _chimeraBehavior.Agent.enabled = false;
        }
    }
}