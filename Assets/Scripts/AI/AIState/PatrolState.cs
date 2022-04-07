using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Chimera
{
    public class PatrolState : ChimeraBaseStates
    {
        public override void Enter(ChimeraStates chimeraStates)
        {
            chimeraStates.Patrol();
        }

        public override void Update(ChimeraStates chimeraStates)
        {
            if (chimeraStates.navMeshAgent.remainingDistance < 1.5f)
            {
                chimeraStates.timer += Time.deltaTime;

                if (chimeraStates.timer >= chimeraStates.patrolTime)
                {
                    chimeraStates.index++;
                    chimeraStates.timer = 0;
                    if (chimeraStates.index >= chimeraStates.directPoints.Length - 1)
                    {
                        chimeraStates.index = 0;
                        //chimeraStates.navMeshAgent.destination = chimeraStates.directPoints[chimeraStates.index].position;
                        //____________________________________________________________________________________________________________________________
                        //Here to change State
                        //Wander();
                    }
                    else
                    {
                        chimeraStates.navMeshAgent.destination = chimeraStates.directPoints[chimeraStates.index].position;
                    }

                }
            }
        }
    }
}

