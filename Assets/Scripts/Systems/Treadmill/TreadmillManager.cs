using System.Collections.Generic;
using UnityEngine;

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

    [Header("Planes")]
    [SerializeField] Camera _expeditionCamera = null;

    private bool _initialized = false;
    public List<Chimera> ChimeraList { get => _chimeraList;}

    public Transform FirstChimeraPosition { get => _firstChimera; }
    public Transform SecondChimeraPosition { get => _secondChimera; }
    public Transform ThirdChimeraPosition { get => _thirdChimera; }
    public Transform FourthChimeraPosition { get => _fourthChimera; }
    public Transform FifthChimeraPosition { get => _fifthChimera; }
    public bool IsRunning { get; set; }

    public TreadmillManager Initialize()
    {

        IsRunning = false;

        _initialized = true;

        _expeditionCamera.gameObject.SetActive(false);

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
            _expeditionCamera.gameObject.SetActive(true);
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

    private void Reposition(GameObject gameObject)
    {
        gameObject.transform.position = _startNode.position;
    }

    public void Warp()
    {
        int index = 0;
        foreach(Chimera chimera in _chimeraList)
        {
            chimera.transform.position = FirstChimeraPosition.position;

            if(_chimeraList.Count == 2)
            {
                ++index;
                chimera.transform.position = SecondChimeraPosition.position;
                if(index == 1)
                {
                    chimera.transform.position = ThirdChimeraPosition.position;
                }
            }
            else if(_chimeraList.Count == 3)
            {
                index++;
                chimera.transform.position = FirstChimeraPosition.position;
                if (index == 1)
                {
                    chimera.transform.position = FourthChimeraPosition.position;
                }
                if (index == 2)
                {
                    chimera.transform.position = FifthChimeraPosition.position;
                }
            }
        }
    }

}
