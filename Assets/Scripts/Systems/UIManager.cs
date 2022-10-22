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
    [SerializeField] private EvolutionBuilderUI _evolutionBuilderUI = null;
    private bool _uiActive = true;

    public HabitatUI HabitatUI { get => _habitatUI; }
    public MainMenuUI MainMenuUI { get => _mainMenuUI; }
    public StartingUI StartingUI { get => _startingUI; }
    public EvolutionBuilderUI EvolutionBuilderUI { get => _evolutionBuilderUI; }
    public bool UIActive { get => _uiActive; }

    public void SetAudioManager(AudioManager audioManager) { _habitatUI.SetAudioManager(audioManager); }
    public void SetStarterEnvironment(StarterEnvironment starterEnvironment) { _startingUI.SetStarterEnvironment(starterEnvironment); }
    public void ToggleUI()
    {
        _uiActive = !_uiActive;
        gameObject.SetActive(_uiActive);
    }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _mainMenuUI.Initialize(this);
        _startingUI.Initialize(this);
        _habitatUI.Initialize(this);
        _evolutionBuilderUI.Initialize(this);

        _uiStatefulObject.SetState("Transparent", true);
        _uiActive = true;

        return this;
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
                _uiStatefulObject.SetState("Transparent", true);
                break;
            case SceneType.Builder:
                _uiStatefulObject.SetState("Builder UI", true);
                break;
            default:
                Debug.LogError($"{uiSceneType} is invalid. Please change!");
                break;
        }
    }

    public void EnableUIByType(UIElementType uiElementType)
    {
        switch (uiElementType)
        {
            case UIElementType.All:
            case UIElementType.OpenDetailsButton:
            case UIElementType.ExpeditionButton:
            case UIElementType.MarketplaceChimeraTab:
            case UIElementType.WorldMapButton:
            case UIElementType.OtherFacilityButtons:
            case UIElementType.FossilsWallets:
            case UIElementType.EssenceWallets:
                _habitatUI.EnableUIElementByType(uiElementType); // Habitat UI
                break;
            default:
                Debug.LogError($"{uiElementType} is invalid. Please change!");
                break;
        }
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
}