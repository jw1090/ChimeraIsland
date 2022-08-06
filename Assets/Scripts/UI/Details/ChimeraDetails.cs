using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraDetails : MonoBehaviour
{
    [SerializeField] private Image _chimeraIcon = null;
    [SerializeField] private Image _elementIcon = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _agility = null;
    [SerializeField] private TextMeshProUGUI _intelligence = null;
    [SerializeField] private TextMeshProUGUI _strength = null;
    [SerializeField] private Button _transferButton = null;
    [SerializeField] private Button _addButton = null;
    [SerializeField] private Button _removeButton = null;
    private Chimera _chimera = null;
    private Habitat _habitat = null;
    private UIManager _uiManager = null;
    private ExpeditionManager _expeditionManager = null;
    private int _chimeraSpot = 0;

    public Chimera Chimera { get => _chimera; }

    public void Initialize(int chimeraSpot)
    {
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _uiManager = ServiceLocator.Get<UIManager>();
        _expeditionManager = ServiceLocator.Get<ExpeditionManager>();

        _chimeraSpot = chimeraSpot;

        SetupButtonListeners();
        UpdateDetails();
    }

    private void SetupButtonListeners()
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
        string agilityText = _chimera.GetStatByType(StatType.Agility, out amount) ? amount.ToString() : "Invalid!";
        _agility.text = agilityText;
        string intelligenceText = _chimera.GetStatByType(StatType.Intelligence, out amount) ? amount.ToString() : "Invalid!";
        _intelligence.text = intelligenceText;
        string strengthText = _chimera.GetStatByType(StatType.Strength, out amount) ? amount.ToString() : "Invalid!";
        _strength.text = strengthText;
    }

    public void ToggleButtons(DetailsButtonType detailsButtonType)
    {
        _transferButton.gameObject.SetActive(false);
        _addButton.gameObject.SetActive(false);
        _removeButton.gameObject.SetActive(false);

        switch (detailsButtonType)
        {
            case DetailsButtonType.Standard:
                _transferButton.gameObject.SetActive(true);
                break;
            case DetailsButtonType.Expedition:
                _addButton.gameObject.SetActive(true);
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
        if(_expeditionManager.AddChimera(_chimera) == true) // Success
        {
            _addButton.gameObject.SetActive(false);
            _removeButton.gameObject.SetActive(true);
        }
    }

    private void RemoveChimeraClicked()
    {
        if(_expeditionManager.RemoveChimera(_chimera) == true) // Success
        {
            _addButton.gameObject.SetActive(true);
            _removeButton.gameObject.SetActive(false);
        }
    }
}