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
        Held,
        Training,
    }

    public class ChimeraBehavior : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Transform> _nodes;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private ChimeraBaseState currentState;
        private float _heightOffset = 2f;
        private int _patrolIndex = 0;
        private int _wanderIndex = 0;
        private float _timer = 0; // Timer
        private float _patrolTime = 1f; // Wait time

        // Mapping of state to state object instance.
        public Dictionary<StateEnum, ChimeraBaseState> states = new Dictionary<StateEnum, ChimeraBaseState>();

        public void Start()
        {
            _patrolIndex = 0;
            _wanderIndex = 0;
            _timer = 0;
            _nodes = GameManager.Instance.GetPatrolNodes();

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_nodes[_patrolIndex].position);

            // Add the corresponding state and its object to the dictionary.
            states.Add(StateEnum.Patrol, new PatrolState());
            states.Add(StateEnum.Wander, new WanderState());
            states.Add(StateEnum.Held, new HeldState());

            ChangeState(states[StateEnum.Patrol]);
        }

        void Update()
        {
            currentState.Update();
        }

        public void ChangeState(ChimeraBaseState state)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            currentState = state;
            currentState.Enter(this);
        }

        IEnumerator OnMouseDown()
        {
            Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//3D obj pos to world pos,mouse screen pos to Vector3£¬calculate the distance between mouse and obj
            var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
            // Debug.Log("down");
            ChangeState(states[StateEnum.Held]);
            while (Input.GetMouseButton(0))
            {
                GetComponent<NavMeshAgent>().baseOffset = _heightOffset;
                GetComponent<NavMeshAgent>().isStopped = true;
                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
                transform.position = curPosition;

                yield return new WaitForFixedUpdate();
            }
        }
        
        IEnumerator OnMouseUp()
        {
            GetComponent<NavMeshAgent>().baseOffset = 0;
            GetComponent<NavMeshAgent>().isStopped = false;
            // Debug.Log("up");
            ChangeState(states[StateEnum.Patrol]);
            yield return new WaitForFixedUpdate();
        }

        public int GetPatrolIndex() { return _wanderIndex; }
        public int GetWanderIndex() { return _wanderIndex; }
        public float GetTimer() { return _timer; }
        public float GetPatrolTime() { return _patrolTime; }
        public Transform GetCurrentNode() { return _nodes[_patrolIndex]; }
        public int GetNodeCount() { return _nodes.Count; }
        public float GetAgentDistance() { return _navMeshAgent.remainingDistance; }
        public void IncreasePatrolIndex(int number) { _patrolIndex += number; }
        public void ResetPatrolIndex() { _patrolIndex = 0; }
        public void IncreaseWanderIndex(int number) { _wanderIndex = number; }
        public void ResetWanderIndex() { _wanderIndex = 0; }
        public void AddToTimer(float amount) { _timer += amount; }
        public void ResetTimer() { _timer = 0; }
        public void SetAgentDestination(Vector3 destination) { _navMeshAgent.destination = destination; }
    }
}
