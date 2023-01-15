using System.Collections;
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
        StartCoroutine(FireFlyFade(true));
    }

    public void StopFireflies()
    {
        _module.enabled = false;
        StartCoroutine(FireFlyFade(false));
    }

    private IEnumerator FireFlyFade(bool fadeIn)
    {
        float timer = 0.0f;
        float duration = 5.0f;
        float startingIntensity = _light.intensity;

        if (fadeIn == true)
        {
            _light.gameObject.SetActive(true);
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float progress = timer / duration;

            float newIntensity;
            if (fadeIn == true)
            {
                newIntensity = Mathf.Lerp(startingIntensity, 40.0f, progress);
            }
            else
            {
                newIntensity = Mathf.Lerp(startingIntensity, 0.0f, progress);
            }
            _light.intensity = newIntensity;

            yield return null;
        }

        if (fadeIn == false)
        {
            _light.gameObject.SetActive(false);
        }
    }
}