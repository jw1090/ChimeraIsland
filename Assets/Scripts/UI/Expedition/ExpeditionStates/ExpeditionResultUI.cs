using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _successResults = null;
    [SerializeField] private TextMeshProUGUI _resultsDescription = null;
    [SerializeField] private Button _rewardsCloseButton = null;
    private UIManager _uiManager = null;
    private ExpeditionManager _expeditionManager = null;
    private bool _expeditionSuccess = false;

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
    }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void SetupListeners()
    {
        _uiManager.CreateButtonListener(_rewardsCloseButton, ResultsCloseClick);
    }

    private void ResultsCloseClick()
    {
        if (_expeditionSuccess == true) // Success
        {
            _expeditionManager.SuccessRewards();
            _expeditionSuccess = false;
        }

        _expeditionManager.SetExpeditionState(ExpeditionState.Selection);
        _expeditionManager.ResetSelectedExpedition();

        _uiManager.HabitatUI.ResetStandardUI();
        _uiManager.HabitatUI.ExpeditionButton.ActivateNotification(false);
    }

    public void DetermineReward()
    {
        if (_expeditionManager.RandomSuccesRate() == true)
        {
            _successResults.text = $"Success";

            if (_expeditionManager.SelectedExpedition.Type == ExpeditionType.HabitatUpgrade)
            {
                _resultsDescription.text = $"Your Habitat has been upgraded!";
            }
            else
            {
                _resultsDescription.text = $"You've gained 1 Fossil!";
            }


            _expeditionSuccess = true;
        }
        else
        {
            _successResults.text = $"Failure";
            _resultsDescription.text = $"Train your Chimera and try again!";
            _expeditionSuccess = false;
        }
    }
}