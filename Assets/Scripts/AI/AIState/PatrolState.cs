using System.Collections;
using UnityEngine;

public class PatrolState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;
    private string _patrolAnim = "Walk";

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _chimeraBehavior = chimeraBehavior;
        _chimeraBehavior.EnableNavAgent();
        _chimeraBehavior.SetAgentDestination(_chimeraBehavior.GetCurrentNode().position);
        _chimeraBehavior.EnterAnim(_patrolAnim);

        _chimeraBehavior.TogglePatrolParticle(true);

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
        _chimeraBehavior.TogglePatrolParticle(false);

        _chimeraBehavior.Dropped = false;
        _chimeraBehavior.ExitAnim(_patrolAnim);
    }

    private IEnumerator DroppedReset()
    {
        yield return new WaitForSeconds(0.1f);
        _chimeraBehavior.Dropped = false;
    }
}