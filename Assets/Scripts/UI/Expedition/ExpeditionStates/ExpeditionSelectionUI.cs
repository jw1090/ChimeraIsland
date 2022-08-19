using System.Collections.Generic;
using UnityEngine;

public class ExpeditionSelectionUI : MonoBehaviour
{
    [SerializeField] private List<ExpeditionOptionUI> _expeditionOptions = new List<ExpeditionOptionUI>();
    private ExpeditionManager _expeditionManager = null;

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;

        foreach (ExpeditionOptionUI expeditionOptionUI in _expeditionOptions)
        {
            expeditionOptionUI.SetExpeditionManager(expeditionManager);
        }
    }

    public void SetAudioManager(AudioManager audioManager)
    {
        foreach (ExpeditionOptionUI expeditionOptionUI in _expeditionOptions)
        {
            expeditionOptionUI.SetAudioManager(audioManager);
        }
    }

    public void Initialize()
    {

        foreach (ExpeditionOptionUI option in _expeditionOptions)
        {
            option.Initialize();
        }
    }

    public void DisplayExpeditionOptions()
    {
        if (_expeditionManager.CurrentHabitatProgress == 0)
        {
            StarterUpgrade();
        }
        else if (_expeditionManager.CurrentFossilProgress == 0)
        {
            StarterFossil();
        }
        else
        {
            StandardDisplay();
        }
    }

    private void StarterUpgrade()
    {
        foreach (ExpeditionOptionUI option in _expeditionOptions)
        {
            if (option.ExpeditionType != ExpeditionType.HabitatUpgrade)
            {
                option.gameObject.SetActive(false);
                continue;
            }

            option.LoadExpeditionData();
            option.gameObject.SetActive(true);
        }
    }

    private void StarterFossil()
    {
        foreach (ExpeditionOptionUI option in _expeditionOptions)
        {
            if (option.ExpeditionType != ExpeditionType.Fossils)
            {
                option.gameObject.SetActive(false);
                continue;
            }

            option.gameObject.SetActive(true);
            option.LoadExpeditionData();
        }
    }

    private void StandardDisplay()
    {
        foreach (ExpeditionOptionUI option in _expeditionOptions)
        {
            if (option.ExpeditionType == ExpeditionType.HabitatUpgrade && _expeditionManager.CurrentHabitatProgress >= _expeditionManager.UpgradeMissionBounds)
            {
                option.gameObject.SetActive(false);
                continue;
            }

            option.gameObject.SetActive(true);
            option.LoadExpeditionData();
        }
    }
}