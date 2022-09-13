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
        foreach (var detail in _chimeraDetailsList)
        {
            detail.UpdateDetails();
        }

        ToggleDetailsButtons();
    }

    public void CheckDetails()
    {
        for (int i = 0; i < _chimerasList.Count; ++i)
        {
            _chimeraDetailsList[i].gameObject.SetActive(true);
        }
        UpdateDetailsList();
    }

    private void ToggleDetailsButtons(DetailsButtonType detailsButtonType)
    {
        foreach (var detail in _chimeraDetailsList)
        {
            detail.ToggleButtons(detailsButtonType);
        }
    }

    public void ToggleDetailsButtons()
    {
        if (_expeditionManager.State == ExpeditionState.Setup)
        {
            ToggleDetailsButtons(DetailsButtonType.ExpeditionParty);
        }
        else
        {
            ToggleDetailsButtons(DetailsButtonType.Standard);
        }
    }
}