using System.Collections.Generic;
using UnityEngine;

public class PatrolNodes : MonoBehaviour
{
    [SerializeField] private List<Transform> _nodes;

    public void Initialize()
    {
        foreach(Transform child in transform)
        {
            _nodes.Add(child);
        }
    }

    public List<Transform> GetNodes() { return _nodes; }
}