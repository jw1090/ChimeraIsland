using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title = null;
    [SerializeField] private GameObject _container = null;
    [SerializeField] private StartingChimeraButton _acceptButton = null;
    [SerializeField] private Button _declineButton = null;
    [SerializeField] private StartingChimeraInfo _startingChimeraInfo = null;
    private UIManager _uiManager = null;
    private StarterEnvironment _starterEnvironment = null;

    public Button AcceptButton { get => _acceptButton.GetComponent<Button>(); }
    public Button DeclineButton { get => _declineButton; }

    public void SetChimeraType(ChimeraType chimeraType) { }
    public void SetStarterEnvironment(StarterEnvironment starterEnvironment) { _starterEnvironment = starterEnvironment; }

    public void Initialize(UIManager uIManager)
    {
        _uiManager = uIManager;

        _acceptButton.Initialize();
        _startingChimeraInfo.Initialize();

        SetupListeners();
    }

    public void OnSceneStart()
    {
        _acceptButton.SetupStartingButton();

        ResetUI();
    }

    public void SetupListeners()
    {
        _uiManager.CreateButtonListener(DeclineButton, ResetUI);
        _uiManager.CreateButtonListener(DeclineButton, ResetCamera);
    }

    public void OpenChimeraInfo()
    {
        _container.SetActive(true);
        _title.gameObject.SetActive(false);
    }

    public void LoadChimeraInfo(EvolutionLogic evolutionLogic)
    {
        _acceptButton.SetChimeraType(evolutionLogic.ChimeraType);
        _startingChimeraInfo.LoadChimeraData(evolutionLogic);
    }

    private void ResetUI()
    {
        _container.SetActive(false);
        _title.gameObject.SetActive(true);
    }

    private void ResetCamera()
    {
        _starterEnvironment.CameraToOrigin();
    }
}