using UnityEngine;

public class Fireflies : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem = null;
    [SerializeField] private Light _light = null;
    private ParticleSystem.EmissionModule _module;

    public void Initialize()
    {
        _module = _particleSystem.emission;
        _light.gameObject.SetActive(false);
    }

    public void PlayFireflies()
    {
        _module.enabled = true;
        _light.gameObject.SetActive(true);
    }

    public void StopFireflies()
    {
        _module.enabled = false;
        _light.gameObject.SetActive(false);
    }
}