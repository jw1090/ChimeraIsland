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
    [SerializeField] RawImage _panel = null; 
    [SerializeField] Camera _treadmillCamera = null;

    private bool _initialized = false;
    public List<Chimera> ChimeraList { get => _chimeraList;}

    public Transform FirstChimeraPosition { get => _firstChimera; }
    public Transform SecondChimeraPosition { get => _secondChimera; }
    public Transform ThirdChimeraPosition { get => _thirdChimera; }
    public Transform FourthChimeraPosition { get => _fourthChimera; }
    public Transform FifthChimeraPosition { get => _fifthChimera; }
    public Camera TreadmillCamera { get => _treadmillCamera; }
    public bool IsRunning { get; set; }

    public TreadmillManager Initialize()
    {

        IsRunning = false;

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

        if(IsRunning == true)
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
        RenderTexture render = new RenderTexture(1920,1080,0);
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
        foreach(Chimera chimera in _chimeraList)
        {
            chimera.ChimeraPosition(FirstChimeraPosition);

            if(_chimeraList.Count == 2)
            {
                ++index;
                chimera.ChimeraPosition(SecondChimeraPosition);
                if (index == 1)
                {
                    chimera.ChimeraPosition(ThirdChimeraPosition);
                }
            }
            else if(_chimeraList.Count == 3)
            {
                index++;
                chimera.ChimeraPosition(FirstChimeraPosition);
                if (index == 1)
                {
                    chimera.ChimeraPosition(FourthChimeraPosition);
                }
                if (index == 2)
                {
                    chimera.ChimeraPosition(FifthChimeraPosition);
                }
            }
        }
    }

}
