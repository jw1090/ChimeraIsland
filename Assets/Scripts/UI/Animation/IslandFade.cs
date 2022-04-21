using System.Collections;
using UnityEngine;

public class IslandFade : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] float _delay = 0.5f;

    [Header("References")]
    [SerializeField] GameObject _islandView;
    [SerializeField] GameObject _crossfade;

    public void FadeOnClick()
    {
        _crossfade.gameObject.SetActive(true);
        StartCoroutine(DisableDelay());
    }

    private IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(_delay);
        _islandView.gameObject.SetActive(false);
    }

    public void StartCrossfadeDisable(float fadeDisableDelay)
    {
        StartCoroutine(DisableCrossfade(fadeDisableDelay));
    }

    private IEnumerator DisableCrossfade(float fadeDisableDelay)
    {
        yield return new WaitForSeconds(fadeDisableDelay);

        _crossfade.SetActive(false);
    }
}