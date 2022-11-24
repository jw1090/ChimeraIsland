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

        LoadStatPreferences(evolutionLogic);
    }

    private void LoadStatPreferences(EvolutionLogic evolutionLogic)
    {
        evolutionLogic.GetPreferredStat(evolutionLogic.ChimeraType, out int explorationAmount, out int staminaAmount, out int wisdomAmount);

        StatPreference(_explorationPreference, explorationAmount);
        StatPreference(_staminaPreference, staminaAmount);
        StatPreference(_wisdomPreference, wisdomAmount);
    }

    private void StatPreference(List<GameObject> iconList, int amount)
    {
        foreach (GameObject icon in iconList)
        {
            icon.SetActive(false);
        }

        for (int i = 0; i < Translation(amount); ++i)
        {
            iconList[i].SetActive(true);
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