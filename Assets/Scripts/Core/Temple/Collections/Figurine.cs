using UnityEngine;

public class Figurine : MonoBehaviour
{
    [SerializeField] private ChimeraType _chimeraType = ChimeraType.None;
    public ChimeraType ChimeraType { get => _chimeraType; }
}