using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatManager : MonoBehaviour
{
    [SerializeField] private List<int> _essenceGainPerHabitat = new List<int>();
    public HabitatManager Initialize()
    {
        return this;
    }
}