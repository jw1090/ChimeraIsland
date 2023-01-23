using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private int _tier = 1;

    [SerializeField] private Portal _portal = null;
    [SerializeField] private FireflyFolder _fireflyFolder = null;
    [SerializeField] private List<GameObject> _tierOneOnlyParts = null;
    [SerializeField] private List<GameObject> _tierOneAndTwoOnlyParts = null;
    [SerializeField] private List<GameObject> _tierTwoOnlyParts = null;
    [SerializeField] private List<GameObject> _tierTwoParts = null;
    [SerializeField] private List<GameObject> _tierThreeParts = null;
    [SerializeField] private GameObject _cliffMaterials = null;
    [SerializeField] private Material BrownCliff = null;
    [SerializeField] private Material GreenCliff = null;
    public Portal Portal { get => _portal; }


    public void Initialize()
    {
        _fireflyFolder.Initialize();
    }

    public void ToggleFireflies(bool toggleOn)
    {
        if (_fireflyFolder.IsEmpty == true)
        {
            return;
        }

        if (toggleOn == true)
        {
            _fireflyFolder.PlayFireflies();
        }
        else
        {
            _fireflyFolder.StopFireflies();
        }
    }

    public void SwitchTier(int tier)
    {
        _fireflyFolder.SwitchTier(tier);
        _tier = tier;
        Renderer[] cliffs = _cliffMaterials.transform.GetComponentsInChildren<Renderer>(true);
        switch (_tier)
        {
            case 1:
                for (int i = 0; i < cliffs.Length; i++)
                {
                    cliffs[i].material = BrownCliff;
                }
                foreach (var part in _tierTwoParts)
                {
                    part.SetActive(false);
                }
                foreach (var part in _tierThreeParts)
                {
                    part.SetActive(false);
                }
                foreach (var part in _tierOneOnlyParts)
                {
                    part.SetActive(true);
                }
                foreach (var part in _tierTwoOnlyParts)
                {
                    part.SetActive(false);
                }
                foreach (var part in _tierOneAndTwoOnlyParts)
                {
                    part.SetActive(true);
                }
                break;
            case 2:
                for (int i = 0; i < cliffs.Length; i++)
                {
                    cliffs[i].material = GreenCliff;
                }
                foreach (var part in _tierTwoParts)
                {
                    part.SetActive(true);
                }
                foreach (var part in _tierThreeParts)
                {
                    part.SetActive(false);
                }
                foreach (var part in _tierOneOnlyParts)
                {
                    part.SetActive(false);
                }
                foreach (var part in _tierTwoOnlyParts)
                {
                    part.SetActive(true);
                }
                foreach (var part in _tierOneAndTwoOnlyParts)
                {
                    part.SetActive(true);
                }
                break;
            case 3:
                for (int i = 0; i < cliffs.Length; i++)
                {
                    cliffs[i].material = GreenCliff;
                }
                foreach (var part in _tierTwoParts)
                {
                    part.SetActive(true);
                }
                foreach (var part in _tierThreeParts)
                {
                    part.SetActive(true);
                }
                foreach (var part in _tierOneOnlyParts)
                {
                    part.SetActive(false);
                }
                foreach (var part in _tierTwoOnlyParts)
                {
                    part.SetActive(false);
                }
                foreach (var part in _tierOneAndTwoOnlyParts)
                {
                    part.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}