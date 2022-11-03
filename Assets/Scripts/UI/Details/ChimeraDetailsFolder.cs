using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] private List<ChimeraDetails> _chimeraDetailsList = new List<ChimeraDetails>();
    [SerializeField] private Dropdown _dropdown = null;
    private List<Chimera> _chimerasList = new List<Chimera>();
    private ExpeditionManager _expeditionManager = null;
    private ChimeraOrderType orderType = ChimeraOrderType.Type;

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }

    public void Initialize(UIManager uiManager)
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        foreach (var chimeraDetail in _chimeraDetailsList)
        {
            chimeraDetail.Initialize(uiManager);
        }
        _dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });

        SetupListeners();
    }

    private void DropdownValueChanged()
    {
        orderType = (ChimeraOrderType)_dropdown.value;
        Sort();
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
        Sort();
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

    public void Sort()
    {
        for (int i = 1; i < _chimeraDetailsList.Count; i++)
        {
            if (_chimeraDetailsList[i].gameObject.activeSelf == false) return;
            bool higher = false;
            switch (orderType)
            {
                case ChimeraOrderType.Type:
                    int num = (int)_chimeraDetailsList[i].Chimera.ChimeraType;
                    int num2 = (int)_chimeraDetailsList[i-1].Chimera.ChimeraType;
                    if ((int)_chimeraDetailsList[i].Chimera.ChimeraType > (int)_chimeraDetailsList[i-1].Chimera.ChimeraType)
                    {
                        higher = true;
                    }
                    break;
                case ChimeraOrderType.highestStat:
                    if (_chimeraDetailsList[i].Chimera.GetHighestStat() > _chimeraDetailsList[i - 1].Chimera.GetHighestStat())
                    {
                        higher = true;
                    }
                    break;
                case ChimeraOrderType.AveragePower:
                    if (_chimeraDetailsList[i].Chimera.AveragePower > _chimeraDetailsList[i - 1].Chimera.AveragePower)
                    {
                        higher = true;
                    }
                    break;
                default:
                    Debug.LogError($"Unhandled chimera order type: {orderType}. Please change!");
                    break;
            }
            if(higher == true)
            {
                _chimeraDetailsList[i].gameObject.transform.SetSiblingIndex(i - 1);
                ChimeraDetails temp = _chimeraDetailsList[i];
                _chimeraDetailsList[i] = _chimeraDetailsList[i - 1];
                _chimeraDetailsList[i - 1] = temp;
                i = 0;
            }
        }
    }
}