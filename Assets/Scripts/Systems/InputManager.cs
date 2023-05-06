using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private LayerMask _chimeraPillarLayer = new LayerMask();
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

    private Outline _currentOutline = null;
    private CrystalSpawn _currentCrystalOutline = null;
    private Chimera _currentChimeraOutline = null;
    private OutlineType _currentOutlineType = OutlineType.None;
    private bool _disableOutline = false;
    private bool _recentOutlineCreated = false;

    public event Action<bool, int> HeldStateChange = null;
    public GameObject SphereMarker { get => _sphereMarker; }
    public bool IsHolding { get => _isHolding; }
    public float RotationSpeed { get => _rotationAmount; }
    public void DisableOutline(bool disable) { _disableOutline = disable; }
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
        _chimeraPillarLayer = LayerMask.GetMask("ChimeraPillar");
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

        DetectOutline();
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
                ChimeraType chimeraType = figurineHit.transform.gameObject.GetComponent<Figurine>().ChimeraType;

                _uiManager.TempleUI.EnterGallery(chimeraType);
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

                _disableOutline = true;
                RemoveOutline();

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

                _disableOutline = true;
                RemoveOutline();

                _startingUI.OpenChimeraInfo();
                _startingUI.LoadChimeraInfo(_evolution);

                _cameraUtil.ChimeraCloseUp(_evolution.ChimeraType);

                _audioManager.PlayHeldChimeraSFX(_evolution.ChimeraType);
            }
        }
        else if (Physics.Raycast(ray, out RaycastHit chimeraPillarHit, 300.0f, _chimeraPillarLayer))
        {
            if (_currentScene == SceneType.Temple && _templeUI.CurrentTempleSection == TempleSectionType.Buying)
            {
                _disableOutline = true;
                RemoveOutline();

                _evolution = chimeraPillarHit.transform.gameObject.GetComponent<ChimeraPillar>().EvolutionLogic;
                _cameraUtil.PillarTransition(_evolution.ElementType);
                _templeUI.ChimeraCloseUp(_evolution);
            }
        }
        else if (Physics.Raycast(ray, 300.0f, _portalLayer))
        {
            _habitatUI.OpenExpedition();
        }
        else if (Physics.Raycast(ray, 300.0f, _templeLayer))
        {
            _habitatUI.OpenTemple();
            _sceneChanger.LoadTemple();
        }
        else if (Physics.Raycast(ray, out RaycastHit upgradeHit, 300.0f, _upgradesLayer))
        {
            if (_currentScene == SceneType.Temple)
            {
                UpgradeNode upgrade = upgradeHit.transform.gameObject.GetComponent<UpgradeNode>();

                _templeUI.SelectFacilityUpgrade(upgrade);
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
        _disableOutline = false;
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
        if (_currentScene != SceneType.Habitat)
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
        else if (_rotatingInGallery == true)
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
        else if (Physics.Raycast(ray, 300.0f, _chimeraPillarLayer)
            || Physics.Raycast(ray, 300.0f, _figurineLayer)
            || Physics.Raycast(ray, 300.0f, _chimeraLayer))
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

    private void CreateOutline(RaycastHit raycastHit, OutlineType outlineType)
    {
        if (_disableOutline == true || _recentOutlineCreated == true)
        {
            return;
        }

        Outline outline = null;

        if (outlineType == OutlineType.Crystals)
        {
            CrystalSpawn crystal = raycastHit.transform.GetComponent<CrystalSpawn>();
            crystal.Outline(true);
            _currentCrystalOutline = crystal;
        }
        else if (outlineType == OutlineType.HabitatChimeras)
        {
            Chimera chimera = raycastHit.transform.GetComponent<Chimera>();

            if (_currentChimeraOutline != chimera)
            {
                RemoveOutline();
            }

            outline = chimera.CurrentEvolution.Outline;
            outline.enabled = true;
            _currentOutline = outline;
            _currentChimeraOutline = chimera;
        }
        else
        {
            outline = raycastHit.transform.GetComponent<Outline>();
            outline.enabled = true;
            _currentOutline = outline;
        }

        if (outline != _currentOutline)
        {
            RemoveOutline();
        }
        _currentOutlineType = outlineType;
        _recentOutlineCreated = true;
    }

    private void RemoveOutline()
    {
        if (_currentOutline == null && _currentCrystalOutline == null)
        {
            return;
        }

        if (_currentOutlineType == OutlineType.Crystals)
        {
            _currentCrystalOutline.Outline(false);
            _currentCrystalOutline = null;
        }
        else
        {
            _currentOutline.enabled = false;
            _currentOutline = null;
        }

        _currentOutlineType = OutlineType.None;
    }

    private void DetectOutline()
    {
        _recentOutlineCreated = false;
        switch (_currentScene)
        {
            case SceneType.None:
            case SceneType.MainMenu:
                break;
            case SceneType.Starting:
                DetectOutlineInStarter();
                break;
            case SceneType.Habitat:
                DetectOutlineInHabitat();
                break;
            case SceneType.Temple:
                DetectOutlineInTemple();
                break;
            default:
                Debug.LogError("Invalid Scene Type!");
                break;
        }
    }

    private void DetectOutlineInStarter()
    {
        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit mouseHit, 300.0f, _chimeraLayer))
        {
            CreateOutline(mouseHit, OutlineType.StarterChimeras);
        }
        else if (_currentOutlineType == OutlineType.StarterChimeras)
        {
            RemoveOutline();
        }
    }

    private void DetectOutlineInHabitat()
    {
        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit crystalHit, 300.0f, _crystalLayer))
        {
            if (_currentOutlineType == OutlineType.HabitatChimeras || _currentOutlineType == OutlineType.Portal || _currentOutlineType == OutlineType.Temple)
            {
                RemoveOutline();
            }
            CreateOutline(crystalHit, OutlineType.Crystals);
        }
        else if (_currentOutlineType == OutlineType.Crystals)
        {
            RemoveOutline();
        }
        if (Physics.Raycast(ray, out RaycastHit cHit, 300.0f, _chimeraLayer))
        {
            if (_currentOutlineType == OutlineType.Portal || _currentOutlineType == OutlineType.Temple)
            {
                RemoveOutline();
            }

            CreateOutline(cHit, OutlineType.HabitatChimeras);
        }
        else if (_currentOutlineType == OutlineType.HabitatChimeras)
        {
            RemoveOutline();
        }

        if (Physics.Raycast(ray, out RaycastHit mouseHit, 300.0f, _portalLayer))
        {
            CreateOutline(mouseHit, OutlineType.Portal);
        }
        else if (_currentOutlineType == OutlineType.Portal)
        {
            RemoveOutline();
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 300.0f, _templeLayer))
        {
            CreateOutline(hit, OutlineType.Temple);
        }
        else if (_currentOutlineType == OutlineType.Temple)
        {
            RemoveOutline();
        }
    }

    private void DetectOutlineInTemple()
    {
        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit chimeraHit, 300.0f, _chimeraPillarLayer))
        {
            CreateOutline(chimeraHit, OutlineType.Pillars);
        }
        else if (_currentOutlineType == OutlineType.Pillars)
        {
            RemoveOutline();
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 300.0f, _figurineLayer))
        {
            CreateOutline(hit, OutlineType.Figurines);
        }
        else if (_currentOutlineType == OutlineType.Figurines)
        {
            RemoveOutline();
        }

        if (Physics.Raycast(ray, out RaycastHit upgradeHit, 300.0f, _upgradesLayer))
        {
            UpgradeNode upgradeNode = upgradeHit.transform.GetComponent<UpgradeNode>();
            if (upgradeNode.IsClickable)
            {
                CreateOutline(upgradeHit, OutlineType.Upgrades);
            }
        }
        else if (_currentOutlineType == OutlineType.Upgrades)
        {
            RemoveOutline();
        }
    }
}