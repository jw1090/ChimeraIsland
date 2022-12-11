using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TooltipSections _statExplanation = null;
    [SerializeField] private TooltipSections _preferenceExplanation = null;
    [SerializeField] private TooltipSections _evolutionBonusExplanation = null;
    private bool _initialized = false;

    public bool BeingUsed { get; set; } = false;
    public ChimeraDetails HoverDetails { get; set; } = null;
    public StatType HoverStatType { get; set; } = StatType.None;

    public void Initialize()
    {
        gameObject.SetActive(false);

        _initialized = true;
    }

    public void LateUpdate()
    {
        if (_initialized == false)
        {
            return;
        }

        BeingUsed = false;
        HoverDetails = null;
    }

    public void FollowMouse()
    {
        transform.position = Input.mousePosition + new Vector3(80.0f, 0.0f, 0.0f);
        gameObject.SetActive(true);
    }

    public void LoadStatText()
    {
        switch (HoverStatType)
        {
            case StatType.Exploration:
                LoadStatText("Exploration", "Decreases Expedition Duration");
                break;
            case StatType.Stamina:
                LoadStatText("Stamina", "Increases Max Energy And Energy Regen Rate");
                break;
            case StatType.Wisdom:
                LoadStatText("Wisdom", "Increases Currency Gain From Expeditions");
                break;
            default:
                break;
        }
    }

    private void LoadStatText(string title, string description)
    {
        _statExplanation.Title.text = title;
        _statExplanation.Description.text = description;
    }

    public void LoadPreferenceText(ChimeraType chimeraType)
    {
        string likeDescription = $"Small Discount To {HoverStatType} Training";
        string dislikeDescription = $"Increased Cost To {HoverStatType} Training";

        _preferenceExplanation.gameObject.SetActive(false);

        switch (chimeraType)
        {
            case ChimeraType.A:
            case ChimeraType.A1:
            case ChimeraType.A2:
            case ChimeraType.A3:
                switch (HoverStatType)
                {
                    case StatType.Exploration:
                        break;
                    case StatType.Stamina:
                        _preferenceExplanation.Title.text = "Like";
                        _preferenceExplanation.Description.text = likeDescription;
                        _preferenceExplanation.gameObject.SetActive(true);
                        break;
                    case StatType.Wisdom:
                        _preferenceExplanation.Title.text = "Dislike";
                        _preferenceExplanation.Description.text = dislikeDescription;
                        _preferenceExplanation.gameObject.SetActive(true);
                        break;
                    default:
                        Debug.LogError($"Unhandled stat type: {HoverStatType}. Please change!");
                        break;
                }
                break;
            case ChimeraType.B:
            case ChimeraType.B1:
            case ChimeraType.B2:
            case ChimeraType.B3:
                switch (HoverStatType)
                {
                    case StatType.Exploration:
                        _preferenceExplanation.Title.text = "Like";
                        _preferenceExplanation.Description.text = likeDescription;
                        _preferenceExplanation.gameObject.SetActive(true);
                        break;
                    case StatType.Stamina:
                        _preferenceExplanation.Title.text = "Dislike";
                        _preferenceExplanation.Description.text = dislikeDescription;
                        _preferenceExplanation.gameObject.SetActive(true);
                        break;
                    case StatType.Wisdom:
                        break;
                    default:
                        Debug.LogError($"Unhandled stat type: {HoverStatType}. Please change!");
                        break;
                }
                break;
            case ChimeraType.C:
            case ChimeraType.C1:
            case ChimeraType.C2:
            case ChimeraType.C3:
                switch (HoverStatType)
                {
                    case StatType.Exploration:
                        _preferenceExplanation.Title.text = "Dislike";
                        _preferenceExplanation.Description.text = dislikeDescription;
                        _preferenceExplanation.gameObject.SetActive(true);
                        break;
                    case StatType.Stamina:
                        break;
                    case StatType.Wisdom:
                        _preferenceExplanation.Title.text = "Like";
                        _preferenceExplanation.Description.text = likeDescription;
                        _preferenceExplanation.gameObject.SetActive(true);
                        break;
                    default:
                        Debug.LogError($"Unhandled stat type: {HoverStatType}. Please change!");
                        break;
                }
                break;
            default:
                Debug.LogError($"Unhandled chimera type: {chimeraType}. Please change!");
                break;
        }
    }

    public void LoadEvolutionBonusText(StatType bonusStatType)
    {
        if (bonusStatType == StatType.None || HoverStatType != bonusStatType)
        {
            if (_evolutionBonusExplanation.gameObject.activeInHierarchy == true)
            {
                _evolutionBonusExplanation.gameObject.SetActive(false);
            }
        }
        else
        {
            switch (bonusStatType)
            {
                case StatType.None:
                    break;
                case StatType.Exploration:
                case StatType.Stamina:
                case StatType.Wisdom:
                    if (_evolutionBonusExplanation.gameObject.activeInHierarchy == false)
                    {
                        _evolutionBonusExplanation.gameObject.SetActive(true);
                    }
                    _evolutionBonusExplanation.Title.text = "Evolution Bonus";
                    _evolutionBonusExplanation.Description.text = $"Additional 50% Discount To {bonusStatType} Training";
                    break;
                default:
                    Debug.LogError($"Unhandled stat type: {HoverStatType}. Please change!");
                    break;
            }
        }
    }
}