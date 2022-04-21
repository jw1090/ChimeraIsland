using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Behavior
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
        [Header("General Info")]
        [SerializeField] bool _isActive = false;

        [Header("References")]
        [SerializeField] private List<Transform> _nodes;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private ChimeraBaseState currentState;

        private float _heightOffset = 1.2f;
        private int _patrolIndex = 0;
        private int _wanderIndex = 0;
        private float _timer = 0; // Timer
        private float _patrolTime = 1f; // Wait time
        private bool _isClick;
        private Transform _dragGameObject;
        private LayerMask _canDrag;
        private Vector3 _targetScreenPoint;
        private Camera _mainCamera = null;
        Ray _ray;
        RaycastHit _hit;

        // Mapping of state to state object instance.
        public Dictionary<StateEnum, ChimeraBaseState> states = new Dictionary<StateEnum, ChimeraBaseState>();

        public void Initialize()
        {
            _mainCamera = Camera.main;
            _patrolIndex = 0;
            _wanderIndex = 0;
            _timer = 0;
            _nodes = GetComponent<Chimera>().GetHabitat().GetPatrolNodes();

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_nodes[_patrolIndex].position);

            // Add the corresponding state and its object to the dictionary.
            states.Add(StateEnum.Patrol, new PatrolState());
            states.Add(StateEnum.Wander, new WanderState());
            states.Add(StateEnum.Held, new HeldState());

            _isActive = true;
            _canDrag = 1 << LayerMask.NameToLayer("Chimera");

            ChangeState(states[StateEnum.Patrol]);
        }

        void Update()
        {
            if(_isActive)
            {
                currentState.Update();
            }
            CheckGameObject();
            if (Input.GetMouseButton(0) && _isClick)
            {
                _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                {
                    transform.position = _hit.point + (Vector3.up * _heightOffset);
                    _navMeshAgent.enabled = false;
                    ChangeState(states[StateEnum.Held]);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                _navMeshAgent.enabled = true;
                ChangeState(states[StateEnum.Patrol]);
            }
        }

        bool CheckGameObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100f, _canDrag))
            {
                _isClick = true;
                _dragGameObject = hitInfo.collider.gameObject.transform;
                _targetScreenPoint = _mainCamera.WorldToScreenPoint(_dragGameObject.position);
                return true;
            }
            return false;
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
