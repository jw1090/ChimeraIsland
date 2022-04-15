using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PatrolNodes patrolNodes = null;
    private void Awake()
    {
        patrolNodes.Initialize();
    }
}