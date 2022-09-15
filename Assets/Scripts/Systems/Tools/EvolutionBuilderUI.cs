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

    public void SetEvolutionBuilder(EvolutionBuilder evolutionBuilder) { _evolutionBuilder = evolutionBuilder; }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void SetupDropdownListeners()
    {

    }

    public void SetupButtonListeners()
    {

    }

    public void LoadBaseChimeras()
    {
        _baseChimeraDropdown.ClearOptions();

        List<string> chimeraOptions = new List<string>();
        foreach (Chimera chimera in _evolutionBuilder.BaseChimeras)
        {
            chimeraOptions.Add(chimera.name);
        }

        _baseChimeraDropdown.AddOptions(chimeraOptions);
        _baseChimeraDropdown.value = 0;

        UpdateEvolutionDropdown();
    }

    private void UpdateEvolutionDropdown()
    {
        Chimera chimera = _evolutionBuilder.BaseChimeras[_baseChimeraDropdown.value];

        List<string> evolutionOptions = new List<string>();
        foreach (EvolutionLogic evolution in chimera.CurrentEvolution.PossibleEvolutions)
        {
            evolutionOptions.Add(evolution.name);
        }

        _baseChimeraDropdown.AddOptions(evolutionOptions);
        _baseChimeraDropdown.value = 0;
    }
}