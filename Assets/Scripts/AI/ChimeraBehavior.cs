using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraBehavior : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private BehaviorType behaviorType = BehaviorType.None;

    public BehaviorType GetBehaviorType() { return behaviorType; }
}