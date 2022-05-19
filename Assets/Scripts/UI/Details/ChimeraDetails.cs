using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraDetails : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private int _chimeraSpot = 0;
    [SerializeField] private Habitat _habitat = null;
    [SerializeField] private Chimera _chimera = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _element = null;
    [SerializeField] private TextMeshProUGUI _endurance = null;
    [SerializeField] private TextMeshProUGUI _intelligence = null;
    [SerializeField] private TextMeshProUGUI _strength = null;
    [SerializeField] private TextMeshProUGUI _happiness = null;

    public void Initialize(Habitat habitat, int chimeraSpot)
    {
        _habitat = habitat;
        _chimeraSpot = chimeraSpot;
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

        _icon.sprite = _chimera.GetIcon();
        _level.text = "Level: " + _chimera.Level;
        _element.text = "Element: " + _chimera.ElementalType.ToString();

        int amount;
        string enduranceText = _chimera.GetStatByType(StatType.Endurance, out amount) ? amount.ToString() : "Invalid!";
        _endurance.text = enduranceText;
        string intelligenceText = _chimera.GetStatByType(StatType.Intelligence, out amount) ? amount.ToString() : "Invalid!";
        _intelligence.text = intelligenceText;
        string strengthText = _chimera.GetStatByType(StatType.Strength, out amount) ? amount.ToString() : "Invalid!";
        _strength.text = strengthText;
        string happinessText = _chimera.GetStatByType(StatType.Happiness, out amount) ? amount.ToString() : "Invalid!";
        _happiness.text = happinessText;
    }
}