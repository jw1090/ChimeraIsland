using UnityEngine;

public class Fireflies : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem = null;
    private ParticleSystem.EmissionModule _module;

    public void Initialize()
    {
        _module = _particleSystem.emission;
    }

    public void PlayFireflies()
    {
        _module.enabled = true;
    }

    public void StopFireflies()
    {
        _module.enabled = false;
    }
}