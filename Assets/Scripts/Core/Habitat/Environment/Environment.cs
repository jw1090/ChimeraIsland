using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private List<EnvironmentTier> _tiers = null;
    [SerializeField] private Portal _portal = null;

    public List<EnvironmentTier> Tiers { get => _tiers; }
    public Portal Portal { get => _portal; }
    public void Initialize()
    {
        foreach (var tier in _tiers)
        {
            tier.Initialize();
        }
    }
}