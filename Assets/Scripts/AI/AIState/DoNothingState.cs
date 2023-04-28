using UnityEngine;

public class DoNothingState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _chimeraBehavior = chimeraBehavior;

        _chimeraBehavior.EnterAnim(AnimationType.Idle);
        _chimeraBehavior.Agent.isStopped = true;
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        _chimeraBehavior.Agent.isStopped = false;
    }
}