using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingUI : MonoBehaviour
{
    [SerializeField] private GameObject _title = null;
    [SerializeField] private GameObject _container = null;
    [SerializeField] private StartingChimeraButton _acceptButton = null;
    [SerializeField] private Button _declineButton = null;
    [SerializeField] private ChimeraInfoUI _chimeraInfo = null;
    private UIManager _uiManager = null;
    private CameraUtil _camera = null;

    public Button AcceptButton { get => _acceptButton.GetComponent<Button>(); }
    public Button DeclineButton { get => _declineButton; }

    public void SetCameraUtil(CameraUtil cameraUtil) { _camera = cameraUtil; }

    public void Initialize(UIManager uIManager)
    {
        _uiManager = uIManager;

        _acceptButton.Initialize(_uiManager);
        _chimeraInfo.Initialize(_uiManager);

        SetupListeners();
    }

    public void SceneSetup()
    {
        _acceptButton.SetupStartingButton();
        _chimeraInfo.NoAdditionalSections();

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
        _title.SetActive(false);
    }

    public void LoadChimeraInfo(EvolutionLogic evolutionLogic)
    {
        _acceptButton.SetChimeraType(evolutionLogic.ChimeraType);
        _chimeraInfo.LoadChimeraData(evolutionLogic);
    }

    private void ResetUI()
    {
        _container.SetActive(false);
        _title.SetActive(true);
    }

    private void ResetCamera()
    {
        _camera.CameraToOrigin();
    }
}