using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingLevelManager : MonoBehaviour
{
    [SerializeField] private List<StartingChimeraButton> _startingChimeraButtons;
    
    private void Awake()
    {
        //GameLoader.CallOnComplete(Initialize); TODO: This is not triggering correctly, ask Craig
        StartCoroutine(InitializeAsync());
    }

    private IEnumerator InitializeAsync()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (StartingChimeraButton button in _startingChimeraButtons)
        {
            button.Initialize();
        }
    }
}