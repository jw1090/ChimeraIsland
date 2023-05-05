using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TempleStructure : MonoBehaviour
{
    [SerializeField] private StatefulObject _states = null;
    [SerializeField] private Transform _cameraTransitionNode = null;
    [SerializeField] private GameObject _templeVFX = null;
    private bool _isCompleted = false;
    private AudioManager _audioManager = null;
    private InputManager _inputManager = null;

    public void Initialize()
    {
        _audioManager = ServiceLocator.Get<AudioManager>();
        _inputManager = ServiceLocator.Get<InputManager>();
    }

    public Transform CameraTransitionNode { get => _cameraTransitionNode; }
    public bool IsCompleted { get => _isCompleted; }

    public IEnumerator BuildVFX(CameraUtil cameraUtil)
    {
        bool facilityBuilt = false;

        yield return new WaitUntil(() => cameraUtil.InTransition == false);
        _inputManager.SetInTransition(true);

        float stopwatch = 0.0f;
        _templeVFX.SetActive(true);
        bool sfxStartPlayed = false;
        while (stopwatch < 5)
        {
            if (sfxStartPlayed == false && stopwatch >= 1.0f)
            {
                _audioManager.PlaySFX(EnvironmentSFXType.FacilityBuildStart);
                sfxStartPlayed = true;
            }
            if (facilityBuilt == false && stopwatch >= 3)
            {
                _audioManager.PlaySFX(EnvironmentSFXType.FacilityBuildEnd);
                facilityBuilt = true;
                Build();
            }
            stopwatch += Time.deltaTime;
            yield return null;
        }
        _templeVFX.SetActive(false);
        _inputManager.SetInTransition(false);
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