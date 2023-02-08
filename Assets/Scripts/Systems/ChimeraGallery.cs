using System.Collections;
using UnityEngine;

public class ChimeraGallery : MonoBehaviour
{
    [SerializeField] private CameraUtil _cameraUtil = null;

    [Header("Camera Nodes")]
    [SerializeField] private Transform _cameraTempleNode = null;
    [SerializeField] private Transform _galleryNodeSmall = null;
    [SerializeField] private Transform _galleryNodeMedium = null;
    [SerializeField] private Transform _galleryNodeLarge = null;

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

    private StartingChimeraInfo _chimeraInfo = null;
    private bool _active = false;
    private TempleUI _templeUI;
    private GameObject _currentChimera = null;
    private Vector3 mPrevMousePos = Vector3.zero;
    private Vector3 mPosDelta = Vector3.zero;

    public void Initialize()
    {
        _chimeraInfo = ServiceLocator.Get<UIManager>().TempleUI.ChimeraInfo;
        _chimeraInfo.Initialize();
    }

    public void SetTempleUI(TempleUI templeUI)
    {
        _templeUI = templeUI;
    }

    public IEnumerator StartGallery(ChimeraType chimeraType)
    {
        _templeUI.ShowGalleryUIState();
        GetCurrentChimera(chimeraType);
        _currentChimera.SetActive(true);
        switch (chimeraType)
        {
            case ChimeraType.A3:
                _cameraUtil.gameObject.transform.position = _galleryNodeMedium.position;
                _cameraUtil.gameObject.transform.rotation = _galleryNodeMedium.rotation;
                break;
            case ChimeraType.C2:
            case ChimeraType.C3:
                _cameraUtil.gameObject.transform.position = _galleryNodeLarge.position;
                _cameraUtil.gameObject.transform.rotation = _galleryNodeLarge.rotation;
                break;
            default:
                _cameraUtil.gameObject.transform.position = _galleryNodeSmall.position;
                _cameraUtil.gameObject.transform.rotation = _galleryNodeSmall.rotation;
                break;
        }
        _chimeraInfo.LoadChimeraData(_currentChimera.GetComponent<EvolutionLogic>());
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0) == true);
        _active = true;
    }


    public void ExitGallery()
    {
        _active = false;
        _currentChimera.transform.localRotation = Quaternion.identity;
        _currentChimera.SetActive(false);
        _cameraUtil.gameObject.transform.position = _cameraTempleNode.position;
        _cameraUtil.gameObject.transform.rotation = _cameraTempleNode.rotation;
    }

    private void Update()
    {
        if (_active == false)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            mPosDelta = Input.mousePosition - mPrevMousePos;
            _currentChimera.transform.Rotate(_cameraUtil.gameObject.transform.up, -Vector3.Dot(mPosDelta, _cameraUtil.transform.right), Space.World);
            _currentChimera.transform.localEulerAngles = new Vector3(0.0f, _currentChimera.transform.localEulerAngles.y, 0.0f);
        }
        mPrevMousePos = Input.mousePosition;
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