using AI.Behavior;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private Camera _cameraMain = null;
    private ChimeraBehavior _heldChimera = null;
    private ReleaseSlider _releaseSlider = null;
    private UIManager _uiManager = null;
    private TutorialManager _tutorialManager = null;
    private LayerMask _chimeraLayer = new LayerMask();
    private bool _isInitialized = false;
    private bool _sliderUpdated = false;
    private bool _isHolding = false;
    private bool _debugTutorialInputEnabled = false;
    private HabitatManager _habitatManager = null;

    public event Action<bool, int> HeldStateChange = null;

    public void SetTutorialManager(TutorialManager tutorialManager) { _tutorialManager = tutorialManager; }
    public void SetCamera(Camera camera) { _cameraMain = camera; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }
    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
        _releaseSlider = _uiManager.HabitatUI.ReleaseSlider;
    }

    public InputManager Initialize()
    {
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;

        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _chimeraLayer = LayerMask.GetMask("Chimera");

        _isInitialized = true;

        return this;
    }

    private void OnDebugConfigLoaded()
    {
        _debugTutorialInputEnabled = ServiceLocator.Get<DebugConfig>().DebugTutorialInputEnabled;
        if (_debugTutorialInputEnabled == false)
        {
            Debug.Log("Debug Tutorial Input is DISABLED");
        }
    }

    private void OnDestroy()
    {
        DebugConfig.DebugConfigLoaded -= OnDebugConfigLoaded;
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            RemoveFromFacility();
            HeldCheckAgainstUI();
        }

        if (Input.GetMouseButtonDown(0))
        {
            EnterHeldState();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ResetSliderInfo();
            ExitHeldState();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_uiManager.HabitatUI != null)
            {
                _uiManager.HabitatUI.ToggleSettingsMenu();
            }
        }

        if(_debugTutorialInputEnabled)
        {
            DebugTutorialInput();
        }
    }

    private void RemoveFromFacility()
    {
        if (_cameraMain == null)
        {
            return;
        }

        if (_releaseSlider == null)
        {
            Debug.LogError("Release slider is Null!");
            return;
        }

        if (_heldChimera == true || EventSystem.current.IsPointerOverGameObject())
        {
            ResetSliderInfo();
            return;
        }

        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, 100.0f);

        if (hit.collider == null)
        {
            return;
        }

        if (hit.collider.CompareTag("Facility") == false)
        {
            ResetSliderInfo();
            return;
        }

        _releaseSlider.Hold(hit);
        _releaseSlider.UpdateSliderUI();
        _sliderUpdated = true;
    }

    private void ResetSliderInfo()
    {
        if (_sliderUpdated == false)
        {
            return;
        }

        _releaseSlider.ResetSlider();
        _releaseSlider.UpdateSliderUI();
        _sliderUpdated = false;
    }

    private void EnterHeldState()
    {
        if (_cameraMain == null)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (_isHolding == true)
        {
            return;
        }

        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 300.0f, _chimeraLayer))
        {
            _habitatManager.CurrentHabitat.ActivateGlow(true);
            _heldChimera = hit.transform.gameObject.GetComponent<ChimeraBehavior>();
            HeldStateChange?.Invoke(true, _heldChimera.transform.GetHashCode());
            _isHolding = true;
        }
    }

    private void HeldCheckAgainstUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            ExitHeldState();
            return;
        }
    }

    private void ExitHeldState()
    {
        if (_isHolding == false)
        {
            return;
        }

        _habitatManager.CurrentHabitat.ActivateGlow(false);
        HeldStateChange?.Invoke(false, _heldChimera.transform.GetHashCode());
        _isHolding = false;
        _heldChimera = null;
    }

    private void DebugTutorialInput()
    {
        int currentStageId = (int)_tutorialManager.CurrentStage;

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _uiManager.TutorialDisableUI();
            _tutorialManager.ResetTutorialProgress();
            _tutorialManager.ShowTutorialStage(TutorialStageType.Intro);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int newStageId = ++currentStageId;

            if(newStageId < Enum.GetNames(typeof(TutorialStageType)).Length - 1)
            {
                _tutorialManager.ShowTutorialStage((TutorialStageType)newStageId);
            }
        }
    }
}