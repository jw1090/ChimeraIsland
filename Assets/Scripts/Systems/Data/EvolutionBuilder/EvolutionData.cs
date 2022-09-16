using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionData", menuName = "ScriptableObjects/Evolution", order = 1)]
public class EvolutionData : ScriptableObject
{
    public EvolutionVFXType EvolutionVFXType { get; set; }
    public Color Color { get; set; }
    public float Size { get; set; }
}