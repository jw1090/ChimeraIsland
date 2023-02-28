public class TreadmillState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _chimeraBehavior = chimeraBehavior;
        _chimeraBehavior.Agent.enabled = false;

        _chimeraBehavior.EnterAnim(AnimationType.Walk,true);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        _chimeraBehavior.Agent.enabled = true;
        _chimeraBehavior.EnterAnim(AnimationType.Walk, false);
    }
}