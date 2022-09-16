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
}