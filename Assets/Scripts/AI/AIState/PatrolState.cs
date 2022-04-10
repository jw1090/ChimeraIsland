using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Chimera
{
    public class PatrolState : ChimeraBaseStates
    {
        public override void Enter(ChimeraStates chimeraStates)
        {
            chimeraStates.navMeshAgent.destination = chimeraStates.patrolPoints[chimeraStates.index].position;
            //Debug.Log(chimeraStates.index);
        }

        public override void Update(ChimeraStates chimeraStates)
        {
            if (chimeraStates.navMeshAgent.remainingDistance < 1.5f)
            {
                chimeraStates.timer += Time.deltaTime;

                if (chimeraStates.timer >= chimeraStates.patrolTime)
                {
                    chimeraStates.index++;
                    chimeraStates.WanderIndex++;
                    chimeraStates.timer = 0;
                    if (chimeraStates.index >= chimeraStates.patrolPoints.Length - 1)
                    {
                        chimeraStates.index = 0;
                    }
                    if (chimeraStates.WanderIndex >= chimeraStates.patrolPoints.Length - 1)
                    {
                        chimeraStates.WanderIndex = 0;
                    }

                    chimeraStates.ChangeState(chimeraStates.states[StateEnum.Wander]);
                }
            }
        }

        public override void Exit(ChimeraStates chimeraStates)
        {

        }
    }
}


