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
        [SerializeField] private LayerMask _chimeraLayer;
        [SerializeField] private float _heightOffset = 1.2f;
        [SerializeField] private bool _clicked = false;
        [SerializeField] private bool _isActive = false;

        [Header("References")]
        [SerializeField] private List<Transform> _nodes = null;
        [SerializeField] private NavMeshAgent _navMeshAgent = null;
        [SerializeField] private ChimeraBaseState _currentState = null;
        [SerializeField] private Camera _mainCamera = null;

        public Dictionary<StateEnum, ChimeraBaseState> States { get; private set; } = null;
        public int PatrolIndex { get; private set; } = 0;
        public int WanderIndex { get; private set; } = 0;
        public float PatrolWaitTime { get; private set; } = 1.0f;
        public float Timer { get; private set; } = 0;
        public Vector3 TrainingPosition { get; set; } = Vector3.zero;

        public Transform GetCurrentNode() { return _nodes[PatrolIndex]; }
        public int GetNodeCount() { return _nodes.Count; }
        public float GetAgentDistance() { return _navMeshAgent.remainingDistance; }
        public void SetAgentDestination(Vector3 destination) { _navMeshAgent.destination = destination; }
        public void IncreasePatrolIndex(int number) { PatrolIndex += number; }
        public void ResetPatrolIndex() { PatrolIndex = 0; }
        public void IncreaseWanderIndex(int number) { WanderIndex = number; }
        public void ResetWanderIndex() { WanderIndex = 0; }
        public void AddToTimer(float amount) { Timer += amount; }
        public void ResetTimer() { Timer = 0; }

        private MonoUtil _monoUtil = null;

        public void Initialize(Habitat habitat)
        {
            _monoUtil = ServiceLocator.Get<MonoUtil>();
            _monoUtil.StartCoroutine(InitializeAsync(habitat));
        }

        private IEnumerator InitializeAsync(Habitat habitat)
        {
            //var levelManager = ServiceLocator.Get<LevelManager>();
            //while(levelManager == null)
            //{
            //    levelManager = ServiceLocator.Get<LevelManager>();
            //}

            //while (!levelManager.IsInitialized)
            //{
            //    yield return null;
            //}

            _nodes = habitat.GetPatrolNodes();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            while (_navMeshAgent.isOnNavMesh == false)
            {
                yield return null;
            }

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_nodes[PatrolIndex].position);

            States = new Dictionary<StateEnum, ChimeraBaseState>();
            States.Add(StateEnum.Patrol, new PatrolState());
            States.Add(StateEnum.Wander, new WanderState());
            States.Add(StateEnum.Held, new HeldState());
            States.Add(StateEnum.Training, new TrainingState());

            ChangeState(States[StateEnum.Patrol]);

            _mainCamera = ServiceLocator.Get<CameraController>().CameraCO;
            _isActive = true;
        }

        private void Update()
        {
            if (_isActive == false || _currentState == null)
            {
                return;
            }
            _currentState.Update();

        }

        public void PatrolEnterHeld()
        {
            if (_clicked)
                ChangeState(States[StateEnum.Held]);
        }

        public void WanderEnterHeld()
        {
            if (_clicked)
                ChangeState(States[StateEnum.Held]);
        }

        public void HeldEnterPatrol()
        {
            if (!_clicked)
                ChangeState(States[StateEnum.Patrol]);
        }

        public void HeldExit()
        {
            _navMeshAgent.enabled = true;
        }

        public void ObjFollowMouse()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
            {
                transform.position = hit.point + (Vector3.up * _heightOffset);
                _navMeshAgent.enabled = false;
            }
        }

        public void ChangeState(ChimeraBaseState state)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }
            _currentState = state;
            _currentState.Enter(this);
        }

        public void ChimeraSelect(bool i)
        {
            _clicked = i;
        }
    }
}