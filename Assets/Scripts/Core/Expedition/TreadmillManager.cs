using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillManager : MonoBehaviour
{
    [SerializeField] private GameObject _treadmill = null;
    [SerializeField] private Vector3 _rotation = new Vector3();
    [SerializeField] private float _speed = 0.0f;
    private ExpeditionManager _expeditionManager = null;

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }

    public TreadmillManager Initialize()
    {
        return this;
    }
    public void CheckRotation()
    {
        if(_expeditionManager.State == ExpeditionState.InProgress)
        {
            _treadmill.transform.Rotate(_rotation * _speed * Time.deltaTime);
        }
    }
}
