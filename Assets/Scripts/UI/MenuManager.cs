using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject OpenChimerasButton;
    [SerializeField] private GameObject DetailsPanel;
    [SerializeField] private GameObject CloseChimerasButton;
    [SerializeField] private GameObject Marketplace;

    private void Start()
    {
        CloseAll();
    }

    public void CloseAll()
    {
        OpenChimerasButton.SetActive(true);
        CloseChimerasButton.SetActive(false);
        DetailsPanel.SetActive(false);
        Marketplace.SetActive(false);
    }

    public void OpenChimeraPanel()
    {
        CloseAll();
        OpenChimerasButton.SetActive(false);
        DetailsPanel.SetActive(true);
        CloseChimerasButton.SetActive(true);
    }

    public void OpenMarketplace()
    {
        CloseAll();
        Marketplace.SetActive(true);
    }
}