using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private ChimeraType chimeraType = ChimeraType.None;

    [Header("Stat Growth")]
    [SerializeField] private int agilityGrowth = 1;
    
    [Header("Evolution Info")]
    [SerializeField] private Chimera[] evolutionPaths;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
