using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int _currentEssence = 0;

    [Header("Chimera Remove")]
    [SerializeField] private float _clickHeldSeconds = 2.0f;
    [SerializeField] private float _clickHeldCounter = 0.0f;
    [SerializeField] private GameObject _slider;

    private static GameManager gameManagerInstance;
    public static GameManager Instance { get { return gameManagerInstance; } }
    private IPersistentData _persistentData = null;

    public int GetEssence() { return _currentEssence; }

    // - Basic Singleton Implementation
    private void Initialize()
    {
        Debug.Log("<color=cyan>GameManager Init</color>");
        _persistentData = ServiceLocator.Get<IPersistentData>();
        Debug.Log($"{(_persistentData == null ? "NULL" : "OK")}");
        if (gameManagerInstance != null && gameManagerInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameManagerInstance = this;
        }

        LoadSavedData();
    }

    private void LoadSavedData()
    {
        if (_persistentData != null)
        {
            _currentEssence = _persistentData.LoadDataInt(GameConsts.GameSaveKeys.ESSENCE);
            Debug.Log($"Loaded Essense: {_currentEssence}");
        }
    }

    void Awake()
    {
        GameLoader.CallOnComplete(Initialize);
    }

    private void Update()
    {
        CheckRemove();
    }

    // Increases your essence.
    public void IncreaseEssence(int amount)
    {
        _currentEssence += amount;
        ServiceLocator.Get<MenuManager>().UpdateWallets();
        if (_persistentData != null)
        {
            //Debug.Log("<color=lime>Saving Essence</color>");
            _persistentData.SaveData(GameConsts.GameSaveKeys.ESSENCE, _currentEssence);
        }
        else if(_persistentData == null)
        {
            _persistentData = ServiceLocator.Get<IPersistentData>() as PersistentData;
            //Debug.Log("<color=lime>PERSISTANT DATA IS NULL AAAAAAA</color>");
        }
    }

    // Spends Essence and detects if you can afford it. Return false if you cannot afford and return true if you can.
    public bool SpendEssence(int amount)
    {
        if(_currentEssence - amount < 0)
        {
            return false;
        }

        _currentEssence -= amount;
        ServiceLocator.Get<MenuManager>().UpdateWallets();

        return true;
    }

    private void CheckRemove()
    {
        // Check remove held down.
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 200.0f);

            if(hit.collider == null)
            {
                return;
            }

            if (hit.collider.CompareTag("Facility") && hit.collider.GetComponent<Facility>().IsChimeraStored())
            {
                _clickHeldCounter += Time.deltaTime;
                if (_clickHeldCounter >= _clickHeldSeconds)
                {
                    hit.collider.GetComponent<Facility>().RemoveChimera();
                    _clickHeldCounter = 0.0f;
                }
            }
            else _clickHeldCounter = 0.0f;
        }
        else _clickHeldCounter = 0.0f;

        // Control remove slider
        if (_clickHeldCounter > 0.0f)
        {
            _slider.SetActive(true);
            _slider.transform.position = Input.mousePosition + new Vector3(75.0f, 0.0f, 0.0f);
            _slider.gameObject.GetComponent<Slider>().value = _clickHeldCounter / 2.0f;
        }
        else _slider.SetActive(false);
    }
}