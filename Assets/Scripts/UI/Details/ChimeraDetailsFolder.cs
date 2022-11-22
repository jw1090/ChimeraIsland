using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] private List<ChimeraDetails> _chimeraDetailsList = new List<ChimeraDetails>();
    [SerializeField] private Dropdown _dropdown = null;
    [SerializeField] private Image _bioButton = null;
    [SerializeField] private Image _aquaButton = null;
    [SerializeField] private Image _firaButton = null;
    private UIManager _uiManager = null;
    private List<Chimera> _chimerasList = new List<Chimera>();
    private ExpeditionManager _expeditionManager = null;
    private ChimeraOrderType orderType = ChimeraOrderType.Default;
    private bool _showGrass = true;
    private bool _showWater = true;
    private bool _showFire = true;

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }

    public void Initialize(UIManager uiManager)
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        _uiManager = uiManager;

        foreach (var chimeraDetail in _chimeraDetailsList)
        {
            chimeraDetail.Initialize(uiManager);
        }
        _dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });

        SetupListeners();
    }

    public void ToggleShowGrass() 
    { 
        _showGrass = !_showGrass;
        if(_showGrass == true)
        {
            _bioButton.color = Color.white;
        }
        else
        {
            _bioButton.color = new Color(0.63f, 0.63f, 0.63f);
        }
        CheckShowChimeraBasedOnElement();
    }

    public void ToggleShowWater()
    { 
        _showWater = !_showWater;

        if (_showWater == true)
        {
            _aquaButton.color = Color.white;
        }
        else
        {
            _aquaButton.color = new Color(0.63f, 0.63f, 0.63f);
        }

        CheckShowChimeraBasedOnElement();
    }

    public void ToggleShowFire() 
    { 
        _showFire = !_showFire;

        if (_showFire == true)
        {
            _firaButton.color = Color.white;
        }
        else
        {
            _firaButton.color = new Color(0.63f, 0.63f, 0.63f);
        }

        CheckShowChimeraBasedOnElement(); 
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

        _uiManager.CreateButtonListener(_aquaButton.gameObject.GetComponent<Button>(), ToggleShowWater);
        _uiManager.CreateButtonListener(_bioButton.gameObject.GetComponent<Button>(), ToggleShowGrass);
        _uiManager.CreateButtonListener(_firaButton.gameObject.GetComponent<Button>(), ToggleShowFire);
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

    private void CheckShowChimeraBasedOnElement()
    {
        foreach(var chimeraDetail in _chimeraDetailsList)
        {
            if (chimeraDetail.Chimera == null) return;
            switch (chimeraDetail.Chimera.ElementalType)
            {
                case ElementType.Water:
                    chimeraDetail.gameObject.SetActive(_showWater);
                    break;
                case ElementType.Grass:
                    chimeraDetail.gameObject.SetActive(_showGrass);
                    break;
                case ElementType.Fire:
                    chimeraDetail.gameObject.SetActive(_showFire);
                    break;
                default:
                    Debug.LogError($"Unhandled chimera element type: {chimeraDetail.Chimera.ElementalType}. Please change!");
                    chimeraDetail.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public void Sort()
    {
        for (int p = 0; p <= _chimeraDetailsList.Count - 2; p++)
        {
            for (int i = 0; i <= _chimeraDetailsList.Count - 2; i++)
            {
                if (_chimeraDetailsList[i].Chimera == null || _chimeraDetailsList[i+1].Chimera == null) break;
                bool higher = false;
                switch (orderType)
                {
                    case ChimeraOrderType.Default:
                        if (_chimeraDetailsList[i].Chimera.CurrentEvolution.ChimeradexId > _chimeraDetailsList[i + 1].Chimera.CurrentEvolution.ChimeradexId)
                        {
                            higher = true;
                        }
                        break;
                    case ChimeraOrderType.Exploration:
                        if (_chimeraDetailsList[i].Chimera.Exploration > _chimeraDetailsList[i + 1].Chimera.Exploration)
                        {
                            higher = true;
                        }
                        break;
                    case ChimeraOrderType.Stamina:
                        if (_chimeraDetailsList[i].Chimera.Stamina > _chimeraDetailsList[i + 1].Chimera.Stamina)
                        {
                            higher = true;
                        }
                        break;
                    case ChimeraOrderType.Wisdom:
                        if (_chimeraDetailsList[i].Chimera.Wisdom > _chimeraDetailsList[i + 1].Chimera.Wisdom)
                        {
                            higher = true;
                        }
                        break;
                    case ChimeraOrderType.AveragePower:
                        if (_chimeraDetailsList[i].Chimera.AveragePower > _chimeraDetailsList[i + 1].Chimera.AveragePower)
                        {
                            higher = true;
                        }
                        break;
                    default:
                        Debug.LogError($"Unhandled chimera order type: {orderType}. Please change!");
                        break;
                }

                if (higher == true)
                {
                    _chimeraDetailsList[i].gameObject.transform.SetSiblingIndex(i + 1);
                    ChimeraDetails temp = _chimeraDetailsList[i];
                    _chimeraDetailsList[i] = _chimeraDetailsList[i + 1];
                    _chimeraDetailsList[i + 1] = temp;
                }
            }
        }
    }
}