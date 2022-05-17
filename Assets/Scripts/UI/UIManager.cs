using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Marketplace _marketplace = null;
    [SerializeField] private Button _openChimerasButton = null;
    [SerializeField] private Button _closeChimerasButton = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private TransferMap _transferMap = null;
    [SerializeField] private UIWallet[] _essenceWallets = null;

    public UIManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing MenuManager ... </color>");
        CloseAll();
        InitializeWallets();
        return this;
    }

    public void LoadMarketplace(Habitat habitat)
    {
        _marketplace.Initialize(habitat);
    }

    public void LoadDetails(Habitat habitat)
    {
        _detailsFolder.Initialize(habitat);
    }

    public void CloseAll()
    {
        _marketplace.gameObject.SetActive(false);
        _openChimerasButton.gameObject.SetActive(true);
        _closeChimerasButton.gameObject.SetActive(false);
        _detailsFolder.gameObject.SetActive(false);
    }

    public void OpenDetailsPanel()
    {
        _detailsFolder.CheckDetails();

        CloseAll();
        _openChimerasButton.gameObject.SetActive(false);
        _closeChimerasButton.gameObject.SetActive(true);
        _detailsFolder.gameObject.SetActive(true);
    }

    public void OpenMarketplace()
    {
        CloseAll();
        _marketplace.gameObject.SetActive(true);
    }

    public void UpdateDetails()
    {
        _detailsFolder.UpdateDetailsList();
    }

    private void InitializeWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.Initialize();
        }
    }

    public void UpdateWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.UpdateWallet();
        }
    }

    public void OpenTransferMap(Chimera chimera)
    {
        CloseAll();
        _transferMap.Load(chimera);
        _transferMap.gameObject.SetActive(true);
    }

    public void CloseTransferMap()
    {
        _transferMap.gameObject.SetActive(false);
        OpenDetailsPanel();
    }
}