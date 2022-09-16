using System.Collections.Generic;
using UnityEngine;

public class EvolutionBuilder : MonoBehaviour
{
    [SerializeField] private List<Chimera> _baseChimeras = new List<Chimera>();
    [SerializeField] EvolutionDataManager _evolutionDataManager = null;
    [SerializeField] Transform _chimeraSpawner = null;

    private ChimeraCreator _chimeraCreator = null;
    private Chimera _instantiatedChimera = null;

    public List<Chimera> BaseChimeras { get => _baseChimeras; }

    public void Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _chimeraCreator = ServiceLocator.Get<ChimeraCreator>();
    }

    public void BuildA()
    {
        BuildChimera(ChimeraType.A);
    }

    public void BuildB()
    {
        BuildChimera(ChimeraType.B);
    }

    public void BuildC()
    {
        BuildChimera(ChimeraType.C);
    }

    private void BuildChimera(ChimeraType chimeraType)
    {
        DeleteChimera(chimeraType);

        GameObject newChimera = _chimeraCreator.CreateChimeraByType(chimeraType);
        newChimera.transform.position = _chimeraSpawner.position;
        newChimera.transform.rotation = _chimeraSpawner.rotation;
        newChimera.transform.parent = this.transform;

        _instantiatedChimera = newChimera.GetComponent<Chimera>();
    }

    private void DeleteChimera(ChimeraType chimeraType)
    {
        if (_instantiatedChimera != null)
        {
            Destroy(_instantiatedChimera.gameObject);
        }
    }

    public void ResetChimera()
    {
        BuildChimera(_instantiatedChimera.ChimeraType);
    }
}