using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject _sphereMarker = null;
    private Camera _cameraMain = null;
    private CameraUtil _cameraUtil = null;
    private FreeCamera _freeCamera = null;
    private ChimeraBehavior _heldChimera = null;
    private EvolutionLogic _evolution;
    private UIManager _uiManager = null;
    private HabitatUI _habitatUI = null;
    private StartingUI _startingUI = null;
    private TempleUI _templeUI = null;
    private HabitatManager _habitatManager = null;
    private CurrencyManager _currencyManager = null;
    private DebugConfig _debugConfig = null;
    private ExpeditionManager _expeditionManager = null;
    private AudioManager _audioManager = null;
    private ResourceManager _resourceManager = null;
    private PersistentData _persistentData = null;
    private SceneChanger _sceneChanger = null;
    private Temple _temple = null;
    private LayerMask _chimeraLayer = new LayerMask();
    private LayerMask _crystalLayer = new LayerMask();
    private LayerMask _portalLayer = new LayerMask();
    private LayerMask _templeLayer = new LayerMask();
    private LayerMask _upgradesLayer = new LayerMask();
    private LayerMask _groundLayer = new LayerMask();
    private LayerMask _figurineLayer = new LayerMask();
    private bool _isInitialized = false;
    private bool _inTransition = false;
    private bool _isHolding = false;
    private bool _debugCurrencyInputEnabled = false;
    private bool _debugHabitatUpgradeInputEnabled = false;
    private bool _debugViewEnabled = false;
    private bool _freeCameraActive = false;
    private bool _rotatingInGallery = false;
    private float _rotationAmount = 2.0f;
    private SceneType _currentScene = SceneType.None;

    public event Action<bool, int> HeldStateChange = null;
    public GameObject SphereMarker { get => _sphereMarker; }
    public bool IsHolding { get => _isHolding; }
    public float RotationSpeed { get => _rotationAmount; }

    public void SetCurrentScene(SceneType sceneType) { _currentScene = sceneType; }
    public void SetChimeraRotationSpeed(float speed)
    {
        _rotationAmount = speed;
        _persistentData.SetSpinSpeed(speed);
    }
    public void SetInTransition(bool value) { _inTransition = value; }
    public void SetRotatingInGallery(bool rotating) { _rotatingInGallery = rotating; }
    public void SetCurrencyManager(CurrencyManager currencyManager) { _currencyManager = currencyManager; }
    public void SetCameraUtil(CameraUtil cameraUtil)
    {
        _cameraUtil = cameraUtil;
        _cameraMain = _cameraUtil.CameraCO;
    }

    public void SetSceneChanger(SceneChanger sceneChanger) { _sceneChanger = sceneChanger; }
    public void SetFreeCamera(FreeCamera freeCamera) { _freeCamera = freeCamera; }
    public void SetHabitatManager(HabitatManager habitatManager) { _habitatManager = habitatManager; }
    public void SetExpeditionManager(ExpeditionManager expeditionManager) { _expeditionManager = expeditionManager; }
    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }
    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
        _habitatUI = _uiManager.HabitatUI;
        _startingUI = _uiManager.StartingUI;
        _templeUI = _uiManager.TempleUI;
    }
    public void SetTemple(Temple temple)
    {
        _temple = temple;
    }

    public InputManager Initialize()
    {
        DebugConfig.DebugConfigLoaded += OnDebugConfigLoaded;
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _persistentData = ServiceLocator.Get<PersistentData>();

        _chimeraLayer = LayerMask.GetMask("Chimera");
        _crystalLayer = LayerMask.GetMask("Crystal");
        _portalLayer = LayerMask.GetMask("Portal");
        _templeLayer = LayerMask.GetMask("Temple");
        _upgradesLayer = LayerMask.GetMask("UpgradeNode");
        _groundLayer = LayerMask.GetMask("Ground");
        _figurineLayer = LayerMask.GetMask("Figurine");
        _sphereMarker.SetActive(false);

        _rotationAmount = _persistentData.SettingsData.spinSpeed;

        _isInitialized = true;

        return this;
    }


    private void OnDebugConfigLoaded()
    {
        _debugConfig = ServiceLocator.Get<DebugConfig>();

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

        _debugViewEnabled = _debugConfig.EnableDebugViewInput;
        if (_debugViewEnabled == false)
        {
            Debug.Log("Debug View Input is DISABLED");
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

        if (MouseInScreenSpace() == true)
        {
            CursorChange();
        }

        if (Input.GetMouseButtonDown(0))
        {
            LeftClickDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ExitHeldState();
        }

        if (_cameraUtil != null && _freeCameraActive == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                if (_currentScene == SceneType.Habitat)
                {
                    _cameraUtil.CameraZoom();
                }
            }

            if (_inTransition == true)
            {
                return;
            }

            if (MovementInputCheck() == true)
            {
                if (_habitatUI.MenuOpen == false && _uiManager.TutorialOpen == false)
                {
                    _cameraUtil.CameraUpdate();
                }
            }
        }
        else if (_freeCamera != null)
        {
            _freeCamera.CameraUpdate();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _uiManager.HabitatUI.ToggleSettingsMenu();
        }

        if (_debugCurrencyInputEnabled == true)
        {
            DebugCurrencyInput();
        }

        if (_debugHabitatUpgradeInputEnabled == true)
        {
            DebugHabitatUpgradeInput();
        }

        if (_debugViewEnabled == true)
        {
            DebugViewInput();
        }
    }

    private void FixedUpdate()
    {
        if (_isInitialized == false)
        {
            return;
        }

        if (Input.GetMouseButton(1))
        {
            RotateChimeraCheck();
        }
    }

    private bool MovementInputCheck()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow);
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

        if (_inTransition == true || _cameraUtil.InTransition == true)
        {
            return;
        }

        // Ray Priority: Crystal > Chimera > Portal > Temple > Upgrades > Ground
        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit crystalHit, 300.0f, _crystalLayer))
        {
            CrystalSpawn crystal = crystalHit.transform.gameObject.GetComponent<CrystalSpawn>();
            crystal.Harvest();

            return;
        }
        else if (Physics.Raycast(ray, out RaycastHit figurineHit, 300.0f, _figurineLayer))
        {
            if (_currentScene == SceneType.Temple)
            {
                _temple.ChimeraGallery.StartGallery(figurineHit.transform.gameObject.GetComponent<Figurine>().ChimeraType);
            }
        }
        else if (Physics.Raycast(ray, out RaycastHit chimeraHit, 300.0f, _chimeraLayer))
        {
            if (_currentScene == SceneType.Habitat)
            {
                if (_isHolding == true)
                {
                    return;
                }

                _heldChimera = chimeraHit.transform.gameObject.GetComponent<ChimeraBehavior>();

                if (_heldChimera.Chimera.ReadyToEvolve == true)
                {
                    StartCoroutine(_heldChimera.Chimera.EvolveChimera());
                }
                else
                {
                    _habitatManager.CurrentHabitat.ActivateGlow(true);
                    HeldStateChange?.Invoke(true, _heldChimera.transform.GetHashCode());
                    _isHolding = true;
                }
            }
            else if (_currentScene == SceneType.Starting)
            {
                _evolution = chimeraHit.transform.gameObject.GetComponent<EvolutionLogic>();

                _startingUI.OpenChimeraInfo();
                _startingUI.LoadChimeraInfo(_evolution);

                _cameraUtil.ChimeraCloseUp(_evolution.ChimeraType);

                _audioManager.PlayHeldChimeraSFX(_evolution.ChimeraType);
            }
            else if (_currentScene == SceneType.Temple)
            {
                if(_templeUI.InGallery == false)
                {
                    _evolution = chimeraHit.transform.gameObject.GetComponent<ChimeraPillar>().EvolutionLogic;

                    _templeUI.BuyChimera(_evolution);
                }
            }
        }
        else if (Physics.Raycast(ray, 300.0f, _portalLayer))
        {
            _habitatUI.OpenExpedition();
        }
        else if (Physics.Raycast(ray, 300.0f, _templeLayer))
        {
            _sceneChanger.LoadTemple();
        }
        else if (Physics.Raycast(ray, out RaycastHit upgradeHit, 300.0f, _upgradesLayer))
        {
            if (_currentScene == SceneType.Temple)
            {
                UpgradeNode upgrade = upgradeHit.transform.gameObject.GetComponent<UpgradeNode>();

                _templeUI.BuyFacility(upgrade);
            }
        }
        else if (Physics.Raycast(ray, out RaycastHit hit, 300.0f, _groundLayer))
        {
            if (hit.collider.transform.gameObject.tag == "Water")
            {
                _habitatManager.CurrentHabitat.TapVFX.ActivateEffect(TapVFXType.Water, hit);
            }
            else if (hit.collider.transform.gameObject.tag == "Stone")
            {
                _habitatManager.CurrentHabitat.TapVFX.ActivateEffect(TapVFXType.Stone, hit);
            }
            else if (hit.collider.transform.gameObject.tag == "Dirt")
            {
                _habitatManager.CurrentHabitat.TapVFX.ActivateEffect(TapVFXType.Ground, hit);
            }
            else if (hit.collider.transform.gameObject.tag == "Tree")
            {
                _habitatManager.CurrentHabitat.TapVFX.ActivateEffect(TapVFXType.Tree, hit);
            }
        }
    }

    private void RotateChimeraCheck()
    {
        if (_isHolding == true)
        {
            _heldChimera.transform.Rotate(Vector3.up, _rotationAmount);
        }
    }

    private void CursorChange()
    {
        CursorType newCursorType = GetCursorSprite();

        Cursor.SetCursor(_resourceManager.GetCursorTexture(newCursorType), Vector2.zero, CursorMode.Auto);
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
        if(_currentScene != SceneType.Habitat)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            _expeditionManager.CompleteCurrentUpgradeExpedition();
        }
    }

    private void DebugViewInput()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            _uiManager.ToggleUI();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_freeCamera == null)
            {
                return;
            }

            _freeCameraActive = !_freeCameraActive; // Toggle

            _freeCamera.CameraCO.enabled = _freeCameraActive;
            _cameraUtil.CameraCO.enabled = !_freeCameraActive;
            Cursor.visible = !_freeCameraActive;

            if (_uiManager.UIActive == _freeCameraActive) // Toggle UI off when going to free cam and vice versa.
            {
                _uiManager.ToggleUI();
            }

            if (_freeCameraActive == false)
            {
                _freeCamera.transform.localPosition = new Vector3(0, 0, 0);
                _freeCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public CursorType GetCursorSprite()
    {
        if (_cameraMain == null)
        {
            return CursorType.Default;
        }
        else if (_inTransition == true || _cameraUtil.InTransition == true)
        {
            return CursorType.Default;
        }
        else if(_rotatingInGallery == true)
        {
            return CursorType.Rotate;
        }
        else if (_isHolding == true)
        {
            return CursorType.Dragging;
        }

        // Ray Priority: Crystal > Chimera > Portal > Temple > Upgrades > Ground
        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, 300.0f, _crystalLayer))
        {
            return CursorType.Minable;
        }
        else if (Physics.Raycast(ray, 300.0f, _chimeraLayer) 
            || Physics.Raycast(ray, 300.0f, _figurineLayer))
        {
            return CursorType.Dragable;
        }
        else if (Physics.Raycast(ray, 300.0f, _portalLayer)
            || Physics.Raycast(ray, 300.0f, _templeLayer))
        {
            return CursorType.Clickable;
        }
        else if (Physics.Raycast(ray, 300.0f, _upgradesLayer))
        {
            return CursorType.Dragable;
        }

        return CursorType.Default;
    }

    private bool MouseInScreenSpace() // Return false if out of screen.
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width)
        {
            return false;
        }

        if (Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return false;
        }

        return true;
    }
}