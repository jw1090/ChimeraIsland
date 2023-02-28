using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreadmillManager : MonoBehaviour
{
    [Header("Nodes")]
    [SerializeField] Transform _startNode = null;
    [SerializeField] Transform _endNode = null;

    [Header("Chimera Information")]
    [SerializeField] private List<Chimera> _chimeraList = null;
    [SerializeField] Transform _firstChimera = null;
    [SerializeField] Transform _secondChimera = null;
    [SerializeField] Transform _thirdChimera = null;
    [SerializeField] Transform _fourthChimera = null;
    [SerializeField] Transform _fifthChimera = null;

    [Header("Planes")]
    [SerializeField] List<GameObject> _planes = null;

    [Header("Camera")]
    [SerializeField] Camera _treadmillCamera = null;

    private bool _initialized = false;
    private bool _isRunning = false;

    public List<Chimera> ChimeraList { get => _chimeraList; }
    public bool IsRunning { get => _isRunning; }

    public void SetIsRunning(bool isRunning) { _isRunning = isRunning; }


    public TreadmillManager Initialize()
    {
        _initialized = true;
        _treadmillCamera.gameObject.SetActive(false);

        return this;
    }

    private void FixedUpdate()
    {
        if (_initialized == false)
        {
            return;
        }

        if (_isRunning == true)
        {
            _treadmillCamera.gameObject.SetActive(true);
            foreach (GameObject planes in _planes)
            {
                planes.transform.position += new Vector3(-2.0f * Time.deltaTime, 0, 0);
                if (planes.transform.position.x <= _endNode.position.x)
                {
                    Reposition(planes);
                }
            }
        }
    }

    public void Render(RawImage image)
    {
        RenderTexture render = new RenderTexture(1920, 1080, 0);
        _treadmillCamera.targetTexture = render;
        image.texture = render;
    }


    private void Reposition(GameObject gameObject)
    {
        gameObject.transform.position = _startNode.position;
    }

    public void Warp()
    {
        int index = 0;
        foreach (Chimera chimera in _chimeraList)
        {
            WarpChimera(chimera, _firstChimera);

            if (_chimeraList.Count == 2)
            {
                ++index;

                WarpChimera(chimera, _secondChimera);

                if (index == 1)
                {
                    WarpChimera(chimera, _thirdChimera);
                }
            }
            else if (_chimeraList.Count == 3)
            {
                index++;

                WarpChimera(chimera, _firstChimera);

                if (index == 1)
                {
                    WarpChimera(chimera, _fourthChimera);
                }
                if (index == 2)
                {
                    WarpChimera(chimera, _fifthChimera);
                }
            }
        }
    }

    private void WarpChimera(Chimera chimera, Transform newTransform)
    {
        chimera.transform.position = newTransform.position;
        chimera.transform.rotation = newTransform.rotation;
    }
}