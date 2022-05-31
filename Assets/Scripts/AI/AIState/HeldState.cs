using UnityEngine;

namespace AI.Behavior
{
    public class HeldState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private float _heightOffset = 1.2f;

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _chimeraBehavior.BoxCollider.enabled = false;
            _chimeraBehavior.CameraController.IsHolding = true;
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
            if (_chimeraBehavior.Clicked == false)
            {
                _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Patrol]);
            }
        }
    }
}