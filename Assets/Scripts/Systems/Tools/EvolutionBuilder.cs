using System.Collections.Generic;
using UnityEngine;

public class EvolutionBuilder : MonoBehaviour
{
    [SerializeField] EvolutionDataManager _evolutionDataManager = null;
    [SerializeField] Transform _chimeraSpawner = null;

    private ChimeraCreator _chimeraCreator = null;
    private Chimera _selectedChimera = null;
    private List<Chimera> _baseChimeras = new List<Chimera>();

    public List<Chimera> BaseChimeras { get => _baseChimeras; }

    public void Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _chimeraCreator = ServiceLocator.Get<ChimeraCreator>();
    }

    public void BuildAll()
    {
        BuildChimera(ChimeraType.A);
        BuildChimera(ChimeraType.B);
        BuildChimera(ChimeraType.C);

        SelectChimera(ChimeraType.A);
    }

    public void SelectChimera(ChimeraType chimeraType)
    {
        if (_selectedChimera != null)
        {
            _selectedChimera.gameObject.SetActive(false);
        }

        switch (chimeraType)
        {
            case ChimeraType.A:
            case ChimeraType.A1:
            case ChimeraType.A2:
            case ChimeraType.A3:
                _selectedChimera = _baseChimeras[0];
                break;
            case ChimeraType.B:
            case ChimeraType.B1:
            case ChimeraType.B2:
            case ChimeraType.B3:
                _selectedChimera = _baseChimeras[1];
                break;
            case ChimeraType.C:
            case ChimeraType.C1:
            case ChimeraType.C2:
            case ChimeraType.C3:
                _selectedChimera = _baseChimeras[2];
                break;
            default:
                Debug.LogError($"Chimera Type [{chimeraType}] is invalid. Please fix!");
                break;
        }

        _selectedChimera.gameObject.SetActive(true);
    }

    private void BuildChimera(ChimeraType chimeraType)
    {
        GameObject newChimera = _chimeraCreator.CreateChimeraByType(chimeraType);
        newChimera.transform.position = _chimeraSpawner.position;
        newChimera.transform.rotation = _chimeraSpawner.rotation;
        newChimera.transform.parent = this.transform;
        newChimera.SetActive(false);

        Chimera chimeraComp = newChimera.GetComponent<Chimera>();
        chimeraComp.InitializeForBuilder();

        _baseChimeras.Add(chimeraComp);
    }

    public void SaveVFXInstructions()
    {

    }
}