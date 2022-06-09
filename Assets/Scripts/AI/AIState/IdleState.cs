using UnityEngine;

namespace AI.Behavior
{
    public class IdleState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private float _idleTimer = 0.0f;
        private float _idleDuration = 5.0f;
        private string _animIdle = "Idle";

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _idleTimer = _idleDuration;

            _chimeraBehavior.EnterAnim(_animIdle);
        }

        public override void Update()
        {
            _idleTimer -= Time.deltaTime;

            if (_idleTimer <= 0.0f)
            {
                _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Patrol]);
            }
        }

        public override void Exit()
        {
            _idleTimer = 0.0f;
        }
    }
}