using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] List<ChimeraDetails> chimeraDetailsList;

    public void Subscribe(ChimeraDetails details)
    {
        if (chimeraDetailsList == null)
        {
            chimeraDetailsList = new List<ChimeraDetails>();
        }

        chimeraDetailsList.Add(details);
    }

    private void OnEnable()
    {
        foreach(ChimeraDetails chimeraDetails in chimeraDetailsList)
        {

        }
    }
}