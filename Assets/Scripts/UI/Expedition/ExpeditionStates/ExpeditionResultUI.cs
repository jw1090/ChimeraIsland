using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _successResults = null;
    [SerializeField] private TextMeshProUGUI _resultsDescription = null;
    [SerializeField] private Button _rewardsCloseButton = null;
    private ExpeditionUI _expeditionUI = null;
    private UIManager _uiManager = null;
    private ExpeditionManager _expeditionManager = null;
    private bool _expeditionSuccess = false;

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
    }

    public void Initialize(ExpeditionUI expeditionUI, UIManager uiManager)
    {
        _expeditionUI = expeditionUI;
        _uiManager = uiManager;
    }

    public void SetupListeners()
    {
        _uiManager.CreateButtonListener(_rewardsCloseButton, ResultsCloseClick);
    }

    private void ResultsCloseClick()
    {
        _expeditionManager.SetExpeditionState(ExpeditionState.Selection);

        if (_expeditionSuccess == true) // Success
        {
            _expeditionManager.SuccessRewards();

            if (_expeditionManager.SelectedExpedition.Type == ExpeditionType.HabitatUpgrade
                || _expeditionManager.SelectedExpedition.Type == ExpeditionType.Fossils
                && _expeditionManager.CurrentFossilProgress == 1)
            {
                _uiManager.HabitatUI.ResetStandardUI();
            }
            else
            {
                _expeditionUI.OpenExpeditionUI();
            }
        }
        else
        {
            _expeditionUI.OpenExpeditionUI();
        }

        _expeditionManager.ResetSelectedExpedition();
        _expeditionSuccess = false;
    }

    public void DetermineReward(bool bypass = false)
    {
        bool results = bypass ? bypass : _expeditionManager.RandomSuccesRate();

        if (results == true)
        {
            _successResults.text = $"Success";

            ExpeditionData expeditionData = _expeditionManager.SelectedExpedition;

            switch (expeditionData.Type)
            {
                case ExpeditionType.Essence:
                    _resultsDescription.text = $"You've gained {expeditionData.ActualAmountGained} Essence!";
                    break;
                case ExpeditionType.Fossils:
                    if (expeditionData.UnlocksNewChimera == true)
                    {
                        _resultsDescription.text = $"You unlocked a new Chimera in the Marketplace and gained {expeditionData.ActualAmountGained} Fossils!";
                    }
                    else
                    {
                        _resultsDescription.text = $"You gained {expeditionData.ActualAmountGained} Fossils!";
                    }
                    break;
                case ExpeditionType.HabitatUpgrade:
                    switch (expeditionData.UpgradeType)
                    {
                        case HabitatRewardType.Waterfall:
                            _resultsDescription.text = $"You unlocked the Waterfall Facility!";
                            break;
                        case HabitatRewardType.CaveExploring:
                            _resultsDescription.text = $"You unlocked the Explorable Cave Facility!";
                            break;
                        case HabitatRewardType.RuneStone:
                            _resultsDescription.text = $"You unlocked the Rune Stones Facility!";
                            break;
                        case HabitatRewardType.Habitat:
                            _resultsDescription.text = $"You upgraded the Habitat. Facilities can be upgraded in the Marketplace!";
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
            if (bypass == true)
            {
                ResultsCloseClick();
            }
        }
        else
        {
            _successResults.text = $"Failure";
            _resultsDescription.text = $"Train your Chimeras and try again!";
            _expeditionSuccess = false;
        }
    }
}