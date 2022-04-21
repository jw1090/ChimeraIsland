using System.Collections;
using UnityEngine;

public class IslandFade : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] float delay = 0.5f;

    [Header("References")]
    [SerializeField] GameObject islandView;
    [SerializeField] GameObject crossfade;

    public void FadeOnClick()
    {
        crossfade.gameObject.SetActive(true);
        StartCoroutine(DisableDelay());
    }

    private IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(delay);
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