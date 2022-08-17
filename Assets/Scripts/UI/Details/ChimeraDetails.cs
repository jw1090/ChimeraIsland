using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraDetails : MonoBehaviour
{
    [SerializeField] private Image _chimeraIcon = null;
    [SerializeField] private Image _elementIcon = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _exploration = null;
    [SerializeField] private TextMeshProUGUI _stamina = null;
    [SerializeField] private TextMeshProUGUI _wisdom = null;
    [SerializeField] private StatefulObject _statefulButtons = null;
    [SerializeField] private Button _transferButton = null;
    [SerializeField] private Button _addButton = null;
    [SerializeField] private Button _removeButton = null;
    [SerializeField] private TextMeshProUGUI _occupiedText = null;
    private Chimera _chimera = null;
    private Habitat _habitat = null;
    private UIManager _uiManager = null;
    private ExpeditionManager _expeditionManager = null;
    private int _chimeraSpot = 0;

    public Chimera Chimera { get => _chimera; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void HabitatDetailsSetup(int chimeraSpot)
    {
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _expeditionManager = ServiceLocator.Get<ExpeditionManager>();

        _chimeraSpot = chimeraSpot;

        UpdateDetails();
    }

    public void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_transferButton, TransferMapClicked);
        _uiManager.CreateButtonListener(_addButton, AddChimeraClicked);
        _uiManager.CreateButtonListener(_removeButton, RemoveChimeraClicked);
    }

    public void UpdateDetails()
    {
        if (_habitat == null)
        {
            return;
        }

        if (_habitat.ActiveChimeras.Count <= _chimeraSpot)
        {
            gameObject.SetActive(false);
            return;
        }

        _chimera = _habitat.ActiveChimeras[_chimeraSpot];

        _name.text = $"{_chimera.Name}";
        _level.text = $"{_chimera.Level}";
        _chimeraIcon.sprite = _chimera.ChimeraIcon;
        _elementIcon.sprite = _chimera.ElementIcon;


        int amount = 0;
        string staminaText = _chimera.GetStatByType(StatType.Stamina, out amount) ? amount.ToString() : "Invalid!";
        _stamina.text = staminaText;
        string wisdomText = _chimera.GetStatByType(StatType.Wisdom, out amount) ? amount.ToString() : "Invalid!";
        _wisdom.text = wisdomText;
        string explorationText = _chimera.GetStatByType(StatType.Exploration, out amount) ? amount.ToString() : "Invalid!";
        _exploration.text = explorationText;
    }

    public void ToggleButtons(DetailsButtonType detailsButtonType)
    {
        if (_chimera == null)
        {
            return;
        }

        if (_chimera.InFacility == true)
        {
            _statefulButtons.SetState("Occupied");
            _occupiedText.text = $"Training";

            return;
        }
        else if (_chimera.OnExpedition == true)
        {
            _statefulButtons.SetState("Occupied");
            _occupiedText.text = $"On Expedition";

            return;
        }

        switch (detailsButtonType)
        {
            case DetailsButtonType.Standard:
                _statefulButtons.SetState("Grazing", true);
                break;
            case DetailsButtonType.Expedition:
                if (_expeditionManager.HasChimeraBeenAdded(_chimera) == true)
                {
                    _statefulButtons.SetState("Remove Button");
                }
                else
                {
                    _statefulButtons.SetState("Add Button");
                }
                break;
            default:
                Debug.LogWarning($"{detailsButtonType} is not a valid type. Please fix!");
                break;
        }
    }

    private void TransferMapClicked()
    {
        _uiManager.HabitatUI.OpenTransferMap(_chimera);
    }

    private void AddChimeraClicked()
    {
        if (_expeditionManager.AddChimera(_chimera) == true) // Success
        {
            _statefulButtons.SetState("Remove Button");
        }
    }

    private void RemoveChimeraClicked()
    {
        if (_expeditionManager.RemoveChimera(_chimera) == true) // Success
        {
            _statefulButtons.SetState("Add Button");
        }
    }
}