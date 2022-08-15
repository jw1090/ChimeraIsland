using System.Collections.Generic;
using UnityEngine;

public class ExpeditionSelectionUI : MonoBehaviour
{
    [SerializeField] private List<ExpeditionOptionUI> _expeditionOptions = new List<ExpeditionOptionUI>();

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        foreach(ExpeditionOptionUI expeditionOptionUI in _expeditionOptions)
        {
            expeditionOptionUI.SetExpeditionManager(expeditionManager);
        }
    }


    public void Initialize()
    {
        foreach (ExpeditionOptionUI option in _expeditionOptions)
        {
            option.Initialize();
        }
    }

    public void SetupListeners()
    {

    }

    public void DisplayExpeditionOptions()
    {
        foreach (ExpeditionOptionUI option in _expeditionOptions)
        {
            option.LoadExpeditionData();
        }
    }

    
}