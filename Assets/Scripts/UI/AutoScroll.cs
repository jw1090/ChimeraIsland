using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] ScrollRect _scroll = null;
    [SerializeField] float _speed = 1.5f;
    private float _lastVal = 0f;
    private bool _scrolling = false;

    public void Initialize()
    {
        _scroll.onValueChanged.AddListener(StopScroll);
    }

    private void StopScroll(Vector2 val)
    {
        Debug.Log($"Speed: {val.y - _lastVal}");
        if (val.y - _lastVal < -_speed/1000 || (val.y - _lastVal < _speed - .1f && val.y - _lastVal > _speed / 1000))
        {
            _scrolling = false;
        }
        _lastVal = val.y;
    }

    public void StartScrolling()
    {
        _scrolling = true;
    }

    private void FixedUpdate()
    {
        if (gameObject.activeInHierarchy == false || _scrolling == false) return;
        _scroll.content.localPosition += new Vector3(0,_speed,0);
    }
}
