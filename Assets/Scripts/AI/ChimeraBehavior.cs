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
        private CameraUtil _cameraUtil = null;
        private Chimera _chimera = null;
        private ChimeraBaseState _currentState = null;
        private Dictionary<StateEnum, ChimeraBaseState> _states = new Dictionary<StateEnum, ChimeraBaseState>();
        private List<Transform> _nodes = null;
        private NavMeshAgent _navMeshAgent = null;
        private bool _isActive = false;
        private int _patrolIndex = 0;
        private bool _stateEnabled = false;

        public BoxCollider BoxCollider { get => GetChimeraCollider(); }
        public Camera MainCamera { get => _mainCamera; }
        public CameraUtil CameraUtil { get => _cameraUtil; }
        public Dictionary<StateEnum, ChimeraBaseState> States { get => _states; }
        public NavMeshAgent Agent { get => _navMeshAgent; }
        public bool StateEnabled { get => _stateEnabled; }
        public int PatrolIndex { get => _patrolIndex; }

        public bool Dropped { get; set; } = false;
        public bool WasClicked { get; set; } = false;

        public Transform GetCurrentNode() { return _nodes[Random.Range(0, _nodes.Count)]; }
        private BoxCollider GetChimeraCollider() { return _chimera.BoxCollider; }
        public int GetNodeCount() { return _nodes.Count; }
        public float GetAgentDistance() { return _navMeshAgent.remainingDistance; }
        public ChimeraType GetChimeraType() { return _chimera.ChimeraType; }

        public void SetAgentSpeed(float agentSpeed) { _navMeshAgent.speed = agentSpeed; }
        public void SetAgentDestination(Vector3 destination) { _navMeshAgent.destination = destination; }
        public void IncreasePatrolIndex() { _patrolIndex += 1; }
        public void ResetPatrolIndex() { _patrolIndex = 0; }

        public void Initialize()
        {
            ServiceLocator.Get<InputManager>().HeldStateChange += OnHeldStateChanged;

            _nodes = ServiceLocator.Get<HabitatManager>().CurrentHabitat.PatrolNodes;
            _cameraUtil = ServiceLocator.Get<CameraUtil>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _chimera = GetComponent<Chimera>();

            _navMeshAgent.enabled = false;

            _mainCamera = CameraUtil.CameraCO;

            _states.Add(StateEnum.Patrol, new PatrolState());
            _states.Add(StateEnum.Wander, new WanderState());
            _states.Add(StateEnum.Held, new HeldState());
            _states.Add(StateEnum.Training, new TrainingState());
            _states.Add(StateEnum.Idle, new IdleState());
            _animator = _chimera.Animator;

            ChangeState(_states[StateEnum.Patrol]);

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
            if (_isActive == false || _currentState == null || _stateEnabled == false)
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
                    if(_chimera.ReadyToEvolve == true)
                    {
                        _chimera.ActivateEvolve();
                    }
                    else
                    {
                        ChangeState(_states[StateEnum.Held]);
                    }
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
                _stateEnabled = false;
            }

            _currentState = state;
            _currentState.Enter(this);
            _stateEnabled = true;
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