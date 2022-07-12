using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _newGameButton = null;
    [SerializeField] private Button _loadGameButton = null;

    public Button NewGameButton { get => _newGameButton; }
    public Button LoadGameButton { get => _loadGameButton; }
}