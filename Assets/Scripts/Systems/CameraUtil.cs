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
    [SerializeField] private float _moveSpeed = 12.0f;
    [SerializeField] private int _screenEdgeSize = 50;

    [Header("Collision")]
    [SerializeField] private float _sphereRadius = 2.0f;
    [SerializeField] private float _offset = 1.5f;

    private Camera _cameraCO = null;
    private Coroutine _transitionCoroutine = null;
    private HabitatManager _habitatManager = null;
    private InputManager _inputManager = null;
    private Rect _upRect = new Rect();
    private Rect _downRect = new Rect();
    private Rect _rightRect = new Rect();
    private Rect _leftRect = new Rect();
    private bool _initialized = false;
    private bool _canMoveUp = true;
    private bool _canMoveDown = true;
    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private float _zoom = 90.0f;
    private float _standardTransitionSpeed = 0.06f;
    private float _findTransitionSpeed = 0.05f;

    public bool IsHolding { get; set; }
    public Camera CameraCO { get => _cameraCO; }

    public CameraUtil Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _habitatManager = ServiceLocator.Get<HabitatManager>();
        _inputManager = ServiceLocator.Get<InputManager>();
        _cameraCO = Camera.main;

        _upRect = new Rect(1f, Screen.height - _screenEdgeSize, Screen.width, _screenEdgeSize);
        _downRect = new Rect(1f, 1f, Screen.width, _screenEdgeSize);
        _rightRect = new Rect(1f, 1f, _screenEdgeSize, Screen.height);
        _leftRect = new Rect(Screen.width - _screenEdgeSize, 1f, _screenEdgeSize, Screen.height);

        CameraZoom();

        _initialized = true;

        return this;
    }

    private void Update()
    {
        if (_initialized == false)
        {
            return;
        }

        DragChimeraMovement();

        CameraCollisionCheck();
    }

    public void CameraMovement()
    {
        if (_initialized == false)
        {
            return;
        }

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

        transform.position = transform.position + direction * _moveSpeed * Time.deltaTime;
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

    public void FacilityCameraShift(FacilityType facilityType)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        Vector3 facilityPosition = _habitatManager.CurrentHabitat.GetFacility(facilityType).CameraTransitionNode.position;
        facilityPosition.y = this.transform.position.y;
        _transitionCoroutine = StartCoroutine(MoveCamera(facilityPosition, _standardTransitionSpeed));
    }

    public void ChimeraCameraShift()
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        Vector3 spawnPosition = _habitatManager.CurrentHabitat.SpawnPoint.position;
        spawnPosition.y = this.transform.position.y;
        spawnPosition.z += 10.0f;
        _transitionCoroutine = StartCoroutine(MoveCamera(spawnPosition, _standardTransitionSpeed));
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
}