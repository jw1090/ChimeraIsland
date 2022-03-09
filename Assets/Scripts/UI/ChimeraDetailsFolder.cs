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

    public void ActivateDetailsPanel()
    {
        List<Chimera> chimerasList = GameManager.Instance.GetActiveHabitat().GetChimeras();

        for (int i = 0; i < chimerasList.Count; ++i)
        {
            chimeraDetailsList[i].gameObject.SetActive(true);
        }
    }
}