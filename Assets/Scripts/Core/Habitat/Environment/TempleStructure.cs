using UnityEngine;

public class TempleStructure : MonoBehaviour
{
    [SerializeField] private StatefulObject _states = null;
    [SerializeField] private Transform _cameraTransitionNode = null;
    private bool _isCompleted = false;

    public Transform CameraTransitionNode { get => _cameraTransitionNode; }
    public bool IsCompleted { get => _isCompleted; }

    public void Build()
    {
        _states.SetState("Completed Temple", true);
        _isCompleted = true;
    }

    public void ResetTemple()
    {
        _states.SetState("Ruined Temple", true);
        _isCompleted = false;
    }
}