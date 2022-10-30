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

    [Header("Edge Follow")]
    [SerializeField] private int _screenEdgeSize = 50;

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
    private PersistentData _persistentData = null;
    private Rect _upRect = new Rect();
    private Rect _downRect = new Rect();
    private Rect _rightRect = new Rect();
    private Rect _leftRect = new Rect();
    private SceneType _sceneType = SceneType.None;
    private bool _initialized = false;
    private bool _canMoveUp = true;
    private bool _canMoveDown = true;
    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private float _zoom = 90.0f;
    private float _standardTransitionSpeed = 0.06f;
    private float _findTransitionSpeed = 0.05f;
    public Camera CameraCO { get => _cameraCO; }
    public SceneType SceneType { get => _sceneType; }
    public bool IsHolding { get; set; }
    public bool IsNaming { get; set; }
    public float Speed { get => _speed; }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        _persistentData.SetSpeed(speed);
    }

    public void SetStarterEnvironment(StarterEnvironment starterEnvironment) { _starterEnvironment = starterEnvironment; }

    public CameraUtil Initialize(SceneType sceneType)
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _persistentData = ServiceLocator.Get<PersistentData>();
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _sceneType = sceneType;

        _speed = _persistentData.SettingsData.cameraSpeed;

        if (_sceneType == SceneType.Habitat)
        {
            _upRect = new Rect(1f, Screen.height - _screenEdgeSize, Screen.width, _screenEdgeSize);
            _downRect = new Rect(1f, 1f, Screen.width, _screenEdgeSize);
            _rightRect = new Rect(1f, 1f, _screenEdgeSize, Screen.height);
            _leftRect = new Rect(Screen.width - _screenEdgeSize, 1f, _screenEdgeSize, Screen.height);

            CameraZoom();

            _freeCamera.Initialize(_speed);

            _inputManager.SetFreeCamera(_freeCamera);
        }

        _initialized = true;

        return this;
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
        DragChimeraMovement();
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

    private void DragChimeraMovement()
    {
        if (IsHolding == false)
        {
            return;
        }

        Vector3 direction = Vector3.zero;

        bool moveDown = _upRect.Contains(Input.mousePosition) && _canMoveUp;
        bool moveUp = _downRect.Contains(Input.mousePosition) && _canMoveDown;
        bool moveLeft = _leftRect.Contains(Input.mousePosition) && _canMoveRight;
        bool moveRight = _rightRect.Contains(Input.mousePosition) && _canMoveLeft;

        direction.z = moveUp ? 1 : moveDown ? -1 : 0;
        direction.x = moveLeft ? -1 : moveRight ? 1 : 0;

        transform.position = transform.position + direction * _speed * Time.deltaTime;
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

    private void CameraShift(Vector3 position)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        _transitionCoroutine = StartCoroutine(MoveCamera(position, _standardTransitionSpeed));
    }

    private void CameraShift(Vector3 position, Quaternion rotation)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        _transitionCoroutine = StartCoroutine(MoveCamera(position, rotation, _standardTransitionSpeed));
    }

    public void TempleCameraShift()
    {
        Vector3 templeposition = _habitatManager.CurrentHabitat.Temple.CameraTransitionNode.position;
        templeposition.y = this.transform.position.y;

        CameraShift(templeposition);
    }

    public void FacilityCameraShift(FacilityType facilityType)
    {
        Vector3 facilityPosition = _habitatManager.CurrentHabitat.GetFacility(facilityType).CameraTransitionNode.position;
        facilityPosition.y = this.transform.position.y;

        CameraShift(facilityPosition);
    }

    public void FindChimeraCameraShift(Chimera chimera)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        _transitionCoroutine = StartCoroutine(TrackingChimeraCamera(chimera, _findTransitionSpeed));
    }

    private IEnumerator MoveCamera(Vector3 target, float time)
    {
        _inputManager.SetInTransition(true);
        while (Vector3.Distance(transform.position, target) > 0.5f)
        {
            yield return new WaitForSeconds(0.01f);

            transform.position = Vector3.Lerp(transform.position, target, time);
        }
        _inputManager.SetInTransition(false);
    }

    private IEnumerator MoveCamera(Vector3 target, Quaternion rotation, float time)
    {
        _inputManager.SetInTransition(true);

        while (Vector3.Distance(transform.position, target) > 0.2f)
        {
            yield return new WaitForSeconds(0.01f);

            transform.position = Vector3.Lerp(transform.position, target, time);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, time);
        }

        _inputManager.SetInTransition(false);
    }

    private IEnumerator TrackingChimeraCamera(Chimera chimera, float time)
    {
        _inputManager.SetInTransition(true);

        Vector3 chimeraPosition = chimera.transform.position;
        chimeraPosition.y = this.transform.position.y;
        chimeraPosition.z += 10.0f;

        while (Vector3.Distance(transform.position, chimeraPosition) > 2.0f)
        {
            yield return new WaitForSeconds(0.01f);

            chimeraPosition = chimera.transform.position;
            chimeraPosition.y = this.transform.position.y;
            chimeraPosition.z += 10.0f;

            transform.position = Vector3.Lerp(transform.position, chimeraPosition, time);
        }

        _inputManager.SetInTransition(false);
    }

    public void ChimeraCloseUp(ChimeraType chimeraType)
    {
        Vector3 nodePosition = Vector3.zero;
        Quaternion nodeRotation = Quaternion.identity;

        switch (chimeraType)
        {
            case ChimeraType.A:
                nodePosition = _starterEnvironment.ANode.position;
                nodeRotation = _starterEnvironment.ANode.rotation;
                break;
            case ChimeraType.B:
                nodePosition = _starterEnvironment.BNode.position;
                nodeRotation = _starterEnvironment.BNode.rotation;
                break;
            case ChimeraType.C:
                nodePosition = _starterEnvironment.CNode.position;
                nodeRotation = _starterEnvironment.CNode.rotation;
                break;
            default:
                Debug.LogWarning($"{chimeraType} is not a valid type. Please fix!");
                break;
        }

        _starterEnvironment.ShowChimera(chimeraType);
        CameraShift(nodePosition, nodeRotation);
    }

    public void CameraToOrigin()
    {
        _starterEnvironment.ShowAllChimeras();
        CameraShift(_starterEnvironment.OriginNode.position, _starterEnvironment.OriginNode.rotation);
    }
}