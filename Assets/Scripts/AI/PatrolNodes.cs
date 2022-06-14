using System.Collections.Generic;
using UnityEngine;

public class PatrolNodes : MonoBehaviour
{
    private List<Transform> _nodes = new List<Transform>();
    private bool _initialized = false;

    public List<Transform> Nodes { get => _nodes; }
    public bool Initialized { get => _initialized; }

    public void Initialize()
    {
        foreach (Transform child in transform)
        {
            _nodes.Add(child);
        }
        _initialized = true;
    }
}