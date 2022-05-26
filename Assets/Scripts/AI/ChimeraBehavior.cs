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
        private List<Transform> _nodes = null;
        private Animator _animator = null;
        private BoxCollider _boxCollider = null;
        private Camera _mainCamera = null;
        private ChimeraBaseState _currentState = null;
        private NavMeshAgent _navMeshAgent = null;
        private float _heightOffset = 1.2f;
        private bool _clicked = false;
        private bool _isActive = false;

        public Dictionary<StateEnum, ChimeraBaseState> States { get; private set; } = new Dictionary<StateEnum, ChimeraBaseState>();
        public Animator Animator { get => _animator; }
        public NavMeshAgent Agent { get => _navMeshAgent; }
        public BoxCollider BoxCollider { get => _boxCollider; }
        public CameraController CameraController { get; set; }
        public Vector3 TrainingPosition { get; set; } = Vector3.zero;
        public int PatrolIndex { get; private set; } = 0;
        public int WanderIndex { get; private set; } = 0;
        public float PatrolWaitTime { get; private set; } = 1.0f;
        public float Timer { get; private set; } = 0;

        public Transform GetCurrentNode() { return _nodes[Random.Range(0, _nodes.Count)]; }
        public int GetNodeCount() { return _nodes.Count; }
        public float GetAgentDistance() { return _navMeshAgent.remainingDistance; }
        public void SetAgentDestination(Vector3 destination) { _navMeshAgent.destination = destination; }
        public void IncreasePatrolIndex(int number) { PatrolIndex += number; }
        public void ResetPatrolIndex() { PatrolIndex = 0; }
        public void IncreaseWanderIndex(int number) { WanderIndex = number; }
        public void ResetWanderIndex() { WanderIndex = 0; }
        public void AddToTimer(float amount) { Timer += amount; }
        public void ResetTimer() { Timer = 0; }

        public void Initialize()
        {
            _nodes = ServiceLocator.Get<HabitatManager>().CurrentHabitat.PatrolNodes;
            CameraController = ServiceLocator.Get<CameraController>();
            _mainCamera = CameraController.CameraCO;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _boxCollider = GetComponent<BoxCollider>();

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_nodes[PatrolIndex].position);

            States.Add(StateEnum.Patrol, new PatrolState());
            States.Add(StateEnum.Wander, new WanderState());
            States.Add(StateEnum.Held, new HeldState());
            States.Add(StateEnum.Training, new TrainingState());
            _animator = GetComponentInChildren<Animator>();

            ChangeState(States[StateEnum.Patrol]);

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
            {
                ChangeState(States[StateEnum.Held]);
            }
        }

        public void WanderEnterHeld()
        {
            if (_clicked)
            {
                ChangeState(States[StateEnum.Held]);
            }
        }

        public void HeldEnterPatrol()
        {
            if (!_clicked)
            {
                ChangeState(States[StateEnum.Patrol]);
            }
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

        public int RandomPatrol()
        {
            int i = Random.Range(0, GetNodeCount());
            return i;
        }
    }
}