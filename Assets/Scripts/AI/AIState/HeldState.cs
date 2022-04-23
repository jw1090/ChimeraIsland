using UnityEngine;

namespace AI.Behavior
{
    public class HeldState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private Camera _mainCamera = null;
        private LayerMask _canDrag;
        private bool _isClick;
        private Transform _dragGameObject;
        private Vector3 _targetScreenPoint;

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _mainCamera = Camera.main;
            _chimeraBehavior = chimeraBehavior;
            //Debug.Log("Enter Held");
        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }
}