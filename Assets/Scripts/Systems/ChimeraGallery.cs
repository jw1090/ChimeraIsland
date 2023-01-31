using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraGallery : MonoBehaviour
{
    [SerializeField] private CameraUtil _cameraUtil = null;

    [Header("Camera Nodes")]
    [SerializeField] private Transform _cameraTempleNode = null;
    [SerializeField] private Transform _cameraGalleryNode = null;

    [Header("ChimeraModels")]
    [SerializeField] private GameObject _chimeraA = null;
    [SerializeField] private GameObject _chimeraA1 = null;
    [SerializeField] private GameObject _chimeraA2 = null;
    [SerializeField] private GameObject _chimeraA3 = null;
    [SerializeField] private GameObject _chimeraB = null;
    [SerializeField] private GameObject _chimeraB1 = null;
    [SerializeField] private GameObject _chimeraB2 = null;
    [SerializeField] private GameObject _chimeraB3 = null;
    [SerializeField] private GameObject _chimeraC = null;
    [SerializeField] private GameObject _chimeraC1 = null;
    [SerializeField] private GameObject _chimeraC2 = null;
    [SerializeField] private GameObject _chimeraC3 = null;

    private TempleUI _templeUI;
    private GameObject _currentChimera = null;

    public void Initialize()
    {

    }

    public void SetTempleUI(TempleUI templeUI)
    {
        _templeUI = templeUI;
    }

    public void StartGallery(ChimeraType chimeraType)
    {
        _templeUI.ShowGalleryUIState();
        GetCurrentChimera(chimeraType);
        _currentChimera.SetActive(true);
        _cameraUtil.gameObject.transform.position = _cameraGalleryNode.position;
        _cameraUtil.gameObject.transform.rotation = _cameraGalleryNode.rotation;
    }


    public void ExitGallery()
    {
        _currentChimera.SetActive(false);
        _cameraUtil.gameObject.transform.position = _cameraTempleNode.position;
        _cameraUtil.gameObject.transform.rotation = _cameraTempleNode.rotation;
    }

    private void GetCurrentChimera(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                _currentChimera = _chimeraA;
                break;
            case ChimeraType.A1:
                _currentChimera = _chimeraA1;
                break;
            case ChimeraType.A2:
                _currentChimera = _chimeraA2;
                break;
            case ChimeraType.A3:
                _currentChimera = _chimeraA3;
                break;
            case ChimeraType.B:
                _currentChimera = _chimeraB;
                break;
            case ChimeraType.B1:
                _currentChimera = _chimeraB1;
                break;
            case ChimeraType.B2:
                _currentChimera = _chimeraB2;
                break;
            case ChimeraType.B3:
                _currentChimera = _chimeraB3;
                break;
            case ChimeraType.C:
                _currentChimera = _chimeraC;
                break;
            case ChimeraType.C1:
                _currentChimera = _chimeraC1;
                break;
            case ChimeraType.C2:
                _currentChimera = _chimeraC2;
                break;
            case ChimeraType.C3:
                _currentChimera = _chimeraC3;
                break;
            default:
                Debug.LogError($"chimeraType is not valid [{chimeraType}] please change!");
                break;
        }
    }
}
