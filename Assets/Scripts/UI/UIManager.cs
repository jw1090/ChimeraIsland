using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Marketplace _marketplace = null;
    [SerializeField] private Button _openChimerasButton = null;
    [SerializeField] private Button _closeChimerasButton = null;
    [SerializeField] private ChimeraDetailsFolder _detailsFolder = null;
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

    public void InitializeMarketplace(Habitat habitat)
    {
        _marketplace.Initialize(habitat);
    }

    public void InitializeDetails(Habitat habitat)
    {
        _detailsFolder.Initialize(habitat);
    }

    public void InitializeWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.Initialize();
        }
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

    public void UpdateWallets()
    {
        foreach (var wallet in _essenceWallets)
        {
            wallet.UpdateWallet();
        }
    }
}