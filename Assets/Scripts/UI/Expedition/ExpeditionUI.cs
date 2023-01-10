using UnityEngine;
using UnityEngine.UI;

public class ExpeditionUI : MonoBehaviour
{
    [SerializeField] private ExpeditionSelectionUI _selectionPanel = null;
    [SerializeField] private ExpeditionSetupUI _setupPanel = null;
    [SerializeField] private ExpeditionInProgressUI _inProgressPanel = null;
    [SerializeField] private ExpeditionResultUI _resultPanel = null;
    [SerializeField] private StatefulObject _backgroundUIStates = null;
    [SerializeField] private StatefulObject _foregroundUIStates = null;
    [SerializeField] private Button _closeButton = null;

    private ExpeditionManager _expeditionManager = null;
    private UIManager _uiManager = null;
    private HabitatUI _habitatUI = null;

    public ExpeditionManager expeditionManager { get => _expeditionManager; }
    public ExpeditionSetupUI SetupUI { get => _setupPanel; }
    public ExpeditionInProgressUI InProgressUI { get => _inProgressPanel; }
    public StatefulObject ForegroundUIStates { get => _foregroundUIStates; }
    public StatefulObject BackgroundStates { get => _backgroundUIStates; }
    public ExpeditionResultUI ExpeditionResult { get => _resultPanel; }
    public Button CloseButton { get => _closeButton; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
        _selectionPanel.SetExpeditionManager(expeditionManager);
        _setupPanel.SetExpeditionManager(expeditionManager);
        _resultPanel.SetExpeditionManager(expeditionManager);
    }

    public void SetAudioManager(AudioManager audioManager)
    {
        _selectionPanel.SetAudioManager(audioManager);
        _setupPanel.SetAudioManager(audioManager);
        _resultPanel.SetAudioManager(audioManager);
    }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _habitatUI = _uiManager.HabitatUI;

        _selectionPanel.Initialize();
        _setupPanel.Initialize(this, uiManager);
        _resultPanel.Initialize(this, uiManager);

        SetupListeners();
    }

    public void SetupListeners()
    {
        _uiManager.CreateButtonListener(_closeButton, _uiManager.HabitatUI.ResetStandardUI);

        _setupPanel.SetupListeners();
        _resultPanel.SetupListeners();
    }

    public void ExpeditionButtonClick()
    {
        _uiManager.HabitatUI.OpenExpedtionSelectionDetails();

        OpenExpeditionUI();
    }

    public void OpenExpeditionUI()
    {
        switch (_expeditionManager.State)
        {
            case ExpeditionState.Selection:
            case ExpeditionState.Setup:
                _backgroundUIStates.SetState("Selection Panel", true);
                _foregroundUIStates.SetState("Transparent", true);
                _expeditionManager.LoadExpeditionOptions();
                _selectionPanel.DisplayExpeditionOptions();
                break;
            case ExpeditionState.InProgress:
                _backgroundUIStates.SetState("Setup Panel", true);
                _foregroundUIStates.SetState("In Progress Panel", true);
                break;
            case ExpeditionState.Result:
                _backgroundUIStates.SetState("Setup Panel", true);
                _foregroundUIStates.SetState("Results Panel", true);
                break;
            default:
                Debug.LogWarning($"Expedition state is not valid [{_expeditionManager.State}]. Please change!");
                break;
        }

        this.gameObject.SetActive(true);
    }

    public void LoadExpeditionSetup()
    {
        SetupUI.ToggleConfirmButton(false);

        _backgroundUIStates.SetState("Setup Panel");
        _foregroundUIStates.SetState("Transparent");

        _setupPanel.LoadExpeditionData();
        _expeditionManager.ExpeditionSetup();

        _habitatUI.UpdateHabitatUI();
    }

    public void CloseExpeditionUI()
    {
        _foregroundUIStates.SetState("Transparent");
        this.gameObject.SetActive(false);
        if(_expeditionManager.State == ExpeditionState.Setup)
        {
            _expeditionManager.RemoveAllChimeras();
        }
    }

    public void TimerComplete()
    {
        _inProgressPanel.gameObject.SetActive(false);
        _resultPanel.gameObject.SetActive(true);

        _expeditionManager.SetExpeditionState(ExpeditionState.Result);

        _resultPanel.DetermineReward();

        _expeditionManager.SetPortalColor();
    }

    public void CompleteCurrentHabitatExpedition()
    {
        _expeditionManager.SetExpeditionState(ExpeditionState.Result);

        _resultPanel.DetermineReward(true);

        _expeditionManager.SetPortalColor();
    }
}