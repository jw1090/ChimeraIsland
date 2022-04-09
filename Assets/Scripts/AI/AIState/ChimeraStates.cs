using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace AI.Chimera
{
    public class ChimeraStates : MonoBehaviour
    {
        ChimeraBaseStates currentStates;

        public int index = 0;
        public float patrolTime = 1f; //wait time
        public float timer = 0; //timer

        [Header("References")]
        public NavMeshAgent navMeshAgent;
        public Transform[] directPoints;
        public PatrolState patrolState = new PatrolState();
        public WanderState wanderState = new WanderState(); 
        public HeldState heldState = new HeldState();

        void OnEnable()
        {
            directPoints = GameObject.FindGameObjectWithTag("PosMgr").GetComponent<PostionPoints>().PositionPoints;
            index = 0;
            //directPoints = GameManager.Instance.GetActiveHabitat().GetPatrolNodes();

            navMeshAgent = GetComponent<NavMeshAgent>();

            navMeshAgent.isStopped = false;

            navMeshAgent.SetDestination(directPoints[index].position);
            //  navMeshAgent.destination = directPoints[index].position;
            timer = 0;
        }

        void Start()
        {
            ChangeState(patrolState);
        }

        // Update is called once per frame
        void Update()
        {
            currentStates.Update(this);
        }

        public void ChangeState(ChimeraBaseStates states)
        {
            currentStates = states;
            currentStates.Enter(this);
        }
        
        public void Wander()
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

                        //if (GameObject.FindGameObjectWithTag("PosMgr").GetComponent<MoveState>().transformsFacilities.Count != 0)
                        //{
                        //    this.GetComponent<PlayerController>().enabled = true;
                        //    this.enabled = false;
                        //    Debug.Log("Last Patnodes");
                        //}
                        index = 0;
                        navMeshAgent.destination = directPoints[index].position;
                    }
                    else
                    {
                        navMeshAgent.destination = directPoints[index].position;
                    }
                }
            }
        }

        public void Patrol()
        {
            if (navMeshAgent.remainingDistance < 1.5f)
            {
                timer += Time.deltaTime;

                if (timer >= patrolTime)
                {
                    index++;

                    timer = 0;
                    if (index >= directPoints.Length - 1)
                    {
                        index = 0;
                        navMeshAgent.destination = directPoints[index].position;
                    }
                    else
                    {
                        navMeshAgent.destination = directPoints[index].position;
                    }
                }
            }
        }
    }
}
