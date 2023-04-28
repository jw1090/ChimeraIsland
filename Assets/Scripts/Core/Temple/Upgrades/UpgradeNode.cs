using UnityEngine;

public class UpgradeNode : MonoBehaviour
{
    [SerializeField] private int _tier = 0;
    private FacilityType _facilityType = FacilityType.None;
    private StatefulObject _statefulObject = null;

    bool _isClickable = false;

    public int Tier { get => _tier; }
    public FacilityType FacilityType { get => _facilityType; }
    public StatefulObject StatefulObject { get => _statefulObject; }
    public bool IsClickable { get => _isClickable; }

    public void SetIsClickable(bool isClickable) { _isClickable = isClickable; }

    public void Initialize(FacilityType facilityType)
    {
        _facilityType = facilityType;

        _statefulObject = GetComponent<StatefulObject>();
    }
}