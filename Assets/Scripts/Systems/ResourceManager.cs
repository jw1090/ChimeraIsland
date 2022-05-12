using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private GameObject _chimeraBasePrefabA = null;
    [SerializeField] private Chimera _chimeraBrainA = null;
    [SerializeField] private Chimera _chimeraBrainB = null;
    [SerializeField] private Chimera _chimeraBrainC = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabA = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabA1 = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabA2 = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabA3 = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabB = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabB1 = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabB2 = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabB3 = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabC = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabC1 = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabC2 = null;
    [SerializeField] private EvolutionLogic _chimeraEvolutionPrefabC3 = null;
    public ResourceManager Initialize()
    {
        Debug.Log($"<color=lime> {this.GetType()} Initialized!</color>");

        _chimeraBasePrefabA = Resources.Load<GameObject>("Chimera/A");
        _chimeraBrainA = Resources.Load<Chimera>("Chimera/A");
        _chimeraBrainB = Resources.Load<Chimera>("Chimera/B");
        _chimeraBrainC = Resources.Load<Chimera>("Chimera/C");
        _chimeraEvolutionPrefabA = Resources.Load<EvolutionLogic>("Chimera/Models/Family A/A Model");
        _chimeraEvolutionPrefabA1 = Resources.Load<EvolutionLogic>("Chimera/Models/Family A/A1 Model");
        _chimeraEvolutionPrefabA2 = Resources.Load<EvolutionLogic>("Chimera/Models/Family A/A2 Model");
        _chimeraEvolutionPrefabA3 = Resources.Load<EvolutionLogic>("Chimera/Models/Family A/A3 Model");
        _chimeraEvolutionPrefabB = Resources.Load<EvolutionLogic>("Chimera/Models/Family B/B Model");
        _chimeraEvolutionPrefabB1 = Resources.Load<EvolutionLogic>("Chimera/Models/Family B/B1 Model");
        _chimeraEvolutionPrefabB2 = Resources.Load<EvolutionLogic>("Chimera/Models/Family B/B2 Model");
        _chimeraEvolutionPrefabB3 = Resources.Load<EvolutionLogic>("Chimera/Models/Family B/B3 Model");
        _chimeraEvolutionPrefabC = Resources.Load<EvolutionLogic>("Chimera/Models/Family C/C Model");
        _chimeraEvolutionPrefabC1 = Resources.Load<EvolutionLogic>("Chimera/Models/Family C/C1 Model");
        _chimeraEvolutionPrefabC2 = Resources.Load<EvolutionLogic>("Chimera/Models/Family C/C2 Model");
        _chimeraEvolutionPrefabC3 = Resources.Load<EvolutionLogic>("Chimera/Models/Family C/C3 Model");

        return this;
    }

    public Chimera GetChimeraBrain(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
            case ChimeraType.A1:
            case ChimeraType.A2:
            case ChimeraType.A3:
                return _chimeraBrainA;
            case ChimeraType.B:
            case ChimeraType.B1:
            case ChimeraType.B2:
            case ChimeraType.B3:
                return _chimeraBrainB;
            case ChimeraType.C:
            case ChimeraType.C1:
            case ChimeraType.C2:
            case ChimeraType.C3:
                return _chimeraBrainC;
            default:
                Debug.LogWarning($"Unhandled prefab type {type}");
                return null;
        }
    }

    public GameObject GetChimeraPrefab(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
            case ChimeraType.A1:
            case ChimeraType.A2:
            case ChimeraType.A3:
                return _chimeraBasePrefabA;
            case ChimeraType.B:
            case ChimeraType.B1:
            case ChimeraType.B2:
            case ChimeraType.B3:
            case ChimeraType.C:
            case ChimeraType.C1:
            case ChimeraType.C2:
            case ChimeraType.C3:
            default:
                Debug.LogWarning($"Unhandled prefab type {type}");
                return null;
        }
    }

    public EvolutionLogic GetChimeraEvolution(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
                return _chimeraEvolutionPrefabA;
            case ChimeraType.A1:
                return _chimeraEvolutionPrefabA1;
            case ChimeraType.A2:
                return _chimeraEvolutionPrefabA2;
            case ChimeraType.A3:
                return _chimeraEvolutionPrefabA3;
            case ChimeraType.B:
                return _chimeraEvolutionPrefabB;
            case ChimeraType.B1:
                return _chimeraEvolutionPrefabB1;
            case ChimeraType.B2:
                return _chimeraEvolutionPrefabB2;
            case ChimeraType.B3:
                return _chimeraEvolutionPrefabB3;
            case ChimeraType.C:
                return _chimeraEvolutionPrefabC;
            case ChimeraType.C1:
                return _chimeraEvolutionPrefabC1;
            case ChimeraType.C2:
                return _chimeraEvolutionPrefabC2;
            case ChimeraType.C3:
                return _chimeraEvolutionPrefabC3;
            default:
                Debug.LogWarning($"Unhandled prefab type {type}");
                return null;
        }
    }
}
