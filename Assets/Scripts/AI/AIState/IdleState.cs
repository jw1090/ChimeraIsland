using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : MonoBehaviour
{
    public int index = 0;
    public float patrolTime = 1f; //wait time

    private float timer = 0; //timer

    [Header("References")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    public Transform[] directPoints;

    void OnEnable()
    {
        index = 0;

        //directPoints = GameManager.Instance.GetActiveHabitat().GetPatrolNodes();

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
                if (index >= directPoints.Length - 1)
                {
                   
                    if ( GetComponent<FacilitiesState>().transformsFacilities.Count != 0)
                    {
                        this.GetComponent<PlayerController>().enabled = true;
                        this.enabled = false;
                        Debug.Log("Last Patnodes");
                    }
                    else
                    {
                        index = 0;
                        navMeshAgent.destination = directPoints[index].position;
                    }
                   
                }
                else
                {
                    navMeshAgent.destination = directPoints[index].position;
                }

            }
        }
    }
}
