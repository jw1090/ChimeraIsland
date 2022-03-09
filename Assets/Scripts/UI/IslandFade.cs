using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandFade : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] float delay = 0.2f;

    [Header("References")]
    [SerializeField] GameObject crossfade;

    public void FadeOnClick()
    {
        crossfade.gameObject.SetActive(true);
        StartCoroutine(DisableDelay());
        StartCoroutine(GameManager.Instance.DisableCrossfade(crossfade));
    }

    private IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
}