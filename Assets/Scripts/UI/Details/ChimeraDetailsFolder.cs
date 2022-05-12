using System.Collections.Generic;
using UnityEngine;

public class ChimeraDetailsFolder : MonoBehaviour
{
    [SerializeField] private List<Chimera> _chimerasList;
    [SerializeField] private List<ChimeraDetails> _chimeraDetailsList;

    public void Initialize(Habitat habitat)
    {
        Debug.Log("<color=Yellow> Loading Details ... </color>");

        _chimerasList = habitat.ChimeraPrefabs;

        int chimeraSpot = 0;
        foreach (Transform child in transform)
        {
            ChimeraDetails details = child.GetComponent<ChimeraDetails>();

            _chimeraDetailsList.Add(details);
            details.Initialize(habitat, chimeraSpot++);
        }

        CheckDetails();
    }

    public void UpdateDetailsList()
    {
        foreach(var detail in _chimeraDetailsList)
        {
            detail.UpdateDetails();
        }
    }

    public void CheckDetails()
    {
        for (int i = 0; i < _chimerasList.Count; ++i)
        {
            _chimeraDetailsList[i].gameObject.SetActive(true);
        }
        UpdateDetailsList();
    }
}
