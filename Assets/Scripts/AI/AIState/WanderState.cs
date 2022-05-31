using UnityEngine;

namespace AI.Behavior
{
    public class WanderState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private bool _isOver = false;
        private float _patrolRange = 10.0f;
        private float _totalTimer = 0.0f;
        private float _patrolTimer = 0.0f;
        private string _animWalk = "Walk";

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _totalTimer = 10000.0f;
            _patrolTimer = 10.0f;
            _isOver = false;

            _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_chimeraBehavior.gameObject.transform.position.y));

            _chimeraBehavior.EnterAnim(_animWalk);
        }

        public override void Update()
        {
            _chimeraBehavior.HeldEnterCheck();
            _totalTimer -= Time.deltaTime;
            _patrolTimer -= Time.deltaTime;

            if (_totalTimer <= 0.0f)
            {
                _isOver = true;
                _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Patrol]);
            }

            if (_patrolTimer <= 0.0f && !_isOver)
            {
                _patrolTimer = 2.0f;
                _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_chimeraBehavior.gameObject.transform.position.y));
            }
        }

        public override void Exit()
        {
            _totalTimer = 0.0f;
            _patrolTimer = 0.0f;
        }

        private Vector3 GetNewWayPoint(float positionY)
        {
            float randomX = Random.Range(-_patrolRange, _patrolRange);
            float randomZ = Random.Range(-_patrolRange, _patrolRange);

            Vector3 agentPos = _chimeraBehavior.GetCurrentNode().position;

            Vector3 randomPoint = new Vector3
            (
                agentPos.x + randomX,
                positionY,
                agentPos.z + randomZ
            );

            return randomPoint;
        }
    }
}