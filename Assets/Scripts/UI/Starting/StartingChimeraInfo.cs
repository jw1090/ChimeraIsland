using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingChimeraInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _chimeraName = null;
    [SerializeField] private Image _icon = null;
    [SerializeField] private List<StatefulObject> _explorationPreference = new List<StatefulObject>();
    [SerializeField] private List<StatefulObject> _staminaPreference = new List<StatefulObject>();
    [SerializeField] private List<StatefulObject> _wisdomPreference = new List<StatefulObject>();
    [SerializeField] private TextMeshProUGUI _chimeraInfo = null;
    private ResourceManager _resourceManager = null;
    private EvolutionLogic _evolution = null;

    public EvolutionLogic EvolutionLogic { get => _evolution; }

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void LoadChimeraData(EvolutionLogic evolutionLogic)
    {
        _evolution = evolutionLogic;
        _chimeraName.text = evolutionLogic.Name;
        _icon.sprite = _resourceManager.GetElementSprite(evolutionLogic.ElementType);
        _chimeraInfo.text = evolutionLogic.BackgroundInfo;

        LoadStatPreferences(evolutionLogic);
    }

    private void LoadStatPreferences(EvolutionLogic evolutionLogic)
    {
        StatPreference(_explorationPreference, evolutionLogic.ExplorationPreference);
        StatPreference(_staminaPreference, evolutionLogic.StaminaPreference);
        StatPreference(_wisdomPreference, evolutionLogic.WisdomPreference);
    }

    private void StatPreference(List<StatefulObject> iconList, StatPreferenceType preference)
    {
        foreach (StatefulObject icon in iconList)
        {
            icon.SetState("Empty", true);
        }

        int amount = 0;

        switch (preference)
        {
            case StatPreferenceType.Dislike:
                amount = 1;
                break;
            case StatPreferenceType.Neutral:
                amount = 2;
                break;
            case StatPreferenceType.Like:
                amount = 3;
                break;
            default:
                break;
        }

        for (int i = 0; i < amount; ++i)
        {
            iconList[i].SetState("Filled", true);
        }
    }
}