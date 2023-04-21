using UnityEngine;

public class StarterEnvironment : MonoBehaviour
{
    [Header("Chimeras")]
    [SerializeField] EvolutionLogic _chimeraA = null;
    [SerializeField] EvolutionLogic _chimeraB = null;
    [SerializeField] EvolutionLogic _chimeraC = null;

    [Header("Position Nodes")]
    [SerializeField] Transform _originNode = null;
    [SerializeField] Transform _cameraANode = null;
    [SerializeField] Transform _cameraBNode = null;
    [SerializeField] Transform _cameraCNode = null;

    public Transform OriginNode { get => _originNode; }
    public Transform ANode { get => _cameraANode; }
    public Transform BNode { get => _cameraBNode; }
    public Transform CNode { get => _cameraCNode; }
    public EvolutionLogic ChimeraA { get => _chimeraA; }
    public EvolutionLogic ChimeraB { get => _chimeraB; }
    public EvolutionLogic ChimeraC { get => _chimeraC; }

    public void ShowAllChimeras()
    {
        _chimeraA.gameObject.SetActive(true);
        _chimeraB.gameObject.SetActive(true);
        _chimeraC.gameObject.SetActive(true);
    }

    public void ShowChimera(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                _chimeraB.gameObject.SetActive(false);
                _chimeraC.gameObject.SetActive(false);
                break;
            case ChimeraType.B:
                _chimeraA.gameObject.SetActive(false);
                _chimeraC.gameObject.SetActive(false);
                break;
            case ChimeraType.C:
                _chimeraA.gameObject.SetActive(false);
                _chimeraB.gameObject.SetActive(false);
                break;
            default:
                Debug.LogError($"Chimera type {chimeraType} is invalid!");
                break;
        }
    }
}