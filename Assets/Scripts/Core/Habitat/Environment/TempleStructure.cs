using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TempleStructure : MonoBehaviour
{
    [SerializeField] private StatefulObject _states = null;
    [SerializeField] private Transform _cameraTransitionNode = null;
    [SerializeField] private GameObject _templeVFX = null;
    private bool _isCompleted = false;

    public Transform CameraTransitionNode { get => _cameraTransitionNode; }
    public bool IsCompleted { get => _isCompleted; }

    public IEnumerator BuildVFX(CameraUtil cameraUtil)
    {
        bool facilityBuilt = false;

        yield return new WaitUntil(() => cameraUtil.InTransition == false);

        float stopwatch = 0.0f;
        _templeVFX.SetActive(true);
        while (stopwatch < 5)
        {
            if (facilityBuilt == false && stopwatch >= 3)
            {
                facilityBuilt = true;
                Build();
            }
            stopwatch += Time.deltaTime;
            yield return null;
        }
        _templeVFX.SetActive(false);
    }

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