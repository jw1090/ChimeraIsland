using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tutorialText = null;
    [SerializeField] private Image _image = null;
    [SerializeField] private float _maxDuration = 3.0f;
    [SerializeField] private float _initialDelay = 1.0f;
    private Coroutine _fadeCoroutine = null;
    private float _progress = 0.0f;

    public void Initialize()
    {
        gameObject.SetActive(false);
    }

    public void CreateAlert(string alertText)
    {
        _tutorialText.text = alertText;
        gameObject.SetActive(true);
        UpdateAlpha(1.0f);

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(FadeAlertCoroutine());
    }

    private IEnumerator FadeAlertCoroutine()
    {
        _progress = 0.0f;

        yield return new WaitForSeconds(_initialDelay);

        while (_tutorialText.color.a > 0)
        {
            yield return new WaitForSeconds(0.0f);

            _progress += Time.deltaTime;
            UpdateAlpha(Mathf.Lerp(1.0f, 0.0f, _progress / _maxDuration));
        }
        gameObject.SetActive(false);

        _fadeCoroutine = null;
    }

    private void UpdateAlpha(float currentAlpha)
    {
        _tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, currentAlpha);
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, currentAlpha * 0.6f);
    }
}