using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int currentEssence = 0;

    [Header("Habitat")]
    [SerializeField] private Habitat _ActiveHabitat;
    [SerializeField] private Habitat _Habitat1;

    [Header("References")]
    [SerializeField] private Camera cam = null;
    [SerializeField] private ChimeraDetailsFolder chimeraDetailsFolder = null;
    [SerializeField] private TextMeshProUGUI[] essenceWallets = null;

    private static GameManager gameManagerInstance;
    public static GameManager Instance { get { return gameManagerInstance; } }

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
                //chimera.GetComponent<Chimera>().ChimeraTap();
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

    public void UpdateDetailsUI()
    {
        chimeraDetailsFolder.UpdateDetailsList();
    }

    #region Getters & Setters
    public int GetEssence() { return currentEssence; }
    public Habitat GetActiveHabitat() { return _ActiveHabitat; }
    #endregion
}