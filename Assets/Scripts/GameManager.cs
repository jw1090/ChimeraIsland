using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int currentEssence = 0;

    [Header("Habitats")]
    [SerializeField] private Habitat activeHabitat;
    [SerializeField] private Habitat Habitat1;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI essenceWallet;

    private static GameManager gameManagerInstance;
    public static GameManager Instance { get { return gameManagerInstance; } }

    // - Made by: Joe 2/2/2022
    // - Basic Singleton Implementation
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

    private void Start()
    {
        essenceWallet.text = currentEssence.ToString();
    }

    // - Made by: Joe 2/2/2022
    // - Increases your essence.
    public void IncreaseEssence(int amount)
    {
        currentEssence += amount;
        essenceWallet.text = currentEssence.ToString();
    }

    // - Made by: Joe 2/2/2022
    // - Spends Essence and detects if you can afford it. Return false if you cannot afford and return true if you can.
    public bool SpendEssence(int amount)
    {
        if(currentEssence - amount < 0)
        {
            return false;
        }

        currentEssence -= amount;
        essenceWallet.text = currentEssence.ToString();

        return true;
    }

    // - Adds Chimera to the habitat by pressing the add chimera button.
    // - Make sure the capacity has room and that you are not instantiating the prefab under the map
    public bool AddChimera(Chimera chimera, Habitat newHabitat)
    {
        return false;
    }

    // - Moves Chimera from one habitat to another
    public bool TransferChimera(Chimera chimera, Habitat originHabitat, Habitat newHabitat)
    {
        return false;
    }

    #region Getters & Setters
    public int GetEssence() { return currentEssence; }
    public Habitat GetActiveHabitat() { return activeHabitat; }
    #endregion
}