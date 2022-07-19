using UnityEngine;

namespace AI.Behavior
{
    public class WanderState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private Vector3 _wanderPoint = Vector3.zero;
        private bool _enabled = false;
        private float _wanderRange = 5.0f;
        private float _wanderTimer = 0.0f;
        private float _wanderDuration = 5.0f;
        private string _wanderAnim = "Walk";

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _wanderTimer = _wanderDuration;

            _chimeraBehavior.EnterAnim(_wanderAnim);

            _wanderPoint = GetNewWayPoint();
            _chimeraBehavior.SetAgentDestination(_wanderPoint);

            _enabled = true;
        }

        public override void Update()
        {
            if(_enabled == false)
            {
                return;
            }

            _wanderTimer -= Time.deltaTime;

            if (_wanderTimer <= 0.0f)
            {
                _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Patrol]);
            }
            else if (_chimeraBehavior.GetAgentDistance() < 1.5f)
            {
                _wanderPoint = GetNewWayPoint();
                _chimeraBehavior.SetAgentDestination(_wanderPoint);
            }
        }

        public override void Exit()
        {
            _enabled = false;

            _chimeraBehavior.ExitAnim(_wanderAnim);
        }

        private Vector3 GetNewWayPoint()
        {
            float randomX = Random.Range(-_wanderRange, _wanderRange);
            float randomZ = Random.Range(-_wanderRange, _wanderRange);

            Vector3 agentPos = _chimeraBehavior.gameObject.transform.position;

            Vector3 randomPoint = new Vector3
            (
                agentPos.x + randomX,
                agentPos.y,
                agentPos.z + randomZ
            );

            return randomPoint;
        }
    }
}