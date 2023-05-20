using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceManager : MonoBehaviour
{
    private Sprite _defaultChimeraSprite = null;
    private Sprite _chimeraASprite = null;
    private Sprite _chimeraA1Sprite = null;
    private Sprite _chimeraA2Sprite = null;
    private Sprite _chimeraA3Sprite = null;
    private Sprite _chimeraBSprite = null;
    private Sprite _chimeraB1Sprite = null;
    private Sprite _chimeraB2Sprite = null;
    private Sprite _chimeraB3Sprite = null;
    private Sprite _chimeraCSprite = null;
    private Sprite _chimeraC1Sprite = null;
    private Sprite _chimeraC2Sprite = null;
    private Sprite _chimeraC3Sprite = null;

    private Sprite _waterSprite = null;
    private Sprite _fireSprite = null;
    private Sprite _grassSprite = null;
    private Sprite _waterDetailsSprite = null;
    private Sprite _fireDetailsSprite = null;
    private Sprite _grassDetailsSprite = null;

    private Sprite _staminaSprite = null;
    private Sprite _wisdomSprite = null;
    private Sprite _explorationSprite = null;

    private Sprite _staminaSpriteBorder = null;
    private Sprite _wisdomSpriteBorder = null;
    private Sprite _explorationSpriteBorder = null;

    private Sprite _essenceSprite = null;
    private Sprite _fossilSprite = null;
    private Sprite _upgradeSprite = null;

    private Sprite _essenceBG = null;
    private Sprite _fossilBG = null;
    private Sprite _upgradeBG = null;

    private Texture2D _mouseDefault = null;
    private Texture2D _mouseClickable = null;
    private Texture2D _mouseDragable = null;
    private Texture2D _mouseDragging = null;
    private Texture2D _mouseMinable = null;
    private Texture2D _mouseRotate = null;

    private GameObject _chimeraBasePrefabA = null;
    private GameObject _chimeraBasePrefabB = null;
    private GameObject _chimeraBasePrefabC = null;
    private GameObject _chimeraEvolutionPrefabA = null;
    private GameObject _chimeraEvolutionPrefabA1 = null;
    private GameObject _chimeraEvolutionPrefabA2 = null;
    private GameObject _chimeraEvolutionPrefabA3 = null;
    private GameObject _chimeraEvolutionPrefabB = null;
    private GameObject _chimeraEvolutionPrefabB1 = null;
    private GameObject _chimeraEvolutionPrefabB2 = null;
    private GameObject _chimeraEvolutionPrefabB3 = null;
    private GameObject _chimeraEvolutionPrefabC = null;
    private GameObject _chimeraEvolutionPrefabC1 = null;
    private GameObject _chimeraEvolutionPrefabC2 = null;
    private GameObject _chimeraEvolutionPrefabC3 = null;

    private GameObject _inputManager = null;
    private GameObject _uiManagerPrefab = null;
    private GameObject _questManagerPrefab = null;

    public GameObject InputManager { get => _inputManager; }
    public GameObject UIManager { get => _uiManagerPrefab; }
    public GameObject QuestManager { get => _uiManagerPrefab; }

    public ResourceManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _defaultChimeraSprite = Resources.Load<Sprite>("Chimera/DefaultChimera-Icon");
        _chimeraASprite = Resources.Load<Sprite>("Chimera/A-Icon");
        _chimeraA1Sprite = Resources.Load<Sprite>("Chimera/A1-Icon");
        _chimeraA2Sprite = Resources.Load<Sprite>("Chimera/A2-Icon");
        _chimeraA3Sprite = Resources.Load<Sprite>("Chimera/A3-Icon");
        _chimeraBSprite = Resources.Load<Sprite>("Chimera/B-Icon");
        _chimeraB1Sprite = Resources.Load<Sprite>("Chimera/B1-Icon");
        _chimeraB2Sprite = Resources.Load<Sprite>("Chimera/B2-Icon");
        _chimeraB3Sprite = Resources.Load<Sprite>("Chimera/B3-Icon");
        _chimeraCSprite = Resources.Load<Sprite>("Chimera/C-Icon");
        _chimeraC1Sprite = Resources.Load<Sprite>("Chimera/C1-Icon");
        _chimeraC2Sprite = Resources.Load<Sprite>("Chimera/C2-Icon");
        _chimeraC3Sprite = Resources.Load<Sprite>("Chimera/C3-Icon");

        _waterSprite = Resources.Load<Sprite>("Elements/water");
        _fireSprite = Resources.Load<Sprite>("Elements/fire");
        _grassSprite = Resources.Load<Sprite>("Elements/grass");
        _waterDetailsSprite = Resources.Load<Sprite>("Elements/waterDetails");
        _fireDetailsSprite = Resources.Load<Sprite>("Elements/fireDetails");
        _grassDetailsSprite = Resources.Load<Sprite>("Elements/grassDetails");

        _staminaSprite = Resources.Load<Sprite>("Stats/Stamina");
        _wisdomSprite = Resources.Load<Sprite>("Stats/Wisdom");
        _explorationSprite = Resources.Load<Sprite>("Stats/Exploration");

        _staminaSpriteBorder = Resources.Load<Sprite>("Stats/StaminaBorder");
        _wisdomSpriteBorder = Resources.Load<Sprite>("Stats/WisdomBorder");
        _explorationSpriteBorder = Resources.Load<Sprite>("Stats/ExplorationBorder");

        _essenceSprite = Resources.Load<Sprite>("Expedition/Essence");
        _fossilSprite = Resources.Load<Sprite>("Expedition/Fossil");
        _upgradeSprite = Resources.Load<Sprite>("Expedition/Upgrade");

        _essenceBG = Resources.Load<Sprite>("Expedition/EssenceBG");
        _fossilBG = Resources.Load<Sprite>("Expedition/FossilBG");
        _upgradeBG = Resources.Load<Sprite>("Expedition/UpgradeBG");

        _mouseDefault = Resources.Load<Texture2D>("Mouse/Default Cursor");
        _mouseClickable = Resources.Load<Texture2D>("Mouse/Clickable Cursor");
        _mouseDragable = Resources.Load<Texture2D>("Mouse/Dragable Cursor");
        _mouseDragging = Resources.Load<Texture2D>("Mouse/Dragging Cursor");
        _mouseMinable = Resources.Load<Texture2D>("Mouse/Minable Cursor");
        _mouseRotate = Resources.Load<Texture2D>("Mouse/Rotate Cursor");

        _chimeraBasePrefabA = Resources.Load<GameObject>("Chimera/A");
        _chimeraBasePrefabB = Resources.Load<GameObject>("Chimera/B");
        _chimeraBasePrefabC = Resources.Load<GameObject>("Chimera/C");
        _chimeraEvolutionPrefabA = Resources.Load<GameObject>("Chimera/Models/Family A/A Model");
        _chimeraEvolutionPrefabA1 = Resources.Load<GameObject>("Chimera/Models/Family A/A1 Model");
        _chimeraEvolutionPrefabA2 = Resources.Load<GameObject>("Chimera/Models/Family A/A2 Model");
        _chimeraEvolutionPrefabA3 = Resources.Load<GameObject>("Chimera/Models/Family A/A3 Model");
        _chimeraEvolutionPrefabB = Resources.Load<GameObject>("Chimera/Models/Family B/B Model");
        _chimeraEvolutionPrefabB1 = Resources.Load<GameObject>("Chimera/Models/Family B/B1 Model");
        _chimeraEvolutionPrefabB2 = Resources.Load<GameObject>("Chimera/Models/Family B/B2 Model");
        _chimeraEvolutionPrefabB3 = Resources.Load<GameObject>("Chimera/Models/Family B/B3 Model");
        _chimeraEvolutionPrefabC = Resources.Load<GameObject>("Chimera/Models/Family C/C Model");
        _chimeraEvolutionPrefabC1 = Resources.Load<GameObject>("Chimera/Models/Family C/C1 Model");
        _chimeraEvolutionPrefabC2 = Resources.Load<GameObject>("Chimera/Models/Family C/C2 Model");
        _chimeraEvolutionPrefabC3 = Resources.Load<GameObject>("Chimera/Models/Family C/C3 Model");

        _uiManagerPrefab = Resources.Load<GameObject>("UI Manager");
        _inputManager = Resources.Load<GameObject>("Input Manager");
        _questManagerPrefab = Resources.Load<GameObject>("Quest Manager");

        return this;
    }

    public Texture2D GetCursorTexture(CursorType cursorType)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return _mouseDefault;
        }

        switch (cursorType)
        {
            case CursorType.Default:
                return _mouseDefault;
            case CursorType.Clickable:
                return _mouseClickable;
            case CursorType.Dragable:
                return _mouseDragable;
            case CursorType.Dragging:
                return _mouseDragging;
            case CursorType.Minable:
                return _mouseMinable;
            case CursorType.Rotate:
                return _mouseRotate;
            default:
                Debug.LogError($"Unhandled cursor type: {cursorType}. Please change!");
                return null;
        }
    }

    public GameObject GetChimeraBasePrefab(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
            case ChimeraType.A1:
            case ChimeraType.A2:
            case ChimeraType.A3:
                return _chimeraBasePrefabA;
            case ChimeraType.B:
            case ChimeraType.B1:
            case ChimeraType.B2:
            case ChimeraType.B3:
                return _chimeraBasePrefabB;
            case ChimeraType.C:
            case ChimeraType.C1:
            case ChimeraType.C2:
            case ChimeraType.C3:
                return _chimeraBasePrefabC;
            default:
                Debug.LogError($"Unhandled chimera type: {chimeraType}. Please change!");
                return null;
        }
    }

    public GameObject GetChimeraEvolution(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
                return _chimeraEvolutionPrefabA;
            case ChimeraType.A1:
                return _chimeraEvolutionPrefabA1;
            case ChimeraType.A2:
                return _chimeraEvolutionPrefabA2;
            case ChimeraType.A3:
                return _chimeraEvolutionPrefabA3;
            case ChimeraType.B:
                return _chimeraEvolutionPrefabB;
            case ChimeraType.B1:
                return _chimeraEvolutionPrefabB1;
            case ChimeraType.B2:
                return _chimeraEvolutionPrefabB2;
            case ChimeraType.B3:
                return _chimeraEvolutionPrefabB3;
            case ChimeraType.C:
                return _chimeraEvolutionPrefabC;
            case ChimeraType.C1:
                return _chimeraEvolutionPrefabC1;
            case ChimeraType.C2:
                return _chimeraEvolutionPrefabC2;
            case ChimeraType.C3:
                return _chimeraEvolutionPrefabC3;
            default:
                Debug.LogError($"Unhandled chimera type: {type}");
                return null;
        }
    }

    public Sprite GetChimeraSprite(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
                return _chimeraASprite;
            case ChimeraType.A1:
                return _chimeraA1Sprite;
            case ChimeraType.A2:
                return _chimeraA2Sprite;
            case ChimeraType.A3:
                return _chimeraA3Sprite;
            case ChimeraType.B:
                return _chimeraBSprite;
            case ChimeraType.B1:
                return _chimeraB1Sprite;
            case ChimeraType.B2:
                return _chimeraB2Sprite;
            case ChimeraType.B3:
                return _chimeraB3Sprite;
            case ChimeraType.C:
                return _chimeraCSprite;
            case ChimeraType.C1:
                return _chimeraC1Sprite;
            case ChimeraType.C2:
                return _chimeraC2Sprite;
            case ChimeraType.C3:
                return _chimeraC3Sprite;
            default:
                Debug.LogWarning($"Returning Default Sprite, please change.");
                return _defaultChimeraSprite;
        }
    }

    public Sprite GetElementSprite(ElementType elementalType)
    {
        switch (elementalType)
        {
            case ElementType.Water:
                return _waterSprite;
            case ElementType.Fire:
                return _fireSprite;
            case ElementType.Grass:
                return _grassSprite;
            default:
                Debug.LogWarning($"{elementalType} is invalid, please change!");
                return null;
        }
    }

    public Sprite GetElementSimpleSprite(ElementType elementalType)
    {
        switch (elementalType)
        {
            case ElementType.Water:
                return _waterDetailsSprite;
            case ElementType.Fire:
                return _fireDetailsSprite;
            case ElementType.Grass:
                return _grassDetailsSprite;
            default:
                Debug.LogWarning($"{elementalType} is invalid, please change!");
                return null;
        }
    }

    public Sprite GetStatSprite(StatType statType)
    {
        switch (statType)
        {
            case StatType.Stamina:
                return _staminaSprite;
            case StatType.Wisdom:
                return _wisdomSprite;
            case StatType.Exploration:
                return _explorationSprite;
            default:
                Debug.LogWarning($"{statType} is invalid, please change!");
                return null;
        }
    }

    public Sprite GetModifierSprite(ModifierType badgeType)
    {
        switch (badgeType)
        {
            case ModifierType.Water:
                return _waterSprite;
            case ModifierType.Grass:
                return _grassSprite;
            case ModifierType.Fire:
                return _fireSprite;
            case ModifierType.Stamina:
                return _staminaSpriteBorder;
            case ModifierType.Wisdom:
                return _wisdomSpriteBorder;
            case ModifierType.Exploration:
                return _explorationSpriteBorder;
            default:
                Debug.LogWarning($"Badge Type [{badgeType}] is invalid, please change!");
                return null;
        }
    }

    public Sprite GetExpeditionTypeSprite(ExpeditionType expeditionType)
    {
        switch (expeditionType)
        {
            case ExpeditionType.Essence:
                return _essenceSprite;
            case ExpeditionType.Fossils:
                return _fossilSprite;
            case ExpeditionType.HabitatUpgrade:
                return _upgradeSprite;
            default:
                Debug.LogWarning($"Expedition Type [{expeditionType}] is invalid, please change!");
                return null;
        }
    }

    public Sprite GetExpeditionTypeSpriteBG(ExpeditionType expeditionType)
    {
        switch (expeditionType)
        {
            case ExpeditionType.Essence:
                return _essenceBG;
            case ExpeditionType.Fossils:
                return _fossilBG;
            case ExpeditionType.HabitatUpgrade:
                return _upgradeBG;
            default:
                Debug.LogWarning($"Expedition Type [{expeditionType}] is invalid, please change!");
                return null;
        }
    }
}