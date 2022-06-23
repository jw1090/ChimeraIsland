using UnityEngine;

namespace AI.Behavior
{
    public class HeldState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private string _heldAnim = "Held";
        private float _heightOffset = 1.0f;

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _chimeraBehavior.BoxCollider.enabled = false;
            _chimeraBehavior.CameraController.IsHolding = true;

            _chimeraBehavior.EnterAnim(_heldAnim);
        }

        public override void Update()
        {
            ObjFollowMouse();
            HeldReleaseCheck();
        }

        public override void Exit()
        {
            _chimeraBehavior.Agent.enabled = true;
            _chimeraBehavior.BoxCollider.enabled = true;
            _chimeraBehavior.CameraController.IsHolding = false;
            _chimeraBehavior.Dropped = true;
            _chimeraBehavior.ExitAnim(_heldAnim);
        }

        private void ObjFollowMouse()
        {
            Ray ray = _chimeraBehavior.MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
            {
                _chimeraBehavior.transform.position = hit.point + (Vector3.up * _heightOffset);
                _chimeraBehavior.Agent.enabled = false;
            }
        }

        private void HeldReleaseCheck()
        {
            if (_chimeraBehavior.WasClicked == false)
            {
                _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Patrol]);
            }
        }
    }
}