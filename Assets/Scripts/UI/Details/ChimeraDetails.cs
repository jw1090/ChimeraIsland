using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraDetails : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private int _chimeraSpot = 0;
    [SerializeField] private Habitat _habitat = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _element = null;
    [SerializeField] private TextMeshProUGUI _endurance = null;
    [SerializeField] private TextMeshProUGUI _intelligence = null;
    [SerializeField] private TextMeshProUGUI _strength = null;
    [SerializeField] private TextMeshProUGUI _happiness = null;
    [SerializeField] private OpenTransferMapButton _openTransferMapButton = null;

    public Chimera ChimeraInfo{ get; private set; } = null;

    public void Initialize(Habitat habitat, int chimeraSpot)
    {
        _habitat = habitat;
        _chimeraSpot = chimeraSpot;
        UpdateDetails();
        _openTransferMapButton.Initalize(this);
    }

    public void UpdateDetails()
    {
        if (_habitat == null)
        {
            return;
        }

        if (_habitat.Chimeras.Count <= _chimeraSpot)
        {
            gameObject.SetActive(false);
            return;
        }

        ChimeraInfo = _habitat.Chimeras[_chimeraSpot];

        _icon.sprite = ChimeraInfo.GetIcon();
        _level.text = "Level: " + ChimeraInfo.Level;
        _element.text = "Element: " + ChimeraInfo.GetElementalType().ToString();

        int amount;
        string enduranceText = ChimeraInfo.GetStatByType(StatType.Endurance, out amount) ? amount.ToString() : "Invalid!";
        _endurance.text = enduranceText;
        string intelligenceText = ChimeraInfo.GetStatByType(StatType.Intelligence, out amount) ? amount.ToString() : "Invalid!";
        _intelligence.text = intelligenceText;
        string strengthText = ChimeraInfo.GetStatByType(StatType.Strength, out amount) ? amount.ToString() : "Invalid!";
        _strength.text = strengthText;
        string happinessText = ChimeraInfo.GetStatByType(StatType.Happiness, out amount) ? amount.ToString() : "Invalid!";
        _happiness.text = happinessText;
    }
}