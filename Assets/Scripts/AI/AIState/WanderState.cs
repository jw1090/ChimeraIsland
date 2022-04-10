using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.Chimera
{
    public class WanderState : ChimeraBaseStates
    {
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

            chimeraStates.navMeshAgent.destination = RandomPos.Instance.GetNewWayPoint();
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
                chimeraStates.navMeshAgent.destination = RandomPos.Instance.GetNewWayPoint();
            }
        }
        public override void Exit(ChimeraStates chimeraStates)
        {
            TotelTimer = 0f;
            PartTimer = 0f;
        }
    }
}

