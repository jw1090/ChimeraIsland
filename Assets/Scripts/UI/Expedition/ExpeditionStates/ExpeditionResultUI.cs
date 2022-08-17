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

            ExpeditionData expeditionData = _expeditionManager.SelectedExpedition;

            switch (expeditionData.Type)
            {
                case ExpeditionType.Essence:
                    _resultsDescription.text = $"You've gained {expeditionData.AmountGained} Fossil!";
                    break;
                case ExpeditionType.Fossils:
                    _resultsDescription.text = $"You've gained {expeditionData.AmountGained} Fossils!";
                    break;
                case ExpeditionType.HabitatUpgrade:
                    switch (expeditionData.UpgradeType)
                    {
                        case HabitatRewardType.Waterfall:
                            break;
                        case HabitatRewardType.CaveExploring:
                            break;
                        case HabitatRewardType.RuneStone:
                            break;
                        case HabitatRewardType.Habitat:
                            _resultsDescription.text = $"Your Habitat has been upgraded!";
                            break;
                        default:
                            Debug.LogError($"Upgrade type is invalid [{expeditionData.UpgradeType}], please change!");
                            break;
                    }
                    break;
                default:
                    Debug.LogWarning($"Reward type is not valid [{expeditionData.Type}], please change!");
                    break;
            }

            _expeditionSuccess = true;
        }
        else
        {
            _successResults.text = $"Failure";
            _resultsDescription.text = $"Train your Chimeras and try again!";
            _expeditionSuccess = false;
        }
    }
}