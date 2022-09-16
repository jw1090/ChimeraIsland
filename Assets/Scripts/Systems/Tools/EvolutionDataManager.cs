using UnityEngine;

public class EvolutionDataManager : MonoBehaviour
{
    [SerializeField] private EvolutionData _a1EvolutionData = null;
    [SerializeField] private EvolutionData _a2EvolutionData = null;
    [SerializeField] private EvolutionData _a3EvolutionData = null;
    [SerializeField] private EvolutionData _b1EvolutionData = null;
    [SerializeField] private EvolutionData _b2EvolutionData = null;
    [SerializeField] private EvolutionData _b3EvolutionData = null;
    [SerializeField] private EvolutionData _c1EvolutionData = null;
    [SerializeField] private EvolutionData _c2EvolutionData = null;
    [SerializeField] private EvolutionData _c3EvolutionData = null;

    public EvolutionData A1EvolutionData { get => _a1EvolutionData; }
    public EvolutionData A2EvolutionData { get => _a2EvolutionData; }
    public EvolutionData A3EvolutionData { get => _a3EvolutionData; }
    public EvolutionData B1EvolutionData { get => _b1EvolutionData; }
    public EvolutionData B2EvolutionData { get => _b2EvolutionData; }
    public EvolutionData B3EvolutionData { get => _b3EvolutionData; }
    public EvolutionData C1EvolutionData { get => _c1EvolutionData; }
    public EvolutionData C2EvolutionData { get => _c2EvolutionData; }
    public EvolutionData C3EvolutionData { get => _c3EvolutionData; }

    public EvolutionData GetEvolutionDataByType(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A1:
                return _a1EvolutionData;
            case ChimeraType.A2:
                return _a2EvolutionData;
            case ChimeraType.A3:
                return _a3EvolutionData;
            case ChimeraType.B1:
                return _b1EvolutionData;
            case ChimeraType.B2:
                return _b2EvolutionData;
            case ChimeraType.B3:
                return _b3EvolutionData;
            case ChimeraType.C1:
                return _c1EvolutionData;
            case ChimeraType.C2:
                return _c2EvolutionData;
            case ChimeraType.C3:
                return _c3EvolutionData;
            default:
                Debug.LogError($"Chimera Type [{chimeraType}] is invalid. Please fix!");
                return null;
        }
    }
}