using UnityEngine;

namespace AI.Behavior
{
    public class IdleState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private bool _enabled = false;
        private float _idleTimer = 0.0f;
        private float _idleDuration = 5.0f;
        private string _idleAnim = "Idle";

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _idleTimer = _idleDuration;
            _chimeraBehavior.EnterAnim(_idleAnim);
            _chimeraBehavior.Agent.isStopped = true;

            _enabled = true;
        }

        public override void Update()
        {
            if (_enabled == false)
            {
                return;
            }

            _idleTimer -= Time.deltaTime;

            if (_idleTimer <= 0.0f)
            {
                _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Patrol]);
            }
        }

        public override void Exit()
        {
            _enabled = false;

            _chimeraBehavior.Agent.isStopped = false;
            _idleTimer = 0.0f;
            _chimeraBehavior.ExitAnim(_idleAnim);
        }
    }
}