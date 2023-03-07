using UnityEngine;

public class WanderState : ChimeraBaseState
{
    private ChimeraBehavior _chimeraBehavior = null;
    private Vector3 _wanderPoint = Vector3.zero;
    private float _wanderRange = 5.0f;
    private float _wanderTimer = 0.0f;
    private float _wanderDuration = 5.0f;

    public override void Enter(ChimeraBehavior chimeraBehavior)
    {
        _chimeraBehavior = chimeraBehavior;
        _wanderTimer = _wanderDuration;

        _chimeraBehavior.EnterAnim(AnimationType.Walk);

        _wanderPoint = GetNewWayPoint();
        _chimeraBehavior.SetAgentDestination(_wanderPoint);
    }

    public override void Update()
    {
        _wanderTimer -= Time.deltaTime;

        if (_wanderTimer <= 0.0f)
        {
            _chimeraBehavior.ChangeState(ChimeraBehaviorState.Patrol);
        }
        else if (_chimeraBehavior.GetAgentDistance() < 1.5f)
        {
            _wanderPoint = GetNewWayPoint();
            _chimeraBehavior.SetAgentDestination(_wanderPoint);
        }
    }

    public override void Exit()
    {

    }

    private Vector3 GetNewWayPoint()
    {
        float randomX = Random.Range(-_wanderRange, _wanderRange);
        float randomZ = Random.Range(-_wanderRange, _wanderRange);

        Vector3 agentPos = _chimeraBehavior.gameObject.transform.position;

        Vector3 randomPoint = new Vector3
        (
            agentPos.x + randomX,
            agentPos.y,
            agentPos.z + randomZ
        );

        return randomPoint;
    }
}