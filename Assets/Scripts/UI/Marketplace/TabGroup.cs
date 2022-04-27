using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<TabPress> tabButtons;
    [SerializeField] private Color tabIdle;
    [SerializeField] private Color tabHover;
    [SerializeField] private Color tabActive;
    [SerializeField] private TabPress selectedTab;
    [SerializeField] private List<GameObject> objectToSwap;

    public void Subscribe(TabPress button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabPress>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabPress button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.GetComponent<Image>().color = tabHover;
        }
    }
    public void OnTabExit(TabPress button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabPress button)
    {
        selectedTab = button;
        ResetTabs();
        button.GetComponent<Image>().color = tabActive;

        int pos = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectToSwap.Count; ++i)
        {
            if(i == pos)
            {
                objectToSwap[i].SetActive(true);
            }
            else
            {
                objectToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(var button in tabButtons)
        {
            if( selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.GetComponent<Image>().color = tabIdle;
        }
    }
}
