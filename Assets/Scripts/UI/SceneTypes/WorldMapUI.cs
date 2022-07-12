using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    [SerializeField] private Button _stonePlainsButton = null;
    [SerializeField] private Button _treeOfLifeButton = null;
    [SerializeField] private Button _ashLandsButton = null;

    public Button StonePlainsButton { get => _stonePlainsButton; }
    public Button TreeOfLifeButton { get => _treeOfLifeButton; }
    public Button AshLandsButton { get => _ashLandsButton; }
}