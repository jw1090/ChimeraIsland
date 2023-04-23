using UnityEngine;

public class UpgradeSections : MonoBehaviour
{
    [SerializeField] private FacilityType _facilityType = FacilityType.None;
    [SerializeField] private UpgradeNode _tier1 = null;
    [SerializeField] private UpgradeNode _tier2 = null;
    [SerializeField] private UpgradeNode _tier3 = null;
    [SerializeField] private GameObject _rubble = null;
    [SerializeField] private Material _upgradeConnectionOn = null;
    [SerializeField] private MeshRenderer _upgradeConnection1 = null;
    [SerializeField] private MeshRenderer _upgradeConnection2 = null;
    private HabitatManager _habitatManager = null;

    public void Initialize()
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();

        _tier1.Initialize(_facilityType);
        _tier2.Initialize(_facilityType);
        _tier3.Initialize(_facilityType);

        EvaluateUpgradeStates();
    }

    public void EvaluateUpgradeStates()
    {
        int currentTier = _habitatManager.GetFacilityTier(_facilityType);

        switch (currentTier)
        {
            case 0:
                _rubble.SetActive(true);
                _tier1.StatefulObject.SetState("To Buy", true);
                _tier2.StatefulObject.SetState("To Buy", true);
                _tier3.StatefulObject.SetState("To Buy", true);
                break;
            case 1:
                _rubble.SetActive(false);
                _tier1.StatefulObject.SetState("Active", true);
                _tier2.StatefulObject.SetState("To Buy", true);
                _tier3.StatefulObject.SetState("To Buy", true);
                break;
            case 2:
                _rubble.SetActive(false);
                _upgradeConnection1.material = _upgradeConnectionOn;
                _tier1.StatefulObject.SetState("Active", true);
                _tier2.StatefulObject.SetState("Active", true);
                _tier3.StatefulObject.SetState("To Buy", true);
                break;
            case 3:
                _rubble.SetActive(false);
                _upgradeConnection1.material = _upgradeConnectionOn;
                _upgradeConnection2.material = _upgradeConnectionOn;
                _tier1.StatefulObject.SetState("Active", true);
                _tier2.StatefulObject.SetState("Active", true);
                _tier3.StatefulObject.SetState("Active", true);
                break;
            default:
                Debug.LogError($"Current facility tier for {_facilityType} is invalid, Tier: {currentTier}");
                break;
        }
    }
}