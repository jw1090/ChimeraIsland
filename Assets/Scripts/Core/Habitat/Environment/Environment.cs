using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private Portal _portal = null;
    [SerializeField] private FireflyFolder _fireflyFolder = null;
    [SerializeField] private GameObject _tierOneOnlyParts = null;
    [SerializeField] private GameObject _tierOneAndTwoOnlyParts = null;
    [SerializeField] private GameObject _tierTwoOnlyParts = null;
    [SerializeField] private GameObject _tierTwoAndThreeParts = null;
    [SerializeField] private GameObject _tierThreeOnlyParts = null;
    [SerializeField] private GameObject _cliffMaterials = null;
    [SerializeField] private GameObject _waterfallMaterials = null;
    [SerializeField] private Material _brownCliff = null;
    [SerializeField] private Material _greenCliff = null;
    [SerializeField] private Material _greenCliffWaterfall = null;
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

    public void SwitchWaterfallTier(int tier)
    {
        Renderer[] cliffs = _waterfallMaterials.transform.GetComponentsInChildren<Renderer>(true);
        switch (tier)
        {
            case 0:
                for (int i = 0; i < cliffs.Length; i++)
                {
                    cliffs[i].material = _brownCliff;
                }
                break;
            case 1:
            case 2:
            case 3:
                for (int i = 0; i < cliffs.Length; i++)
                {
                    cliffs[i].material = _greenCliffWaterfall;
                }
                break;
            default:
                Debug.LogWarning($"facilityTier is not valid [{tier}] please change!");
                break;
        }

    }

    public void SwitchTier(int tier)
    {
        _fireflyFolder.SwitchTier(tier);
        Renderer[] cliffs = _cliffMaterials.transform.GetComponentsInChildren<Renderer>(true);
        switch (tier)
        {
            case 1:
                for (int i = 0; i < cliffs.Length; i++)
                {
                    cliffs[i].material = _brownCliff;
                }
                _tierOneOnlyParts.SetActive(true);
                _tierOneAndTwoOnlyParts.SetActive(true);
                _tierTwoOnlyParts.SetActive(false);
                _tierTwoAndThreeParts.SetActive(false);
                _tierThreeOnlyParts.SetActive(false);
                break;
            case 2:
                for (int i = 0; i < cliffs.Length; i++)
                {
                    cliffs[i].material = _greenCliff;
                }
                _tierOneOnlyParts.SetActive(false);
                _tierOneAndTwoOnlyParts.SetActive(true);
                _tierTwoOnlyParts.SetActive(true);
                _tierTwoAndThreeParts.SetActive(true);
                _tierThreeOnlyParts.SetActive(false);
                break;
            case 3:
                for (int i = 0; i < cliffs.Length; i++)
                {
                    cliffs[i].material = _greenCliff;
                }
                _tierOneOnlyParts.SetActive(false);
                _tierOneAndTwoOnlyParts.SetActive(false);
                _tierTwoOnlyParts.SetActive(false);
                _tierTwoAndThreeParts.SetActive(true);
                _tierThreeOnlyParts.SetActive(true);
                break;
            default:
                Debug.LogWarning($"facilityTier is not valid [{tier}] please change!");
                break;
        }
    }
}