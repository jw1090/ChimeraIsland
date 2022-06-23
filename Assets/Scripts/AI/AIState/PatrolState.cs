using System.Collections;
using UnityEngine;

namespace AI.Behavior
{
    public class PatrolState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private string _patrolAnim = "Walk";

        public override void Enter(ChimeraBehavior chimeraBehaviors)
        {
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

            _chimeraBehavior.IncreasePatrolIndex();

            if (_chimeraBehavior.PatrolIndex > _chimeraBehavior.GetNodeCount() - 1)
            {
                _chimeraBehavior.ResetPatrolIndex();
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