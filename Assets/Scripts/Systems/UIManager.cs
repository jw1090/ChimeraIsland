using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scene Types")]
    [SerializeField] private StatefulObject _uiStatefulObject = null;
    [SerializeField] private HabitatUI _habitatUI = null;
    [SerializeField] private MainMenuUI _mainMenuUI = null;
    [SerializeField] private StartingUI _startingUI = null;
    [SerializeField] private WorldMapUI _worldMapUI = null;

    public HabitatUI HabitatUI { get => _habitatUI; }
    public MainMenuUI MainMenuUI { get => _mainMenuUI; }
    public StartingUI StartingUI { get => _startingUI; }
    public WorldMapUI WorldMapUI { get => _worldMapUI; }

    public void SetAudioManager(AudioManager audioManager) 
    {
        _startingUI.SetAudioManager(audioManager);
        _habitatUI.SetAudioManager(audioManager);
    }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _mainMenuUI.Initialize(this);
        _startingUI.Initialize();
        _habitatUI.Initialize(this);

        _uiStatefulObject.SetState("Transparent", true);

        return this;
    }

    public void ShowUIByScene(SceneType uiSceneType)
    {
        Debug.Log($"<color=Cyan> Show {uiSceneType} UI.</color>");

        switch (uiSceneType)
        {
            case SceneType.Habitat:
                _uiStatefulObject.SetState("Habitat UI", true);
                _habitatUI.ResetStandardUI();
                _habitatUI.LoadCurrentUIProgress();
                break;
            case SceneType.MainMenu:
                _uiStatefulObject.SetState("Main Menu UI", true);
                break;
            case SceneType.Starting:
                _uiStatefulObject.SetState("Starting UI", true);
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
}