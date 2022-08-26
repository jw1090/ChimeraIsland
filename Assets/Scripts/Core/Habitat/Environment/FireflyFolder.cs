using System.Collections.Generic;
using UnityEngine;

public class FireflyFolder : MonoBehaviour
{
    [SerializeField] private List<Fireflies> _fireflies = new List<Fireflies>();

    public bool IsEmpty { get => _fireflies.Count == 0; }

    public void PlayFireflies()
    {
        foreach (var fireflies in _fireflies)
        {
            fireflies.PlayFireflies();
        }
    }

    public void StopFireflies()
    {
        foreach (var fireflies in _fireflies)
        {
            fireflies.StopFireflies();
        }
    }
}