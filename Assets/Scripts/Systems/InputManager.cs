using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject _sphereMarker = null;
    private Camera _cameraMain = null;
    private CameraUtil _cameraUtil = null;
    private ChimeraBehavior _heldChimera = null;
    private UIManager _uiManager = null;
    private HabitatUI _habitatUI = null;
    private TutorialManager _tutorialManager = null;
    private HabitatManager _habitatManager = null;
    private CurrencyManager _currencyManager = null;
    private DebugConfig _debugConfig = null;
    ExpeditionManager _expeditionManager = null;
    private LayerMask _chimeraLayer = new LayerMask();
    private LayerMask _crystalLayer = new LayerMask();
    private LayerMask _portalLayer = new LayerMask();
    private LayerMask _templeLayer = new LayerMask();
    private bool _isInitialized = false;
    private bool _inTransition = false;
    private bool _isHolding = false;
    private bool _debugTutorialInputEnabled = false;
    private bool _debugCurrencyInputEnabled = false;
    private bool _debugHabitatUpgradeInputEnabled = false;
    private const float _rotationAmount = 0.8f;

    public event Action<bool, int> HeldStateChange = null;
    public GameObject SphereMarker { get => _sphereMarker; }
    public bool IsHolding { get => _isHolding; }

    public void SetInTransition(bool value) { _inTransition = value; }
    public void SetCurrencyManager(CurrencyManager currencyManager) { _currencyManager = currencyManager; }
    public void SetTutorialManager(TutorialManager tutorialManager) { _tutorialManager = tutorialManager; }
    public void SetCameraUtil(CameraUtil cameraUtil)
    {
        _cameraUtil = cameraUtil;
        _cameraMain = _cameraUtil.CameraCO;
    }

    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }
    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }
    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
        _habitatUI = _uiManager.HabitatUI;
    }

    public InputManager Initialize()
    {
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;

        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _chimeraLayer = LayerMask.GetMask("Chimera");
        _crystalLayer = LayerMask.GetMask("Crystal");
        _portalLayer = LayerMask.GetMask("Portal");
        _templeLayer = LayerMask.GetMask("Temple");
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

        _debugHabitatUpgradeInputEnabled = _debugConfig.DebugHabitatUpgradeInputEnabled;
        if (_debugHabitatUpgradeInputEnabled == false)
        {
            Debug.Log("Debug Habitat Upgrade Input is DISABLED");
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
            HeldCheckAgainstUI();
        }
        if (Input.GetMouseButton(1))
        {
            RotateChimeraCheck();
        }

        if (Input.GetMouseButtonDown(0))
        {
            LeftClickDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ExitHeldState();
        }

        if (_cameraUtil != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                _cameraUtil.CameraZoom();
            }

            if (_inTransition == true)
            {
                return;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (_habitatUI.MenuOpen == false && _habitatUI.TutorialOpen == false)
                {
                    _cameraUtil.CameraMovement();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_habitatUI != null)
            {
                _habitatUI.ToggleSettingsMenu();
            }
        }

        if (_debugTutorialInputEnabled)
        {
            DebugTutorialInput();
        }

        if (_debugCurrencyInputEnabled)
        {
            DebugCurrencyInput();
        }

        if (_debugHabitatUpgradeInputEnabled)
        {
            DebugHabitatUpgradeInput();
        }
    }

    private void LeftClickDown()
    {
        if (_cameraMain == null)
        {
            return;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (_inTransition == true)
        {
            return;
        }

        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit crystalHit, 300.0f, _crystalLayer))
        {
            CrystalSpawn crystal = crystalHit.transform.gameObject.GetComponent<CrystalSpawn>();
            crystal.Harvest();

            return;
        }
        else if (Physics.Raycast(ray, 300.0f, _portalLayer))
        {
            _habitatUI.OpenExpedition();
        }
        else if (Physics.Raycast(ray, out RaycastHit chimeraHit, 300.0f, _chimeraLayer))
        {
            if (_isHolding == true)
            {
                return;
            }

            _heldChimera = chimeraHit.transform.gameObject.GetComponent<ChimeraBehavior>();

            if (_heldChimera.Chimera.ReadyToEvolve == true)
            {
                _heldChimera.Chimera.EvolveChimera();
            }
            else
            {
                _habitatManager.CurrentHabitat.ActivateGlow(true);
                HeldStateChange?.Invoke(true, _heldChimera.transform.GetHashCode());
                _isHolding = true;
            }
        }
        else if (Physics.Raycast(ray, 300.0f, _templeLayer))
        {
            _habitatUI.OpenMarketplace();
        }
    }

    private void RotateChimeraCheck()
    {
        if (_isHolding == true)
        {
            _heldChimera.transform.Rotate(Vector3.up, _rotationAmount);
        }
    }

    private void HeldCheckAgainstUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
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
            _tutorialManager.ResetTutorialProgress();
            _tutorialManager.ShowTutorialStage(TutorialStageType.Intro);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int newStageId = ++currentStageId;

            if (newStageId < Enum.GetNames(typeof(TutorialStageType)).Length - 1)
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

    private void DebugHabitatUpgradeInput()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            _expeditionManager.CompleteCurrentUpgradeExpedition();
        }
    }
}