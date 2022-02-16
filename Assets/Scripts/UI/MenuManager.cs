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
    [SerializeField] private GameObject HabitatShop;

    private static MenuManager menuManagerInstance;
    public static MenuManager Instance { get { return menuManagerInstance; } }

    // - Made by: Joe 2/16/2022
    // - Basic Singleton Implementation
    private void Initialize()
    {
        if (menuManagerInstance != null && menuManagerInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            menuManagerInstance = this;
        }
    }

    void Awake()
    {
        Initialize();
    }

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
        HabitatShop.SetActive(false);
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

    public void OpenHabitatShop()
    {
        CloseAll();
        HabitatShop.SetActive(true);
    }
}