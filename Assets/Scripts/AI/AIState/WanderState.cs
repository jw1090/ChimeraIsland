using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Chimera
{
    public class WanderState : ChimeraBaseStates
    {
        [SerializeField] private float patrolRange = 10f;
        //total wander time
        private float TotalTimer;
        //time of wander point
        private float PartTimer;
        //wander over
        private bool IsOver;

        public override void Enter(ChimeraBehaviors chimeraBehaviors)
        {
            TotalTimer = 10.0f;
            PartTimer = 2.0f;
            IsOver = false;
            chimeraBehaviors.navMeshAgent.destination = GetNewWayPoint(chimeraBehaviors.gameObject.transform.position.y);
        }

        public override void Update(ChimeraBehaviors chimeraBehaviors)
        {
            TotalTimer -= Time.deltaTime;
            PartTimer -= Time.deltaTime;

            if (TotalTimer <= 0f)
            {
                IsOver = true;
                chimeraBehaviors.ChangeState(chimeraBehaviors.states[StateEnum.Patrol]);
            }
            if (PartTimer <= 0f && !IsOver)
            {
                PartTimer = 2f;
                chimeraBehaviors.navMeshAgent.destination = GetNewWayPoint(chimeraBehaviors.gameObject.transform.position.y);
            }
        }

        public override void Exit(ChimeraBehaviors chimeraBehaviors)
        {
            TotalTimer = 0.0f;
            PartTimer = 0.0f;
        }

        public Vector3 GetNewWayPoint(float positionY)
        {
            float randomX = Random.Range(-patrolRange, patrolRange);
            float randomZ = Random.Range(-patrolRange, patrolRange);
            // Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            Vector3 randomPoint = new Vector3
            (
                AI.Chimera.ChimeraBehaviors.Instance.patrolPoints[AI.Chimera.ChimeraBehaviors.Instance.index].position.x + randomX,
                positionY,
                AI.Chimera.ChimeraBehaviors.Instance.patrolPoints[AI.Chimera.ChimeraBehaviors.Instance.index].position.z + randomZ
            );
            return randomPoint;
        }
    }
}