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

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void LoadChimeraData(EvolutionLogic evolutionLogic)
    {
        _chimeraName.text = evolutionLogic.Name;
        _icon.sprite = _resourceManager.GetElementSprite(evolutionLogic.ElementType);
        _chimeraInfo.text = evolutionLogic.BackgroundInfo;
    }
}