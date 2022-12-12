using UnityEngine;

public class Temple : MonoBehaviour
{
    [Header("Sections")]
    [SerializeField] TempleCollections _templeCollections = null;
    [SerializeField] TempleBuyChimeras _templeBuyChimeras = null;

    [Header("Position Nodes")]
    [SerializeField] Transform _startNode = null;
    [SerializeField] Transform _buyingNode = null;
    [SerializeField] Transform _collectionNode = null;
    [SerializeField] Transform _upgradeNode = null;

    public TempleBuyChimeras TempleBuyChimeras { get => _templeBuyChimeras; }
    public TempleCollections TempleCollections { get => _templeCollections; }
    public Transform StartNode { get => _startNode; }
    public Transform BuyingNode { get => _buyingNode; }
    public Transform CollectionNode { get => _collectionNode; }
    public Transform UpgradeNode { get => _upgradeNode; }

    public Temple Initialize()
    {
        _templeBuyChimeras.Initialize(this);
        _templeCollections.Initialize();

        return this;
    }

    public void SceneSetup()
    {
        _templeCollections.Build();
    }
}