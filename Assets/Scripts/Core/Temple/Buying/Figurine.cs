using UnityEngine;

public class Figurine : MonoBehaviour
{
    [SerializeField] EvolutionLogic _evolutionLogic = null;

    public EvolutionLogic EvolutionLogic { get => _evolutionLogic; }
}