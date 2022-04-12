using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Chimera
{
    public class PatrolState : ChimeraBaseStates
    {
        public override void Enter(ChimeraBehaviors chimeraBehaviors)
        {
            chimeraBehaviors.navMeshAgent.destination = chimeraBehaviors.patrolPoints[chimeraBehaviors.index].position;
       
        }
        public override void Update(ChimeraBehaviors ChimeraBehaviors)
        {
            if (ChimeraBehaviors.navMeshAgent.remainingDistance < 1.5f)
            {
                ChimeraBehaviors.timer += Time.deltaTime;

                if (ChimeraBehaviors.timer >= ChimeraBehaviors.patrolTime)
                {

                    ChimeraBehaviors.index++;
                    ChimeraBehaviors.WanderIndex++;
                    ChimeraBehaviors.timer = 0;
                    if (ChimeraBehaviors.index >= ChimeraBehaviors.patrolPoints.Length - 1)
                    {
                        ChimeraBehaviors.index = 0;
                    }
                    if (ChimeraBehaviors.WanderIndex >= ChimeraBehaviors.patrolPoints.Length - 1)
                    {
                        ChimeraBehaviors.WanderIndex = 0;
                    }

                    ChimeraBehaviors.ChangeState(ChimeraBehaviors.states[StateEnum.Wander]);
                }
            }
        }
        public override void Exit(ChimeraBehaviors ChimeraBehaviors)
        {

        }
    }
}


