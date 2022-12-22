using Cinemachine.Utility;
using System.Collections;
using UnityEngine;

public class CameraUtil : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private float _sprintMultiplier = 1.5f;

    [Header("Zoom")]
    [SerializeField] private float _zoomAmount = 20.0f;
    [SerializeField] private float _minZoom = 20.0f;
    [SerializeField] private float _maxZoom = 90.0f;

    [Header("Collision")]
    [SerializeField] private float _sphereRadius = 2.0f;
    [SerializeField] private float _offset = 1.5f;

    [Header("Referenes")]
    [SerializeField] private FreeCamera _freeCamera = null;
    [SerializeField] private Camera _cameraCO = null;

    private Coroutine _transitionCoroutine = null;
    private HabitatManager _habitatManager = null;
    private InputManager _inputManager = null;
    private StarterEnvironment _starterEnvironment = null;
    private Temple _templeEnvironment = null;
    private PersistentData _persistentData = null;
    private SceneType _sceneType = SceneType.None;
    private bool _initialized = false;
    private bool _canMoveUp = true;
    private bool _canMoveDown = true;
    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private float _zoom = 90.0f;
    private float _transitionDuration = 1.0f;

    public Camera CameraCO { get => _cameraCO; }
    public bool IsHolding { get; set; }
    public bool IsNaming { get; set; }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        _persistentData.SetSpeed(speed);
    }

    public void SetStarterEnvironment(StarterEnvironment starterEnvironment) { _starterEnvironment = starterEnvironment; }
    public void SetTempleEnvironment(Temple templeEnvironment) { _templeEnvironment = templeEnvironment; }

    public CameraUtil Initialize(SceneType sceneType)
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _sceneType = sceneType;

        _initialized = true;

        return this;
    }

    public void SceneSetup()
    {
        if (_sceneType == SceneType.Habitat)
        {
            CameraZoom();

            _speed = _persistentData.SettingsData.cameraSpeed;
            _freeCamera.Initialize(_speed);

            _inputManager.SetFreeCamera(_freeCamera);
        }
        else if (_sceneType == SceneType.Temple)
        {
            transform.position = _templeEnvironment.StartNode.position;
        }
    }

    public void CameraUpdate()
    {
        if (_initialized == false)
        {
            return;
        }

        if (_sceneType != SceneType.Habitat)
        {
            return;
        }

        CameraCollisionCheck();
        if (IsNaming == false)
        {
            CameraMovement();
        }
    }

    private void CameraMovement()
    {
        Vector3 direction = Vector3.zero;
        float panSpeed = (Input.GetKey(KeyCode.LeftShift)) ? _sprintMultiplier * _speed : _speed;

        bool moveUp = Input.GetKey(KeyCode.W) && _canMoveUp;
        bool moveDown = Input.GetKey(KeyCode.S) && _canMoveDown;
        bool moveLeft = Input.GetKey(KeyCode.A) && _canMoveLeft;
        bool moveRight = Input.GetKey(KeyCode.D) && _canMoveRight;

        direction.z = moveUp ? -1 : moveDown ? 1 : 0;
        direction.x = moveLeft ? 1 : moveRight ? -1 : 0;

        transform.position = transform.position + direction * panSpeed * Time.deltaTime;
    }

    public void CameraZoom()
    {
        _zoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomAmount;
        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        CameraCO.fieldOfView = _zoom;
    }

    private void CameraCollisionCheck()
    {
        Vector3 newPosition = transform.localPosition;
        RaycastHit hit;
        float pushback = _offset * 0.5f;

        Debug.DrawRay(transform.position, Vector3.forward * _offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.back * _offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.left * _offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.right * _offset, Color.yellow);

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.forward, out hit, _offset))
        {
            if (hit.transform.CompareTag("CameraBounds"))
            {
                newPosition.z = transform.localPosition.z - pushback;

                _canMoveDown = false;
            }
        }
        else
        {
            _canMoveDown = true;
        }

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.back, out hit, _offset))
        {
            if (hit.transform.CompareTag("CameraBounds"))
            {
                _canMoveUp = false;
            }
        }
        else
        {
            _canMoveUp = true;
        }

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.right, out hit, _offset))
        {
            if (hit.transform.CompareTag("CameraBounds"))
            {
                _canMoveLeft = false;
            }
        }
        else
        {
            _canMoveLeft = true;
        }

        if (Physics.SphereCast(transform.position, _sphereRadius, Vector3.left, out hit, _offset))
        {
            if (hit.transform.CompareTag("CameraBounds"))
            {
                _canMoveRight = false;
            }
        }
        else
        {
            _canMoveRight = true;
        }
    }

    public void TempleCameraShift()
    {
        Transform transform = _habitatManager.CurrentHabitat.Temple.CameraTransitionNode;

        CameraShift(transform);
    }

    public void FacilityCameraShift(FacilityType facilityType)
    {
        Transform transform = _habitatManager.CurrentHabitat.GetFacility(facilityType).CameraTransitionNode;

        CameraShift(transform);
    }

    private void CameraShift(Transform target, bool rotate = false)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        MoveCamera(target, rotate);
    }

    public void FindChimeraCameraShift(Chimera chimera)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        Vector3 targetPosition;
        targetPosition = new Vector3(chimera.transform.position.x, this.transform.position.y, chimera.transform.position.z); // Lock Camera Y

        _transitionCoroutine = StartCoroutine(MoveCamera(targetPosition, Quaternion.identity, false));
    }

    private void MoveCamera(Transform target, bool rotate)
    {
        _transitionCoroutine = StartCoroutine(MoveCamera(target.position, target.rotation, rotate));
    }

    private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation, bool rotate)
    {
        _inputManager.SetInTransition(true);

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float time = 0.0f;
        while (time < _transitionDuration)
        {
            yield return new WaitForSeconds(0.001f);

            time += Time.deltaTime;
            float progress = time / _transitionDuration;

            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            if (rotate == true)
            {
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            }
        }

        _inputManager.SetInTransition(false);
    }

    public void ChimeraCloseUp(ChimeraType chimeraType)
    {
        Transform nodeTransform = null;

        switch (chimeraType)
        {
            case ChimeraType.A:
                nodeTransform = _starterEnvironment.ANode;
                break;
            case ChimeraType.B:
                nodeTransform = _starterEnvironment.BNode;
                break;
            case ChimeraType.C:
                nodeTransform = _starterEnvironment.CNode;
                break;
            default:
                Debug.LogWarning($"{chimeraType} is not a valid type. Please fix!");
                break;
        }

        _starterEnvironment.ShowChimera(chimeraType);
        CameraShift(nodeTransform, true);
    }

    public void TempleTransition(TempleSectionType templeSectionType)
    {
        Transform nodeTransform = null;

        switch (templeSectionType)
        {
            case TempleSectionType.None:
                break;
            case TempleSectionType.Buying:
                nodeTransform = _templeEnvironment.BuyingNode;
                break;
            case TempleSectionType.Upgrades:
                nodeTransform = _templeEnvironment.UpgradeNode;
                break;
            case TempleSectionType.Collection:
                nodeTransform = _templeEnvironment.CollectionNode;
                break;
            case TempleSectionType.Habitat:
                nodeTransform = _templeEnvironment.StartNode;
                break;
            default:
                Debug.LogWarning($"{templeSectionType} is not a valid type. Please fix!");
                break;
        }

        CameraShift(nodeTransform, true);
    }

    public void CameraToOrigin()
    {
        _starterEnvironment.ShowAllChimeras();
        CameraShift(_starterEnvironment.OriginNode, true);
    }
}