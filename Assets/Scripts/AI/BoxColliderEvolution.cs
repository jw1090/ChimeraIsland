using AI.Behavior;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderEvolution : MonoBehaviour
{
    public ChimeraBehavior _heldChimera;
    private BoxCollider box;

    void Awake()
    {
        box = GetComponent<BoxCollider>();
        _heldChimera = transform.parent.GetComponent<ChimeraBehavior>();
        _heldChimera.SetBoxCollider(box);
    }

    void Update()
    {

    }
}
