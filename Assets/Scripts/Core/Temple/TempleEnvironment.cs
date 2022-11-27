using UnityEngine;

public class TempleEnvironment : MonoBehaviour
{
    [Header("Position Nodes")]
    [SerializeField] Transform _buyingNode = null;
    [SerializeField] Transform _collectionNode = null;
    [SerializeField] Transform _upgradeNode = null;

    public Transform BuyingNode { get => _buyingNode; }
    public Transform CollectionNode { get => _collectionNode; }
    public Transform UpgradeNode { get => _upgradeNode; }
}