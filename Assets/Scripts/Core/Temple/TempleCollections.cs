using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleCollections : MonoBehaviour
{
    [Header("Chimeras")]
    [SerializeField] private GameObject _a = null;
    [SerializeField] private GameObject _a1 = null;
    [SerializeField] private GameObject _a2 = null;
    [SerializeField] private GameObject _a3 = null;
    [SerializeField] private GameObject _b = null;
    [SerializeField] private GameObject _b1 = null;
    [SerializeField] private GameObject _b2 = null;
    [SerializeField] private GameObject _b3 = null;
    [SerializeField] private GameObject _c = null;
    [SerializeField] private GameObject _c1 = null;
    [SerializeField] private GameObject _c2 = null;
    [SerializeField] private GameObject _c3 = null;
    private HabitatManager _habitatManager = null;

    private void Awake()
    {
        _habitatManager = ServiceLocator.Get<HabitatManager>();
        Build();
    }
    private void Build()
    { 
        if(_habitatManager.ChimeraCollections.AUnlocked == true)
        {
            _a.SetActive(true);
        }
        if(_habitatManager.ChimeraCollections.A1Unlocked == true)
        {
            _a1.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.A2Unlocked == true)
        {
            _a2.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.A3Unlocked == true)
        {
            _a3.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.BUnlocked == true)
        {
            _b.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.B1Unlocked == true)
        {
            _b1.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.B2Unlocked == true)
        {
            _b2.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.B3Unlocked == true)
        {
            _b3.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.CUnlocked == true)
        {
            _c.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.C1Unlocked == true)
        {
            _c1.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.C2Unlocked == true)
        {
            _c2.SetActive(true);
        }
        if (_habitatManager.ChimeraCollections.C3Unlocked == true)
        {
            _c3.SetActive(true);
        }
    }
}
