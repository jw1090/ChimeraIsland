using UnityEngine;

namespace AI.Behavior
{
    public class TrainingState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private Vector3 _stayPos = new Vector3();
        //private Vector3 _patrolPos = new Vector3();
        //private float _wanderRange = 2f;
        //private float _wanderTimer = 0.0f;

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            //_wanderTimer = 2.0f;
            //_patrolPos = _chimeraBehavior.TrainingPosition;
            _stayPos = _chimeraBehavior.TrainingPosition;
            //_chimeraBehavior.SetAgentDestination(GetNewWayPoint(_patrolPos.y));
            _chimeraBehavior.BoxCollider.enabled = false;
            _chimeraBehavior.Agent.enabled = false;
        }

        public override void Update()
        {
            //_wanderTimer -= Time.deltaTime;
            //if (_wanderTimer <= 0f)
            //{
            //    _wanderTimer = 2f;
            //    _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_patrolPos.y));
            //}
        }

        public override void Exit()
        {
            //_wanderTimer = 0.0f;
            _chimeraBehavior.BoxCollider.enabled = true;
            _chimeraBehavior.Agent.enabled = true;

        }

        //private Vector3 GetNewWayPoint(float positionY)
        //{
        //    float randomX = Random.Range(-_wanderRange, _wanderRange);
        //    float randomZ = Random.Range(-_wanderRange, _wanderRange);

        //    Vector3 randomPoint = new Vector3
        //    (
        //        _patrolPos.x + randomX,
        //        positionY,
        //        _patrolPos.z + randomZ
        //    );

        //    return randomPoint;
        //}
    }
}