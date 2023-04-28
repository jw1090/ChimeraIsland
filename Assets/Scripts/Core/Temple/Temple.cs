using UnityEngine;

public class Temple : MonoBehaviour
{
    [SerializeField] ChimeraGallery _chimeraGallery = null;

    [Header("Sections")]
    [SerializeField] TempleCollections _templeCollections = null;
    [SerializeField] TempleBuyChimeras _templeBuyChimeras = null;
    [SerializeField] TempleUpgrades _templeUpgrades = null;

    [Header("Position Nodes")]
    [SerializeField] Transform _startNode = null;
    [SerializeField] Transform _buyingNode = null;
    [SerializeField] Transform _collectionNode = null;
    [SerializeField] Transform _upgradeNode = null;

    [Header("Pillar Position Nodes")]
    [SerializeField] Transform _waterNode = null;
    [SerializeField] Transform _fireNode = null;
    [SerializeField] Transform _grassNode = null;

    [Header("Pillars")]
    [SerializeField] GameObject _waterPillar = null;
    [SerializeField] GameObject _firePillar = null;
    [SerializeField] GameObject _grassPillar = null;

    private TutorialManager _tutorialManager = null;
    public TempleBuyChimeras TempleBuyChimeras { get => _templeBuyChimeras; }
    public TempleCollections TempleCollections { get => _templeCollections; }
    public TempleUpgrades TempleUpgrades { get => _templeUpgrades; }
    public Transform StartNode { get => _startNode; }
    public Transform BuyingCamNode { get => _buyingNode; }
    public Transform CollectionCamNode { get => _collectionNode; }
    public Transform UpgradeCamNode { get => _upgradeNode; }
    public Transform WaterNode { get => _waterNode; }
    public Transform FireNode { get => _fireNode; }
    public Transform GrassNode { get => _grassNode; }
    public ChimeraGallery ChimeraGallery { get => _chimeraGallery; }
    public GameObject WaterPillar { get => _waterPillar; }
    public GameObject FirePillar { get => _firePillar; }
    public GameObject GrassPillar { get => _grassPillar; }

    public Temple Initialize()
    {
        _tutorialManager = ServiceLocator.Get<TutorialManager>();
        _templeBuyChimeras.Initialize(this);
        _templeCollections.Initialize();
        _templeUpgrades.Initalize();
        _chimeraGallery.Initialize();

        return this;
    }

    public void SceneSetup()
    {
        _templeCollections.Build();
        _tutorialManager.ShowTutorialStageDelay(TutorialStageType.BuyChimera, 1.5f);
    }
}