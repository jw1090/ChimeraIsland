using UnityEngine;

namespace AI.Behavior
{
    public class PatrolState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        public override void Enter(ChimeraBehavior chimeraBehaviors)
        {
            _chimeraBehavior = chimeraBehaviors;
            _chimeraBehavior.SetAgentDestination(_chimeraBehavior.GetCurrentNode().position);
        }

        public override void Update()
        {
            if (_chimeraBehavior.GetAgentDistance() < 1.5f)
            {
                _chimeraBehavior.AddToTimer(Time.deltaTime);

                if (_chimeraBehavior.GetTimer() >= _chimeraBehavior.GetPatrolTime())
                {
                    _chimeraBehavior.IncreasePatrolIndex(1);
                    _chimeraBehavior.IncreaseWanderIndex(1);
                    _chimeraBehavior.ResetTimer();

                    if (_chimeraBehavior.GetPatrolIndex() > _chimeraBehavior.GetNodeCount() - 1)
                    {
                        _chimeraBehavior.ResetPatrolIndex();
                    }

                    if (_chimeraBehavior.GetWanderIndex() > _chimeraBehavior.GetNodeCount() - 1)
                    {
                        _chimeraBehavior.ResetWanderIndex();
                    }

                    _chimeraBehavior.ChangeState(_chimeraBehavior.states[StateEnum.Wander]);
                }
            }
        }

        public override void Exit()
        {

        }
    }
}