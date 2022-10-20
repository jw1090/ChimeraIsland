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

    public Button AcceptButton { get => _acceptButton.Button; }
    public Button DeclineButton { get => _declineButton; }

    public ChimeraType ChimeraType { get => _acceptButton.ChimeraType; }
    public void SetChimeraType(ChimeraType chimeraType) { _acceptButton.SetChimeraType(chimeraType); }

    public void Initialize(UIManager uIManager)
    {
        _uiManager = uIManager;
        _acceptButton.Initialize();

        SetupListeners();
    }

    public void OnSceneStart()
    {
        _acceptButton.SetupStartingButton();
        ResetUI();
    }

    public void SetupListeners()
    {
        _uiManager.CreateButtonListener(DeclineButton,ResetUI);
        _uiManager.CreateButtonListener(AcceptButton, ChimeraDecision);
    }

    public void OpenChimeraInfo()
    {
        _container.SetActive(true);
        _title.gameObject.SetActive(false);
    }
    public void LoadChimeraInfo()
    {
        _startingChimeraInfo.LoadChimeraData(_acceptButton.GetChimeraName(),_acceptButton.GetChimeraElement(),_acceptButton.GetChimeraBio());
    }

    private void ResetUI()
    {
        _container.SetActive(false);
        _title.gameObject.SetActive(true);
    }

    private void ChimeraDecision()
    {
        _acceptButton.ChimeraClicked(ChimeraType);
    }


}