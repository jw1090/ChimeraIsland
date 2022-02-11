using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class idleMoveAI : MonoBehaviour
{
    public Transform[] directPoints;
    private int index = 0;
    public float patroTime = 2f;//wait time

    private float timer = 0;//timer
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.destination = directPoints[index].position;

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

                index %= 11;
                timer = 0;
                if (index >= directPoints.Length)
                {
                    index = 0;
                }
                navMeshAgent.destination = directPoints[index].position;
            }
        }

    }
}