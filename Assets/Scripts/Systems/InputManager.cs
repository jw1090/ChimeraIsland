using AI.Behavior;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject _sphereMarker = null;
    private Camera _cameraMain = null;
    private CameraUtil _cameraUtil = null;
    private ChimeraBehavior _heldChimera = null;
    //private ReleaseSlider _releaseSlider = null;
    private UIManager _uiManager = null;
    private HabitatUI _habitatUI = null;
    private TutorialManager _tutorialManager = null;
    private HabitatManager _habitatManager = null;
    private CurrencyManager _currencyManager = null;
    private DebugConfig _debugConfig = null;
    private LayerMask _chimeraLayer = new LayerMask();
    private bool _isInitialized = false;
    //private bool _sliderUpdated = false;
    private bool _isHolding = false;
    private bool _debugTutorialInputEnabled = false;
    private bool _debugCurrencyInputEnabled = false;

    public event Action<bool, int> HeldStateChange = null;
    public GameObject SphereMarker { get => _sphereMarker; }

    public void SetCurrencyManager(CurrencyManager currencyManager) { _currencyManager = currencyManager; }
    public void SetTutorialManager(TutorialManager tutorialManager) { _tutorialManager = tutorialManager; }
    public void SetCameraUtil(CameraUtil cameraUtil)
    {
        _cameraUtil = cameraUtil;
        _cameraMain = _cameraUtil.CameraCO;
    }

    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }
    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
        _habitatUI = _uiManager.HabitatUI;
        //_releaseSlider = _habitatUI.ReleaseSlider;
    }

    public InputManager Initialize()
    {
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;

        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _chimeraLayer = LayerMask.GetMask("Chimera");
        _sphereMarker.SetActive(false);

        _isInitialized = true;

        return this;
    }

    private void OnDebugConfigLoaded()
    {
        _debugConfig = ServiceLocator.Get<DebugConfig>();

        _debugTutorialInputEnabled = _debugConfig.DebugTutorialInputEnabled;
        if (_debugTutorialInputEnabled == false)
        {
            Debug.Log("Debug Tutorial Input is DISABLED");
        }

        _debugCurrencyInputEnabled = _debugConfig.DebugCurrencyInputEnabled;
        if (_debugCurrencyInputEnabled == false)
        {
            Debug.Log("Debug Currency Input is DISABLED");
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
            //RemoveFromFacility();
            HeldCheckAgainstUI();
        }

        if (Input.GetMouseButtonDown(0))
        {
            EnterHeldState();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //ResetSliderInfo();
            ExitHeldState();
        }

        if (_cameraUtil != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                _cameraUtil.CameraZoom();
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (_habitatUI.MenuOpen == false)
                {
                    _cameraUtil.CameraMovement();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
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

        if(_debugCurrencyInputEnabled)
        {
            DebugCurrencyInput();
        }
    }

    /*
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
    */

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

    private void DebugCurrencyInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _currencyManager.IncreaseEssence(_debugConfig.DebugEssenceGain);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _currencyManager.IncreaseFossils(_debugConfig.DebugFossilGain);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _currencyManager.ResetCurrency();
        }
    }
}