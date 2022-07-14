using AI.Behavior;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera _cameraMain = null;
    private ChimeraBehavior _heldChimera = null;
    private ReleaseSlider _releaseSlider = null;
    private HabitatUI _habitatUI = null;
    private TutorialManager _tutorialManager = null;
    private LayerMask _chimeraLayer = new LayerMask();
    private bool _isInitialized = false;
    private bool _sliderUpdated = false;
    private bool _isHolding = false;
    private bool _debugTutorialInputEnabled = false;

    public void SetTutorialManager(TutorialManager tutorialManager) { _tutorialManager = tutorialManager; }
    public void SetCamera(Camera camera) { _cameraMain = camera; }
    public void SetHabitatUI(HabitatUI habitatUI)
    {
        _habitatUI = habitatUI;
        _releaseSlider = _habitatUI.ReleaseSlider;
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
            if(_habitatUI != null)
            {
                _habitatUI.ToggleSettingsMenu();
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
            Chimera chimera = hit.transform.gameObject.GetComponent<EvolutionLogic>().ChimeraBrain;
            _heldChimera = chimera.GetComponent<ChimeraBehavior>();
            _heldChimera.WasClicked = true;
            _isHolding = true;
        }
    }

    private void ExitHeldState()
    {
        if (_isHolding == false)
        {
            return;
        }

        _heldChimera.GetComponent<ChimeraBehavior>().WasClicked = false;
        _isHolding = false;
        _heldChimera = null;
    }

    private void DebugTutorialInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _habitatUI.DisableUI();
            _tutorialManager.ShowTutorialStage(TutorialStageType.Intro);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.Marketplace);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.PurchasingFacilities);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.Details);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.Expeditions);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.NewFacilities);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.Evolution);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.Fossils);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.WorldMap);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _tutorialManager.ShowTutorialStage(TutorialStageType.NewHabitats);
        }
    }
}