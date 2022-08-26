using UnityEngine;

public class Fireflies : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem = null;

    public void PlayFireflies()
    {
        _particleSystem.Play();
    }

    public void StopFireflies()
    {
        _particleSystem.Stop();
    }
}