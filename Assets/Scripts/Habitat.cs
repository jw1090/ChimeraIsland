using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private bool isActive = false;

    [Header("Stat Bonus")]
    [SerializeField] private int baseExperience = 1;

    [Header("Tick Info")]
    [SerializeField] private float tickDuration;

    [Header("Resources")]
    [SerializeField] private float currentEssence;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}