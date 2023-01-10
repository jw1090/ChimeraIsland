using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] ScrollRect _scroll = null;
    [SerializeField] float _speed = 1.5f;
    private float _lastVal = 0f;
    private bool _scrolling = false;
    private bool _ignoreOnce = false;

    public void Initialize()
    {
        _scroll.onValueChanged.AddListener(StopScroll);
    }

    private void FixedUpdate()
    {
        if (_scrolling == false)
        {
            return;
        }

        _scroll.content.localPosition += new Vector3(0, _speed, 0);
    }

    private void StopScroll(Vector2 val)
    {
        if (_scrolling == false)
        {
            return;
        }

        if (_ignoreOnce == true)
        {
            StartCoroutine(StartDelayCoroutine());

            return;
        }

        if (val.y - _lastVal < -_speed / 1000 || (val.y - _lastVal < _speed - 0.1f && val.y - _lastVal > _speed / 1000))
        {
            _scrolling = false;
        }

        _lastVal = val.y;
    }

    private IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        _scrolling = true;
        _ignoreOnce = false;
    }

    public void StartScrolling()
    {
        _scroll.content.localPosition = new Vector3(_scroll.content.localPosition.x, 0.0f, _scroll.content.localPosition.z);
        _scrolling = true;
        _ignoreOnce = true;
    }
}