using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionData", menuName = "ScriptableObjects/Evolution", order = 1)]
public class EvolutionData : ScriptableObject
{
    public EvolutionVFXType EvolutionVFXType;
    public Color Color;
    public float Size;

    public void InsertData(EvolutionData dataToCopy)
    {
        EvolutionVFXType = dataToCopy.EvolutionVFXType;
        Color = dataToCopy.Color;
        Size = dataToCopy.Size;
    }
}