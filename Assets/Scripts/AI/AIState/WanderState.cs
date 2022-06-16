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
        private string _wanderAnim = "Walking";

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            Debug.Log($"<color=green>[FSM] Enter {this.GetType()}</color>");
            _chimeraBehavior = chimeraBehavior;
            _totalTimer = 5.0f;
            _patrolTimer = 2.5f;
            _isOver = false;

            _chimeraBehavior.EnterAnim(_wanderAnim);
            _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_chimeraBehavior.gameObject.transform.position.y));
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
                _patrolTimer = 2.5f;
                _chimeraBehavior.SetAgentDestination(GetNewWayPoint(_chimeraBehavior.gameObject.transform.position.y));
            }
        }

        public override void Exit()
        {
            _totalTimer = 0.0f;
            _patrolTimer = 0.0f;
            _chimeraBehavior.ExitAnim(_wanderAnim);
        }

        private Vector3 GetNewWayPoint(float positionY)
        {
            float randomX = Random.Range(-_patrolRange, _patrolRange);
            float randomZ = Random.Range(-_patrolRange, _patrolRange);

            Vector3 agentPos = _chimeraBehavior.gameObject.transform.position;

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