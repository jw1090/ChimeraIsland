using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ChimeraBehaviorState
{
    Patrol,
    Wander,
    Held,
    Training,
    Idle,
}

public class ChimeraBehavior : MonoBehaviour
{
    private HabitatUI _habitatUI;
    private Animator _animator = null;
    private Camera _mainCamera = null;
    private CameraUtil _cameraUtil = null;
    private Chimera _chimera = null;
    private ChimeraBaseState _currentState = null;
    private PatrolState _patrolState = null;
    private WanderState _wanderState = null;
    private HeldState _heldState = null;
    private TrainingState _trainingState = null;
    private IdleState _idleState = null;
    private NavMeshAgent _navMeshAgent = null;
    private List<Transform> _nodes = null;
    private bool _isActive = false;
    private int _patrolIndex = 0;
    private bool _stateEnabled = false;

    public BoxCollider BoxCollider { get => GetChimeraCollider(); }
    public Camera MainCamera { get => _mainCamera; }
    public CameraUtil CameraUtil { get => _cameraUtil; }
    public NavMeshAgent Agent { get => _navMeshAgent; }
    public Chimera Chimera { get => _chimera; }
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

        _habitatUI = ServiceLocator.Get<UIManager>().HabitatUI;
        _nodes = ServiceLocator.Get<HabitatManager>().CurrentHabitat.PatrolNodes;
        _cameraUtil = ServiceLocator.Get<CameraUtil>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _chimera = GetComponent<Chimera>();

        _navMeshAgent.enabled = false;

        _mainCamera = CameraUtil.CameraCO;

        _patrolState = new PatrolState();
        _wanderState = new WanderState();
        _heldState = new HeldState();
        _trainingState = new TrainingState();
        _idleState = new IdleState();

        _animator = _chimera.Animator;
    }

    public void StartAI()
    {
        ChangeState(ChimeraBehaviorState.Patrol);
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
            if (wasClicked == true)
            {
                ChangeState(ChimeraBehaviorState.Held);
                _habitatUI.ActivateChimeraPopUp(_chimera);
            }
            else
            {
                ChangeState(ChimeraBehaviorState.Patrol);
                _habitatUI.DeactivateChimeraPopUp();
            }
        }
    }

    public void ChangeState(ChimeraBehaviorState state)
    {
        ChimeraBaseState newState = DetermineState(state);

        if (_currentState == newState)
        {
            return; // Already current state, don't swap.
        }

        if (_currentState != null)
        {
            _currentState.Exit();
            _stateEnabled = false;
        }

        _currentState = newState;
        _currentState.Enter(this);
        _stateEnabled = true;
    }

    private ChimeraBaseState DetermineState(ChimeraBehaviorState state)
    {
        switch (state)
        {
            case ChimeraBehaviorState.Patrol:
                return _patrolState;
            case ChimeraBehaviorState.Wander:
                return _wanderState;
            case ChimeraBehaviorState.Held:
                return _heldState;
            case ChimeraBehaviorState.Training:
                return _trainingState;
            case ChimeraBehaviorState.Idle:
                return _idleState;
            default:
                Debug.LogError($"Invalid State [{state}], Please Fix!");
                return null;
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

    public void EvaluateParticlesOnEvolve()
    {
        if (_currentState == _patrolState || _currentState == _wanderState)
        {
            _chimera.CurrentEvolution.TogglePatrolParticles(true);
        }
        else if (_currentState == _idleState)
        {
            _chimera.CurrentEvolution.ToggleIdleParticles(true);
        }
    }

    public void StopParticles()
    {
        _chimera.CurrentEvolution.TogglePatrolParticles(false);
        _chimera.CurrentEvolution.ToggleIdleParticles(false);
    }

    public void ActivatePatrolParticles()
    {
        _chimera.CurrentEvolution.TogglePatrolParticles(true);
    }

    public void ActivateIdleParticles()
    {
        _chimera.CurrentEvolution.ToggleIdleParticles(true);
    }
}