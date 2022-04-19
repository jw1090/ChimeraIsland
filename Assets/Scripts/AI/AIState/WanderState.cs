using UnityEngine;

namespace AI.Chimera
{
    public class WanderState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private float _patrolRange = 10.0f;
        private float _totalTimer = 0.0f; // Total wander time.
        private float _partTimer = 0.0f; // Time of wander point.
        private bool _isOver = false; // Wander over.

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _totalTimer = 10.0f;
            _partTimer = 2.0f;
            _isOver = false;
            _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_chimeraBehavior.gameObject.transform.position.y));
        }

        public override void Update()
        {
            _totalTimer -= Time.deltaTime;
            _partTimer -= Time.deltaTime;

            if (_totalTimer <= 0f)
            {
                _isOver = true;
                _chimeraBehavior.ChangeState(_chimeraBehavior.states[StateEnum.Patrol]);
            }
            if (_partTimer <= 0f && !_isOver)
            {
                _partTimer = 2f;
                _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_chimeraBehavior.gameObject.transform.position.y));
            }
        }

        public override void Exit()
        {
            _totalTimer = 0.0f;
            _partTimer = 0.0f;
        }

        public Vector3 GetNewWayPoint(float positionY)
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