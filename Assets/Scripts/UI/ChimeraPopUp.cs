using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraPopUp : MonoBehaviour
{
    private Chimera _chimera = null;
    private ResourceManager _resourceManager = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private Image _type = null;
    [SerializeField] private TextMeshProUGUI _exploration = null;
    [SerializeField] private TextMeshProUGUI _stamina = null;
    [SerializeField] private TextMeshProUGUI _wisdom = null;
    [SerializeField] private Color _prefferedGoldColor = new Color();
    [SerializeField] private Color _defaultColor = new Color();
    [SerializeField] private List<Image> _statIcons = new List<Image>();

    public void SetChimera(Chimera chimera)
    {
        _chimera = chimera;

        _name.text = chimera.Name;
        _type.sprite = _resourceManager.GetElementDetailsSprite(_chimera.ElementalType);
        _exploration.text = chimera.Exploration.ToString();
        _stamina.text = chimera.Stamina.ToString();
        _wisdom.text = chimera.Wisdom.ToString();
        DetermineStatGlow();
    }

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        NoPrefferedStat();
    }

    public void DetermineStatGlow()
    {
        if (_chimera == null)
        {
            return;
        }

        switch (_chimera.PreferredStat)
        {
            case StatType.None:
                NoPrefferedStat();
                break;
            case StatType.Exploration:
                _statIcons[0].color = _prefferedGoldColor;
                break;
            case StatType.Stamina:
                _statIcons[1].color = _prefferedGoldColor;
                break;
            case StatType.Wisdom:
                _statIcons[2].color = _prefferedGoldColor;
                break;
            default:
                Debug.LogError($"Unhandled stat [{_chimera.PreferredStat}] please fix!");
                break;
        }
    }

    private void NoPrefferedStat()
    {
        foreach (Image statBG in _statIcons)
        {
            statBG.color = _defaultColor;
        }
    }
}