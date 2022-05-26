using System.Collections.Generic;
using UnityEngine;

public class PatrolNodes : MonoBehaviour
{
    [SerializeField] private List<Transform> _nodes;

    private bool _initialized = false;

    public bool Initialized { get => _initialized; }

    public void Initialize()
    {
        foreach(Transform child in transform)
        {
            _nodes.Add(child);
        }
        _initialized = true;
    }

    public List<Transform> GetNodes() { return _nodes; }
}