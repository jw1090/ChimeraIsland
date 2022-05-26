using UnityEngine;

namespace AI.Behavior
{
    public class TrainingState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private float _patrolRange = 2f;
        private float _partTimer = 0.0f; // Time of wander point.
        private Vector3 _patrolPos = new Vector3();

        public void SetPos(Vector3 pos)
        {
            _patrolPos = pos;
        }

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _partTimer = 2.0f;
            _patrolPos = _chimeraBehavior.TrainingPosition;
            _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_patrolPos.y));
        }

        public override void Update()
        {
            _partTimer -= Time.deltaTime;
            if (_partTimer <= 0f)
            {
                _partTimer = 2f;
                _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_patrolPos.y));
            }
        }

        public override void Exit()
        {
            _partTimer = 0.0f;
        }

        public Vector3 GetNewWayPoint(float positionY)
        {
            float randomX = Random.Range(-_patrolRange, _patrolRange);
            float randomZ = Random.Range(-_patrolRange, _patrolRange);
            Vector3 randomPoint = new Vector3
            (
                _patrolPos.x + randomX,
                positionY,
                _patrolPos.z + randomZ
            );

            return randomPoint;
        }
    }
}