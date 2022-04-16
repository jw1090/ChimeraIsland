using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int currentEssence = 0;

    [Header("Habitat")]
    [SerializeField] private Habitat _ActiveHabitat = null;

    [Header("References")]
    [SerializeField] private Camera cam = null;
    [SerializeField] private PatrolNodes patrolNodes = null;
    [SerializeField] private ChimeraDetailsFolder chimeraDetailsFolder = null;
    [SerializeField] private TextMeshProUGUI[] essenceWallets = null;

    [Header("Chimera Remove")]
    [SerializeField] private float clickHeldSeconds = 2.0f;
    [SerializeField] private GameObject slider;
    private static GameManager gameManagerInstance;
    public static GameManager Instance { get { return gameManagerInstance; } }
    private RaycastHit hitInfo;
    private float clickHeldCounter = 0.0f;
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
    private void Update()
    {
        checkRemove();
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

    public List<Transform> GetPatrolNodes()
    {
        return patrolNodes.GetNodes();
    }
    private void checkRemove()
    {
        //check remove held down
        if (Input.GetMouseButton(0))
        {
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hitInfo.collider.tag == "Facility" && hitInfo.collider.GetComponent<Facility>().IsActive() && hitInfo.collider.GetComponent<Facility>().isChimeraStored())
            {
                clickHeldCounter += Time.deltaTime;
                if (clickHeldCounter >= clickHeldSeconds)
                {
                    hitInfo.collider.GetComponent<Facility>().RemoveChimera();
                    clickHeldCounter = 0.0f;
                }
            }
            else clickHeldCounter = 0.0f;
        }
        else clickHeldCounter = 0.0f;

        //control remove slider
        if (clickHeldCounter > 0.0f)
        {
            slider.SetActive(true);
            slider.transform.position = Input.mousePosition + new Vector3(75.0f, 0.0f, 0.0f);
            slider.gameObject.GetComponent<Slider>().value = clickHeldCounter / 2.0f;
        }
        else slider.SetActive(false);
    }
    #region Getters & Setters
    public int GetEssence() { return currentEssence; }
    public Habitat GetActiveHabitat() { return _ActiveHabitat; }
    #endregion
}