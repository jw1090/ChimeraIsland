using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class idleMoveAI : MonoBehaviour
{
    public Transform[] directPoints;
    public int index = 0;
    public float patroTime = 2f;//wait time

    private float timer = 0;//timer
    private NavMeshAgent navMeshAgent;

    void OnEnable()
    {
        index = 0;
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

            if (timer >= patroTime)
            {
                index++;

               // index %= 11;
                timer = 0;
                if (index >= directPoints.Length-1)
                {
                   // index = 0;
                    //巡逻点最后一个点已经到达
                    this.GetComponent<PlayerContruller>().enabled = true;
                    this.enabled = false;
                    print("到达最后一个点转到待机器械交互");

                }
                else
                {
                    navMeshAgent.destination = directPoints[index].position;
                }
              
            }
        }

    }
}