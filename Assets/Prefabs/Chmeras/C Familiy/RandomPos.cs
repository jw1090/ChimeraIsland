using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPos : MonoBehaviour
{
    public float sightRadius;
    public float patrolRange;

    private Vector3 wayPoint;
    private Vector3 startpPoint;
    private NavMeshAgent agent;
    private float speed;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        startpPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetNewWayPoint()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(startpPoint.x + randomX, transform.position.y, startpPoint.z + randomZ);
        wayPoint = randomPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}
