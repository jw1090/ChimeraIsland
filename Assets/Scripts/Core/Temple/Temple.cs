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

    private TutorialManager _tutorialManager = null;

    public TempleBuyChimeras TempleBuyChimeras { get => _templeBuyChimeras; }
    public TempleCollections TempleCollections { get => _templeCollections; }
    public TempleUpgrades TempleUpgrades { get => _templeUpgrades; }
    public Transform StartNode { get => _startNode; }
    public Transform BuyingCamNode { get => _buyingNode; }
    public Transform CollectionCamNode { get => _collectionNode; }
    public Transform UpgradeCamNode { get => _upgradeNode; }
    public ChimeraGallery ChimeraGallery { get => _chimeraGallery; }

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
        _tutorialManager.ShowTutorialStage(TutorialStageType.BuyChimera);
    }
}