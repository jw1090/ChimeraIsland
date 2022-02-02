using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject Habitat1;
    [SerializeField] private GameObject Habitat2;
    [SerializeField] private GameObject Habitat3;
    [SerializeField] private GameObject Habitat4;
    [SerializeField] private GameObject Habitat5;

    private static GameManager gameManagerInstance;
    public static GameManager Instance { get { return gameManagerInstance; } }

    //  - Made by: Joe 2/2/2022
    //  - Basic Singleton Implementation
    private void Initialize()
    {
        if (gameManagerInstance != null && gameManagerInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameManagerInstance = this;
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