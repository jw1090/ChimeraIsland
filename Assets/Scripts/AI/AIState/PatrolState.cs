using System.Collections;
using UnityEngine;

public class PatrolState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _chimeraBehavior = chimeraBehavior;

        _chimeraBehavior.SetAgentDestination(_chimeraBehavior.GetCurrentNode().position);
        _chimeraBehavior.EnterAnim(AnimationType.Walk, true);

        ServiceLocator.Get<MonoUtil>().StartCoroutineEx(DroppedReset());
    }

    public override void Update()
    {
        if (_chimeraBehavior.Dropped)
        {
            return;
        }

        if (_chimeraBehavior.GetAgentDistance() >= 1.5f)
        {
            return;
        }

        if (_chimeraBehavior.GetAgentDistance() == 0.0f)
        {
            _chimeraBehavior.SetAgentDestination(_chimeraBehavior.GetCurrentNode().position);
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
                _chimeraBehavior.ChangeState(ChimeraBehaviorState.Idle);
                break;
            case 1:
                _chimeraBehavior.ChangeState(ChimeraBehaviorState.Wander);
                break;
        }
    }

    public override void Exit()
    {
        _chimeraBehavior.Dropped = false;
        _chimeraBehavior.EnterAnim(AnimationType.Walk, false);
    }

    private IEnumerator DroppedReset()
    {
        yield return new WaitForSeconds(0.1f);
        _chimeraBehavior.Dropped = false;
    }
}