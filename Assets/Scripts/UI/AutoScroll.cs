using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] ScrollRect _scroll = null;
    [SerializeField] float _speed = 0.2f;
    private bool _scrolling = false;

    public void Scroll(bool scrolling)
    {
        _scrolling = scrolling;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy == false || _scrolling == false) return;
        _scroll.content.localPosition += new Vector3(0,_speed,0);
    }
}
