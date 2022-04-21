using System.Collections.Generic;
using UnityEngine;

public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] List<ChimeraDetails> _chimeraDetailsList;
    [SerializeField] List<Chimera> _chimerasList;

    public void Initialize()
    {
        _chimerasList = GameManager.Instance.GetActiveHabitat().GetChimeras();

        if(_chimeraDetailsList != null)
        {
            for(int i = 0; i < _chimerasList.Count; ++i)
            {
                _chimeraDetailsList[i].gameObject.SetActive(true);
            }
        }
    }

    public void UpdateDetailsList()
    {
        foreach(var detail in _chimeraDetailsList)
        {
            detail.UpdateDetails();
        }
    }
}