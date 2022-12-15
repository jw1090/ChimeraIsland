using UnityEngine;

public class ChimeraPillar : MonoBehaviour
{
    [SerializeField] private EvolutionLogic _evolutionLogic = null;

    public EvolutionLogic EvolutionLogic { get => _evolutionLogic; }
}