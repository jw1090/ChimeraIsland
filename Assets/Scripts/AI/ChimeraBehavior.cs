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
        //Idle,
    }

    public class ChimeraBehavior : MonoBehaviour
    {
        private Animator _animator = null;
        private BoxCollider _boxCollider = null;
        private Camera _mainCamera = null;
        private CameraController _cameraController = null;
        private ChimeraBaseState _currentState = null;
        private Dictionary<StateEnum, ChimeraBaseState> _states = new Dictionary<StateEnum, ChimeraBaseState>();
        private List<Transform> _nodes = null;
        private NavMeshAgent _navMeshAgent = null;
        private bool _isActive = false;
        private float _timer = 0.0f;
        private int _patrolIndex = 0;
        private int _wanderIndex = 0;

        public BoxCollider BoxCollider { get => _boxCollider; }
        public Camera MainCamera { get => _mainCamera; }
        public CameraController CameraController { get => _cameraController; }
        public Dictionary<StateEnum, ChimeraBaseState> States { get => _states; }
        public NavMeshAgent Agent { get => _navMeshAgent; }
        public float Timer { get => _timer; }
        public int PatrolIndex { get => _patrolIndex; }
        public int WanderIndex { get => _wanderIndex; }

        public Vector3 TrainingPosition { get; set; } = Vector3.zero;
        public bool Clicked { get; set; } = false;

        public Transform GetCurrentNode() { return _nodes[Random.Range(0, _nodes.Count)]; }
        public int GetNodeCount() { return _nodes.Count; }
        public float GetAgentDistance() { return _navMeshAgent.remainingDistance; }
        public void SetAgentDestination(Vector3 destination) { _navMeshAgent.destination = destination; }
        public void IncreasePatrolIndex(int number) { _patrolIndex += number; }
        public void ResetPatrolIndex() { _patrolIndex = 0; }
        public void IncreaseWanderIndex(int number) { _wanderIndex = number; }
        public void ResetWanderIndex() { _wanderIndex = 0; }
        public void AddToTimer(float amount) { _timer += amount; }
        public void ResetTimer() { _timer = 0; }

        public void Initialize()
        {
            _nodes = ServiceLocator.Get<HabitatManager>().CurrentHabitat.PatrolNodes;
            _cameraController = ServiceLocator.Get<CameraController>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _boxCollider = GetComponent<Chimera>().BoxCollider;

            _mainCamera = CameraController.CameraCO;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_nodes[PatrolIndex].position);

            _states.Add(StateEnum.Patrol, new PatrolState());
            _states.Add(StateEnum.Wander, new WanderState());
            _states.Add(StateEnum.Held, new HeldState());
            _states.Add(StateEnum.Training, new TrainingState());
            //_states.Add(StateEnum.Idle, new IdleState());
            _animator = GetComponentInChildren<Animator>();

            ChangeState(_states[StateEnum.Patrol]);

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

        public void ChangeState(ChimeraBaseState state)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = state;
            _currentState.Enter(this);
        }

        public void HeldEnterCheck()
        {
            if (Clicked == true)
            {
                ChangeState(_states[StateEnum.Held]);
            }
        }

        public void EnterAnim(string _animationState)
        {
            if (_animator == null)
            {
                return;
            }

            _animator.Play(_animationState);
        }
    }
}