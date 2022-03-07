using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNodeManager : MonoBehaviour
{
    [SerializeField] List<Transform> patrolNodes;

    public List<Transform> GetPatrolNodes() { return patrolNodes; }
}