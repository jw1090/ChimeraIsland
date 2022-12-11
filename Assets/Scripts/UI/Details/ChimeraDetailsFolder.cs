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
    [SerializeField] private Transform _detailsHierarchyParent = null;

    private UIManager _uiManager = null;
    private HabitatManager _habitatManager = null;
    private ExpeditionManager _expeditionManager = null;
    private List<Chimera> _chimerasList = new List<Chimera>();
    private ChimeraOrderType orderType = ChimeraOrderType.AveragePower;
    private GameObject _detailsPrefab = null;
    private bool _showGrass = true;
    private bool _showWater = true;
    private bool _showFire = true;

    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }

    public void Initialize(UIManager uiManager, GameObject detailsPrefab)
    {
        _uiManager = uiManager;
        _detailsPrefab = detailsPrefab;

        _habitatManager = ServiceLocator.Get<HabitatManager>();

        SetupListeners();
    }

    private void SetupListeners()
    {
        _uiManager.CreateButtonListener(_aquaButton.gameObject.GetComponent<Button>(), ToggleShowWater);
        _uiManager.CreateButtonListener(_bioButton.gameObject.GetComponent<Button>(), ToggleShowGrass);
        _uiManager.CreateButtonListener(_firaButton.gameObject.GetComponent<Button>(), ToggleShowFire);

        _dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
    }

    public void IncreaseChimeraDetailsInstance()
    {
        if (_detailsHierarchyParent.childCount > _habitatManager.ChimerasInHabitat.Count)
        {
            return;
        }

        GameObject newDetailsGO = Instantiate(_detailsPrefab, _detailsHierarchyParent);
        ChimeraDetails newDetailsComp = newDetailsGO.GetComponent<ChimeraDetails>();

        newDetailsComp.Initialize(_uiManager);
        _chimeraDetailsList.Add(newDetailsComp);

        newDetailsGO.SetActive(false);
    }

    public void ToggleShowGrass()
    {
        _showGrass = !_showGrass;
        if (_showGrass == true)
        {
            _bioButton.color = Color.white;
        }
        else
        {
            _bioButton.color = new Color(0.63f, 0.63f, 0.63f);
        }
        EvaluateVisibleChimera();
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

        EvaluateVisibleChimera();
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

        EvaluateVisibleChimera();
    }

    private void DropdownValueChanged()
    {
        orderType = (ChimeraOrderType)_dropdown.value;
        Sort();
    }

    public void HabitatDetailsSetup()
    {
        _chimerasList = _habitatManager.CurrentHabitat.ActiveChimeras;

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
        EvaluateVisibleChimera();
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

    private void EvaluateVisibleChimera()
    {
        foreach (var chimeraDetail in _chimeraDetailsList)
        {
            if (chimeraDetail.Chimera == null)
            {
                return;
            }

            if (chimeraDetail.Chimera.ElementalType == ElementType.None)
            {
                return;
            }

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
        //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        //sw.Start();

        for (int p = 0; p <= _chimeraDetailsList.Count - 2; p++)
        {
            for (int i = 0; i <= _chimeraDetailsList.Count - 2; i++)
            {
                if (_chimeraDetailsList[i].Chimera == null || _chimeraDetailsList[i + 1].Chimera == null)
                {
                    break;
                }

                bool lower = false;
                switch (orderType)
                {
                    case ChimeraOrderType.Exploration:
                        if (_chimeraDetailsList[i].Chimera.Exploration < _chimeraDetailsList[i + 1].Chimera.Exploration)
                        {
                            lower = true;
                        }
                        break;
                    case ChimeraOrderType.Stamina:
                        if (_chimeraDetailsList[i].Chimera.Stamina < _chimeraDetailsList[i + 1].Chimera.Stamina)
                        {
                            lower = true;
                        }
                        break;
                    case ChimeraOrderType.Wisdom:
                        if (_chimeraDetailsList[i].Chimera.Wisdom < _chimeraDetailsList[i + 1].Chimera.Wisdom)
                        {
                            lower = true;
                        }
                        break;
                    case ChimeraOrderType.AveragePower:
                        if (_chimeraDetailsList[i].Chimera.AveragePower < _chimeraDetailsList[i + 1].Chimera.AveragePower)
                        {
                            lower = true;
                        }
                        break;
                    default:
                        Debug.LogError($"Unhandled chimera order type: {orderType}. Please change!");
                        break;
                }

                if (lower == true)
                {
                    _chimeraDetailsList[i].gameObject.transform.SetSiblingIndex(i + 1);
                    ChimeraDetails temp = _chimeraDetailsList[i];
                    _chimeraDetailsList[i] = _chimeraDetailsList[i + 1];
                    _chimeraDetailsList[i + 1] = temp;
                }
            }
        }

        //sw.Stop();
        //Debug.Log(sw.ElapsedMilliseconds);
    }
}