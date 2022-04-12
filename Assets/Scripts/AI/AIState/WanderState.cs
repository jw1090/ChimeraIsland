using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Chimera
{
    public class WanderState : ChimeraBaseStates
    {
        public float patrolRange = 10f;
        private Vector3 wayPoint;
        //total wander time
        public float TotelTimer;
        //time of wander point
        public float PartTimer;
        //wander over
        public bool IsOver;

        public override void Enter(ChimeraBehaviors chimeraBehaviors)
        {
            TotelTimer = 10f;
            PartTimer = 2f;
            IsOver = false;
            chimeraBehaviors.navMeshAgent.destination = GetNewWayPoint(chimeraBehaviors.gameObject.transform.position.y);
        }

        public override void Update(ChimeraBehaviors chimeraBehaviors)
        {
            TotelTimer -= Time.deltaTime;
            PartTimer -= Time.deltaTime;
            // Debug.Log(TotelTimer);
            if (TotelTimer <= 0f)
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
            TotelTimer = 0f;
            PartTimer = 0f;
        }

        public Vector3 GetNewWayPoint(float positionY)
        {
            float randomX = Random.Range(-patrolRange, patrolRange);
            float randomZ = Random.Range(-patrolRange, patrolRange);

            // Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            Vector3 randomPoint = new Vector3
            (
                AI.Chimera.ChimeraBehaviors.Instance.patrolPoints[AI.Chimera.ChimeraBehaviors.Instance.WanderIndex].position.x + randomX,
                positionY,
                AI.Chimera.ChimeraBehaviors.Instance.patrolPoints[AI.Chimera.ChimeraBehaviors.Instance.WanderIndex].position.z + randomZ
            );
            wayPoint = randomPoint;
            return wayPoint;
        }
    }
}