using System.Collections;
using UnityEngine;

public class IslandFade : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] float _delay = 0.5f;

    [Header("References")]
    [SerializeField] private GameObject islandView = null;
    [SerializeField] private GameObject crossfade = null;

    public void FadeOnClick()
    {
        crossfade.gameObject.SetActive(true);
        StartCoroutine(DisableDelay());
    }

    private IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(_delay);
        islandView.gameObject.SetActive(false);
    }

    public void StartCrossfadeDisable(float fadeDisableDelay)
    {
        StartCoroutine(DisableCrossfade(fadeDisableDelay));
    }

    private IEnumerator DisableCrossfade(float fadeDisableDelay)
    {
        yield return new WaitForSeconds(fadeDisableDelay);

        crossfade.SetActive(false);
    }
}