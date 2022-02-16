using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private static MenuManager menuManagerInstance;
    public static MenuManager Instance { get { return menuManagerInstance; } }

    // - Made by: Joe 2/2/2022
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}