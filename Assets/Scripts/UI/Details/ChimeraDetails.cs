using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraDetails : MonoBehaviour
{
    [SerializeField] private DetailsTransferButton _detailsTransferButton = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _element = null;
    [SerializeField] private TextMeshProUGUI _endurance = null;
    [SerializeField] private TextMeshProUGUI _intelligence = null;
    [SerializeField] private TextMeshProUGUI _strength = null;
    [SerializeField] private Button _transferButton = null;
    [SerializeField] private Button _addButton = null;
    [SerializeField] private Button _removeButton = null;
    private Chimera _chimera = null;
    private Habitat _habitat = null;
    private int _chimeraSpot = 0;

    public Chimera Chimera { get => _chimera; }

    public void Initialize(int chimeraSpot)
    {
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _chimeraSpot = chimeraSpot;

        _detailsTransferButton.Initialize(this);

        UpdateDetails();
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

        _icon.sprite = _chimera.Icon;
        _name.text = $"{_chimera.Name}";
        _level.text = $"{_chimera.Level}";
        _element.text = $"{_chimera.ElementalType}";

        int amount = 0;
        string enduranceText = _chimera.GetStatByType(StatType.Endurance, out amount) ? amount.ToString() : "Invalid!";
        _endurance.text = enduranceText;
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
}