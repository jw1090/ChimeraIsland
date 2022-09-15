using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionBuilderUI : MonoBehaviour
{
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
}