using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private Vector3 _pos = Vector3.zero;

    [Header("Zoom")]
    [SerializeField] private float _zoom = 80.0f;
    [SerializeField] private float _zoomAmount = 20.0f;
    [SerializeField] private float _minZoom = 40.0f;
    [SerializeField] private float _maxZoom = 90.0f;

    [Header("CameraFollow")]
    [SerializeField] private bool _isUseMoveOnScreenEdge = true;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private int _ScreenEdgeSize = 20;

    private bool _MoveUp;
    private bool _MoveDown;
    private bool _MoveRight;
    private bool _MoveLeft;

    private Rect _RightRect;
    private Rect _UpRect;
    private Rect _DownRect;
    private Rect _LeftRect;

    //private Material _mat;
    private Vector3 _dir = Vector3.zero;

    public Camera CameraCO { get; private set; }

    public CameraController Initialize()
    {
        Debug.Log("<color=Orange> Initializing Camera Logic ... </color>");
        CameraCO = GetComponent<Camera>();
        _pos = transform.position;
        //CreateLineMaterial();

        return this;
    }

    private void Update()
    {
        _pos = transform.position;
        ScreenMove();
        CameraMovement();
        CameraZoom();
    }

    private void CameraMovement()
    {
        float panSpeed = (Input.GetKey(KeyCode.LeftShift)) ? 2 * _speed : _speed;

        if (Input.GetKey(KeyCode.W))
        {
            _pos.z -= panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _pos.x += panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _pos.x -= panSpeed * Time.deltaTime;
        }
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        if (_horizontal != 0 || _vertical != 0)
            transform.position = _pos;
    }

    private void CameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            _zoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomAmount;
            _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
            CameraCO.fieldOfView = _zoom;
        }
    }

    private void ScreenMove()
    {
        if (_isUseMoveOnScreenEdge)
        {
            _UpRect = new Rect(1f, Screen.height - _ScreenEdgeSize, Screen.width, _ScreenEdgeSize);
            _DownRect = new Rect(1f, 1f, Screen.width, _ScreenEdgeSize);
            _RightRect = new Rect(1f, 1f, _ScreenEdgeSize, Screen.height);
            _LeftRect = new Rect(Screen.width - _ScreenEdgeSize, 1f, _ScreenEdgeSize, Screen.height);

            _MoveDown = (_UpRect.Contains(Input.mousePosition));
            _MoveUp = (_DownRect.Contains(Input.mousePosition));
            _MoveLeft = (_LeftRect.Contains(Input.mousePosition));
            _MoveRight = (_RightRect.Contains(Input.mousePosition));

            _dir.z = _MoveUp ? 1 : _MoveDown ? -1 : 0;
            _dir.x = _MoveLeft ? -1 : _MoveRight ? 1 : 0;

            transform.position = Vector3.Lerp(transform.position, transform.position + _dir * _moveSpeed, Time.deltaTime);
        }
    }

    //void CreateLineMaterial()
    //{
    //    if (!_mat)
    //    {
    //        Shader shader = Shader.Find("Hidden/Internal-Colored");
    //        _mat = new Material(shader);
    //        _mat.hideFlags = HideFlags.HideAndDontSave;
    //        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //        _mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
    //        _mat.SetInt("_ZWrite", 0);
    //    }
    //}

    //void OnPostRender()
    //{
    //    if (isUseMoveOnScreenEdge && isDebugScreenEdge)
    //    {
    //        DrawRect(UpRect, MoveUp, Color.cyan, Color.red);
    //        DrawRect(DownRect, MoveDown, Color.green, Color.red);
    //        DrawRect(LeftRect, MoveLeft, Color.yellow, Color.red);
    //        DrawRect(RightRect, MoveRight, Color.blue, Color.red);
    //    }
    //}

    //private void DrawRect(Rect rect, bool isMouseEnter, Color normalColor, Color HeighLightColor)
    //{
    //    if (isMouseEnter)
    //    {
    //        DrawScreenRect(rect, HeighLightColor);
    //    }
    //    else
    //    {
    //        DrawScreenRect(rect, normalColor);
    //    }
    //}

    //private void DrawScreenRect(Rect rect, Color color)
    //{
    //    GL.LoadOrtho();
    //    GL.Begin(GL.LINES);
    //    {
    //        _mat.SetPass(0);
    //        GL.Color(color);
    //        GL.Vertex3(rect.xMin / Screen.width, rect.yMin / Screen.height, 0);
    //        GL.Vertex3(rect.xMin / Screen.width, rect.yMax / Screen.height, 0);

    //        GL.Vertex3(rect.xMin / Screen.width, rect.yMax / Screen.height, 0);
    //        GL.Vertex3(rect.xMax / Screen.width, rect.yMax / Screen.height, 0);

    //        GL.Vertex3(rect.xMax / Screen.width, rect.yMax / Screen.height, 0);
    //        GL.Vertex3(rect.xMax / Screen.width, rect.yMin / Screen.height, 0);

    //        GL.Vertex3(rect.xMax / Screen.width, rect.yMin / Screen.height, 0);
    //        GL.Vertex3(rect.xMin / Screen.width, rect.yMin / Screen.height, 0);
    //    }
    //    GL.End();
    //}
}