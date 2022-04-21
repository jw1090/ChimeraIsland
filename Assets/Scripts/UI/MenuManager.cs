using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ChimeraDetailsFolder _chimeraDetailsFolder = null;
    [SerializeField] private GameObject _openDetailsButton = null;
    [SerializeField] private GameObject _closeDetailsButton = null;
    [SerializeField] private GameObject _detailsPanel = null;
    [SerializeField] private GameObject _marketplacePanel = null;

    public void Initialize()
    {
        Debug.Log("<color=Orange> Initializing MenuManager ... </color>");
        _chimeraDetailsFolder.Initialize();
        CloseAll();
    }

    public void CloseAll()
    {
        _openDetailsButton.SetActive(true);
        _closeDetailsButton.SetActive(false);
        _detailsPanel.SetActive(false);
        _marketplacePanel.SetActive(false);
    }

    public void OpenDetailsPanel()
    {
        CloseAll();
        _openDetailsButton.SetActive(false);
        _closeDetailsButton.SetActive(true);
        _detailsPanel.SetActive(true);
    }

    public void OpenMarketplace()
    {
        CloseAll();
        _marketplacePanel.SetActive(true);
    }
}