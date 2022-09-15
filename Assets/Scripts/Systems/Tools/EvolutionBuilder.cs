using System.Collections.Generic;
using UnityEngine;

public class EvolutionBuilder : MonoBehaviour
{
    private List<Chimera> _baseChimeras = new List<Chimera>();

    public List<Chimera> BaseChimeras { get => _baseChimeras; }

    public void Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");
    }
}