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

    [Header("References")]
    [SerializeField] private TextMeshProUGUI[] essenceWallets;
    [SerializeField] private Camera cam;

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
        UpdateWallets();
    }

    // - Made by: Joe 2/2/2022
    // - Increases your essence.
    public void IncreaseEssence(int amount)
    {
        currentEssence += amount;
        UpdateWallets();
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
        UpdateWallets();

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

    private void ChimeraMouseTap()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mouse_pos = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mouse_pos);

            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider.CompareTag("Chimera"))
            {
                //Debug.Log("Tap on a chimera.");
                Transform chimera = hit.collider.gameObject.transform.parent;
                chimera.GetComponent<Chimera>().ChimeraTap();
            }
        }
    }

    private void UpdateWallets()
    {
        foreach (var wallet in essenceWallets)
        {
            wallet.text = currentEssence.ToString();
        }
    }

    public int FacilityAffinityCheck(StatType compareType)
    {
        int value = 0;
        foreach (Facility facility in GetActiveHabitat().GetFacilities())
        {
            StatType statType = facility.GetStatType();

            if (compareType == statType)
            {
                value = 1;
            }
            else
            {
                value = -1;
            }
        }
            return value;
    }

    //public int ElementalAffinityCheck(ElementalType compareType)
    //{
    //    int value = 0;
    //    foreach (Chimera chimera in GetActiveHabitat().GetChimeras())
    //    {
    //        ElementalType elementalType = chimera.GetElementalType();

    //        if (Mathf.Abs(compareType - elementalType) == 3)
    //        {
    //            value = 1;
    //        }
    //        else if (Mathf.Abs(compareType - elementalType) == 1)
    //        {
    //            value = -1;
    //        }

            

    //    }
    //    return value;
    //}

    #region Getters & Setters
    public int GetEssence() { return currentEssence; }
    public Habitat GetActiveHabitat() { return activeHabitat; }
    #endregion
}
