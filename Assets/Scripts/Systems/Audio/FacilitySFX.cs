using UnityEngine;

public class FacilitySFX : MonoBehaviour
{
    [Header("Ambient SFX")]
    [SerializeField] private AudioSource _ambientSource = null;
    [SerializeField] private AudioClip _ambientSFX = null;

    [Header("Facility SFX")]
    [SerializeField] private AudioSource _trainingSource = null;
    [SerializeField] private AudioClip _trainingSFX = null;

    public void Initialize()
    {
        _ambientSource.clip = _ambientSFX;
        _trainingSource.clip = _trainingSFX;
    }

    public void BuildSFX()
    {
        _ambientSource.Play();
    }

    public void PlaySFX()
    {
        _trainingSource.Play();
    }

    public void StopSFX()
    {
        _trainingSource.Stop();
    }
}
