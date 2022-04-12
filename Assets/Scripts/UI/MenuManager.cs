using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject OpenChimerasButton;
    [SerializeField] private GameObject ChimerasPanel;
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
        ChimerasPanel.SetActive(false);
        Marketplace.SetActive(false);
    }

    public void OpenChimeraPanel()
    {
        CloseAll();
        OpenChimerasButton.SetActive(false);
        ChimerasPanel.SetActive(true);
        CloseChimerasButton.SetActive(true);
    }

    public void OpenMarketplace()
    {
        CloseAll();
        Marketplace.SetActive(true);
    }
}