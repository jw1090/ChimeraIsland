using System.Collections.Generic;
using UnityEngine;

public class DetailsManager : MonoBehaviour
{
    [SerializeField] private List<ChimeraDetailsFolder> _detailsFolders = new List<ChimeraDetailsFolder>();
    [SerializeField] private GameObject _details4Wide = null;
    [SerializeField] private GameObject _detailsPrefab = null;
    private HabitatManager _habitatManager = null;
    private UIManager _uiManager = null;

    public bool IsOpen { get => _details4Wide.activeInHierarchy; }

    public void SetExpeditionManager(ExpeditionManager expeditionManager)
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsFolders)
        {
            detailsPanel.SetExpeditionManager(expeditionManager);
        }
    }

    public void Initialize(UIManager uiManager)
    {
        Debug.Log($"<color=Yellow> Initializing {this.GetType()} ... </color>");

        _uiManager = uiManager;

        _habitatManager = ServiceLocator.Get<HabitatManager>();

        foreach (ChimeraDetailsFolder detailsPanel in _detailsFolders)
        {
            detailsPanel.Initialize(uiManager, _detailsPrefab);
        }

        for (int i = 0; i < _habitatManager.ChimerasInHabitat.Count; ++i)
        {
            IncreaseChimeraSlots();
        }
    }

    public void CloseDetails()
    {
        _details4Wide.SetActive(false);
        _uiManager.Tooltip.gameObject.SetActive(false);
    }

    public void OpenStandardDetails()
    {
        CheckDetails();
        _details4Wide.SetActive(true);
    }

    public void HabitatDetailsSetup()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsFolders)
        {
            detailsPanel.HabitatDetailsSetup();
        }
    }

    public void CheckDetails()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsFolders)
        {
            detailsPanel.CheckDetails();
        }
    }

    public void UpdateDetailsList()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsFolders)
        {
            detailsPanel.UpdateDetailsList();
        }
    }

    public void DetailsStatGlow()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsFolders)
        {
            detailsPanel.DetailsStatGlow();
        }
    }

    public void IncreaseChimeraSlots()
    {
        foreach (ChimeraDetailsFolder detailsPanel in _detailsFolders)
        {
            detailsPanel.IncreaseChimeraDetailsInstance();
        }
    }
}