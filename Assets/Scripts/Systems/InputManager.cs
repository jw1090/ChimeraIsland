using AI.Behavior;
using System;
using UnityEngine;

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

        _habitatManager = ServiceLocator.Get<HabitatManager>();

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
        }

        if ((Input.GetMouseButtonDown(0)))
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

        if (_heldChimera == true)
        {
            ResetSliderInfo();
            return;
        }

        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 200.0f);

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

        if (_isHolding == true)
        {
            return;
        }
        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _chimeraLayer))
        {
            _habitatManager.CurrentHabitat.ActivateGlow(true);
            Chimera chimera = hit.transform.gameObject.GetComponent<EvolutionLogic>().ChimeraBrain;
            _heldChimera = chimera.GetComponent<ChimeraBehavior>();
            HeldStateChange?.Invoke(true, _heldChimera.transform.GetHashCode());
            _isHolding = true;
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int newStageId = --currentStageId;

            if (newStageId >= 0)
            {
                _tutorialManager.ShowTutorialStage((TutorialStageType)newStageId);
            }
        }
    }
}