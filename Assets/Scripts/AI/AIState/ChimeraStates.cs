using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace AI.Chimera
{
    public enum StateEnum
    {
        Patrol,
        Wander,
        Held
    }
    public class ChimeraStates : MonoBehaviour
    {
        public static ChimeraStates Instance;
        ChimeraBaseStates currentStates;

        public int index = 0;
        public int WanderIndex = 0;
        public float patrolTime = 1f; //wait time
        public float timer = 0; //timer

        [Header("References")]
        public NavMeshAgent navMeshAgent;
        public Transform[] patrolPoints;

        //Mapping of state to state object instance
        public Dictionary<StateEnum, ChimeraBaseStates> states = new Dictionary<StateEnum, ChimeraBaseStates>();
        void OnEnable()
        {
            patrolPoints = GameObject.FindGameObjectWithTag("PosMgr").GetComponent<PostionPoints>().PositionPoints;
            index = 0;
            WanderIndex = -1;

            //directPoints = GameManager.Instance.GetActiveHabitat().GetPatrolNodes();
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(patrolPoints[index].position);
            //  navMeshAgent.destination = directPoints[index].position;
            timer = 0;
        }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        void Start()
        {
            //Add the corresponding state and its object to the dictionary
            states.Add(StateEnum.Patrol, new PatrolState());
            states.Add(StateEnum.Wander, new WanderState());
            states.Add(StateEnum.Held, new HeldState());

            ChangeState(states[StateEnum.Patrol]);
        }
        void Update()
        {
            currentStates.Update(this);
        }
        public void ChangeState(ChimeraBaseStates states)
        {
            if (currentStates != null)
            {
                currentStates.Exit(this);
            }
            currentStates = states;
            currentStates.Enter(this);
        }
    }
}
