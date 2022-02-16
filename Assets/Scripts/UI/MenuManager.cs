using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject OpenChimeras;
    [SerializeField] private GameObject ChimeraPanel;

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


}