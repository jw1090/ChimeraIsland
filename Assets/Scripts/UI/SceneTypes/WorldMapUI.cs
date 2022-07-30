using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    [SerializeField] private Button _stonePlainsButton = null;
    [SerializeField] private Button _treeOfLifeButton = null;

    public Button StonePlainsButton { get => _stonePlainsButton; }
    public Button TreeOfLifeButton { get => _treeOfLifeButton; }
}