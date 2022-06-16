using System.Collections;
using UnityEngine;

namespace AI.Behavior
{
    public class PatrolState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private float _waitTime = 1.0f;
        private string _patrolAnim = "Walking";

        public override void Enter(ChimeraBehavior chimeraBehaviors)
        {
            Debug.Log($"<color=green>[FSM] Enter {this.GetType()}</color>");
            _chimeraBehavior = chimeraBehaviors;
            _chimeraBehavior.SetAgentDestination(_chimeraBehavior.GetCurrentNode().position);

            _chimeraBehavior.EnterAnim(_patrolAnim);

            ServiceLocator.Get<MonoUtil>().StartCoroutineEx(DroppedReset());
        }

        public override void Update()
        {
            _chimeraBehavior.HeldEnterCheck();

            if (_chimeraBehavior.GetAgentDistance() >= 1.5f)
            {
                return;
            }

            _chimeraBehavior.AddToTimer(Time.deltaTime);

            if (_chimeraBehavior.Timer < _waitTime)
            {
                return;
            }

            _chimeraBehavior.IncreasePatrolIndex(1);
            _chimeraBehavior.IncreaseWanderIndex(1);
            _chimeraBehavior.ResetTimer();

            if (_chimeraBehavior.PatrolIndex > _chimeraBehavior.GetNodeCount() - 1)
            {
                _chimeraBehavior.ResetPatrolIndex();
            }

            if (_chimeraBehavior.WanderIndex > _chimeraBehavior.GetNodeCount() - 1)
            {
                _chimeraBehavior.ResetWanderIndex();
            }

            switch (Random.Range(0, 2))
            {
                case 0:
                    _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Idle]);
                    break;
                case 1:
                    _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Wander]);
                    break;
            }
        }

        public override void Exit()
        {
            _chimeraBehavior.Dropped = false;
            _chimeraBehavior.ExitAnim(_patrolAnim);
        }

        private IEnumerator DroppedReset()
        {
            yield return new WaitForSeconds(0.1f);
            _chimeraBehavior.Dropped = false;
        }
    }
}