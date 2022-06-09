using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectToSwap = new List<GameObject>();
    [SerializeField] private Color _tabIdle;
    [SerializeField] private Color _tabHover;
    [SerializeField] private Color _tabActive;
    private List<TabPress> _tabButtons = new List<TabPress>();
    private TabPress _selectedTab = null;

    public void Initialize()
    {
        foreach (Transform child in transform)
        {
            TabPress tab = child.GetComponent<TabPress>();

            _tabButtons.Add(tab);
            tab.Initialize(this);
        }
    }

    public void OnTabEnter(TabPress button)
    {
        ResetTabs();
        if (_selectedTab == null || button != _selectedTab)
        {
            button.GetComponent<Image>().color = _tabHover;
        }
    }

    public void OnTabExit()
    {
        ResetTabs();
    }

    public void OnTabSelected(TabPress button)
    {
        _selectedTab = button;
        ResetTabs();
        button.GetComponent<Image>().color = _tabActive;

        int pos = button.transform.GetSiblingIndex();
        for (int i = 0; i < _objectToSwap.Count; ++i)
        {
            if (i == pos)
            {
                _objectToSwap[i].SetActive(true);
            }
            else
            {
                _objectToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (var button in _tabButtons)
        {
            if (_selectedTab != null && button == _selectedTab)
            {
                continue;
            }
            button.GetComponent<Image>().color = _tabIdle;
        }
    }
}
