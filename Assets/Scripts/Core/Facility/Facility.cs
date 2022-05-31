using UnityEngine;
using UnityEngine.AI;

public class Facility : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] private FacilityType _facilityType = FacilityType.None;
    [SerializeField] private StatType _statType = StatType.None;
    [SerializeField] private int _statModifier = 1;
    [SerializeField] private int _price = 50;

    [Header("Chimera Info")]
    [SerializeField] private Chimera _storedChimera = null;
    [SerializeField] private FacilityIcon _icon = null;

    [Header("Reference")]
    [SerializeField] private GameObject _rubbleObject = null;
    [SerializeField] private GameObject _tier1Object = null;

    private bool _isInitialized = false;
    private int _currentTier = 0;

    public bool IsInitialized { get => _isInitialized; }
    public int CurrentTier { get => _currentTier; }

    public FacilityType GetFacilityType() { return _facilityType; }
    public int GetPrice() { return _price; }

    public bool IsChimeraStored()
    {
        if (_isInitialized == false)
        {
            return false;
        }

        if (_storedChimera == null)
        {
            return false;
        }

        return true;
    }

    // Logic for buying a facility. Enables mesh renderer which is used to visualize the game object.
    public void BuyFacility()
    {
        BoxCollider _collider = GetComponent<BoxCollider>();
        _price = (int)(_price * 7.5);
        ++_currentTier;

        if (_currentTier == 1)
        {
            _isInitialized = true;
            Debug.Log(_facilityType + " was purchased!");

            _rubbleObject.SetActive(false);
            _collider.enabled = true;
            _tier1Object.SetActive(true);
        }
        else
        {
            ++_statModifier;
            Debug.Log(_facilityType + " was increased to Tier " + CurrentTier + "!");
        }

        int newMod = _statModifier + 1;

        Debug.Log(_facilityType + " now generates " + newMod + " " + _statType + " for Chimeras per tick!");
    }

    // Called to properly link a chimera to a facility and adjust its states properly.
    public bool PlaceChimera(Chimera chimera)
    {
        if (_storedChimera != null) // Something is already in the facility.
        {
            Debug.Log("Cannot add " + chimera + ". " + _storedChimera + " is already in this facility.");
            return false;
        }

        _icon.gameObject.SetActive(true);
        _icon.GetComponent<FacilityIcon>().SetIcon(chimera.GetIcon());
        _storedChimera = chimera;
        _storedChimera.SetInFacility(true);

        chimera.gameObject.transform.localPosition = gameObject.transform.localPosition;

        Debug.Log(_storedChimera + " added to the facility.");
        return true;
    }

    // Removes Chimera from facility and cleans up chimera and facility logic.
    public bool RemoveChimera()
    {
        if (_storedChimera == null) // Facility is empty.
        {
            Debug.Log("Cannot remove Chimera, facility is empty.");
            return false;
        }

        AI.Behavior.ChimeraBehavior chimeraBehavior = _storedChimera.gameObject.GetComponent<AI.Behavior.ChimeraBehavior>();
        chimeraBehavior.ChangeState(chimeraBehavior.States[AI.Behavior.StateEnum.Patrol]);

        _icon.RemoveIcon();
        _icon.gameObject.SetActive(false);
        _storedChimera.SetInFacility(false);

        Debug.Log(_storedChimera + " has been removed from the facility.");

        NavMeshHit myNavHit;

        // Find nearby walkable position.
        if (NavMesh.SamplePosition(transform.position, out myNavHit, 100, -1))
        {
            _storedChimera.transform.position = myNavHit.position;
        }
        _storedChimera = null;

        return true;
    }

    public void FacilityTick()
    {
        if(_storedChimera != null)
        {
            _icon.SetIcon(_storedChimera.GetIcon());
            _storedChimera.ExperienceTick(_statType, _statModifier);
            _storedChimera.ExperienceTick(_statType, _statModifier);

            FlatStatBoost();
            HappinessCheck();
        }
    }

    private void FlatStatBoost()
    {
        _storedChimera.ExperienceTick(StatType.Endurance, 1);
        _storedChimera.ExperienceTick(StatType.Intelligence, 1);
        _storedChimera.ExperienceTick(StatType.Strength, 1);
    }

    private void HappinessCheck()
    {
        if(_storedChimera.StatPreference == _statType)
        {
            int happinessAmount = 1;

            _storedChimera.ChangeHappiness(happinessAmount);
        }
    }
}