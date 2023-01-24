using System.Collections.Generic;
using UnityEngine;

public class FireflyFolder : MonoBehaviour
{
    [SerializeField] private List<Fireflies> _fireflies = new List<Fireflies>();

    public bool IsEmpty { get => _fireflies.Count == 0; }
    private int _tier = 1;

    public void Initialize()
    {
        foreach (var fireflies in _fireflies)
        {
            fireflies.Initialize();
        }
    }

    public void SwitchTier(int tier)
    {
        _tier = tier;
    }

    public void PlayFireflies()
    {
        int counter;
        switch (_tier)
        {
            case 2:
                counter = _fireflies.Count - 2;
                break;
            case 3:
                counter = _fireflies.Count;
                break;
            default:
                Debug.LogError($"Habitat Tier is not valid [{_tier}] please change!");
                return;
        }

        foreach (var fireflies in _fireflies)
        {
            if (counter <= 0) return;
            fireflies.PlayFireflies();
            counter--;
        }
    }

    public void StopFireflies()
    {
        int counter;
        switch (_tier)
        {
            case 2:
                counter = _fireflies.Count - 2;
                break;
            case 3:
                counter = _fireflies.Count;
                break;
            default:
                Debug.LogError($"Habitat Tier is not valid [{_tier}] please change!");
                return;
        }

        foreach (var fireflies in _fireflies)
        {
            if (counter <= 0) return;
            fireflies.StopFireflies();
            counter--;
        }
    }
}