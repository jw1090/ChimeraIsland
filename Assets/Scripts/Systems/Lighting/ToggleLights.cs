using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLights : MonoBehaviour
{
    [SerializeField] private Light _dayLight = null;
    [SerializeField] private Light _nightLight = null;
    private void Start()
    {
        _dayLight.gameObject.SetActive(true);
        _nightLight.gameObject.SetActive(false);
    }

    private void Update()
    {

    }

}
