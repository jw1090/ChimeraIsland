using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChimeraGallery : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;

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

    private InputManager _inputManager = null;
    private ChimeraInfoUI _chimeraInfo = null;
    private bool _active = false;
    private GameObject _currentChimera = null;
    private Vector3 _prevMousePos = Vector3.zero;
    private Vector3 _posDelta = Vector3.zero;
    private string _currentAnim = "";
    private Animator _currentAnimator = null;
    private bool _clickedDownOnChimera = false;

    public void Initialize()
    {
        _chimeraInfo = ServiceLocator.Get<UIManager>().TempleUI.GalleryChimeraInfo;
        _inputManager = ServiceLocator.Get<InputManager>();
    }

    public void EnterGallery(ChimeraType chimeraType)
    {
        StartCoroutine(EnterGalleryCoroutine(chimeraType));
    }

    private IEnumerator EnterGalleryCoroutine(ChimeraType chimeraType)
    {
        LoadCurrentChimera(chimeraType);
        _currentChimera.SetActive(true);
        _currentAnimator = _currentChimera.GetComponent<Animator>();

        EvolutionLogic evolutionLogic = _currentChimera.GetComponent<EvolutionLogic>();
        _chimeraInfo.LoadChimeraData(evolutionLogic);

        switch (chimeraType)
        {
            case ChimeraType.A3:
                _camera.gameObject.transform.position = _galleryNodeMedium.position;
                _camera.gameObject.transform.rotation = _galleryNodeMedium.rotation;
                break;
            case ChimeraType.C2:
            case ChimeraType.C3:
                _camera.gameObject.transform.position = _galleryNodeLarge.position;
                _camera.gameObject.transform.rotation = _galleryNodeLarge.rotation;
                break;
            default:
                _camera.gameObject.transform.position = _galleryNodeSmall.position;
                _camera.gameObject.transform.rotation = _galleryNodeSmall.rotation;
                break;
        }

        yield return new WaitUntil(() => Input.GetMouseButtonUp(0) == true);

        _active = true;
    }

    public void ExitGallery()
    {
        _active = false;
        _currentChimera.transform.localRotation = Quaternion.identity;
        _currentChimera.SetActive(false);
        _camera.transform.position = _cameraTempleNode.position;
        _camera.transform.rotation = _cameraTempleNode.rotation;
    }

    private void Update()
    {
        if (_active == false)
        {
            return;
        }
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (_clickedDownOnChimera == false && _inputManager.GetCursorSprite() == CursorType.Dragable)
            {
                _clickedDownOnChimera = true;
                _inputManager.SetRotatingInGallery(true);
            }
            if (_clickedDownOnChimera == true)
            {
                _posDelta = Input.mousePosition - _prevMousePos;
                _currentChimera.transform.Rotate(_camera.gameObject.transform.up, -Vector3.Dot(_posDelta, _camera.transform.right), Space.World);
                _currentChimera.transform.localEulerAngles = new Vector3(0.0f, _currentChimera.transform.localEulerAngles.y, 0.0f);
            }
        }
        else
        {
            _inputManager.SetRotatingInGallery(false);
            _clickedDownOnChimera = false;
        }
        _prevMousePos = Input.mousePosition;
    }

    public void SetAnimation(string animationName)
    {
        if (_currentAnimator == null)
        {
            return;
        }

        if (_currentAnim != "")
        {
            _currentAnimator.SetBool(_currentAnim, false);
        }

        _currentAnim = animationName;
        _currentAnimator.SetBool(_currentAnim, true);
    }

    private void LoadCurrentChimera(ChimeraType chimeraType)
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
