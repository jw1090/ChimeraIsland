using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scene Types")]
    [SerializeField] private StatefulObject _uiStatefulObject = null;
    [SerializeField] private HabitatUI _habitatUI = null;
    [SerializeField] private MainMenuUI _mainMenuUI = null;
    [SerializeField] private StartingUI _startingUI = null;
    [SerializeField] private TempleUI _templeUI = null;
    [SerializeField] private EvolutionBuilderUI _evolutionBuilderUI = null;

    [Header("Settings")]
    [SerializeField] private SettingsUI _settingsUI = null;
    private bool _uiVisible = true;

    public MainMenuUI MainMenuUI { get => _mainMenuUI; }
    public StartingUI StartingUI { get => _startingUI; }
    public HabitatUI HabitatUI { get => _habitatUI; }
    public TempleUI TempleUI { get => _templeUI; }
    public EvolutionBuilderUI EvolutionBuilderUI { get => _evolutionBuilderUI; }
    public SettingsUI SettingsUI { get => _settingsUI; }
    public bool InHabitatState { get => _uiStatefulObject.CurrentState.StateName == "Habitat UI"; }
    public bool UIActive { get => _uiVisible; }

    public void SetAudioManager(AudioManager audioManager) 
    {
        _settingsUI.SetAudioManager(audioManager);
        _habitatUI.SetAudioManager(audioManager);
        _templeUI.SetAudioManager(audioManager);

        _settingsUI.InitializeVolumeSettings();
    }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _settingsUI.Initialize(this);

        _mainMenuUI.Initialize(this);
        _startingUI.Initialize(this);
        _habitatUI.Initialize(this);
        _templeUI.Initialize(this);
        _evolutionBuilderUI.Initialize(this);

        InitializeWallets();

        _uiStatefulObject.SetState("Transparent", true);

        return this;
    }

    private void InitializeWallets()
    {
        _habitatUI.InitializeWallets();
        _templeUI.InitializeWallets();
    }

    public void CreateButtonListener(Button button, Action action)
    {
        if (button != null)
        {
            button.onClick.AddListener
            (delegate
            {
                action?.Invoke();
            });
        }
        else
        {
            Debug.LogError($"{button} is null! Please Fix");
        }
    }

    public void CreateDropdownListener(TMP_Dropdown dropdown, Action action)
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener
            (delegate
            {
                action?.Invoke();
            });
        }
        else
        {
            Debug.LogError($"{dropdown} is null! Please Fix");
        }
    }

    public void ShowUIByScene(SceneType uiSceneType)
    {
        Debug.Log($"<color=Cyan> Show {uiSceneType} UI.</color>");

        switch (uiSceneType)
        {
            case SceneType.MainMenu:
                _uiStatefulObject.SetState("Main Menu UI", true);
                break;
            case SceneType.Starting:
                _uiStatefulObject.SetState("Starting UI", true);
                break;
            case SceneType.Habitat:
                _uiStatefulObject.SetState("Habitat UI", true);
                _habitatUI.ResetStandardUI();
                _habitatUI.LoadCurrentUIProgress();
                break;
            case SceneType.Temple:
                _uiStatefulObject.SetState("Temple UI", true);
                break;
            case SceneType.Builder:
                _uiStatefulObject.SetState("Builder UI", true);
                break;
            default:
                Debug.LogError($"{uiSceneType} is invalid. Please change!");
                break;
        }
    }

    public void UpdateEssenceWallets()
    {
        _habitatUI.UpdateEssenceWallets();
        _templeUI.UpdateEssenceWallets();
    }

    public void UpdateFossilWallets()
    {
        _habitatUI.UpdateFossilWallets();
        _templeUI.UpdateFossilWallets();
    }

    public void ToggleUI()
    {
        _uiVisible = !_uiVisible;
        gameObject.SetActive(_uiVisible);
    }
}