using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Marketplace _marketplace = null;
    [SerializeField] private Button _openDetailsButton = null;
    [SerializeField] private Button _closeDetailsButton = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
    [SerializeField] private TransferMap _transferMap = null;
    [SerializeField] private ReleaseSlider _releaseSlider = null;
    [SerializeField] private UITutorialOverlay _tutorialOverlay = null;
    [SerializeField] private UIWallet[] _essenceWallets = null;
    private UISceneChange _sceneChange = null;

    public ReleaseSlider ReleaseSlider { get => _releaseSlider; }
    public UITutorialOverlay TutorialOverlay { get => _tutorialOverlay; }

    public UIManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _sceneChange = GetComponent<UISceneChange>();

        CloseAll();

        _tutorialOverlay.Initialize();
        _sceneChange.Initialize();

        return this;
    }

    public void InitializeUIElements()
    {
        _marketplace.Initialize();
        _detailsFolder.Initialize();
        InitializeWallets();
        _transferMap.Initialize();
    }

    private void InitializeWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.Initialize();
        }
    }

    public void CloseAll()
    {
        _openDetailsButton.gameObject.SetActive(true);
        _marketplace.gameObject.SetActive(false);
        _closeDetailsButton.gameObject.SetActive(false);
        _detailsFolder.gameObject.SetActive(false);
        _transferMap.gameObject.SetActive(false);
    }

    public void OpenDetailsPanel()
    {
        _detailsFolder.CheckDetails();

        CloseAll();
        _openDetailsButton.gameObject.SetActive(false);
        _closeDetailsButton.gameObject.SetActive(true);
        _detailsFolder.gameObject.SetActive(true);
    }

    public void OpenMarketplace()
    {
        CloseAll();
        _marketplace.gameObject.SetActive(true);
    }

    public void OpenTransferMap(Chimera chimera)
    {
        CloseAll();
        _transferMap.Open(chimera);
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