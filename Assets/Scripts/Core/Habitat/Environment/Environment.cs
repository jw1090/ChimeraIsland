using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private List<EnvironmentTier> _tiers = null;

    public List<EnvironmentTier> Tiers { get => _tiers; }
}