using UnityEngine;

public class HabitatSources : MonoBehaviour
{
    [SerializeField] private AudioSource _caveSource = null;
    [SerializeField] private AudioSource _runeStoneSource = null;
    [SerializeField] private AudioSource _waterfallSource = null;

    public AudioSource CaveSource { get => _caveSource; }
    public AudioSource RuneStoneSource { get => _runeStoneSource; }
    public AudioSource WaterfallSource { get => _waterfallSource; }
}