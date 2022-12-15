using UnityEngine;

public class Temple : MonoBehaviour
{
    [Header("Sections")]
    [SerializeField] TempleCollections _templeCollections = null;
    [SerializeField] TempleBuyChimeras _templeBuyChimeras = null;
    [SerializeField] TempleUpgrades _templeUpgrades = null;

    [Header("Position Nodes")]
    [SerializeField] Transform _startNode = null;
    [SerializeField] Transform _buyingNode = null;
    [SerializeField] Transform _collectionNode = null;
    [SerializeField] Transform _upgradeNode = null;

    [Header("Collection Nodes")]
    [SerializeField] Transform _waterNode = null;
    [SerializeField] Transform _fireNode = null;
    [SerializeField] Transform _grassNode = null;

    [Header("Upgrade Nodes")]
    [SerializeField] Transform _explorationNode = null;
    [SerializeField] Transform _wisdomNode = null;
    [SerializeField] Transform _staminaNode = null;

    [Header("Buying Nodes")]
    [SerializeField] Transform _aNode = null;
    [SerializeField] Transform _bNode = null;
    [SerializeField] Transform _cNode = null;

    public TempleBuyChimeras TempleBuyChimeras { get => _templeBuyChimeras; }
    public TempleCollections TempleCollections { get => _templeCollections; }
    public TempleUpgrades TempleUpgrades { get => _templeUpgrades; }
    public Transform StartNode { get => _startNode; }
    public Transform BuyingNode { get => _buyingNode; }
    public Transform CollectionNode { get => _collectionNode; }
    public Transform UpgradeNode { get => _upgradeNode; }
    public Transform WaterNode { get => _waterNode; }
    public Transform FireNode { get => _fireNode; }
    public Transform GrassNode { get => _grassNode; }
    public Transform ANode { get => _aNode; }
    public Transform BNode { get => _bNode; }
    public Transform CNode { get => _cNode; }
    public Transform ExplorationNode { get => _explorationNode; }
    public Transform WisdomNode { get => _wisdomNode; }
    public Transform StaminaNode { get => _staminaNode; }

    public Temple Initialize()
    {
        _templeBuyChimeras.Initialize(this);
        _templeCollections.Initialize();
        _templeUpgrades.Initalize();

        return this;
    }

    public void SceneSetup()
    {
        _templeCollections.Build();
    }
}