using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraDetails : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Habitat _habitat;
    [SerializeField] private int _chimeraSpot;
    [SerializeField] private Chimera _chimera;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _intelligence;
    [SerializeField] private TextMeshProUGUI _endurance;
    [SerializeField] private TextMeshProUGUI _strength;
    [SerializeField] private TextMeshProUGUI _happiness;
    [SerializeField] private TextMeshProUGUI _element;

    public void Initialize(Habitat habitat, int chimeraSpot)
    {
        _habitat = habitat;
        _chimeraSpot = chimeraSpot;
        UpdateDetails();
    }

    public void UpdateDetails()
    {
        // Check if the active chimer count matches the slot.
        if (_habitat.GetChimeras().Count <= _chimeraSpot)
        {
            gameObject.SetActive(false);
            return;
        }

        _chimera = _habitat.GetChimeras()[_chimeraSpot];

        Debug.Log("Chimera: " + _habitat.GetChimeras()[_chimeraSpot]);

        _level.text = "Level: " + _chimera.GetLevel();
        _endurance.text = _chimera.GetStatByType(StatType.Endurance).ToString();
        _intelligence.text = _chimera.GetStatByType(StatType.Intelligence).ToString();
        _strength.text = _chimera.GetStatByType(StatType.Strength).ToString();
        _happiness.text = _chimera.GetStatByType(StatType.Happiness).ToString();
        _element.text = "Element: " + _chimera.GetElementalType().ToString();
        _icon.sprite = _chimera.GetIcon();
    }

    public int GetChimeraSpot() { return _chimeraSpot; }
}