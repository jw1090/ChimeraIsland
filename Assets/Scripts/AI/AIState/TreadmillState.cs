public class TreadmillState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _chimeraBehavior = chimeraBehavior;
        _chimeraBehavior.Agent.enabled = false;

        _chimeraBehavior.EnterAnim(AnimationType.Walk);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        _chimeraBehavior.Agent.enabled = true;
    }
}