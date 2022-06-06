using UnityEngine;

public class FacilitySign : MonoBehaviour
{
    [SerializeField] SignType _type = SignType.None;
    [SerializeField] SpriteRenderer _enduranceIcon = null;
    [SerializeField] SpriteRenderer _expeditionIcon = null;
    [SerializeField] SpriteRenderer _intelligenceIcon = null;
    [SerializeField] SpriteRenderer _strengthIcon = null;

    private void Awake()
    {
        LevelManager.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        ResetIcons();
        SetSign();
    }

    private void SetSign()
    {
        switch (_type)
        {
            case SignType.Endurance:
                _enduranceIcon.gameObject.SetActive(true);
                return;
            case SignType.Expedition:
                _expeditionIcon.gameObject.SetActive(true);
                return;
            case SignType.Intelligence:
                _intelligenceIcon.gameObject.SetActive(true);
                return;
            case SignType.Strength:
                _strengthIcon.gameObject.SetActive(true);
                return;
            default:
                Debug.LogError("Default StatType please change!");
                break;
        }
    }

    private void ResetIcons()
    {
        _enduranceIcon.gameObject.SetActive(false);
        _intelligenceIcon.gameObject.SetActive(false);
        _strengthIcon.gameObject.SetActive(false);
    }
}