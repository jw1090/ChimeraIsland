using UnityEngine;

public class ExpeditionUI : MonoBehaviour
{
    [SerializeField] private ExpeditionSelectionUI _selectionPanel = null;
    [SerializeField] private ExpeditionSetupUI _setupPanel = null;
    [SerializeField] private ExpeditionInProgressUI _inProgressPanel = null;
    [SerializeField] private ExpeditionResultUI _resultPanel = null;
    [SerializeField] private StatefulObject _backgroundUIStates = null;
    [SerializeField] private StatefulObject _foregroundUIStates = null;

    public ExpeditionSelectionUI SelectionUI { get => _selectionPanel; }
    public ExpeditionSetupUI SetupUI { get => _setupPanel; }
    public ExpeditionInProgressUI InProgressUI { get => _inProgressPanel; }
    public ExpeditionResultUI ResultsUI { get => _resultPanel; }
    public StatefulObject BackgroundUIStates { get => _backgroundUIStates; }
    public StatefulObject ForegroundUIStates { get => _foregroundUIStates; }

    public bool ExpeditionSuccess { get; set; }

    private ExpeditionManager _expeditionManager = null;
    private UIManager _uiManager = null;
    private ChimeraDetailsFolder _detailsFolder = null;

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
        _setupPanel.SetExpeditionManager(expeditionManager);
        _resultPanel.SetExpeditionManager(expeditionManager);
    }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _detailsFolder = _uiManager.HabitatUI.DetailsPanel;

        _selectionPanel.Initialize();
        _setupPanel.Initialize(uiManager, this);
        _resultPanel.Initialize(uiManager, this);

        SetupListeners();
    }

    public void SetupListeners()
    {
        _selectionPanel.SetupListeners();
        _setupPanel.SetupListeners();
        _resultPanel.SetupListeners();
    }

    public void OpenExpeditionUI()
    {
        _uiManager.HabitatUI.OpenStandardDetailsPanel();

        switch (_expeditionManager.State)
        {
            case ExpeditionState.Selection:
                _backgroundUIStates.SetState("Selection Panel");
                _foregroundUIStates.SetState("Transparent");
                _selectionPanel.LoadSelectionExpeditions();
                break;
            case ExpeditionState.InProgress:
                _backgroundUIStates.SetState("Selection Panel");
                _foregroundUIStates.SetState("In Progress Panel");
                break;
            case ExpeditionState.Result:
                _backgroundUIStates.SetState("Selection Panel");
                _foregroundUIStates.SetState("Results Panel");
                break;
            default:
                Debug.LogWarning($"Expedition state is not valid [{_expeditionManager.State}]. Please change!");
                break;
        }

        this.gameObject.SetActive(true);
    }

    public void LoadExpeditionSetup()
    {
        _setupPanel.ResetChimeraIcons();

        if (_expeditionManager.State == ExpeditionState.Selection)
        {
            _expeditionManager.ExpeditionSetup();
            ExpeditionSuccess = false;
        }
    }

    public void CloseExpeditionUI()
    {
        _inProgressPanel.gameObject.SetActive(false);
        _resultPanel.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    public void TimerComplete()
    {
        _uiManager.HabitatUI.ExpeditionButton.ActivateNotification(true);

        _inProgressPanel.gameObject.SetActive(false);
        _resultPanel.gameObject.SetActive(true);

        _expeditionManager.SetExpeditionState(ExpeditionState.Result);

        _resultPanel.DetermineReward();
    }

    public void PostExpeditionCleanup(bool onExpedition)
    {
        _expeditionManager.PostExpeditionCleanup(onExpedition);

        _detailsFolder.ToggleDetailsButtons(DetailsButtonType.Expedition);
    }
}