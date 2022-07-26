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
        Idle,
    }

    public class ChimeraBehavior : MonoBehaviour
    {
        private Animator _animator = null;
        private Camera _mainCamera = null;
        private CameraController _cameraController = null;

        private Chimera _chimera = null;
        private ChimeraBaseState _currentState = null;
        private Dictionary<StateEnum, ChimeraBaseState> _states = new Dictionary<StateEnum, ChimeraBaseState>();
        private List<Transform> _nodes = null;
        private NavMeshAgent _navMeshAgent = null;
        private bool _isActive = false;
        private int _patrolIndex = 0;

        public BoxCollider BoxCollider { get => GetChimeraCollider(); }
        public Camera MainCamera { get => _mainCamera; }
        public CameraController CameraController { get => _cameraController; }
        public Dictionary<StateEnum, ChimeraBaseState> States { get => _states; }
        public NavMeshAgent Agent { get => _navMeshAgent; }
        public int PatrolIndex { get => _patrolIndex; }

        public bool WasClicked { get; set; } = false;
        public bool Dropped { get; set; } = false;

        public Transform GetCurrentNode() { return _nodes[Random.Range(0, _nodes.Count)]; }
        private BoxCollider GetChimeraCollider() { return _chimera.BoxCollider; }
        public int GetNodeCount() { return _nodes.Count; }
        public float GetAgentDistance() { return _navMeshAgent.remainingDistance; }
        public void SetAgentDestination(Vector3 destination) { _navMeshAgent.destination = destination; }
        public void IncreasePatrolIndex() { _patrolIndex += 1; }
        public void ResetPatrolIndex() { _patrolIndex = 0; }


        public void Initialize()
        {
            ServiceLocator.Get<InputManager>().HeldStateChange += OnHeldStateChanged;

            _nodes = ServiceLocator.Get<HabitatManager>().CurrentHabitat.PatrolNodes;
            _cameraController = ServiceLocator.Get<CameraController>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.enabled = false;
            _chimera = GetComponent<Chimera>();

            _mainCamera = CameraController.CameraCO;

            _states.Add(StateEnum.Patrol, new PatrolState());
            _states.Add(StateEnum.Wander, new WanderState());
            _states.Add(StateEnum.Held, new HeldState());
            _states.Add(StateEnum.Training, new TrainingState());
            _states.Add(StateEnum.Idle, new IdleState());
            _animator = _chimera.Animator;

            ChangeState(_states[StateEnum.Idle]);

            _isActive = true;
        }

        public void EnableNavAgent()
        {
            _navMeshAgent.enabled = true;
        }

        private void OnDestroy()
        {
            var inputManager = ServiceLocator.Get<InputManager>();
            if (inputManager != null)
            {
                inputManager.HeldStateChange -= OnHeldStateChanged;
            }
        }

        private void Update()
        {
            if (_isActive == false || _currentState == null)
            {
                return;
            }
            _currentState.Update();
        }

        private void OnHeldStateChanged(bool wasClicked, int id)
        {
            if (transform.GetHashCode() == id)
            {
                if (wasClicked)
                {
                    ChangeState(_states[StateEnum.Held]);
                }
                else
                {
                    ChangeState(_states[StateEnum.Patrol]);
                }
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

        public void HeldEnterCheck()
        {
            if (WasClicked == true)
            {
                ChangeState(_states[StateEnum.Held]);
            }
        }

        public void EnterAnim(string _animationState)
        {
            _animator = _chimera.Animator;

            _animator.SetBool(_animationState, true);
        }

        public void ExitAnim(string _animationState)
        {
            if (_animator == null)
            {
                return;
            }

            _animator.SetBool(_animationState, false);
        }
    }
}