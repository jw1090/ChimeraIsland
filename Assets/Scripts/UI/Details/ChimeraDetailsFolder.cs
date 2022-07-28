using System.Collections.Generic;
using UnityEngine;

public class ChimeraDetailsFolder : MonoBehaviour
{
    private List<Chimera> _chimerasList = new List<Chimera>();
    private List<ChimeraDetails> _chimeraDetailsList = new List<ChimeraDetails>();

    public void Initialize()
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        _chimerasList = ServiceLocator.Get<HabitatManager>().CurrentHabitat.ActiveChimeras;

        int chimeraSpot = 0;
        foreach (Transform child in transform)
        {
            ChimeraDetails details = child.GetComponent<ChimeraDetails>();

            _chimeraDetailsList.Add(details);
            details.Initialize(chimeraSpot++);
        }

        CheckDetails();
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