using System.Collections.Generic;
using UnityEngine;

public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] private List<ChimeraDetails> _chimeraDetailsList = new List<ChimeraDetails>();
    private List<Chimera> _chimerasList = new List<Chimera>();

    public void Initialize(UIManager uiManager)
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        foreach (var chimeraDetail in _chimeraDetailsList)
        {
            chimeraDetail.Initialize(uiManager);
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

    public void SetupButtonListeners()
    {
        foreach (var detail in _chimeraDetailsList)
        {
            detail.SetupButtonListeners();
        }
    }

    public void UpdateDetailsList()
    {
        foreach (var detail in _chimeraDetailsList)
        {
            detail.UpdateDetails();
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

    public void ToggleDetailsButtons(DetailsButtonType detailsButtonType)
    {
        foreach (var detail in _chimeraDetailsList)
        {
            detail.ToggleButtons(detailsButtonType);
        }
    }
}