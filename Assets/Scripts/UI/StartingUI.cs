using System.Collections.Generic;
using UnityEngine;

public class StartingUI : MonoBehaviour
{
    [SerializeField] private List<StartingChimeraButton> _startingChimeraButtons;

    public void Initialize()
    {
        foreach (StartingChimeraButton button in _startingChimeraButtons)
        {
            button.Initialize();
        }
    }
}