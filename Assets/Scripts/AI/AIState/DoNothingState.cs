using UnityEngine;

public class DoNothingState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;
    private float _idleTimer = 0.0f;
    private float _idleDuration = 5.0f;

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _chimeraBehavior = chimeraBehavior;
        _idleTimer = _idleDuration;

        _chimeraBehavior.EnterAnim(AnimationType.Idle);
        _chimeraBehavior.Agent.isStopped = true;
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        _chimeraBehavior.Agent.isStopped = false;
        _idleTimer = 0.0f;
    }
}