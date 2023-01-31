using UnityEngine;

public class Figurine : MonoBehaviour
{
    [SerializeField] private EvolutionLogic _evolutionLogic = null;
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    public EvolutionLogic EvolutionLogic { get => _evolutionLogic; }
    public ChimeraType ChimeraType { get => _chimeraType; }
}