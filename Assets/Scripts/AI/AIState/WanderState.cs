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

        public override void Enter(ChimeraStates chimeraStates)
        {
            TotelTimer = 10f;
            PartTimer = 2f;
            IsOver = false;
            chimeraStates.navMeshAgent.destination = GetNewWayPoint(chimeraStates.gameObject.transform.position.y);
        }

        public override void Update(ChimeraStates chimeraStates)
        {
            TotelTimer -= Time.deltaTime;
            PartTimer -= Time.deltaTime;
            // Debug.Log(TotelTimer);
            if (TotelTimer <= 0f)
            {
                IsOver = true;
                chimeraStates.ChangeState(chimeraStates.states[StateEnum.Patrol]);
            }
            if (PartTimer <= 0f && !IsOver)
            {
                PartTimer = 2f;
                chimeraStates.navMeshAgent.destination = GetNewWayPoint(chimeraStates.gameObject.transform.position.y);
            }
        }
        public override void Exit(ChimeraStates chimeraStates)
        {
            TotelTimer = 0f;
            PartTimer = 0f;
        }
        public Vector3 GetNewWayPoint(float positionY)
        {
            float randomX = Random.Range(-patrolRange, patrolRange);
            float randomZ = Random.Range(-patrolRange, patrolRange);

            // Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            Vector3 randomPoint = new Vector3(AI.Chimera.ChimeraStates.Instance.patrolPoints[AI.Chimera.ChimeraStates.Instance.WanderIndex].position.x + randomX,
                                              positionY,
                                              AI.Chimera.ChimeraStates.Instance.patrolPoints[AI.Chimera.ChimeraStates.Instance.WanderIndex].position.z + randomZ);
            wayPoint = randomPoint;
            return wayPoint;
        }
    }

}

