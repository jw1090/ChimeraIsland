using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _marketplace;
    [SerializeField] private Button _openChimerasButton;
    [SerializeField] private Button _closeChimerasButton;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder;
    [SerializeField] private UIWallet[] _essenceWallets = null;

    public UIManager Initialize()
    {
        Debug.Log("<color=Orange> Initializing MenuManager ... </color>");
        CloseAll();
        UpdateWallets();

        return this;
    }

    public void CloseAll()
    {
        _marketplace.SetActive(false);
        _openChimerasButton.gameObject.SetActive(true);
        _closeChimerasButton.gameObject.SetActive(false);
        _detailsFolder.gameObject.SetActive(false);
    }

    public void OpenChimeraPanel()
    {
        CloseAll();
        _openChimerasButton.gameObject.SetActive(false);
        _closeChimerasButton.gameObject.SetActive(true);
        _detailsFolder.gameObject.SetActive(true);
    }

    public void OpenMarketplace()
    {
        CloseAll();
        _marketplace.SetActive(true);
    }

    public void UpdateDetails()
    {
        _detailsFolder.UpdateDetailsList();
    }

    public void UpdateWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.UpdateWallet();
        }
    }
}