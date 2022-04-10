using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPos : MonoBehaviour
{
    public static RandomPos Instance;

    public float patrolRange;
    private Vector3 wayPoint;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Vector3 GetNewWayPoint()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        // Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        Vector3 randomPoint = new Vector3(AI.Chimera.ChimeraStates.Instance.patrolPoints[AI.Chimera.ChimeraStates.Instance.WanderIndex].position.x + randomX,
                                                                    transform.position.y,
                                                                    AI.Chimera.ChimeraStates.Instance.patrolPoints[AI.Chimera.ChimeraStates.Instance.WanderIndex].position.z + randomZ);
        wayPoint = randomPoint;
        return wayPoint;
    }
}
