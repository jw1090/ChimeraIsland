using System.Collections.Generic;
using UnityEngine;

public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] List<ChimeraDetails> chimeraDetailsList;
    [SerializeField] List<Chimera> chimerasList;

    private void OnEnable()
    {
        chimerasList = GameManager.Instance.GetActiveHabitat().GetChimeras();

        if(chimeraDetailsList != null)
        {
            for(int i = 0; i < chimerasList.Count; ++i)
            {
                chimeraDetailsList[i].gameObject.SetActive(true);
            }
        }
    }

    public void UpdateDetailsList()
    {
        foreach(var detail in chimeraDetailsList)
        {
            detail.UpdateDetails();
        }
    }
}