using System.Collections.Generic;
using UnityEngine;

public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] private List<ChimeraDetails> _chimeraDetailsList = new List<ChimeraDetails>();
    private List<Chimera> _chimerasList = new List<Chimera>();
    private ExpeditionManager _expeditionManager = null;

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }

    public void Initialize(UIManager uiManager)
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        foreach (var chimeraDetail in _chimeraDetailsList)
        {
            chimeraDetail.Initialize(uiManager);
        }

        SetupListeners();
    }

    private void SetupListeners()
    {
        foreach (var detail in _chimeraDetailsList)
        {
            detail.SetupButtonListeners();
        }
    }

    public void HabitatDetailsSetup()
    {
        _chimerasList = ServiceLocator.Get<HabitatManager>().CurrentHabitat.ActiveChimeras;

        int chimeraSpot = 0;
        foreach (var chimeraDetail in _chimeraDetailsList)
        {
            chimeraDetail.HabitatDetailsSetup(chimeraSpot++);
        }

        CheckDetails();
    }

    public void UpdateDetailsList()
    {
        // Check Expedition state to determine button layout
        DetailsButtonType detailsButtonType = _expeditionManager.State == ExpeditionState.Setup ? DetailsButtonType.ExpeditionParty : DetailsButtonType.Standard;

        foreach (var detail in _chimeraDetailsList)
        {
            detail.UpdateDetails();
            detail.ToggleButtons(detailsButtonType);
        }
    }

    public void CheckDetails()
    {
        for (int i = 0; i < _chimerasList.Count; ++i)
        {
            _chimeraDetailsList[i].gameObject.SetActive(true);
        }

        UpdateDetailsList();
    }

    public void DetailsStatGlow()
    {
        foreach (var detail in _chimeraDetailsList)
        {
            detail.DetermineStatGlow();
        }
    }
}