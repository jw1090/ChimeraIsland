using System.Collections.Generic;
using UnityEngine;

public class DetailsManager : MonoBehaviour
{
    [SerializeField] private List<ChimeraDetailsFolder> _detailsPanels = new List<ChimeraDetailsFolder>();
    [SerializeField] private StatefulObject _detailsStates = null;
    
    public bool IsOpen { get => _detailsStates.CurrentState.StateName != "Transparent"; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsPanels)
        {
            detailsPanel.SetExpeditionManager(expeditionManager);
        }
    }

    public void Initialize(UIManager uiManager)
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        foreach(ChimeraDetailsFolder detailsPanel in _detailsPanels)
        {
            detailsPanel.Initialize(uiManager);
        }
    }

    public void CloseDetails()
    {
        _detailsStates.SetState("Transparent", true);
    }

    public void OpenStandardDetails()
    {
        _detailsStates.SetState("Details 2-Wide", true);
    }

    public void OpenExpeditionDetails()
    {
        _detailsStates.SetState("Details 2-Wide", true);
    }

    public void HabitatDetailsSetup()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsPanels)
        {
            detailsPanel.HabitatDetailsSetup();
        }
    }

    public void CheckDetails()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsPanels)
        {
            detailsPanel.CheckDetails();
        }
    }

    public void UpdateDetailsList()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsPanels)
        {
            detailsPanel.UpdateDetailsList();
        }
    }

    public void DetailsStatGlow()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsPanels)
        {
            detailsPanel.DetailsStatGlow();
        }
    }
}