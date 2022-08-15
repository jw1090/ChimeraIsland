using UnityEngine;

public class FacilitySFX : MonoBehaviour
{
    [Header("Ambient SFX")]
    [SerializeField] private AudioSource _ambientSource = null;

    [Header("Facility SFX")]
    [SerializeField] private AudioSource _trainingSource = null;

    private AudioClip _ambientSFX = null;
    private AudioClip _trainingSFX = null;

    public AudioSource AmbientSource { get => _ambientSource; }
    public AudioSource TrainingSource { get => _trainingSource; }

    public void Initialize(Facility facility)
    {
        AudioManager audioManager = ServiceLocator.Get<AudioManager>();

        _ambientSFX = audioManager.GetFacilityAmbient(facility.Type);
        _trainingSFX = audioManager.GetFacilityTraining(facility.Type);
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