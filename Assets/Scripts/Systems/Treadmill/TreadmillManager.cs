using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillManager : MonoBehaviour
{
    [Header("Nodes")]
    [SerializeField] Transform _startNode = null;
    [SerializeField] Transform _endNode = null;

    [Header("Chimera Nodes")]
    [SerializeField] Transform _firstChimera = null;
    [SerializeField] Transform _secondChimera = null;
    [SerializeField] Transform _thirdChimera = null;

    [Header("Planes")]
    [SerializeField] List<GameObject> _plane1 = null;

    public Transform FirstChimeraPosition { get => _firstChimera; }
    public Transform SecondChimeraPosition { get => _secondChimera; }
    public Transform ThirdChimeraPosition { get => _thirdChimera; }

    private void FixedUpdate()
    {
        foreach(GameObject planes in _plane1)
        {
            planes.transform.position += new Vector3(-2.0f * Time.deltaTime, 0, 0);
            if (planes.transform.position.x <= _endNode.position.x)
            {
                Reposition(planes);
            }
        }
    }

    private void Reposition(GameObject gameObject)
    {
        gameObject.transform.position = _startNode.position;
    }

}
