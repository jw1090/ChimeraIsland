using System.Collections.Generic;
using UnityEngine;

public class ExpeditionSelectionUI : MonoBehaviour
{
    [SerializeField] private List<ExpeditionOptionUI> _expeditionOptions = new List<ExpeditionOptionUI>();
    private ExpeditionManager _expeditionManager = null;

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        _expeditionManager = expeditionManager;
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

    public void LoadSelectionExpeditions()
    {
        foreach (ExpeditionOptionUI option in _expeditionOptions)
        {
            InsertExpeditionData(option);
        }
    }

    private void InsertExpeditionData(ExpeditionOptionUI option)
    {

    }
}