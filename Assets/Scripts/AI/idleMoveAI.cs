using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleMoveAI : MonoBehaviour
{
    public List<Transform> directPoints;
    public int index = 0;
    public float patrolTime = 1f; //wait time

    private float timer = 0; //timer
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        directPoints = GameManager.Instance.GetActiveHabitat().GetPatrolNodes();

        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;

        navMeshAgent.SetDestination(directPoints[index].position);
        //  navMeshAgent.destination = directPoints[index].position;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.remainingDistance < 1.5f)
        {
            timer += Time.deltaTime;

            if (timer >= patrolTime)
            {
                index++;

               // index %= 11;
                timer = 0;
                if (index >= directPoints.Count - 1)
                {
                   // index = 0;
                    
                    this.GetComponent<PlayerController>().enabled = true;
                    this.enabled = false;
                    Debug.Log("Last facilities");

                }
                else
                {
                    navMeshAgent.destination = directPoints[index].position;
                }
              
            }
        }
    }

    public void ResetIndex() { index = 1; }
}