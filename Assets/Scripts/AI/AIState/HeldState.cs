namespace AI.Behavior
{
    public class HeldState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private string _animationState = "Held";

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _chimeraBehavior.BoxCollider.enabled = false;
            _chimeraBehavior.CameraController.IsHolding = true;

            if (_chimeraBehavior.Animator != null)
            {
                _chimeraBehavior.Animator.SetBool(_animationState, true);
            }
        }

        public override void Update()
        {
            _chimeraBehavior.ObjFollowMouse();
            _chimeraBehavior.HeldEnterPatrol();
        }

        public override void Exit()
        {
            _chimeraBehavior.Agent.enabled = true;
            _chimeraBehavior.BoxCollider.enabled = true;
            _chimeraBehavior.CameraController.IsHolding = false;

            if (_chimeraBehavior.Animator != null)
            {
                _chimeraBehavior.Animator.SetBool(_animationState, false);
            }
        }
    }
}