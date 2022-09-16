using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionData", menuName = "ScriptableObjects/Evolution", order = 1)]
public class EvolutionData : ScriptableObject
{
    public EvolutionVFXType EvolutionVFXType = EvolutionVFXType.None;
    public Color Color = new Color();
    public float Size = 0.0f;

    public void InsertData(EvolutionData dataToCopy)
    {
        EvolutionVFXType = dataToCopy.EvolutionVFXType;
        Color = dataToCopy.Color;
        Size = dataToCopy.Size;
    }
}