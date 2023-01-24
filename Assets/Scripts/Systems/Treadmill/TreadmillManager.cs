using System.Collections;
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

    [Header("Planes")]
    [SerializeField] List<GameObject> _planes = null;
    

    private bool _initialized = false;

    public List<Chimera> ChimeraList { get => _chimeraList;}

    public Transform FirstChimeraPosition { get => _firstChimera; }
    public Transform SecondChimeraPosition { get => _secondChimera; }
    public Transform ThirdChimeraPosition { get => _thirdChimera; }

    public bool IsRunning { get; set; }

    public TreadmillManager Initialize()
    {

        IsRunning = false;

        _initialized = true;

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
        foreach(Chimera chimera in _chimeraList)
        {
             chimera.gameObject.transform.position = FirstChimeraPosition.position;
        }
    }

}
