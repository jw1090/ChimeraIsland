using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionBuilderUI : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] List<Color> _vfxColors = new List<Color>();
    [SerializeField] List<float> _vfxSizes = new List<float>();

    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown _baseChimeraDropdown = null;
    [SerializeField] private TMP_Dropdown _evolutionDropdown = null;
    [SerializeField] private TMP_Dropdown _vfxTypeDropdown = null;
    [SerializeField] private TMP_Dropdown _vfxColorDropdown = null;
    [SerializeField] private TMP_Dropdown _vfxSizeDropdown = null;

    [Header("Buttons")]
    [SerializeField] private Button _playButton = null;
    [SerializeField] private Button _resetButton = null;
    [SerializeField] private Button _saveButton = null;

    private UIManager _uiManager = null;
    private EvolutionBuilder _evolutionBuilder = null;

    public Button PlayButton { get => _playButton; }
    public Button ResetButton { get => _resetButton; }
    public Button SaveButton { get => _saveButton; }

    public void SetEvolutionBuilder(EvolutionBuilder evolutionBuilder) { _evolutionBuilder = evolutionBuilder; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;

        SetupDropdownListeners();
        SetupButtonListeners();
    }

    public void SetupDropdownListeners()
    {
        _uiManager.CreateDropdownListener(_baseChimeraDropdown, OnChangeBaseDropdown);
    }

    public void SetupButtonListeners()
    {
        _uiManager.CreateButtonListener(_saveButton, OnSavePressed);
    }

    public void LoadBaseChimeras()
    {
        _baseChimeraDropdown.ClearOptions();

        List<string> chimeraOptions = new List<string>();
        foreach (Chimera chimera in _evolutionBuilder.BaseChimeras)
        {
            chimeraOptions.Add(chimera.Name);
        }

        _baseChimeraDropdown.AddOptions(chimeraOptions);
        _baseChimeraDropdown.value = 0;

        UpdateEvolutionDropdown();
    }

    private void UpdateEvolutionDropdown()
    {
        _evolutionDropdown.ClearOptions();

        Chimera chimera = _evolutionBuilder.BaseChimeras[_baseChimeraDropdown.value];

        List<string> evolutionOptions = new List<string>();
        foreach (EvolutionLogic evolution in chimera.CurrentEvolution.PossibleEvolutions)
        {
            evolutionOptions.Add(evolution.Name);
        }

        _evolutionDropdown.AddOptions(evolutionOptions);
        _evolutionDropdown.value = 0;
    }

    private void OnChangeBaseDropdown()
    {
        _evolutionBuilder.SelectChimera(GetBaseTypeFromDropdown());

        UpdateEvolutionDropdown();
    }

    private void OnSavePressed()
    {
        _evolutionBuilder.SaveVFXInstructions(GetEvolutionTypeFromDropdown(), CreateEvoltuionData());
    }

    private EvolutionData CreateEvoltuionData()
    {
        EvolutionData evolutionData = new EvolutionData();

        evolutionData.EvolutionVFXType = GetVFXTypeFromDropdown();
        evolutionData.Color = GetColorFromDropdown();
        evolutionData.Size = GetSizeFromDropdown();

        return evolutionData;
    }

    private ChimeraType GetBaseTypeFromDropdown()
    {
        if (_baseChimeraDropdown.value == 0)
        {
            return ChimeraType.A;
        }
        else if (_baseChimeraDropdown.value == 1)
        {
            return ChimeraType.B;
        }
        else
        {
            return ChimeraType.C;
        }
    }

    private ChimeraType GetEvolutionTypeFromDropdown()
    {
        ChimeraType chimeraType = GetBaseTypeFromDropdown();

        switch (chimeraType)
        {
            case ChimeraType.A:
                switch (_evolutionDropdown.value)
                {
                    case 0:
                        return ChimeraType.A1;
                    case 1:
                        return ChimeraType.A2;
                    case 2:
                        return ChimeraType.A3;
                    default:
                        Debug.LogError($"Chimera Type [{chimeraType}] is invalid. Please fix!");
                        return ChimeraType.None;
                }
            case ChimeraType.B:
                switch (_evolutionDropdown.value)
                {
                    case 0:
                        return ChimeraType.B1;
                    case 1:
                        return ChimeraType.B2;
                    case 2:
                        return ChimeraType.B3;
                    default:
                        Debug.LogError($"Chimera Type [{chimeraType}] is invalid. Please fix!");
                        return ChimeraType.None;
                }
            case ChimeraType.C:
                switch (_evolutionDropdown.value)
                {
                    case 0:
                        return ChimeraType.C1;
                    case 1:
                        return ChimeraType.C2;
                    case 2:
                        return ChimeraType.C3;
                    default:
                        Debug.LogError($"Chimera Type [{chimeraType}] is invalid. Please fix!");
                        return ChimeraType.None;
                }
            default:
                Debug.LogError($"Chimera Type [{chimeraType}] is invalid. Please fix!");
                return ChimeraType.None;
        }
    }

    private EvolutionVFXType GetVFXTypeFromDropdown()
    {
        if (_vfxTypeDropdown.value == 0)
        {
            return EvolutionVFXType.GrowingLight;
        }
        else
        {
            return EvolutionVFXType.WaterDrop;
        }
    }

    private Color GetColorFromDropdown()
    {
        return _vfxColors[_vfxColorDropdown.value];
    }

    private float GetSizeFromDropdown()
    {
        return _vfxSizes[_vfxSizeDropdown.value];
    }
}