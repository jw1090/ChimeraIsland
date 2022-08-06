using System.Collections.Generic;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    [SerializeField] private List<ExpeditionData> _habitatExpeditions = new List<ExpeditionData>();
    [SerializeField] private int _currentExpedition = 0;
    private List<Chimera> _chimeras = new List<Chimera>();
    private UIExpedition _uiExpedition = null;

    public ExpeditionData CurrentExpeditionData { get => _habitatExpeditions[_currentExpedition]; }

    public ExpeditionManager Initialize()
    {
        Debug.Log($"<color=Orange> Initializing {this.GetType()} ... </color>");

        _uiExpedition = ServiceLocator.Get<UIManager>().HabitatUI.ExpeditionPanel;

        return this;
    }

    public bool AddChimera(Chimera chimera)
    {
        _chimeras.Add(chimera);
        _uiExpedition.UpdateIcons(_chimeras);

        return true;
    }

    public bool RemoveChimera(Chimera chimera)
    {
        _chimeras.Remove(chimera);
        _uiExpedition.UpdateIcons(_chimeras);

        return true;
    }
}