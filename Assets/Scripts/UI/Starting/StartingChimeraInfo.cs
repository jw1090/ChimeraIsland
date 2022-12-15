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

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void LoadChimeraData(EvolutionLogic evolutionLogic)
    {
        _chimeraName.text = evolutionLogic.Name;
        _icon.sprite = _resourceManager.GetElementSprite(evolutionLogic.ElementType);
        _chimeraInfo.text = evolutionLogic.BackgroundInfo;

        LoadStatPreferences(evolutionLogic);
    }

    private void LoadStatPreferences(EvolutionLogic evolutionLogic)
    {
        evolutionLogic.GetPreferredStat(evolutionLogic.ChimeraType, out int explorationAmount, out int staminaAmount, out int wisdomAmount);

        StatPreference(_explorationPreference, explorationAmount);
        StatPreference(_staminaPreference, staminaAmount);
        StatPreference(_wisdomPreference, wisdomAmount);
    }

    private void StatPreference(List<StatefulObject> iconList, int amount)
    {
        foreach (StatefulObject icon in iconList)
        {
            icon.SetState("Empty", true);
        }

        for (int i = 0; i < Translation(amount); ++i)
        {
            iconList[i].SetState("Filled", true);
        }
    }

    private int Translation(int amount)
    {
        switch (amount)
        {
            case 2:
                return 3;
            case 3:
                return 2;
            case 4:
                return 1;
            default:
                return 0;
        }
    }
}