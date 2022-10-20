using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingChimeraInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _chimeraName = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private List<GameObject> _explorationPreference = new List<GameObject>();
    [SerializeField] private List<GameObject> _staminaPreference = new List<GameObject>();
    [SerializeField] private List<GameObject> _wisdomPreference = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI _chimeraInfo = null;
    private ResourceManager _resourceManager = null;

    public void LoadChimeraData(string name, ElementType iconType, string info)// ElementType iconType, int explorationPreference, int staminaPreference, int wisdomPreference, string info)
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _chimeraName.text = name;
        _icon.sprite = _resourceManager.GetElementSprite(iconType);
        _chimeraInfo.text = info;
    }

}