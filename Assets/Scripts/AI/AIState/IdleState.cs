using UnityEngine;

namespace AI.Behavior
{
    public class IdleState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private Chimera _chimera = null;
        private float _idleTimer = 0.0f;
        private float _idleDuration = 5.0f;
        private string _idleAnim = "Idle";

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            Debug.Log($"<color=green>[FSM] Enter {this.GetType()}</color>");
            _chimeraBehavior = chimeraBehavior;
            _chimera = _chimeraBehavior.GetComponent<Chimera>();
            _idleTimer = _idleDuration;

            _chimeraBehavior.EnterAnim(_idleAnim);
        }

        public override void Update()
        {
            if(_chimera.InFacility == true)
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
            _idleTimer = 0.0f;
            _chimeraBehavior.ExitAnim(_idleAnim);
        }
    }
}