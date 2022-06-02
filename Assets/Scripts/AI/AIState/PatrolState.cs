using System.Collections;
using UnityEngine;

namespace AI.Behavior
{
    public class PatrolState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private float _waitTime = 1.0f;
        private string _animIdle = "Idle";
        private string _animWalk = "Walk";

        public override void Enter(ChimeraBehavior chimeraBehaviors)
        {
            _chimeraBehavior = chimeraBehaviors;
            _chimeraBehavior.SetAgentDestination(_chimeraBehavior.GetCurrentNode().position);

            _chimeraBehavior.EnterAnim(_animWalk);

            ServiceLocator.Get<MonoUtil>().StartCoroutineEx(DroppedReset());
        }

        public override void Update()
        {
            _chimeraBehavior.HeldEnterCheck();

            if(_chimeraBehavior.GetAgentDistance() >= 1.5f)
            {
                return;
            }

            _chimeraBehavior.AddToTimer(Time.deltaTime);
            _chimeraBehavior.EnterAnim(_animIdle);

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

            _chimeraBehavior.ChangeState(_chimeraBehavior.States[StateEnum.Wander]);
        }

        public override void Exit()
        {
            _chimeraBehavior.Dropped = false;
        }

        private IEnumerator DroppedReset()
        {
            yield return new WaitForSeconds(0.1f);
            _chimeraBehavior.Dropped = false;
        }
    }
}