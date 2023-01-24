using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Environment : MonoBehaviour
{

    [Header("Resources")]
    [SerializeField] private Portal _portal = null;
    [SerializeField] private FireflyFolder _fireflyFolder = null;

    [Header("Enviornmental Objects")]
    [SerializeField] private GameObject _tierOneOnlyGO = null;
    [SerializeField] private GameObject _tierOneAndTwoOnlyGO = null;
    [SerializeField] private GameObject _tierTwoOnlyGO = null;
    [SerializeField] private GameObject _tierTwoAndThreeGO = null;
    [SerializeField] private GameObject _tierThreeOnlyGO = null;
    [SerializeField] private List<Renderer> _cliffRenderersList = null;
    [SerializeField] private List<Renderer> _waterfallCliffRenderersList = null;

    [Header("Materials")]
    [SerializeField] private Material _brownCliff = null;
    [SerializeField] private Material _greenCliff = null;

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
        switch (tier)
        {
            case 0:
                for (int i = 0; i < _waterfallCliffRenderersList.Count; i++)
                {
                    _waterfallCliffRenderersList[i].material = _brownCliff;
                }
                break;
            case 1:
            case 2:
            case 3:
                for (int i = 0; i < _waterfallCliffRenderersList.Count; i++)
                {
                    _waterfallCliffRenderersList[i].material = _greenCliff;
                }
                break;
            default:
                Debug.LogError($"facilityTier is not valid [{tier}] please change!");
                break;
        }
    }

    public void SwitchTier(int tier)
    {
        _fireflyFolder.SwitchTier(tier);
        switch (tier)
        {
            case 1:
                for (int i = 0; i < _cliffRenderersList.Count; i++)
                {
                    _cliffRenderersList[i].material = _brownCliff;
                }
                _tierOneOnlyGO.SetActive(true);
                _tierOneAndTwoOnlyGO.SetActive(true);
                _tierTwoOnlyGO.SetActive(false);
                _tierTwoAndThreeGO.SetActive(false);
                _tierThreeOnlyGO.SetActive(false);
                break;
            case 2:
                for (int i = 0; i < _cliffRenderersList.Count; i++)
                {
                    _cliffRenderersList[i].material = _greenCliff;
                }
                _tierOneOnlyGO.SetActive(false);
                _tierOneAndTwoOnlyGO.SetActive(true);
                _tierTwoOnlyGO.SetActive(true);
                _tierTwoAndThreeGO.SetActive(true);
                _tierThreeOnlyGO.SetActive(false);
                break;
            case 3:
                for (int i = 0; i < _cliffRenderersList.Count; i++)
                {
                    _cliffRenderersList[i].material = _greenCliff;
                }
                _tierOneOnlyGO.SetActive(false);
                _tierOneAndTwoOnlyGO.SetActive(false);
                _tierTwoOnlyGO.SetActive(false);
                _tierTwoAndThreeGO.SetActive(true);
                _tierThreeOnlyGO.SetActive(true);
                break;
            default:
                Debug.LogError($"facilityTier is not valid [{tier}] please change!");
                break;
        }
    }
}