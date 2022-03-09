using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] List<ChimeraDetails> chimeraDetailsList;
    [SerializeField] List<Chimera> chimerasList;

    public void Subscribe(ChimeraDetails details)
    {
        if (chimeraDetailsList == null)
        {
            chimeraDetailsList = new List<ChimeraDetails>();
        }

        chimeraDetailsList.Add(details);
    }

    /*
    private void OnEnable()
    {
        chimerasList = GameManager.Instance.GetActiveHabitat().GetChimeras();

        if(chimerasList != null)
        {
            for (int i = 0; i < chimerasList.Count; ++i)
            {
                Debug.Log("Index: " + i);
                Debug.Log("Chimera List Count: " + chimerasList.Count);
                Debug.Log(chimeraDetailsList[i]);
                chimeraDetailsList[i].gameObject.SetActive(true);
            }
        }
    } */
}