using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraDetails : MonoBehaviour
{
    [SerializeField] private Image _chimeraIcon = null;
    [SerializeField] private Image _elementIcon = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TMP_InputField _customName = null;
    [SerializeField] private GameObject _panel = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _exploration = null;
    [SerializeField] private TextMeshProUGUI _stamina = null;
    [SerializeField] private TextMeshProUGUI _wisdom = null;
    [SerializeField] private Slider _energySlider = null;
    [SerializeField] private TextMeshProUGUI _energyText = null;
    [SerializeField] private StatefulObject _statefulButtons = null;
    [SerializeField] private Button _findButton = null;
    [SerializeField] private Button _addButton = null;
    [SerializeField] private Button _removeButton = null;
    [SerializeField] private Button _renameButton = null;
    [SerializeField] private TextMeshProUGUI _occupiedText = null;
    [SerializeField] private RectTransform _explorationTransform = null;
    [SerializeField] private RectTransform _staminaTransform = null;
    [SerializeField] private RectTransform _wisdomTransform = null;

    [Header("Stat Preference")]
    [SerializeField] private Color _prefferedGoldColor = new Color();
    [SerializeField] private Color _defaultColor = new Color();
    [SerializeField] private List<Image> _statIcons = new List<Image>();

    private Chimera _chimera = null;
    private Habitat _habitat = null;
    private UIManager _uiManager = null;
    private Tooltip _tooltip = null;
    private ExpeditionManager _expeditionManager = null;
    private AudioManager _audioManager = null;
    private CameraUtil _cameraUtil = null;
    private int _chimeraSpot = 0;

    public Chimera Chimera { get => _chimera; }

    private Rect GetGlobalPosition(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _tooltip = _uiManager.Tooltip;

        NoPrefferedStat();

        SetupButtonListeners();
    }

    public void HabitatDetailsSetup(int chimeraSpot)
    {
        _habitat = ServiceLocator.Get<HabitatManager>().CurrentHabitat;
        _expeditionManager = ServiceLocator.Get<ExpeditionManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
        _cameraUtil = ServiceLocator.Get<CameraUtil>();

        _customName.gameObject.SetActive(false);

        _chimeraSpot = chimeraSpot;

        UpdateDetails();
    }

    private void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_addButton, AddChimeraClicked);
        _uiManager.CreateButtonListener(_removeButton, RemoveChimeraClicked);
        _uiManager.CreateButtonListener(_findButton, FindChimera);
        _uiManager.CreateButtonListener(_renameButton, LockCamera);
    }

    public void Update()
    {
        if (GetGlobalPosition(_explorationTransform).Contains(Input.mousePosition))
        {
            _uiManager.Tooltip.BeingUsed = true;
            _tooltip.HoverDetails = this;
            _uiManager.Tooltip.HoverStatType = StatType.Exploration;
        }
        else if (GetGlobalPosition(_staminaTransform).Contains(Input.mousePosition))
        {
            _uiManager.Tooltip.BeingUsed = true;
            _tooltip.HoverDetails = this;
            _uiManager.Tooltip.HoverStatType = StatType.Stamina;
        }
        else if (GetGlobalPosition(_wisdomTransform).Contains(Input.mousePosition))
        {
            _uiManager.Tooltip.BeingUsed = true;
            _tooltip.HoverDetails = this;
            _uiManager.Tooltip.HoverStatType = StatType.Wisdom;
        }

        if (_tooltip.BeingUsed == false)
        {
            _tooltip.gameObject.SetActive(false);
        }

        if (_tooltip.HoverDetails != this)
        {
            return;
        }

        if (_tooltip.BeingUsed == true)
        {
            _tooltip.FollowMouse();
            _tooltip.LoadStatText();
            _tooltip.LoadPreferenceText(_chimera.ChimeraType);
            _tooltip.LoadEvolutionBonusText(_chimera.EvolutionBonusStat);
        }
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

        if (_chimera.GetName() == _customName.text)
        {
            _name.text = _customName.text;
        }
        else
        {
            _name.text = $"{_chimera.Name}";
        }
        _level.text = $"Average Power: {_chimera.AveragePower.ToString("F1")}";
        _chimeraIcon.sprite = _chimera.ChimeraIcon;
        _elementIcon.sprite = _chimera.ElementIcon;

        int amount = 0;
        string staminaText = _chimera.GetStatByType(StatType.Stamina, out amount) ? amount.ToString() : "Invalid!";
        _stamina.text = staminaText;
        string wisdomText = _chimera.GetStatByType(StatType.Wisdom, out amount) ? amount.ToString() : "Invalid!";
        _wisdom.text = wisdomText;
        string explorationText = _chimera.GetStatByType(StatType.Exploration, out amount) ? amount.ToString() : "Invalid!";
        _exploration.text = explorationText;

        _energySlider.maxValue = _chimera.MaxEnergy;
        _energySlider.value = _chimera.CurrentEnergy;
        _energyText.text = $"Energy: {_chimera.CurrentEnergy}";
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
        else if (NotEnoughEnergy() == true) // Not Enough Energy
        {
            _statefulButtons.SetState("Occupied");
            _occupiedText.text = $"Energy Too Low";
            return;
        }

        switch (detailsButtonType)
        {
            case DetailsButtonType.Standard:
                _statefulButtons.SetState("Find Button", true);
                break;
            case DetailsButtonType.ExpeditionParty:
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

    // Return true on not enough energy for selected expedition.
    private bool NotEnoughEnergy()
    {
        if (_expeditionManager.SelectedExpedition == null) // No mission currently selected.
        {
            return false;
        }

        if (_expeditionManager.HasChimeraBeenAdded(_chimera) == true) // No need to check if its in a mission if its already been added.
        {
            return false;
        }

        if (_chimera.CurrentEnergy >= _expeditionManager.SelectedExpedition.EnergyDrain) // Can afford the energy cost. 
        {
            return false;
        }

        return true;
    }

    private void AddChimeraClicked()
    {
        if (_expeditionManager.AddChimera(_chimera) == true) // Success
        {
            _statefulButtons.SetState("Remove Button");
            _audioManager.PlayUISFX(SFXUIType.StandardClick);
        }
        else
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);
        }
    }

    private void RemoveChimeraClicked()
    {
        if (_expeditionManager.RemoveChimera(_chimera) == true) // Success
        {
            _statefulButtons.SetState("Add Button");
            _audioManager.PlayUISFX(SFXUIType.StandardClick);
        }
        else
        {
            _audioManager.PlayUISFX(SFXUIType.ErrorClick);
        }
    }

    private void FindChimera()
    {
        if (_uiManager.HabitatUI.MenuOpen)
        {
            _uiManager.HabitatUI.ResetStandardUI();
        }

        _audioManager.PlayUISFX(SFXUIType.ConfirmClick);
        _cameraUtil.FindChimeraCameraShift(Chimera);
    }

    public void DetermineStatGlow()
    {
        if (_chimera == null)
        {
            return;
        }

        NoPrefferedStat();

        switch (_chimera.EvolutionBonusStat)
        {
            case StatType.None:
                break;
            case StatType.Exploration:
                _statIcons[0].color = _prefferedGoldColor;
                break;
            case StatType.Stamina:
                _statIcons[1].color = _prefferedGoldColor;
                break;
            case StatType.Wisdom:
                _statIcons[2].color = _prefferedGoldColor;
                break;
            default:
                Debug.LogError($"Unhandled stat [{_chimera.EvolutionBonusStat}] please fix!");
                break;
        }
    }

    private void LockCamera()
    {
        _cameraUtil.IsNaming = true;
        _name.gameObject.SetActive(false);
        _name.text = "";
        _customName.gameObject.SetActive(true);
        _customName.Select();

        _panel.SetActive(false);
    }

    public void UnlockCamera()
    {
        _cameraUtil.IsNaming = false;
        _chimera.SetCustomName(_customName.text);
        _name.gameObject.SetActive(true);
        _customName.gameObject.SetActive(false);

        UpdateDetails();

        _panel.SetActive(true);
    }

    private void NoPrefferedStat()
    {
        foreach (Image statBG in _statIcons)
        {
            statBG.color = _defaultColor;
        }
    }
}