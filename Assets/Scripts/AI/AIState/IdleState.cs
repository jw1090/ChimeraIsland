using UnityEngine;

public class IdleState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;
    private string _idleAnim = "Idle";
    private float _idleTimer = 0.0f;
    private float _idleDuration = 5.0f;

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _chimeraBehavior = chimeraBehavior;
        _idleTimer = _idleDuration;

        _chimeraBehavior.EnterAnim(_idleAnim);
        _chimeraBehavior.EnableNavAgent();
        _chimeraBehavior.Agent.isStopped = true;

        _chimeraBehavior.ActivateIdleParticles();
    }

    public override void Update()
    {
        _idleTimer -= Time.deltaTime;

        if (_idleTimer <= 0.0f)
        {
            _chimeraBehavior.ChangeState(ChimeraBehaviorState.Patrol);
        }
    }

    public override void Exit()
    {
        _chimeraBehavior.Agent.isStopped = false;
        _idleTimer = 0.0f;
        _chimeraBehavior.ExitAnim(_idleAnim);
    }
}